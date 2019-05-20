using BinManager.Models;
using BinManager.Utilities.Enums;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Esri.ArcGISRuntime.Data;
using BinManager.Utilities.Mappers;
using System.IO;
using Newtonsoft.Json;

namespace BinManager.ViewModels
{   
    public class BinViewModel : BaseViewModel
    {
        #region Constants

        //static int i;
        //private static int minDate = 1950;
        //private static int maxDate = int.TryParse(DateTime.Now.ToString("yyyy"), out i) ? i : 2100;
        //public static List<int> dateRange = SetDateRange();

        //private static List<int> SetDateRange()
        //{
        //    var temp = new List<int>();
        //    for (int i = maxDate; i >= minDate; i--)
        //    {
        //        temp.Add(i);
        //    }

        //    return temp;
        //}
        #endregion

        #region Properties
        public IBinstance Binstance { get; set; }
        public YTYData YTYData { get; set; }
        public bool Edit { get; set; }
        public bool New { get; set; }
        public ArcGISFeature ArcGISFeature { get; set; }
        public Dictionary<int, string> Errors { get; set; }
        //public List<int> DateRange { get; set; }

        #region General Bin Properties
        string yearCollected = string.Empty;
        public string YearCollected
        {
            get { return yearCollected; }
            set { SetProperty(ref yearCollected, value); }
        }

        string createdBy = string.Empty;
        public string CreatedBy
        {
            get { return createdBy; }
            set { SetProperty(ref createdBy, value); }
        }

        string modifiedBy = string.Empty;
        public string ModifiedBy
        {
            get { return modifiedBy; }
            set { SetProperty(ref modifiedBy, value); }
        }

        string identifier = string.Empty;
        public string Identifier
        {
            get { return identifier; }
            set { SetProperty(ref identifier, value); }
        }

        int binTypeIndex = -1;
        public int BinType
        {
            get { return binTypeIndex; }
            set { SetProperty(ref binTypeIndex, value); }
        }

        int ownedIndex = -1;
        public int Owned
        {
            get { return ownedIndex; }
            set { SetProperty(ref ownedIndex, value); }
        }

        int hasHopperIndex = -1;
        public int HasHopper
        {
            get { return hasHopperIndex; }
            set { SetProperty(ref hasHopperIndex, value); }
        }

        int hasDryingDeviceIndex = -1;
        public int HasDryingDevice
        {
            get { return hasDryingDeviceIndex; }
            set { SetProperty(ref hasDryingDeviceIndex, value); }
        }

        int hasGrainHeightIndicatorIndex = -1;
        public int HasGrainHeightIndicator
        {
            get { return hasGrainHeightIndicatorIndex; }
            set { SetProperty(ref hasGrainHeightIndicatorIndex, value); }
        }

        int ladderTypeIndex = -1;
        public int LadderType
        {
            get { return ladderTypeIndex; }
            set { SetProperty(ref ladderTypeIndex, value); }
        }

        string notes = string.Empty;
        public string Notes
        {
            get { return notes; }
            set { SetProperty(ref notes, value); }
        }
        #endregion

        #region Capacity Properties
        //#region Flat Bin Properties

        //string cribWidth = string.Empty;
        //public string CribWidth
        //{
        //    get { return cribWidth; }
        //    set { SetProperty(ref cribWidth, value); }
        //}

        //string cribLength = string.Empty;
        //public string CribLength
        //{
        //    get { return cribLength; }
        //    set { SetProperty(ref cribLength, value); }
        //}
        //#endregion

        //#region Gravity Bin Properties
        //string rectangleHeight = string.Empty;
        //public string RectangleHeight
        //{
        //    get { return rectangleHeight; }
        //    set { SetProperty(ref rectangleHeight, value); }
        //}

        //string rectangleWidth = string.Empty;
        //public string RectangleWidth
        //{
        //    get { return rectangleWidth; }
        //    set { SetProperty(ref rectangleWidth, value); }
        //}

