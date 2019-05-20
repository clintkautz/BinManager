using Esri.ArcGISRuntime.Data;
using BinManager.Models;
using BinManager.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
/* 
* This class is used for mapping an ArcGIS Feature to a IBinstance
*/
namespace BinManager.Utilities.Mappers
{
    public static class FeatureMapper
    {
        public static IBinstance MapToBin(this ArcGISFeature inFeature)
        {
            IBinstance bin = new Binstance();
            bin.Identifier = (string)inFeature.Attributes["identifier"];
            bin.Notes = (string)inFeature.Attributes["notes"];

            try
            {
                bin.CreatedBy = (string)inFeature.Attributes["created_by"];
            }
            catch (Exception)
            {
                bin.CreatedBy = "";
            }

            try
            {
                bin.ModifiedBy = (string)inFeature.Attributes["modified_by"];
            }
            catch (Exception)
            {
                bin.ModifiedBy = "";
            }

            try
            {
                bin.YearCollected = (string)inFeature.Attributes["year_collected"];
            }
            catch (Exception)
            {
                bin.YearCollected = "";
            }

            try
            {
                string owned = (string)inFeature.Attributes["owned_or_leased"];
                owned.ToLowerInvariant();
                if (owned.Equals("owned"))
                {
                    bin.IsLeased = false;
                }
                else if (owned.Equals("leased"))
                {
                    bin.IsLeased = true;
                }
                else
                {
                    bin.IsLeased = null;
                }
            }
            catch (Exception)
            {
                bin.IsLeased = null;
            }

            try
            {
                string dryingDevice = (string)inFeature.Attributes["drying_device"];
                dryingDevice.ToLowerInvariant();
                if (dryingDevice.Equals("true"))
                {
                    bin.HasDryingDevice = true;
                }
                else if (dryingDevice.Equals("false"))
                {
                    bin.HasDryingDevice = false;
                }
                else 
                {
                    bin.HasDryingDevice = null;
                }
            }
            catch (Exception)
            {
                bin.HasDryingDevice = null;
            }

            try
            {
                string grainHeightIndicator = (string)inFeature.Attributes["bin_level_indicator_device"];
                grainHeightIndicator.ToLowerInvariant();
                if (grainHeightIndicator.Equals("true"))
                {
                    bin.HasGrainHeightIndicator = true;
                }
                else if (grainHeightIndicator.Equals("false"))
                {
                    bin.HasGrainHeightIndicator = false;
                }
                else 
                {
                    bin.HasGrainHeightIndicator = null;
                }
            }
            catch (Exception)
            {
                bin.HasGrainHeightIndicator = null;
            }

            try
            {
                string binType = (string)inFeature.Attributes["bin_type"];

                binType = binType.ToLower();

                if (binType.Equals("round_storage"))
                {
                    bin.BinType = Enums.BinTypeEnum.RoundStorage;
                }
                else if (binType.Equals("gravity_wagon"))
                {
                    bin.BinType = Enums.BinTypeEnum.GravityWagon;
                }
                else if (binType.Equals("polygon_structure"))
                {
                    bin.BinType = Enums.BinTypeEnum.PolygonStructure;
                }
                else if (binType.Equals("flat_structure"))
                {
                    bin.BinType = Enums.BinTypeEnum.FlatStructure;

                }
                else
                {
                    bin.BinType = Enums.BinTypeEnum.NotFound;
                }
            }
            catch (Exception)
            {
                bin.BinType = Enums.BinTypeEnum.NotFound;
            }

            try
            {
                string ladderType = (string)inFeature.Attributes["ladder_type"];
                ladderType.ToLowerInvariant();
                if (ladderType.Equals("ladder"))
                {
                    bin.LadderType = Enums.Ladder.Ladder;
                }
                else if (ladderType.Equals("stairs"))
                {
                    bin.LadderType = Enums.Ladder.Stairs;
                }
                else
                {
                    bin.LadderType = Enums.Ladder.None;
                }
            }
            catch (Exception)
            {
                bin.LadderType = Enums.Ladder.None;
            }

            return bin;
        }

        //handle bin type specific mapping below

        public static FlatBin MapBinToFlatType(this Feature inFeature, IBinstance bin)
        {
            FlatBin fbin = new FlatBin(bin);

            double d;
            try
            {
                string s = inFeature.Attributes["crib_height"].ToString();
                fbin.CribLength = double.TryParse(s, out d) ? d : 0.0;
            }
            catch(Exception)
            {
                fbin.CribLength = 0;
            }

            try
            {
                string s = inFeature.Attributes["crib_width"].ToString();
                fbin.CribWidth = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                fbin.CribLength = 0;
            }

            return fbin;

        }

