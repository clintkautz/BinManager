﻿
namespace BinManager.Utilities.Mappers
{
    #region imports
    using BinManager.Models;
    using BinManager.ViewModels;
    using Plugin.Media.Abstractions;
    using System;
    #endregion

    public static class BinViewModelMapper
    {
        public static IBinstance MapToBin(this BinViewModel inViewModel)
        {             
            IBinstance bin = new Binstance();

            if (inViewModel.Binstance.Image != null)
            {
                bin.Image = inViewModel.Binstance.Image;
            }

            if (inViewModel.New)
            {
                bin.CreatedBy = inViewModel.EmpoyeeNumber;
            }
            else if (inViewModel.Edit)
            {
                bin.ModifiedBy = inViewModel.EmpoyeeNumber;
            }
            else
            {
                bin.CreatedBy = inViewModel.CreatedBy;
                bin.ModifiedBy = inViewModel.ModifiedBy;
            }

            bin.Identifier = inViewModel.Identifier;
            
            bin.YearCollected = inViewModel.YearCollected;
            switch (inViewModel.Owned)
            {
                case 0:
                    bin.IsLeased = false;
                    break;
                case 1:
                    bin.IsLeased = true;
                    break;
                default:
                    bin.IsLeased = null;
                    break;
            }

            switch (inViewModel.HasDryingDevice)
            {
                case 0:
                    bin.HasDryingDevice = true;
                    break;
                case 1:
                    bin.HasDryingDevice = false;
                    break;
                default:
                    bin.HasDryingDevice = null;
                    break;
            }

            switch (inViewModel.HasGrainHeightIndicator)
            {
                case 0:
                    bin.HasGrainHeightIndicator = true;
                    break;
                case 1:
                    bin.HasGrainHeightIndicator = false;
                    break;
                default:
                    bin.HasGrainHeightIndicator = null;
                    break;
            }

            switch (inViewModel.LadderType)
            {
                case 0:
                    bin.LadderType = Enums.Ladder.None;
                    break;
                case 1:
                    bin.LadderType = Enums.Ladder.Stairs;
                    break;
                case 2:
                    bin.LadderType = Enums.Ladder.Stairs;
                    break;
            }
            bin.Notes = inViewModel.Notes;

            switch (inViewModel.BinType)
            {
                case 0:
                    bin.BinType = Enums.BinTypeEnum.FlatStructure;
                    break;
                case 1:
                    bin.BinType = Enums.BinTypeEnum.GravityWagon;
                    break;
                case 2:
                    bin.BinType = Enums.BinTypeEnum.PolygonStructure;
                    break;
                case 3:
                    bin.BinType = Enums.BinTypeEnum.RoundStorage;
                    break;
                default:
                    bin.BinType = Enums.BinTypeEnum.NotFound;
                    break;
            }

            return bin;
        }