        //string rectangleLength = string.Empty;
        //public string RectangleLength
        //{
        //    get { return rectangleLength; }
        //    set { SetProperty(ref rectangleLength, value); }
        //}

        //string chuteLength = string.Empty;
        //public string ChuteLength
        //{
        //    get { return chuteLength; }
        //    set { SetProperty(ref chuteLength, value); }
        //}

        //string hopperHeight = string.Empty;
        //public string HopperHeight
        //{
        //    get { return hopperHeight; }
        //    set { SetProperty(ref hopperHeight, value); }
        //}
        //#endregion

        //#region Polygon Bin Properties
        //string sideHeight = string.Empty;
        //public string SideHeight
        //{
        //    get { return sideHeight; }
        //    set { SetProperty(ref sideHeight, value); }
        //}

        //string sideWidth = string.Empty;
        //public string SideWidth
        //{
        //    get { return sideWidth; }
        //    set { SetProperty(ref sideWidth, value); }
        //}

        //string numberOfSides = string.Empty;
        //public string NumberOfSides
        //{
        //    get { return numberOfSides; }
        //    set { SetProperty(ref numberOfSides, value); }
        //}
        //#endregion

        //#region Round Bin Properties
        //string radius = string.Empty;
        //public string Radius
        //{
        //    get { return radius; }
        //    set { SetProperty(ref radius, value); }
        //}

        //string wallHeight = string.Empty;
        //public string WallHeight
        //{
        //    get { return wallHeight; }
        //    set { SetProperty(ref wallHeight, value); }
        //}

        //string roofHeight = string.Empty;
        //public string RoofHeight
        //{
        //    get { return roofHeight; }
        //    set { SetProperty(ref roofHeight, value); }
        //}

        //string roundHopperHeight = string.Empty;
        //public string RoundHopperHeight
        //{
        //    get { return roundHopperHeight; }
        //    set { SetProperty(ref roundHopperHeight, value); }
        //}

        //#endregion

        #endregion

        #region Contents Properties
        //int cropYearIndex = -1;
        //public int CropYear
        //{
        //    get { return cropYearIndex; }
        //    set { SetProperty(ref cropYearIndex, value); }
        //}

        //string crop = string.Empty;
        //public string Crop
        //{
        //    get { return crop; }
        //    set { SetProperty(ref crop, value); }
        //}

        //string grainHeight = string.Empty;
        //public string GrainHeight
        //{
        //    get { return grainHeight; }
        //    set { SetProperty(ref grainHeight, value); }
        //}

        //string grainHopperHeight = string.Empty;
        //public string GrainHopperHeight
        //{
        //    get { return grainHopperHeight; }
        //    set { SetProperty(ref grainHopperHeight, value); }
        //}

        //string grainConeHeight = string.Empty;
        //public string GrainConeHeight
        //{
        //    get { return grainConeHeight; }
        //    set { SetProperty(ref grainConeHeight, value); }
        //}

        //string moisturePercent = string.Empty;
        //public string MoisturePercent
        //{
        //    get { return moisturePercent; }
        //    set { SetProperty(ref moisturePercent, value); }
        //}

        //string moistureFactor = string.Empty;
        //public string MoistureFactor
        //{
        //    get { return moistureFactor; }
        //    set { SetProperty(ref moistureFactor, value); }
        //}

        //string testWeight = string.Empty;
        //public string TestWeight
        //{
        //    get { return testWeight; }
        //    set { SetProperty(ref testWeight, value); }
        //}

        //string packFactor = string.Empty;
        //public string PackFactor
        //{
        //    get { return packFactor; }
        //    set { SetProperty(ref packFactor, value); }
        //}

        //string dockagePercent = string.Empty;
        //public string DockagePercent
        //{
        //    get { return dockagePercent; }
        //    set { SetProperty(ref dockagePercent, value); }
        //}

