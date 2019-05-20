using System;

namespace BinManager.Models
{
    public class RoundBin : Binstance
    {
        //RoundBin specific properties
        public double Circumference { get; set; }
        public double Radius { get; set; }
        public double WallHeight { get; set; }
        public double RoofHeight { get; set; }
        public double HopperHeight { get; set; }

        public bool? HasHopper { get; set; } 

        public RoundBin()
        {
        }

        public RoundBin(IBinstance bin)
        {
            base.BinType = bin.BinType;
            base.BinVolume = bin.BinVolume;
            base.CreatedBy = bin.CreatedBy;
            base.HasDryingDevice = bin.HasDryingDevice;
            base.HasGrainHeightIndicator = bin.HasGrainHeightIndicator;
            base.Identifier = bin.Identifier;
            base.Image = bin.Image;
            base.IsLeased = bin.IsLeased;
            base.LadderType = bin.LadderType;
            base.ModifiedBy = bin.ModifiedBy;
            base.Notes = bin.Notes;
            base.YearCollected = bin.YearCollected;
            base.YTYDatas = bin.YTYDatas;
        }
    }
}
