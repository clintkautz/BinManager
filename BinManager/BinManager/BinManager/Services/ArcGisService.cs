
namespace BinManager
{
    #region imports
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Esri.ArcGISRuntime.Security;
    using Esri.ArcGISRuntime.Http;
    using System.Collections.Generic;
    using Esri.ArcGISRuntime.Data;
    using Esri.ArcGISRuntime.Geometry;
    using Esri.ArcGISRuntime.Mapping;
    using Newtonsoft.Json;
    using System.Text;
    using BinManager.Utilities.Enums;
    using BinManager.Models;
    using BinManager.ViewModels;
    using System.IO;
    using Plugin.Geolocator;
    using Xamarin.Essentials;
    #endregion

    public static class ArcGisService
    {
        #region constants
        public static readonly string YTY_FILE_NAME = "YTY.json";

        public static string AuthenticateUrl = "https://www.arcgis.com/sharing/test";
        public static string NaucWebMapUrl = "https://clint-kautz.maps.arcgis.com";
        public static string WebMapItemUrl = "https://clint-kautz.maps.arcgis.com/home/webmap/viewer.html?webmap=a5d4550d71a846c9be377c961c44ed3b";
        public static string WebMapItemId = "a5d4550d71a846c9be377c961c44ed3b";
        public static string FeatureServiceUrl = "https://services9.arcgis.com/y1JUYZTXUMlYQZ7N/arcgis/rest/services/bininstance/FeatureServer/0";

        private static ServiceFeatureTable _featureTable;
        private static FeatureLayer _featureLayer;

        #endregion

        static ArcGisService()
        {
            _featureTable = new ServiceFeatureTable(new Uri(FeatureServiceUrl));
            _featureLayer = new FeatureLayer(_featureTable);
        }

        #region Authenticate
        public static async Task<ArcCrudEnum> Authenticate(string username, string password)
        {
            try
            {
                if (AuthenticationManager.Current.Credentials.Any()) return ArcCrudEnum.Success;

                var opts = new GenerateTokenOptions();
                opts.TokenAuthenticationType = TokenAuthenticationType.ArcGISToken;

                var authUrl = AuthenticateUrl;
                AuthenticationManager.Current.RegisterServer(new ServerInfo
                {
                    ServerUri = new Uri(AuthenticateUrl),
                    TokenAuthenticationType = TokenAuthenticationType.ArcGISToken

                });

                var cred = await AuthenticationManager.Current.GenerateCredentialAsync
                (new Uri(AuthenticateUrl),
                    username,
                    password,
                    opts
                   );

                AuthenticationManager.Current.AddCredential(cred);

                return ArcCrudEnum.Success;
            }
            catch (ArcGISWebException)
            {
                return ArcCrudEnum.Failure;
            }
        }

        public static async Task<IdentifierAvailableEnum> IdentifierAvailable(string identifier)
        {
            try
            {
                await _featureTable.LoadAsync();

                var query = new QueryParameters();
                query.WhereClause = $"lower(identifier) = '{identifier}'";

                var all = await _featureTable.FeatureLayer.SelectFeaturesAsync(query, Esri.ArcGISRuntime.Mapping.SelectionMode.New);

                // if any, record returned with 'identifier' being attempted to be used
                if(all.Any())
                {
                    return IdentifierAvailableEnum.NotAvailable;
                }
                return IdentifierAvailableEnum.Available;
            }
            catch(ArcGISWebException)
            {
                return IdentifierAvailableEnum.Exception;
            }
        }
        #endregion