        //string dockageFactor = string.Empty;
        //public string DockageFactor
        //{
        //    get { return dockageFactor; }
        //    set { SetProperty(ref dockageFactor, value); }
        //}

        //int conversionFactorIndex = -1;
        //public int ConversionFactor
        //{
        //    get { return conversionFactorIndex; }
        //    set { SetProperty(ref conversionFactorIndex, value); }
        //}

        //string shellFactor = string.Empty;
        //public string ShellFactor
        //{
        //    get { return shellFactor; }
        //    set { SetProperty(ref shellFactor, value); }
        //}

        //string totalDeductionVolume = string.Empty;
        //public string TotalDeductionVolume
        //{
        //    get { return totalDeductionVolume; }
        //    set { SetProperty(ref totalDeductionVolume, value); }
        //}

        //string contentsNotes = string.Empty;
        //public string ContentsNotes
        //{
        //    get { return contentsNotes; }
        //    set { SetProperty(ref contentsNotes, value); }
        //}

        //string grainVolume = string.Empty;
        //public string GrainVolume
        //{
        //    get { return grainVolume; }
        //    set { SetProperty(ref grainVolume, value); }
        //}
        #endregion

        #endregion

        #region Constructors

        public BinViewModel()
        {
            Title = "Add New Bin";
            Binstance = new Binstance();
            YTYData = new YTYData();
            //var test = dateRange;
            //DateRange = dateRange;
            New = true;
            Edit = false;

        }

