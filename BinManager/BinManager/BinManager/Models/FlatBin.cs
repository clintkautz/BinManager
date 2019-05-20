using System;
namespace BinManager.Models
{
    public class FlatBin : Binstance
    {
        public double CribWidth { get; set; }
        public double CribLength { get; set; }

        public FlatBin()
        {
        }

        public FlatBin(IBinstance bin)
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