        #region Add New Bin
        public static async Task<ArcCrudEnum> AddBin(BinViewModel binViewModel)
        {
            IBinstance bin = binViewModel.Binstance;
            IEnumerable<YTYData> yTY = bin.YTYDatas;

            try
            {
                await _featureTable.LoadAsync();

                var attributes = new Dictionary<string, object>();
                attributes.Add("identifier", bin.Identifier);
                attributes.Add("created_by", bin.CreatedBy);
                attributes.Add("modified_by", bin.ModifiedBy);

                switch(bin.BinType)
                {
                    case BinTypeEnum.RoundStorage:
                        attributes.Add("bin_type", "round_storage");
                        break;
                    case BinTypeEnum.GravityWagon:
                        attributes.Add("bin_type", "gravity_wagon");
                        break;
                    case BinTypeEnum.PolygonStructure:
                        attributes.Add("bin_type", "polygon_structure");
                        break;
                    case BinTypeEnum.FlatStructure:
                        attributes.Add("bin_type", "flat_structure");
                        break;
                }

                attributes.Add("year_collected", bin.YearCollected);

                if (bin.IsLeased.HasValue)
                {
                    attributes.Add("owned_or_leased", bin.IsLeased.Value ? "leased" : "owned");
                }

                if (bin.HasDryingDevice.HasValue)
                {
                    attributes.Add("drying_device", bin.HasDryingDevice.Value ? "true" : "false");
                }

                if (bin.HasGrainHeightIndicator.HasValue)
                {
                    attributes.Add("bin_level_indicator_device", bin.HasGrainHeightIndicator.Value ? "true" : "false");
                }

                switch(bin.LadderType)
                {
                    case Ladder.None:
                        attributes.Add("ladder_type", "none");
                        break;
                    case Ladder.Ladder:
                        attributes.Add("ladder_type", "ladder");
                        break;
                    case Ladder.Stairs:
                        attributes.Add("ladder_type", "stairs");
                        break;
                }               

                attributes.Add("notes", bin.Notes);
                                
                //bin type specific logic below
                Type t = bin.GetType();
                if (bin.BinType == BinTypeEnum.FlatStructure)
                {
                    if (t.Equals(typeof(FlatBin)))
                    {
                        FlatBin flat = (FlatBin)bin;
                        attributes.Add("crib_height", flat.CribLength);
                        attributes.Add("crib_width", flat.CribWidth);
                    }                    
                }
                else if (bin.BinType == BinTypeEnum.GravityWagon)
                {
                    if (t.Equals(typeof(GravityBin)))
                    {
                        GravityBin gravityBin = (GravityBin)bin;
                        attributes.Add("chute_length", gravityBin.ChuteLength);
                        attributes.Add("hopper_height", gravityBin.HopperHeight);
                        attributes.Add("rectangle_height", gravityBin.RectangleHeight);
                        attributes.Add("rectangle_length", gravityBin.RectangleLength);
                        attributes.Add("rectangle_width", gravityBin.RectangleWidth);
                    }                    
                }
                else if (bin.BinType == BinTypeEnum.PolygonStructure)
                {
                    if (t.Equals(typeof(PolygonBin)))
                    {
                        PolygonBin polygonBin = (PolygonBin)bin;
                        attributes.Add("side_height", polygonBin.SideHeight);
                        attributes.Add("side_width", polygonBin.SideWidth);
                        attributes.Add("number_of_sides", polygonBin.NumberOfSides);
                    }
                    
                }
                else if (bin.BinType == BinTypeEnum.RoundStorage)
                {                  
                    if (t.Equals(typeof(RoundBin)))
                    {
                        RoundBin round = (RoundBin) bin;
                        if (round.HasHopper.HasValue)
                        {
                            attributes.Add("has_hopper", round.HasHopper.Value ? "true" : "false");
                        }
                        attributes.Add("radius", round.Radius);
                        attributes.Add("wall_height", round.WallHeight);
                        attributes.Add("roof_height", round.RoofHeight);
                        attributes.Add("hopper_height", round.HopperHeight);
                    }                    
                }
                
                MapPoint mp;
                double lat = 0;
                double lon = 0;
                
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 1000;
                    var position = await locator.GetPositionAsync();

                    if (position == null)
                    {
                        Console.WriteLine("Location Service error");
                    }
                    lat = position.Latitude;
                    lon = position.Longitude;


                }
                catch { }

                string lonS = lon.ToString();
                string[] split = lonS.Split('.');
                var charArray = split[1].ToCharArray();
                //seperate good location, make third if necassary
                string first = charArray[0].ToString();
                string second = charArray[1].ToString();
                string third = charArray[2].ToString();
                Random random = new Random();
                int addition = random.Next(25, 75);
                string additionS = addition.ToString();
                string newLong = split[0] + '.' + first + second + third + additionS;
                //latitude
                string latS = lat.ToString();
                string[] splitL = latS.Split('.');
                var charArrayL = splitL[1].ToCharArray();
                //seperate good location, make third if necassary
                string firstL = charArrayL[0].ToString();
                string secondL = charArrayL[1].ToString();
                string thirdL = charArrayL[2].ToString();
                int additionL = random.Next(25, 75);
                string additionSL = additionL.ToString();
                string newLat = splitL[0] + '.' + firstL + secondL + thirdL + additionSL;
                double clusteredLongitude = double.Parse(newLong);
                double clusteredLatitude = double.Parse(newLat);

                mp = new MapPoint(clusteredLongitude, clusteredLatitude);
                
                binViewModel.ArcGISFeature = (ArcGISFeature)_featureTable.CreateFeature(attributes, mp);

                await _featureTable.AddFeatureAsync(binViewModel.ArcGISFeature);

                await binViewModel.ArcGISFeature.LoadAsync();

                if (DeviceInfo.DeviceType == DeviceType.Physical)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        bin.Image.GetStream().CopyTo(memoryStream);
                        // add an attachment - pass the name, mime type, and attachment data (bytes, etc.)
                        await binViewModel.ArcGISFeature.AddAttachmentAsync("Photo.jpg", "image/jpg", memoryStream.ToArray()); //agsFeature
                    }
                }
                                              