        public BinViewModel(ArcGISFeature feature)
        {
            Title = "View Bin";
            New = false;
            Edit = false;

            ArcGISFeature = feature;
            //Binstance = ArcGISFeature.MapToBin();
            //FeatureMapBinType();
            //display contents information
            //SetYTY();
            //if (Binstance.YTYDatas.Count > 0)
            //{
            //    YTYData = Binstance.YTYDatas[0];

                //foreach (var year in dateRange)
                //{
                //    if (year == YTYData.CropYear)
                //    {
                //        CropYear = year;                        
                //        break;
                //    }
                //}

                //Crop = YTYData.Crop;
                //GrainHeight = YTYData.GrainHeight.ToString();
                //GrainHopperHeight = YTYData.GrainHopperHeight.ToString();
                //GrainConeHeight = YTYData.ConeHeight.ToString();
                //MoisturePercent = YTYData.MoistureOfGrain.ToString();
                //MoistureFactor = YTYData.MoistureFactor.ToString();
                //TestWeight = YTYData.TestWeight.ToString();
                //PackFactor = YTYData.PackFactor.ToString();
                //DockagePercent = YTYData.DockagePercent.ToString();
                //DockageFactor = YTYData.DockageFactor.ToString();
                ////GrainVolume = YTYData.TotalVolume.ToString();

                //switch (YTYData.ConversionFactor)
                //{
                //    case 0.4:
                //        ConversionFactor = 0;
                //        break;
                //    case 0.8:
                //        ConversionFactor = 1;
                //        break;
                //    default:
                //        ConversionFactor = -1;
                //        break;
                //}

                //ShellFactor = YTYData.ShellFactor.ToString();
                //TotalDeductionVolume = YTYData.TotalDeductionVolume.ToString();
                //ContentsNotes = YTYData.Notes;
            //}

            //dsiplay general information
            //YearCollected = Binstance.YearCollected;
            //CreatedBy = Binstance.CreatedBy;
            //ModifiedBy = Binstance.ModifiedBy;
            //Identifier = Binstance.Identifier;
            //switch (Binstance.IsLeased)
            //{
            //    case true:
            //        Owned = 0;
            //        break;
            //    case false:
            //        Owned = 1;
            //        break;
            //    default:
            //        Owned = -1;
            //        break;
            //}

            //switch (Binstance.HasDryingDevice)
            //{
            //    case true:
            //        HasDryingDevice = 0;
            //        break;
            //    case false:
            //        HasDryingDevice = 1;
            //        break;
            //    default:
            //        HasDryingDevice = -1;
            //        break;
            //}

            //switch (Binstance.HasGrainHeightIndicator)
            //{
            //    case true:
            //        HasGrainHeightIndicator = 0;
            //        break;
            //    case false:
            //        HasGrainHeightIndicator = 1;
            //        break;
            //    default:
            //        HasGrainHeightIndicator = -1;
            //        break;
            //}

            //switch (Binstance.LadderType)
            //{
            //    case Ladder.None:
            //        LadderType = 0;
            //        break;
            //    case Ladder.Stairs:
            //        LadderType = 1;
            //        break;
            //    case Ladder.Ladder:
            //        LadderType = 2;
            //        break;
            //}

            //Notes = Binstance.Notes;

            //display capacity information
            //switch (Binstance.BinType)
            //{
            //    case BinTypeEnum.FlatStructure:
            //        BinType = 0;
            //        var fbin = new FlatBin(Binstance);
            //        CribWidth = fbin.CribWidth.ToString();
            //        CribLength = fbin.CribLength.ToString();
            //        break;
            //    case BinTypeEnum.GravityWagon:
            //        BinType = 1;
            //        var gbin = new GravityBin(Binstance);
            //        RectangleHeight = gbin.RectangleHeight.ToString();
            //        RectangleLength = gbin.RectangleLength.ToString();
            //        RectangleWidth = gbin.RectangleWidth.ToString();
            //        ChuteLength = gbin.ChuteLength.ToString();
            //        HopperHeight = gbin.HopperHeight.ToString();
            //        break;
            //    case BinTypeEnum.PolygonStructure:
            //        BinType = 2;
            //        var pbin = new PolygonBin(Binstance);
            //        SideHeight = pbin.SideHeight.ToString();
            //        SideWidth = pbin.SideWidth.ToString();
            //        numberOfSides = pbin.NumberOfSides.ToString();
            //        break;
            //    case BinTypeEnum.RoundStorage:
            //        BinType = 3;
            //        var rbin = new RoundBin(Binstance);
            //        Radius = rbin.Radius.ToString();
            //        RoofHeight = rbin.RoofHeight.ToString();
            //        WallHeight = rbin.WallHeight.ToString();
            //        RoundHopperHeight = rbin.HopperHeight.ToString();
            //        break;
            //    case BinTypeEnum.NotFound:
            //        BinType = -1;
            //        break;
            //}           
                        
        }

        #endregion

        #region Mapping

        private void FeatureMapBinType()
        {
            switch (Binstance.BinType)
            {
                //case BinTypeEnum.RoundStorage:
                //    Binstance = ArcGISFeature.MapBinToRoundType(Binstance);
                //    var roundBin = new RoundBin(Binstance);
                //    switch (roundBin.HasHopper)
                //    {
                //        case true:
                //            HasHopper = 0;
                //            break;
                //        case false:
                //            HasHopper = 1;
                //            break;
                //        default:
                //            HasHopper = -1;
                //            break;
                //    }
                //    break;
                //case BinTypeEnum.FlatStructure:
                //    Binstance = ArcGISFeature.MapBinToFlatType(Binstance);
                //    break;
                //case BinTypeEnum.PolygonStructure:
                //    Binstance = ArcGISFeature.MapBinToPolyType(Binstance);
                //    break;
                //case BinTypeEnum.GravityWagon:
                //    Binstance = ArcGISFeature.MapBinToGravityType(Binstance);
                //    break;
                //default:
                //    break;
            }
        }

