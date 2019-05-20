using System;
using System.Collections.Generic;

namespace BinManager.Models
{
    public class YTYData
    {
        public int CropYear { get; set; }
        public string Crop { get; set; }
        public double GrainHeight { get; set; }
        public double GrainHopperHeight { get; set; }
        public double ConeHeight { get; set; }
        public double TotalVolume { get; set; }
        public double MoistureOfGrain { get; set; }
        public double MoistureFactor { get; set; }
        public double TestWeight { get; set; }
        public double PackFactor { get; set; }
        public double DockagePercent { get; set; }
        public double DockageFactor { get; set; }
        public double ConversionFactor { get; set; }
        public double ShellFactor { get; set; }
        public double TotalDeductionVolume { get; set; }
        public string Notes { get; set; }

        public YTYData()
        {

        }

        public YTYData(int CropYear,
            string Crop,
            double GrainHeight,
            double GrainHopperHeight,
            double ConeHeight,
            double TotalVolume,
            double MoistureOfGrain,
            double MoistureFactor,
            double TestWeight,
            double PackFactor,
            double DockagePercent,
            double DockageFactor,
            double ConversionFactor,
            double ShellFactor,
            double TotalDeductionVolume,
            string Notes)
        {
            this.CropYear = CropYear;
            this.Crop = Crop;
            this.GrainHeight = GrainHeight;
            this.GrainHopperHeight = GrainHopperHeight;
            this.ConeHeight = ConeHeight;
            this.TotalVolume = TotalVolume;
            this.MoistureOfGrain = MoistureOfGrain;
            this.MoistureFactor = MoistureFactor;
            this.TestWeight = TestWeight;
            this.PackFactor = PackFactor;
            this.DockagePercent = DockagePercent;
            this.DockageFactor = DockageFactor;
            this.ConversionFactor = ConversionFactor;
            this.ShellFactor = ShellFactor;
            this.TotalDeductionVolume = TotalDeductionVolume;
            this.Notes = Notes;
        }
      
        public override string ToString()
        {
            return "(" + CropYear + "," +
            Crop + "," +
            GrainHeight + "," +
            ConeHeight + "," +
            MoistureOfGrain + "," +
            MoistureFactor + "," +
            TestWeight + "," +
            PackFactor + "," +
            DockagePercent + "," +
            ConversionFactor + "," +
            Notes + ")";
        }

        public Dictionary<string, string> ToStringDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string cropYear;
            if (CropYear == 0)
            {
                cropYear = "0";
            }
            else
            {
                cropYear = CropYear.ToString();
            }
            dict.Add("cropYear", cropYear);

            string crop;
            if (Crop == null)
            {
                crop = string.Empty;
            }
            else
            {
                crop = Crop.ToString();
            }
            dict.Add("crop", crop);

            string grainHeight;
            if (GrainHeight == 0)
            {
                grainHeight = "0";
            }
            else
            {
                grainHeight = GrainHeight.ToString();
            }
            dict.Add("grainHeight", grainHeight);

            string grainHopperHeight;
            if (GrainHopperHeight == 0)
            {
                grainHopperHeight = "0";
            }
            else
            {
                grainHopperHeight = GrainHopperHeight.ToString();
            }
            dict.Add("grainHopperHeight", grainHopperHeight);

            string coneHeight;
            if (ConeHeight == 0)
            {
                coneHeight = "0";
            }
            else
            {
                coneHeight = ConeHeight.ToString();
            }
            dict.Add("coneHeight", coneHeight);

            string totalVolume;
            if (TotalVolume == 0)
            {
                totalVolume = "0";
            }
            else
            {
                totalVolume = TotalVolume.ToString("#,###.##");
            }
            dict.Add("totalVolume", totalVolume);

            string moistureOfGrain;
            if (MoistureOfGrain == 0)
            {
                moistureOfGrain = "0";
            }
            else
            {
                moistureOfGrain = MoistureOfGrain.ToString();
            }
            dict.Add("moistureOfGrain", moistureOfGrain);

            string moistureFactor;
            if (MoistureFactor == 0)
            {
                moistureFactor = "0";
            }
            else
            {
                moistureFactor = MoistureFactor.ToString();
            }
            dict.Add("moistureFactor", moistureFactor);

            string testWeight;
            if (TestWeight == 0)
            {
                testWeight = "0";
            }
            else
            {
                testWeight = TestWeight.ToString();
            }
            dict.Add("testWeight", testWeight);

            string packFactor;
            if (PackFactor == 0)
            {
                packFactor = "0";
            }
            else
            {
                packFactor = PackFactor.ToString();
            }
            dict.Add("packFactor", packFactor);

            string dockagePercent;
            if (DockagePercent == 0)
            {
                dockagePercent = "0";
            }
            else
            {
                dockagePercent = DockagePercent.ToString();
            }
            dict.Add("dockagePercent", dockagePercent);

