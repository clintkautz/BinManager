using System.Collections.Generic;
using System.IO;
using BinManager.Utilities.Enums;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace BinManager.Models
{
    public interface IBinstance
    {
        string Identifier { get; set; }
        MediaFile Image { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        BinTypeEnum BinType { get; set; }
        string YearCollected { get; set; }
        bool? IsLeased { get; set; }
        bool? HasDryingDevice { get; set; }
        bool? HasGrainHeightIndicator { get; set; }
        Ladder LadderType { get; set; }
        string Notes { get; set; }
        string BinVolume { get; set; }
        YTYData YTYData { get; set; }
        List<YTYData> YTYDatas { get; set; }
    }
}