        public void ViewModelMapBinType()
        {
            //Binstance = this.MapToBin();
            //switch (Binstance.BinType)
            //{
            //    case BinTypeEnum.RoundStorage:
            //        Binstance = this.MapBinToRoundType(Binstance);
            //        break;
            //    case BinTypeEnum.FlatStructure:
            //        Binstance = this.MapBinToFlatType(Binstance);
            //        break;
            //    case BinTypeEnum.PolygonStructure:
            //        Binstance = this.MapBinToPolyType(Binstance);
            //        break;
            //    case BinTypeEnum.GravityWagon:
            //        Binstance = this.MapBinToGravityType(Binstance);
            //        break;
            //    default:
            //        break;
            //}
        }

        public void ViewModelMapYTY()
        {
            YTYData = this.MapViewModelToYTY();

            if (Binstance.YTYDatas == null)
            {
                Binstance.YTYDatas.Add(YTYData);
            }
            else
            {
                try
                {
                    foreach (var yty in Binstance.YTYDatas)
                    {
                        if (yty.CropYear == YTYData.CropYear)
                        {
                            Binstance.YTYDatas.Remove(yty);
                            break;

                        }
                    }
                    Binstance.YTYDatas.Add(YTYData);
                }
                catch (Exception)
                {

                }
            }
        }

        #endregion

        #region Year to year 
        private async void SetYTY()
        {
            await ArcGISFeature.LoadAsync();

            IReadOnlyList<Attachment> attachments = await ArcGISFeature.GetAttachmentsAsync();

            foreach (Attachment attachment in attachments)
            {
                if (attachment.Name.Equals(ArcGisService.YTY_FILE_NAME))
                {
                    Stream jsonStream = await attachment.GetDataAsync();
                    StreamReader streamReader = new StreamReader(jsonStream);
                    string jsonString = streamReader.ReadToEnd();

                    Binstance.YTYDatas.Clear();
                    Binstance.YTYDatas.AddRange((List<YTYData>)JsonConvert.DeserializeObject<IEnumerable<YTYData>>(jsonString));
                    break;
                }
            }
        }
        #endregion

        #region Validation

        public async Task<bool> ValdiateAsync()
        {
            Errors = new Dictionary<int, string>();
            bool valid = true;
            Binstance = this.MapToBin();
            //ViewModelMapBinType();
            //ViewModelMapYTY();

            if (string.IsNullOrWhiteSpace(Binstance.Identifier))
            {
                Errors.Add(0, "Identifier is required");
                valid = false;
            }
            else
            {
                var result = await ArcGisService.IdentifierAvailable(Binstance.Identifier.ToLower());

                switch (result)
                {
                    case IdentifierAvailableEnum.Available:
                        break;
                    case IdentifierAvailableEnum.NotAvailable:
                        Errors.Add(0, "Identifier already in use");
                        valid = false;
                        break;
                    case IdentifierAvailableEnum.Exception:
                        Errors.Add(0, "Cannot connect to online services to verify identifier. Please try again");
                        valid = false;
                        break;
                }
            }                       

            if (Binstance.BinType == BinTypeEnum.NotFound)
            {
                Errors.Add(1, "Bin Type is required");
                valid = false;
            }
            else
            {
                //switch (Binstance.BinType)
                //{
                //    case BinTypeEnum.FlatStructure:
                //        valid = ValidateFlatBin(valid);                        
                //        break;
                //    case BinTypeEnum.GravityWagon:
                //        ValidateGravityBin(valid);
                //        break;
                //    case BinTypeEnum.PolygonStructure:
                //        ValidatePolygonBin(valid);
                //        break;
                //    case BinTypeEnum.RoundStorage:
                //        ValidateRoundBin(valid);
                //        break;
                //}
            }

            if (New && string.IsNullOrWhiteSpace(Binstance.CreatedBy))
            {
                Errors.Add(2, "Employee Number is required");
                valid = false;
            }

            return valid;
        }

        //private bool ValidateGravityBin(bool valid)
        //{
        //    bool result = valid;
        //    var bin = new GravityBin(Binstance);

        //    if (bin.ChuteLength == 0)
        //    {
        //        Errors.Add(3, "Chute Length is required");
        //        valid = false;
        //    }