            string dockageFactor;
            if (DockageFactor == 0)
            {
                dockageFactor = "0";
            }
            else
            {
                dockageFactor = DockageFactor.ToString();
            }
            dict.Add("dockageFactor", dockageFactor);

            string conversionFactor;
            if (ConversionFactor == 0)
            {
                conversionFactor = "0";
            }
            else
            {
                conversionFactor = ConversionFactor.ToString();
            }
            dict.Add("conversionFactor", conversionFactor);

            string shellFactor;
            if (ShellFactor == 0)
            {
                shellFactor = "0";
            }
            else
            {
                shellFactor = ShellFactor.ToString();
            }
            dict.Add("shellFactor", shellFactor);

            string totalDeductionVolume;
            if (TotalDeductionVolume == 0)
            {
                totalDeductionVolume = "0";
            }
            else
            {
                totalDeductionVolume = TotalDeductionVolume.ToString();
            }
            dict.Add("totalDeductionVolume", totalDeductionVolume);

            string notes;
            if (Notes == null)
            {
                notes = string.Empty;
            }
            else
            {
                notes = Notes.ToString();
            }
            dict.Add("notes", notes);

            return dict;
        }

        public List<string> ToStringList()
        {
            List<string> list = new List<string>();

            string cropYear;
            if (CropYear == 0)
            {
                cropYear = "0";
            }
            else
            {
                cropYear = CropYear.ToString();
            }
            list.Add("CropYear: " + cropYear);

            string crop;
            if (Crop == null)
            {
                crop = string.Empty;
            }
            else
            {
                crop = Crop.ToString();
            }
            list.Add("Crop: " + crop);

            string grainHeight;
            if (GrainHeight  == 0)
            {
                grainHeight = "0";
            }
            else
            {
                grainHeight = GrainHeight.ToString();
            }
            list.Add("Grain Height: " + grainHeight);

            string hopperHeight;
            if (GrainHopperHeight == 0)
            {
                hopperHeight = "0";
            }
            else
            {
                hopperHeight = GrainHopperHeight.ToString();
            }
            list.Add("Grain Hopper Height: " + hopperHeight.ToString());

            string coneHeight;
            if (ConeHeight == 0)
            {
                coneHeight = "0";
            }
            else
            {
                coneHeight = ConeHeight.ToString();
            }
            list.Add("Cone Height: " + coneHeight);

            string totalVolume;
            if (TotalVolume == 0)
            {
                totalVolume = "0";
            }
            else
            {
                totalVolume = TotalVolume.ToString("#,###.##");
            }
            list.Add("Total Volume: " + totalVolume);

            string moistureOfGrain;
            if (MoistureOfGrain == 0)
            {
                moistureOfGrain = "0";
            }
            else
            {
                moistureOfGrain = MoistureOfGrain.ToString();
            }
            list.Add("Moisture Of Grain: " + moistureOfGrain);

            string moistureFactor;
            if (MoistureFactor == 0)
            {
                moistureFactor = "0";
            }
            else
            {
                moistureFactor = MoistureFactor.ToString();
            }
            list.Add("Moisture Factor: " + moistureFactor);

            string testWeight;
            if (TestWeight == 0)
            {
                testWeight = "0";
            }
            else
            {
                testWeight = TestWeight.ToString();
            }
            list.Add("Test Weight: " + testWeight);

            string packFactor;
            if (PackFactor == 0)
            {
                packFactor = "0";
            }
            else
            {
                packFactor = PackFactor.ToString();
            }
            list.Add("Pack Factor: " + packFactor);

            string dockagePercent;
            if (DockagePercent == 0)
            {
                dockagePercent = "0";
            }
            else
            {
                dockagePercent = DockagePercent.ToString();
            }
            list.Add("Dockage Percent: " + dockagePercent);

            string dockageFactor;
            if (DockageFactor == 0)
            {
                dockageFactor = "0";
            }
            else
            {
                dockageFactor = DockageFactor.ToString();
            }
            list.Add("Dockage Factor: " + dockageFactor);

            string conversionFactor;
            if (ConversionFactor == 0)
            {
                conversionFactor = "0";
            }
            else
            {
                conversionFactor = ConversionFactor.ToString();
            }
            list.Add("Conversion Factor: " + conversionFactor);

            string shellFactor;
            if (ShellFactor == 0)
            {
                shellFactor = "0";
            }
            else
            {
                shellFactor = ShellFactor.ToString();
            }
            list.Add("Shell Factor: " + shellFactor);

            string totalDeductionVolume;
            if (TotalDeductionVolume == 0)
            {
                totalDeductionVolume = "0";
            }
            else
            {
                totalDeductionVolume = TotalDeductionVolume.ToString();
            }
            list.Add("Total Deduction Volume: " + totalDeductionVolume);

            string notes;
            if (Notes == null)
            {
                notes = string.Empty;
            }
            else
            {
                notes = Notes.ToString();
            }
            list.Add("Notes: " + notes);

            return list;
        }
    }
}