                if (yTY == null)
                {
                    yTY = new List<YTYData>();
                }

                //create json
                string jsonString = JsonConvert.SerializeObject(yTY);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);

                List<Attachment> attachmentsToRemove = new List<Attachment>();
                IReadOnlyList<Attachment> attachments = await binViewModel.ArcGISFeature.GetAttachmentsAsync();
                foreach (Attachment attachment in attachments)
                {
                    if (attachment.Name.Equals(YTY_FILE_NAME))
                    {
                        attachmentsToRemove.Add(attachment);
                    }
                }

                await binViewModel.ArcGISFeature.DeleteAttachmentsAsync(attachmentsToRemove);
                
                await binViewModel.ArcGISFeature.AddAttachmentAsync(YTY_FILE_NAME, "application/json", byteArray); //agsFeature

                await _featureTable.UpdateFeatureAsync(binViewModel.ArcGISFeature); //agsFeature

                // push to ArcGIS Online feature service
                IReadOnlyList<EditResult> editResults = await _featureTable.ApplyEditsAsync();

                // check results for errors
                foreach (var er in editResults)
                {
                    if (er.CompletedWithErrors)
                    {
                        return ArcCrudEnum.Failure;
                    }
                }

                return ArcCrudEnum.Success;

            }
            catch (ArcGISWebException)
            {
                return ArcCrudEnum.Exception;
            }
        }
        #endregion

        #region Edit Bin
        public static async Task<ArcCrudEnum> EditBin(BinViewModel binViewModel)
        {
            IBinstance bin = binViewModel.Binstance;
            ArcGISFeature featureToEdit = binViewModel.ArcGISFeature;

            try
            {
                await featureToEdit.LoadAsync();

                featureToEdit.Attributes["identifier"] = bin.Identifier;
                featureToEdit.Attributes["modified_by"] = binViewModel.EmpoyeeNumber;

                switch (bin.BinType)
                {
                    case BinTypeEnum.RoundStorage:
                        featureToEdit.Attributes["bin_type"] = "round_storage";
                        break;
                    case BinTypeEnum.GravityWagon:
                        featureToEdit.Attributes["bin_type"] = "gravity_wagon";
                        break;
                    case BinTypeEnum.PolygonStructure:
                        featureToEdit.Attributes["bin_type"] = "polygon_structure";
                        break;
                    case BinTypeEnum.FlatStructure:
                        featureToEdit.Attributes["bin_type"] = "flat_structure";
                        break;
                }

                featureToEdit.Attributes["year_collected"] = bin.YearCollected;


                if (bin.IsLeased.HasValue)
                {
                    featureToEdit.Attributes["owned_or_leased"] = bin.IsLeased.Value ? "leased" : "owned";
                }

                if (bin.HasDryingDevice.HasValue)
                {
                    featureToEdit.Attributes["drying_device"] = bin.HasDryingDevice.Value ? "true" : "false";
                }

                if (bin.HasGrainHeightIndicator.HasValue)
                {
                    featureToEdit.Attributes["bin_level_indicator_device"] = bin.HasGrainHeightIndicator.Value ? "true" : "false";
                }

                switch (bin.LadderType)
                {
                    case Ladder.None:
                        featureToEdit.Attributes["ladder_type"] = "none";
                        break;
                    case Ladder.Ladder:
                        featureToEdit.Attributes["ladder_type"] = "ladder";
                        break;
                    case Ladder.Stairs:
                        featureToEdit.Attributes["ladder_type"] = "stairs";
                        break;
                }

                featureToEdit.Attributes["notes"] = bin.Notes;

                double dr;
                //double.TryParse(bin.BinVolume, out dr);
                //featureToEdit.Attributes["bin_volume"] = dr;

                //bin type specific logic below
                Type t = bin.GetType();
                if (bin.BinType == BinTypeEnum.FlatStructure)
                {
                    if (t.Equals(typeof(FlatBin)))
                    {
                        FlatBin flat = (FlatBin)bin;
                        featureToEdit.Attributes["crib_height"] = flat.CribLength;
                        featureToEdit.Attributes["crib_width"] = flat.CribWidth;
                    }                    
                }
                else if (bin.BinType == BinTypeEnum.GravityWagon)
                {
                    if (t.Equals(typeof(GravityBin)))
                    {
                        GravityBin gravityBin = (GravityBin)bin;
                        featureToEdit.Attributes["chute_length"] = gravityBin.ChuteLength;
                        featureToEdit.Attributes["hopper_height"] = gravityBin.HopperHeight;
                        featureToEdit.Attributes["rectangle_height"] = gravityBin.RectangleHeight;
                        featureToEdit.Attributes["rectangle_length"] = gravityBin.RectangleLength;
                        featureToEdit.Attributes["rectangle_width"] = gravityBin.RectangleWidth;
                    }                    
                }
                else if (bin.BinType == BinTypeEnum.PolygonStructure)
                {
                    if (t.Equals(typeof(PolygonBin)))
                    {
                        PolygonBin polygonBin = (PolygonBin)bin;
                        featureToEdit.Attributes["side_height"] = polygonBin.SideHeight;
                        featureToEdit.Attributes["side_width"] = polygonBin.SideWidth;
                        featureToEdit.Attributes["number_of_sides"] = polygonBin.NumberOfSides;
                    }                   
                }
                else if (bin.BinType == BinTypeEnum.RoundStorage)
                {
                    if (t.Equals(typeof(PolygonBin)))
                    {
                        RoundBin round = (RoundBin)bin;
                        if (round.HasHopper.HasValue)
                        {
                            featureToEdit.Attributes["has_hopper"] = round.HasHopper.Value ? "true" : "false";
                        }

                        featureToEdit.Attributes["radius"] = round.Radius;
                        featureToEdit.Attributes["wall_height"] = round.WallHeight;
                        featureToEdit.Attributes["roof_height"] = round.RoofHeight;
                        featureToEdit.Attributes["hopper_height"] = round.HopperHeight;
                    }                    
                }

                
                // can't be null
                if (binViewModel.Binstance.YTYDatas == null)
                {
                    binViewModel.Binstance.YTYDatas = new List<YTYData>();
                }
                //use data in _binViewModel

                System.Diagnostics.Debug.Print("Feature can edit attachments" + (featureToEdit.CanEditAttachments ? "Yes" : "No"));
                //-------- Formatting --------
                //create json
                string jsonString = JsonConvert.SerializeObject(binViewModel.Binstance.YTYDatas);

                //System.Diagnostics.Debug.Print(jsonString);
                //System.Diagnostics.Debug.Print(((Binstance)(binViewModel.Binstance)).YTYDatasString());
                // convert json to byte array
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);


                //-------- ARC Connection --------
                //remove old YTYData
                List<Attachment> attachmentsToRemove = new List<Attachment>();
                IReadOnlyList<Attachment> attachments = await featureToEdit.GetAttachmentsAsync();
                foreach (Attachment attachment in attachments)
                {
                    System.Diagnostics.Debug.Print(attachment.Name);
                    if (attachment.Name.Equals(YTY_FILE_NAME))
                    {
                        System.Diagnostics.Debug.Print("Found YTY attachment");
                        attachmentsToRemove.Add(attachment);
                    }
                }
                System.Diagnostics.Debug.Print("Attachments to remove:");
                foreach (Attachment attachment in attachments)
                {
                    System.Diagnostics.Debug.Print(attachment.Name);
                }

                if(attachmentsToRemove.Any())
                {
                    //update the json file
                    await featureToEdit.UpdateAttachmentAsync(attachmentsToRemove.First(), YTY_FILE_NAME, "application/json", byteArray);
                }

                _featureTable = (ServiceFeatureTable)featureToEdit.FeatureTable;

                // update feature after attachment added
                await _featureTable.UpdateFeatureAsync(featureToEdit); //agsFeature

                System.Diagnostics.Debug.Print("Feature table updated");

                // push to ArcGIS Online feature service
                IReadOnlyList<EditResult> editResults = await _featureTable.ApplyEditsAsync();

                System.Diagnostics.Debug.Print("Arc updated");

                foreach (var er in editResults)
                {
                    if (er.CompletedWithErrors)
                    {
                        // handle error (er.Error.Message)
                        return ArcCrudEnum.Failure;
                    }
                }

                return ArcCrudEnum.Success;
            }
            catch (ArcGISWebException)
            {
                return ArcCrudEnum.Exception; 
            }
        }

        #endregion

        #region Delete Bin
        public static async Task<ArcCrudEnum> DeleteBin(ArcGISFeature featureToDelete)
        {
            try
            {
                await _featureTable.DeleteFeatureAsync(featureToDelete);

                // push to ArcGIS Online feature service
                IReadOnlyList<EditResult> editResults = await _featureTable.ApplyEditsAsync();

                foreach (var er in editResults)
                {
                    if (er.CompletedWithErrors)
                    {
                        // handle error (er.Error.Message)
                        return ArcCrudEnum.Failure;
                    }
                }

                return ArcCrudEnum.Success;
            }
            catch (ArcGISWebException)
            {
                return ArcCrudEnum.Exception;
            }
        }

        #endregion

    }
}