        //    if (bin.HopperHeight == 0)
        //    {
        //        Errors.Add(4, "Hopper Heigth is required");
        //        valid = false;
        //    }

        //    if (bin.RectangleHeight == 0)
        //    {
        //        Errors.Add(5, "Rectangle Heigth is required");
        //        valid = false;
        //    }

        //    if (bin.RectangleLength == 0)
        //    {
        //        Errors.Add(6, "Rectangle Length is required");
        //        valid = false;
        //    }

        //    if (bin.RectangleWidth == 0)
        //    {
        //        Errors.Add(7, "Rectangle Width is required");
        //        valid = false;
        //    }

        //    result = ValidateContents(result);
        //    return result;
        //}        

        //private bool ValidatePolygonBin(bool valid)
        //{
        //    bool result = valid;
        //    var bin = new PolygonBin(Binstance);

        //    if (bin.SideHeight == 0)
        //    {
        //        Errors.Add(8, "Side Height is required");
        //        valid = false;
        //    }

        //    if (bin.SideWidth == 0)
        //    {
        //        Errors.Add(9, "Side Width is required");
        //        valid = false;
        //    }

        //    if (bin.NumberOfSides == 0)
        //    {
        //        Errors.Add(10, "Number Of Sides is required");
        //        valid = false;
        //    }

        //    ValidateContents(result);
        //    return result;
        //}

        //private bool ValidateRoundBin(bool valid)
        //{
        //    bool result = valid;
        //    var bin = new RoundBin(Binstance);

        //    if (bin.Radius == 0)
        //    {
        //        Errors.Add(11, "Radius is required");
        //        valid = false;
        //    }

        //    if (bin.RoofHeight == 0)
        //    {
        //        Errors.Add(12, "Roof Height is required");
        //        valid = false;
        //    }

        //    if (bin.WallHeight == 0)
        //    {
        //        Errors.Add(13, "Wall Height is required");
        //        valid = false;
        //    }
        //    //if (bin.HasHopper.Value? true : false)
        //    if (bin.HasHopper != null)
        //    {
        //        if (bin.HasHopper.Value == true && bin.HopperHeight == 0)
        //        {
        //            Errors.Add(14, "Hopper Height is required");
        //            valid = false;
        //        }

        //        if (bin.HasHopper.Value == true && bin.YTYData.GrainHopperHeight == 0)
        //        {
        //            Errors.Add(21, "Grain Hopper Height is required");
        //            valid = false;
        //        }
        //    }

        //    result = ValidateContents(result);
        //    return result;
        //}

        //private bool ValidateFlatBin(bool valid)
        //{
        //    bool result = valid;
        //    var bin = new FlatBin(Binstance);

        //    if (bin.CribLength == 0)
        //    {
        //        Errors.Add(15, "Crib Length is required");
        //        valid = false;
        //    }

        //    if (bin.CribWidth == 0)
        //    {
        //        Errors.Add(16, "Crib Width is required");
        //        valid = false;
        //    }

        //    result = ValidateContents(result);
        //    return result;
        //}

        //private bool ValidateContents(bool valid)
        //{
        //    bool result = valid;            

        //    if (string.IsNullOrWhiteSpace(YTYData.Crop))
        //    {
        //        Errors.Add(17, "Crop is required");
        //        valid = false;
        //    }

        //    if (YTYData.CropYear == 0)
        //    {
        //        Errors.Add(18, "Crop Year is required");
        //        valid = false;
        //    }

        //    if (YTYData.GrainHeight == 0)
        //    {
        //        Errors.Add(19, "Grain Height is required");
        //        valid = false;
        //    }

        //    if (YTYData.ConeHeight == 0)
        //    {
        //        Errors.Add(20, "Cone Height is required");
        //        valid = false;
        //    }

        //    return result;
        //}
        #endregion
    }
}