        public static GravityBin MapBinToGravityType(this Feature inFeature, IBinstance bin)
        {
            GravityBin gbin = new GravityBin(bin);

            double d;
            string s;

            try
            {

                s = inFeature.Attributes["chute_length"].ToString();
                gbin.ChuteLength = double.TryParse(s, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                gbin.ChuteLength = 0;
            }

            try
            {
                s = inFeature.Attributes["hopper_height"].ToString();
                gbin.HopperHeight = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                gbin.HopperHeight = 0;
            }
            
            try
            {
                s = inFeature.Attributes["rectangle_height"].ToString();
                gbin.RectangleHeight = double.TryParse(s, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                gbin.RectangleHeight = 0;
            }
            
            try
            {
                s = inFeature.Attributes["rectangle_length"].ToString();
                gbin.RectangleLength = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                gbin.RectangleLength = 0;
            }
            
            try
            {
                s = inFeature.Attributes["rectangle_width"].ToString();
                gbin.RectangleWidth = double.TryParse(s, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                gbin.RectangleWidth = 0;
            }
            

            return gbin;

        }

        public static PolygonBin MapBinToPolyType(this Feature inFeature, IBinstance bin)
        {
            PolygonBin pbin = new PolygonBin(bin);

            double d;
            string s;
            try
            {
                s = inFeature.Attributes["side_height"].ToString();
                pbin.SideHeight = double.TryParse(s, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                pbin.SideHeight = 0;
            }
               
            try
            {
                s = inFeature.Attributes["side_width"].ToString();
                pbin.SideWidth = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                pbin.SideWidth = 0;
            }
            
            try
            {
                int i;
                s = inFeature.Attributes["number_of_sides"].ToString();
                pbin.NumberOfSides = int.TryParse(s, out i) ? i : 0;
            }
            catch (Exception)
            {
                
                pbin.NumberOfSides = 0;
            }
            

            return pbin;

        }

        public static RoundBin MapBinToRoundType(this Feature inFeature, IBinstance bin)
        {
            RoundBin rbin = new RoundBin(bin);
            try
            {
                string hasHopper = (string)inFeature.Attributes["has_hopper"];
                hasHopper.ToLowerInvariant();
                if (hasHopper.Equals("true"))
                {
                    rbin.HasHopper = true;
                }
                else if (hasHopper.Equals("false"))
                {
                    rbin.HasHopper = false;
                }
                else
                {
                    rbin.HasHopper = null;
                }
            }
            catch (Exception)
            {
                rbin.HasHopper = null;
            }

            double d;
            string s;

            try
            {
                s = inFeature.Attributes["radius"].ToString();
                rbin.Radius = double.TryParse(s, out d) ? d : 0.0;

                rbin.Circumference = rbin.Radius * 2 * Math.PI;
            }
            catch (Exception)
            {
                rbin.Radius = 0;
                rbin.Circumference = 0;
            }
            
            try
            {
                s = inFeature.Attributes["wall_height"].ToString();
                rbin.WallHeight = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.WallHeight = 0;
            }
            
            try
            {
                s = inFeature.Attributes["roof_height"].ToString();
                rbin.RoofHeight = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.RoofHeight = 0;
            }
            
            try
            {
                s = inFeature.Attributes["hopper_height"].ToString();
                rbin.HopperHeight = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.HopperHeight = 0;
            }
            

            return rbin;

        }

        public static RoundBin MapBinToRoundType(this Feature inFeature, RoundBin bin)
        {
            RoundBin rbin = new RoundBin(bin);

            try
            {
                string hasHopper = (string)inFeature.Attributes["has_hopper"];
                hasHopper.ToLowerInvariant();
                if (hasHopper.Equals("true"))
                {
                    rbin.HasHopper = true;
                }
                else if (hasHopper.Equals("false"))
                {
                    rbin.HasHopper = false;
                }
                else
                {
                    rbin.HasHopper = null;
                }
            }
            catch (Exception)
            {
                bin.HasHopper = null;
            }

            double d;
            string s;

            try
            {
                s = inFeature.Attributes["radius"].ToString();
                rbin.Radius = double.TryParse(s, out d) ? d : 0.0;

                rbin.Circumference = rbin.Radius * 2 * Math.PI;
            }
            catch (Exception)
            {
                rbin.Radius = 0;
                rbin.Circumference = 0;
            }
            
            try
            {
                s = inFeature.Attributes["wall_height"].ToString();
                rbin.WallHeight = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.WallHeight = 0;
            }
            
            try
            {
                s = inFeature.Attributes["roof_height"].ToString();
                rbin.RoofHeight = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.RoofHeight = 0;
            }
            
            try
            {
                s = inFeature.Attributes["hopper_height"].ToString();
                rbin.HopperHeight = double.TryParse(s, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.HopperHeight = 0;
            }

            return rbin;
        }
    }
}
