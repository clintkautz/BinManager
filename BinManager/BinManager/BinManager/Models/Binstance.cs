using System.Collections.Generic;
using System.IO;
using BinManager.Utilities.Enums;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace BinManager.Models
{
    public class Binstance : IBinstance
    {
        public string Identifier { get; set; }
        public MediaFile Image { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public BinTypeEnum BinType { get; set; }
        public string YearCollected { get; set; }
        public bool? IsLeased { get; set; }
        public bool? HasDryingDevice { get; set; }
        public bool? HasGrainHeightIndicator { get; set; }
        public Ladder LadderType { get; set; }
        public string Notes { get; set; }
        public string BinVolume { get; set; }
        public YTYData YTYData { get; set; }
        public List<YTYData> YTYDatas { get; set; }

        public Binstance()
        {
            YTYDatas = new List<YTYData>();
        }

        public string YTYDatasString()
        {
            if (YTYDatas == null)
            {
                return "{YTYDatas == null}";
            }
            string result = "{";
            foreach (YTYData yTYData in YTYDatas)
            {
                result = result + "[" + yTYData.ToString() + "]";
            }
            return result + "}";
        }
    }
}