        public static FlatBin MapBinToFlatType(this BinViewModel inViewModel, IBinstance bin)
        {
            FlatBin fbin = new FlatBin(bin);
            double d;
            try
            {
                fbin.CribLength = double.TryParse(inViewModel.CribLength, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                fbin.CribLength = 0;
            }

            try
            {
                fbin.CribWidth = double.TryParse(inViewModel.CribWidth, out d) ? d : 0;
            }
            catch (Exception)
            {
                fbin.CribLength = 0;
            }

            return fbin;
        }

        public static GravityBin MapBinToGravityType(this BinViewModel inViewModel, IBinstance bin)
        {
            GravityBin gbin = new GravityBin(bin);
            double d;
            try
            {
                gbin.ChuteLength = double.TryParse(inViewModel.ChuteLength, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                gbin.ChuteLength = 0;
            }

            try
            {
                gbin.HopperHeight = double.TryParse(inViewModel.HopperHeight, out d) ? d : 0;
            }
            catch (Exception)
            {
                gbin.HopperHeight = 0;
            }

            try
            {
                gbin.RectangleHeight = double.TryParse(inViewModel.RectangleHeight, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                gbin.RectangleHeight = 0;
            }

            try
            {
                gbin.RectangleLength = double.TryParse(inViewModel.RectangleLength, out d) ? d : 0;
            }
            catch (Exception)
            {
                gbin.RectangleLength = 0;
            }

            try
            {
                gbin.RectangleWidth = double.TryParse(inViewModel.RectangleWidth, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                gbin.RectangleWidth = 0;
            }


            return gbin;
        }

        public static PolygonBin MapBinToPolyType(this BinViewModel inViewModel, IBinstance bin)
        {
            PolygonBin pbin = new PolygonBin(bin);

            double d;
            try
            {
                pbin.SideHeight = double.TryParse(inViewModel.SideHeight, out d) ? d : 0.0;
            }
            catch (Exception)
            {
                pbin.SideHeight = 0;
            }

            try
            {
                pbin.SideWidth = double.TryParse(inViewModel.SideWidth, out d) ? d : 0;
            }
            catch (Exception)
            {
                pbin.SideWidth = 0;
            }

            try
            {
                int i;
                pbin.NumberOfSides = int.TryParse(inViewModel.NumberOfSides, out i) ? i : 0;
            }
            catch (Exception)
            {

                pbin.NumberOfSides = 0;
            }

            return pbin;
        }

        public static RoundBin MapBinToRoundType(this BinViewModel inViewModel, IBinstance bin)
        {
            RoundBin rbin = new RoundBin(bin);

            try
            {
                switch (inViewModel.HasHopper)
                {
                    case 0:
                        rbin.HasHopper = true;
                        break;
                    case 1:
                        rbin.HasHopper = false;
                        break;
                    default:
                        rbin.HasHopper = null;
                        break;
                }
            }
            catch (Exception)
            {
                rbin.HasHopper = null;
            }

            double d;
            try
            {
                rbin.Radius = double.TryParse(inViewModel.Radius, out d) ? d : 0.0;

                rbin.Circumference = rbin.Radius * 2 * Math.PI;
            }
            catch (Exception)
            {
                rbin.Radius = 0;
                rbin.Circumference = 0;
            }

            try
            {
                rbin.WallHeight = double.TryParse(inViewModel.WallHeight, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.WallHeight = 0;
            }

            try
            {
                rbin.RoofHeight = double.TryParse(inViewModel.RoofHeight, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.RoofHeight = 0;
            }

            try
            {
                rbin.HopperHeight = double.TryParse(inViewModel.HopperHeight, out d) ? d : 0;
            }
            catch (Exception)
            {
                rbin.HopperHeight = 0;
            }

            return rbin;
        }

        public static void MapBinstanceToViewModel(this BinViewModel inViewModel)
        {
            IBinstance oldBin = inViewModel.Binstance;

            inViewModel.Identifier = oldBin.Identifier;

            switch (oldBin.BinType)
            {
                case Enums.BinTypeEnum.FlatStructure:
                    inViewModel.BinType = 0;
                    inViewModel.FlatBinstanceToViewModel();
                    break;
                case Enums.BinTypeEnum.GravityWagon:
                    inViewModel.BinType = 1;
                    inViewModel.GravityBinstanceToViewModel();
                    break;
                case Enums.BinTypeEnum.PolygonStructure:
                    inViewModel.BinType = 2;
                    inViewModel.PolyBinstanceToViewModel();
                    break;
                case Enums.BinTypeEnum.RoundStorage:
                    inViewModel.BinType = 3;
                    inViewModel.RoundBinstanceToViewModel();
                    break;
                case Enums.BinTypeEnum.NotFound:
                    inViewModel.BinType = -1;
                    break;
            }

            switch (oldBin.IsLeased)
            {
                case true:
                    inViewModel.Owned = 0;
                    break;
                case false:
                    inViewModel.Owned = 1;
                    break;
                case null:
                    inViewModel.Owned = -1;
                    break;
            }

            switch (oldBin.HasDryingDevice)
            {
                case true:
                    inViewModel.HasDryingDevice = 0;
                    break;
                case false:
                    inViewModel.HasDryingDevice = 1;
                    break;
                case null:
                    inViewModel.HasDryingDevice = -1;
                    break;
            }

            switch (oldBin.HasGrainHeightIndicator)
            {
                case true:
                    inViewModel.HasGrainHeightIndicator = 0;
                    break;
                case false:
                    inViewModel.HasGrainHeightIndicator = 1;
                    break;
                case null:
                    inViewModel.HasGrainHeightIndicator = -1;
                    break;
            }

            switch (oldBin.LadderType)
            {
                case Enums.Ladder.None:
                    inViewModel.LadderType = 0;
                    break;
                case Enums.Ladder.Stairs:
                    inViewModel.LadderType = 1;
                    break;
                case Enums.Ladder.Ladder:
                    inViewModel.LadderType = 2;
                    break;
                default:
                    inViewModel.LadderType = -1;
                    break;
            }

            inViewModel.Notes = oldBin.Notes;

        }

        private static void RoundBinstanceToViewModel(this BinViewModel inViewModel)
        {
            RoundBin round = (RoundBin)inViewModel.Binstance;
            if (round.HasHopper.HasValue)
            {
                switch (round.HasHopper)
                {
                    case true:
                        inViewModel.HasHopper = 0;
                        break;
                    case false:
                        inViewModel.HasHopper = 1;
                        break;
                    case null:
                        inViewModel.HasHopper = -1;
                        break;
                }
                
            }
            inViewModel.Radius = round.Radius.ToString();
            inViewModel.WallHeight = round.WallHeight.ToString();
            inViewModel.RoofHeight = round.RoofHeight.ToString();
            inViewModel.HopperHeight = round.HopperHeight.ToString();
        }

        private static void PolyBinstanceToViewModel(this BinViewModel inViewModel)
        {
            PolygonBin polygonBin = (PolygonBin)inViewModel.Binstance;
            inViewModel.SideHeight = polygonBin.SideHeight.ToString();
            inViewModel.SideWidth = polygonBin.SideWidth.ToString();
            inViewModel.NumberOfSides = polygonBin.NumberOfSides.ToString();
        }

        private static void GravityBinstanceToViewModel(this BinViewModel inViewModel)
        {
            GravityBin gravityBin = (GravityBin)inViewModel.Binstance;
            inViewModel.ChuteLength = gravityBin.ChuteLength.ToString();
            inViewModel.HopperHeight = gravityBin.HopperHeight.ToString();
            inViewModel.RectangleHeight = gravityBin.RectangleHeight.ToString();
            inViewModel.RectangleLength = gravityBin.RectangleLength.ToString();
            inViewModel.RectangleWidth = gravityBin.RectangleWidth.ToString();
        }

        private static void FlatBinstanceToViewModel(this BinViewModel inViewModel)
        {
            FlatBin flat = (FlatBin)inViewModel.Binstance;
            inViewModel.CribLength = flat.CribLength.ToString();
            inViewModel.CribWidth = flat.CribWidth.ToString();
        }

        public static YTYData MapViewModelToYTY(this BinViewModel inViewModel)
        {
            YTYData yty = new YTYData();

            //yty.Crop = inViewModel.Crop;
            //yty.CropYear = inViewModel.CropYear;

            //double d;
            //yty.GrainHeight = double.TryParse(inViewModel.GrainHeight, out d) ? d : 0;
            //yty.GrainHopperHeight = double.TryParse(inViewModel.GrainHopperHeight, out d) ? d : 0;
            //yty.ConeHeight = double.TryParse(inViewModel.GrainConeHeight, out d) ? d : 0;
            //yty.MoistureOfGrain = double.TryParse(inViewModel.MoisturePercent, out d) ? d : 0;
            //yty.MoistureFactor = double.TryParse(inViewModel.MoistureFactor, out d) ? d : 0;
            //yty.TestWeight = double.TryParse(inViewModel.TestWeight, out d) ? d : 0;
            //yty.PackFactor = double.TryParse(inViewModel.PackFactor, out d) ? d : 0;
            //yty.DockagePercent = double.TryParse(inViewModel.DockagePercent, out d) ? d : 0;
            //yty.DockageFactor = double.TryParse(inViewModel.DockageFactor, out d) ? d : 0;
            //yty.TotalVolume = double.TryParse(inViewModel.GrainVolume, out d) ? d : 0;
            //switch (inViewModel.ConversionFactor)
            //{
            //    case 0:
            //        yty.ConversionFactor = 0.4;
            //        break;
            //    case 1:
            //        yty.ConversionFactor = 0.8;
            //        break;
            //    default:
            //        yty.ConversionFactor = 0;
            //        break;
            //}

            //yty.ShellFactor = double.TryParse(inViewModel.ShellFactor, out d) ? d : 0;
            //yty.TotalDeductionVolume = double.TryParse(inViewModel.TotalDeductionVolume, out d) ? d : 0;
            //yty.Notes = inViewModel.ContentsNotes;
            return yty;
        }

    }
}
