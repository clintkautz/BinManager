using System;
using System.Collections.Generic;
using System.Text;

namespace BinManager.Models
{
    public class GravityBin : Binstance
    {
        public double RectangleHeight { get; set; }
        public double RectangleWidth { get; set; }
        public double RectangleLength { get; set; }
        public double ChuteLength { get; set; }
        public double HopperHeight { get; set; }

        public GravityBin()
        {
        }

        public GravityBin(IBinstance bin)
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
