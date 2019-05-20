using System;
namespace BinManager.Models
{
    public class PolygonBin : Binstance
    {
        //PolygonBin Properties
        public double SideHeight { get; set; }
        public double SideWidth { get; set; }
        public int NumberOfSides { get; set; }

        public PolygonBin()
        {
        }

        public PolygonBin(IBinstance bin)
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
