using BinManager.Models;
using BinManager.Utilities.Enums;
using BinManager.Utilities.Extensions;
using BinManager.ViewModels;
using Esri.ArcGISRuntime.Data;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using BinManager.Utilities.Mappers;

namespace BinManager.Views
{
    #region imports
    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BinPage : TabbedPage
    {
        #region Constants
        BinViewModel viewModel;
        double d;
        static int i;
        private static Dictionary<int, string> DefaultLabels = SetLabels();


        #endregion

        #region Constructors
        //Add New Bin (no photo)
        public BinPage()
        {
            InitializeComponent();
            
            BindingContext = viewModel = new BinViewModel();           

            YearCollected.Text = DateTime.Now.ToString("yyyy");
            YearCollected.IsEnabled = false;
            viewModel.Binstance.YearCollected = YearCollected.Text;

            BinPicFrame.IsVisible = false;
            CreatedByLabel.IsVisible = false;
            CreatedByLabel.IsEnabled = false;
            CreatedBy.IsVisible = false;
            CreatedBy.IsEnabled = false;
            ModifiedByLabel.IsVisible = false;
            ModifiedByLabel.IsEnabled = false;
            ModifiedBy.IsVisible = false;
            ModifiedBy.IsEnabled = false;

            HasHopperHopperDisable();
            HideCapacityFields();

            //CropYear.ItemsSource = viewModel.DateRange;
            //SetCropYear();
            //HideContentsFields();

        }
        //Add New Bin (photo)
        public BinPage(MediaFile photo)
        {
            InitializeComponent();

            BindingContext = viewModel = new BinViewModel();
            Cancel.IsEnabled = false;
            YearCollected.Text = DateTime.Now.ToString("yyyy");
            YearCollected.IsEnabled = false;
            viewModel.Binstance.YearCollected = YearCollected.Text;

            BinPicFrame.IsVisible = false;
            BinPic.Source = ImageSource.FromStream(() => photo.GetStream());
            BinPicFrame.IsVisible = true;

            CreatedByLabel.IsVisible = false;
            CreatedByLabel.IsEnabled = false;
            CreatedBy.IsVisible = false;
            CreatedBy.IsEnabled = false;
            ModifiedByLabel.IsVisible = false;
            ModifiedByLabel.IsEnabled = false;
            ModifiedBy.IsVisible = false;
            ModifiedBy.IsEnabled = false;

            HasHopperHopperDisable();
            HideCapacityFields();

            //CropYear.ItemsSource = viewModel.DateRange;
            //SetCropYear();
            //HideContentsFields();

        }

        //View New Bin
        public BinPage(ArcGISFeature feature)
        {
            InitializeComponent();

            BindingContext = viewModel = new BinViewModel(feature);
            Cancel.IsEnabled = true;
            Cancel.Text = "Delete";
            Save.Text = "Edit";

            BinPicFrame.IsVisible = false;
            SetImage();
            
            GeneralStack.IsEnabled = false;
            CapcaityStack.IsEnabled = false;

            //CropYear.ItemsSource = viewModel.DateRange;
            //SetCropYear();
            //CalcVolume();
            //ContentStack.IsEnabled = false;
        }

        #endregion

        #region Save
        private async void Save_Clicked(object sender, EventArgs e)
        {
            activityIndicator_save.On();
            activityIndicatorCapacity_save.On();
            if (viewModel.New)
            {
                RemoveErrors();

                if (await LocationEnabledAsync())
                {
                    if (await viewModel.ValdiateAsync())
                    {
                        GeneralStack.IsEnabled = false;
                        CapcaityStack.IsEnabled = false;
                        await SaveAsync();
                    }
                    else
                    {
                        activityIndicator_save.Off();
                        activityIndicatorCapacity_save.Off();
                        await DisplayErrorsAsync();
                    }
                }
            }
            else if (viewModel.Edit)
            {
                RemoveErrors();
                if (await viewModel.ValdiateAsync())
                {
                    GeneralStack.IsEnabled = false;
                    CapcaityStack.IsEnabled = false;
                    await EditAsync();
                }
                else
                {
                    activityIndicator_save.Off();
                    activityIndicatorCapacity_save.Off();
                    await DisplayErrorsAsync();
                }
            }
            else
            {//Edit Clicked for the first time since last save
                activityIndicator_save.Off();
                activityIndicatorCapacity_save.Off();
                viewModel.Edit = true;
                Save.Text = "Save";
                Cancel.Text = "Cancel";                
                //enable stacks
                SetGeneralPage();
                SetCapcityPage();
                //add cancel button
            }
        }

        private async Task EditAsync()
        {
            var response = await ArcGisService.EditBin(viewModel);

            switch (response)
            {
                case ArcCrudEnum.Success:
                    await DisplayAlert("", "Bin successfully edited", "Ok");
                    activityIndicator_save.Off();
                    activityIndicatorCapacity_save.Off();
                    viewModel.New = false;
                    viewModel.Edit = false;
                    Save.Text = "Edit";
                    Cancel.Text = "Delete";
                    //disable stacks
                    SetGeneralPage();
                    SetCapcityPage();
                    break;
                case ArcCrudEnum.Failure:
                    await DisplayAlert("Error", "Error occurred", "Ok");
                    activityIndicator_save.Off();
                    activityIndicatorCapacity_save.Off();
                    break;
                case ArcCrudEnum.Exception:
                    await DisplayAlert("Error", "Failed to connect to online services. Please try again", "Ok");
                    activityIndicatorCapacity_save.Off();
                    activityIndicator_save.Off();
                    break;
            }
        }

        private async Task SaveAsync()
        {
            ArcCrudEnum addResult = await ArcGisService.AddBin(viewModel);

            switch (addResult)
            {
                case ArcCrudEnum.Success:
                    await DisplayAlert("Success!", "New bin successfully added", "Ok");
                    activityIndicator_save.Off();
                    activityIndicatorCapacity_save.Off();
                    viewModel.New = false;
                    viewModel.Edit = false;
                    Save.Text = "Edit";
                    Cancel.Text = "Delete";
                    Cancel.IsEnabled = true;
                    //disable stacks
                    SetGeneralPage();
                    SetCapcityPage();
                    break;
                case ArcCrudEnum.Failure:
                    await DisplayAlert("Error", "Error occurred, try again", "Ok");
                    activityIndicator_save.Off();
                    activityIndicatorCapacity_save.Off();
                    break;
                case ArcCrudEnum.Exception:
                    await DisplayAlert("Error", "Failed to connect to online services. Please try again", "Ok");
                    activityIndicator_save.Off();
                    activityIndicatorCapacity_save.Off();
                    break;
            }
        }

        private async Task<bool> LocationEnabledAsync()
        {
            var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (locationStatus == PermissionStatus.Denied
                || locationStatus == PermissionStatus.Disabled
                || locationStatus == PermissionStatus.Restricted)
            {
                await DisplayAlert("Error", "Location services disabled. Please enable to add entry", "Ok");
                Device.OpenUri(new Uri("app-settings:"));
                return false;
            }

            return true;
        }

        private void RemoveErrors()
        {
            if (viewModel.Errors != null)
            {
                //general bin required fields
                IdentifierLbl.Text = DefaultLabels[0];
                IdentifierLbl.TextColor = Color.Black;
                BinTypeLabel.Text = DefaultLabels[1];
                BinTypeLabel.TextColor = Color.Black;
                EmployeeLabel.Text = DefaultLabels[2];
                EmployeeLabel.TextColor = Color.Black;

                //gravity bin required capacity fields
                ChuteLengthLabel.Text = DefaultLabels[3];
                ChuteLengthLabel.TextColor = Color.Black;
                HopperHeightLabel.Text = DefaultLabels[4];
                HopperHeightLabel.TextColor = Color.Black;
                RectangleHeightLabel.Text = DefaultLabels[5];
                RectangleHeightLabel.TextColor = Color.Black;
                RectangleLengthLabel.Text = DefaultLabels[6];
                RectangleLengthLabel.TextColor = Color.Black;
                RectangleWidthLabel.Text = DefaultLabels[7];
                RectangleWidthLabel.TextColor = Color.Black;

                //polygon bin required capacity fields
                SideHeightLabel.Text = DefaultLabels[8];
                SideHeightLabel.TextColor = Color.Black;
                SideWidthLabel.Text = DefaultLabels[9];
                SideWidthLabel.TextColor = Color.Black;
                NumberOfSidesLabel.Text = DefaultLabels[10];
                NumberOfSidesLabel.TextColor = Color.Black;

                //round bin required capacity fields
                RadiusLabel.Text = DefaultLabels[11];
                RadiusLabel.TextColor = Color.Black;
                RoofHeightLabel.Text = DefaultLabels[12];
                RoofHeightLabel.TextColor = Color.Black;
                WallHeightLabel.Text = DefaultLabels[13];
                WallHeightLabel.TextColor = Color.Black;
                RoundHopperHeightLabel.Text = DefaultLabels[14];
                RoundHopperHeightLabel.TextColor = Color.Black;

                //flat bin required capacity fields
                CribLengthLabel.Text = DefaultLabels[15];
                CribLengthLabel.TextColor = Color.Black;
                CribWidthLabel.Text = DefaultLabels[16];
                CribWidthLabel.TextColor = Color.Black;

                //contents required fields
                //CropLabel.Text = DefaultLabels[17];
                //CropLabel.TextColor = Color.Black;
                //CropYearLabel.Text = DefaultLabels[18];
                //CropYearLabel.TextColor = Color.Black;
                //GrainHeightLabel.Text = DefaultLabels[19];
                //GrainHeightLabel.TextColor = Color.Black;
                //GrainConeHeightLabel.Text = DefaultLabels[20];
                //GrainConeHeightLabel.TextColor = Color.Black;
                //GrainHopperHeightLabel.Text = DefaultLabels[21];
                //GrainHopperHeightLabel.TextColor = Color.Black;

            }

        }

        private static Dictionary<int, string> SetLabels()
        {
            var dictionary = new Dictionary<int, string>();
            //general bin required fields
            dictionary.Add(0, "Identifier:");
            dictionary.Add(1, "Bin Type:");
            dictionary.Add(2, "Employee #:");

            //gravity bin required capacity fields
            dictionary.Add(3, "Chute length:");
            dictionary.Add(4, "Hopper Height:");
            dictionary.Add(5, "Rectangle Height:");
            dictionary.Add(6, "Rectangle Length:");
            dictionary.Add(7, "Rectangle Width:");
            
            //polygon bin required capacity fields
            dictionary.Add(8, "Side Height:");
            dictionary.Add(9, "Side Width:");
            dictionary.Add(10, "Number Of Sides:");
            
            //round bin required capacity fields
            dictionary.Add(11, "Radius:");
            dictionary.Add(12, "Roof Height:");
            dictionary.Add(13, "Wall Height:");
            dictionary.Add(14, "Hopper Height:");

            //flat bin required capacity fields
            dictionary.Add(15, "Crib Length:");
            dictionary.Add(16, "Crib Width:");

            //contents required fields
            dictionary.Add(17, "Crop:");
            dictionary.Add(18, "Crop Year:");
            dictionary.Add(19, "Grain Height:");
            dictionary.Add(20, "Grain Cone Height:");
            dictionary.Add(21, "Grain in Hopper Height:");

            return dictionary;
        }

        private async Task DisplayErrorsAsync()
        {          
            foreach (var key in viewModel.Errors.Keys)
            {
                switch (key)
                {
                    //general bin required fields
                    
                    case 0:
                        IdentifierLbl.Text = viewModel.Errors[0];
                        IdentifierLbl.TextColor = Color.Red;
                        break;
                    case 1:
                        BinTypeLabel.Text = viewModel.Errors[1];
                        BinTypeLabel.TextColor = Color.Red;
                        break;
                    case 2:
                        EmployeeLabel.Text = viewModel.Errors[2];
                        EmployeeLabel.TextColor = Color.Red;
                        break;

                    //gravity bin required capacity fields
                    case 3:
                        ChuteLengthLabel.Text = viewModel.Errors[3];
                        ChuteLengthLabel.TextColor = Color.Red;
                        break;
                    case 4:
                        HopperHeightLabel.Text = viewModel.Errors[4];
                        HopperHeightLabel.TextColor = Color.Red;
                        break;
                    case 5:
                        RectangleHeightLabel.Text = viewModel.Errors[5];
                        RectangleHeightLabel.TextColor = Color.Red;
                        break;
                    case 6:
                        RectangleLengthLabel.Text = viewModel.Errors[6];
                        RectangleLengthLabel.TextColor = Color.Red;
                        break;
                    case 7:
                        RectangleWidthLabel.Text = viewModel.Errors[7];
                        RectangleWidthLabel.TextColor = Color.Red;
                        break;

                    //polygon bin required capacity fields
                    case 8:
                        SideHeightLabel.Text = viewModel.Errors[8];
                        SideHeightLabel.TextColor = Color.Red;
                        break;
                    case 9:
                        SideWidthLabel.Text = viewModel.Errors[9];
                        SideWidthLabel.TextColor = Color.Red;
                        break;
                    case 10:
                        NumberOfSidesLabel.Text = viewModel.Errors[10];
                        NumberOfSidesLabel.TextColor = Color.Red;
                        break;

                    //round bin required capacity fields
                    case 11:
                        RadiusLabel.Text = viewModel.Errors[11];
                        RadiusLabel.TextColor = Color.Red;
                        break;
                    case 12:
                        RoofHeightLabel.Text = viewModel.Errors[12];
                        RoofHeightLabel.TextColor = Color.Red;
                        break;
                    case 13:
                        WallHeightLabel.Text = viewModel.Errors[13];
                        WallHeightLabel.TextColor = Color.Red;
                        break;
                    case 14:
                        RoundHopperHeightLabel.Text = viewModel.Errors[14];
                        RoundHopperHeightLabel.TextColor = Color.Red;
                        break;

                    //flat bin required capacity fields
                    case 15:
                        CribLengthLabel.Text = viewModel.Errors[15];
                        CribLengthLabel.TextColor = Color.Red;
                        break;
                    case 16:
                        CribWidthLabel.Text = viewModel.Errors[16];
                        CribWidthLabel.TextColor = Color.Red;
                        break;

                        //contents required fields
                        //case 17:
                        //    CropLabel.Text = viewModel.Errors[17];
                        //    CropLabel.TextColor = Color.Red;
                        //    break;
                        //case 18:
                        //    CropYearLabel.Text = viewModel.Errors[18];
                        //    CropYearLabel.TextColor = Color.Red;
                        //    break;
                        //case 19:
                        //    GrainHeightLabel.Text = viewModel.Errors[19];
                        //    GrainHeightLabel.TextColor = Color.Red;
                        //    break;
                        //case 20:
                        //    GrainConeHeightLabel.Text = viewModel.Errors[20];
                        //    GrainConeHeightLabel.TextColor = Color.Red;
                        //    break;
                        //case 21:
                        //    GrainHopperHeightLabel.Text = viewModel.Errors[21];
                        //    GrainHopperHeightLabel.TextColor = Color.Red;
                        //    break;
                }                
            }

            await DisplayAlert("Notice!", "Please enter required values in red", "Ok");
        }
        #endregion

        #region Cancel

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Edit && !viewModel.New)
            {//cancel edit
                Cancel.Text = "Delete";
                Save.Text = "Edit";
                viewModel.Edit = false;
                viewModel.Binstance = viewModel.ArcGISFeature.MapToBin();
                //viewModel.FeatureMapBinType();
                //viewModel.ViewModelMapBinType();
            }
            else if (!viewModel.New)
            {//delete
                HandleDelete();
            }
        }

        private async void HandleDelete()
        {
            var confirm = await DisplayAlert("Delete", "Are you sure you want to delete this bin record?", "No", "Yes");

            if (!confirm)
            {
                var response = await ArcGisService.DeleteBin(viewModel.ArcGISFeature);

                switch (response)
                {
                    case ArcCrudEnum.Success:
                        await DisplayAlert("", "Bin successfully deleted", "Ok");
                        //activityIndicator_edit.Off();
                        await Navigation.PopToRootAsync();
                        break;
                    case ArcCrudEnum.Failure:
                        await DisplayAlert("Error", "Error occurred", "Ok");
                        //activityIndicator_edit.Off();
                        break;
                    case ArcCrudEnum.Exception:
                        await DisplayAlert("Error", "Failed to connect to online services. Please try again", "Ok");
                        //activityIndicator_edit.Off();
                        break;
                }
            }
        }

        #endregion

        #region Set General Page
        private void SetGeneralPage()
        {
            if (viewModel.New || viewModel.Edit)
            {
                GeneralStack.IsEnabled = true;
                if (viewModel.Edit)
                {
                    CreatedByLabel.IsVisible = true;
                    CreatedByLabel.IsEnabled = false;
                    CreatedBy.IsVisible = true;
                    CreatedBy.IsEnabled = false;
                    ModifiedByLabel.IsVisible = true;
                    ModifiedByLabel.IsEnabled = false;
                    ModifiedBy.IsVisible = true;
                    ModifiedBy.IsEnabled = false;
                }
            }
            else
            {
                GeneralStack.IsEnabled = false;
            }
        }
        #endregion

        #region SetContents Page
        private void SetContentsPage()
        {
            //ContentsLabel.TextColor = Color.Black;

            //VolumeLabel.IsEnabled = true;
            //VolumeLabel.IsVisible = true;

            //ContentStack.IsVisible = true;
            //ContentStack.IsEnabled = true;

            //switch (viewModel.BinType)
            //{
            //    case -1:
            //        HideContentsFields();
            //        break;
            //    case 0:
            //        SetFlatContents();
            //        break;                
            //    case 1:
            //        SetGravityContents();
            //        break;
            //    case 2:
            //        SetPolygonContents();
            //        break;
            //    case 3:
            //        SetRoundContents();
            //        break;
            //    default:
            //        HideContentsFields();
            //        break;
            //}

            //SetCropYear();
        }

        //private void SetPolygonContents()
        //{
        //    ContentsLabel.Text = "Enter Polygon Bin Contents";
        //    GrainHopperHeightLabel.IsVisible = false;
        //    GrainHopperHeightLabel.IsEnabled = false;
        //    GrainHopperHeight.IsVisible = false;
        //    GrainHopperHeight.IsEnabled = false;
        //}

        //private void SetGravityContents()
        //{
        //    ContentsLabel.Text = "Enter Gravity Bin Contents";
        //    GrainHopperHeightLabel.IsVisible = true;
        //    GrainHopperHeightLabel.IsEnabled = true;
        //    GrainHopperHeight.IsVisible = true;
        //    GrainHopperHeight.IsEnabled = true;
        //}

        //private void SetRoundContents()
        //{
        //    ContentsLabel.Text = "Enter Round Bin Contents";
        //    GrainHopperHeightLabel.IsVisible = true;
        //    GrainHopperHeightLabel.IsEnabled = true;
        //    GrainHopperHeight.IsVisible = true;
        //    GrainHopperHeight.IsEnabled = true;
        //}

        //private void SetFlatContents()
        //{
        //    ContentsLabel.Text = "Enter Flat Bin Contents";
        //    GrainHopperHeightLabel.IsVisible = false;
        //    GrainHopperHeightLabel.IsEnabled = false;
        //    GrainHopperHeight.IsVisible = false;
        //    GrainHopperHeight.IsEnabled = false;
        //}

        //private void HideContentsFields()
        //{
        //    ContentsLabel.Text = "Bin Type is required";
        //    ContentsLabel.TextColor = Color.Red;

        //    VolumeLabel.IsEnabled = false;
        //    VolumeLabel.IsVisible = false;

        //    ContentStack.IsVisible = false;
        //    ContentStack.IsEnabled = false;

        //}


        #endregion

        #region SetCapcity Page
        private void SetCapcityPage()
        {
            switch (viewModel.BinType)
            {
                case -1:
                    HideCapacityFields();
                    break;
                case 0:
                    SetFlatCapacity();
                    break;
                case 1:
                    SetGravityCapacity();

                    break;
                case 2:
                    SetPolygonCapacity();
                    break;
                case 3:
                    SetRoundCapacity();
                    break;
            }
        }

        private void SetPolygonCapacity()
        {
            CapcaityStack.IsEnabled = true;
            CapcaityStack.IsVisible = true;

            TypeLable.Text = "Polygon Bin Type Capacity";
            TypeLable.TextColor = Color.Black;

            PolygonStack.IsVisible = true;
            if (viewModel.New || viewModel.Edit)
            {
                PolygonStack.IsEnabled = true;
            }
            else
            {
                PolygonStack.IsEnabled = false;
            }

            FlatStack.IsEnabled = false;
            FlatStack.IsVisible = false;
            GravityStack.IsEnabled = false;
            GravityStack.IsVisible = false;
            RoundStack.IsEnabled = false;
            RoundStack.IsVisible = false;
        }

        private void SetGravityCapacity()
        {
            CapcaityStack.IsEnabled = true;
            CapcaityStack.IsVisible = true;

            TypeLable.Text = "Gravity Bin Type Capacity";
            TypeLable.TextColor = Color.Black;

            GravityStack.IsVisible = true;
            if (viewModel.New || viewModel.Edit)
            {
                GravityStack.IsEnabled = true;
            }
            else
            {
                GravityStack.IsEnabled = false;
            }

            FlatStack.IsEnabled = false;
            FlatStack.IsVisible = false;
            PolygonStack.IsEnabled = false;
            PolygonStack.IsVisible = false;
            RoundStack.IsEnabled = false;
            RoundStack.IsVisible = false;
        }

        private void SetRoundCapacity()
        {
            CapcaityStack.IsEnabled = true;
            CapcaityStack.IsVisible = true;

            TypeLable.Text = "Round Bin Type Capacity";
            TypeLable.TextColor = Color.Black;

            RoundStack.IsVisible = true;
            if(viewModel.New || viewModel.Edit)
            {
                RoundStack.IsEnabled = true;
            }
            else
            {
                RoundStack.IsEnabled = false;
            }

            FlatStack.IsEnabled = false;
            FlatStack.IsVisible = false;
            GravityStack.IsEnabled = false;
            GravityStack.IsVisible = false;
            PolygonStack.IsEnabled = false;
            PolygonStack.IsVisible = false;            
        }

        private void SetFlatCapacity()
        {
            CapcaityStack.IsEnabled = true;
            CapcaityStack.IsVisible = true;

            TypeLable.Text = "Falt Bin Type Capacity";
            TypeLable.TextColor = Color.Black;

            FlatStack.IsVisible = true;
            if (viewModel.New || viewModel.Edit)
            {
                FlatStack.IsEnabled = true;
            }
            else
            {
                FlatStack.IsEnabled = false;
            }

            GravityStack.IsEnabled = false;
            GravityStack.IsVisible = false;
            PolygonStack.IsEnabled = false;
            PolygonStack.IsVisible = false;
            RoundStack.IsEnabled = false;
            RoundStack.IsVisible = false;
        }

        private void HideCapacityFields()
        {
            TypeLable.Text = "Bin Type is required";
            TypeLable.TextColor = Color.Red;
            CapcaityStack.IsEnabled = false;
            CapcaityStack.IsVisible = false;
        }
        #endregion

        #region General Field Events

        private void EmployeeNumTxtBox_Changed(object sender, TextChangedEventArgs e)
        {
            viewModel.EmpoyeeNumber = EmployeeNumTxtBox.Text;
        }

        private void BinType_Changed(object sender, EventArgs e)
        {
            switch (BinType.SelectedIndex)
            {
                case 0:
                    HasHopperHopperDisable();
                    break;
                case 1:
                    HasHopperHopperDisable();
                    break;
                case 2:
                    HasHopperHopperDisable();
                    break;
                case 3:
                    HasHopperHopperEnable();
                    break;
                case -1:
                default:
                    break;
            }
                        
            SetCapcityPage();
            //SetContentsPage();

        }

        private void HasHopperHopperEnable()
        {
            HasHopperLabel.IsEnabled = true;
            HasHopperLabel.IsVisible = true;
            HasHopper.IsEnabled = true;
            HasHopper.IsVisible = true;
        }

        private void HasHopperHopperDisable()
        {
            HasHopperLabel.IsEnabled = false;
            HasHopperLabel.IsVisible = false;
            HasHopper.IsEnabled = false;
            HasHopper.IsVisible = false;
        }

        #endregion

        #region Contents Field Events

        private async void SetImage()
        {
            await viewModel.ArcGISFeature.LoadAsync();

            var attachments = await viewModel.ArcGISFeature.GetAttachmentsAsync();

            foreach (var attach in attachments)
            {
                if (attach.ContentType.Contains(@"image/"))
                {
                    using (var stream = await attach.GetDataAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            var asByte = memoryStream.ToArray();

                            BinPic.Source = ImageSource.FromStream(() => new MemoryStream(asByte));
                            BinPicFrame.IsVisible = true;
                        }
                    }
                }
            }

        }

        //set the crop year picker to the year of the current yty object 
        private void SetCropYear()
        {  
            int yr;

            if (viewModel.YTYData.CropYear != 0)
            {
                yr = viewModel.YTYData.CropYear;
            }
            else
            {//current year if this is a new yty object                
                yr = DateTime.Now.Year;
                yr = CheckCropYear(yr);
            }

            //foreach (var year in CropYear.Items)
            //{
            //    int itemYear = int.TryParse(year, out i) ? i : 0;
            //    if (itemYear == yr)
            //    {
            //        CropYear.SelectedItem = yr;
            //        viewModel.YTYData.CropYear = yr;
            //        break;
            //    }
            //}
        }

        //determine if the current year already exists
        private int CheckCropYear(int year)
        {

            foreach (var yty in viewModel.Binstance.YTYDatas)
            {
                if (yty.CropYear == year)
                {
                    return CheckCropYear(year - 1);
                }
            }

            return year;

        }

        private async void OnDateSelectedAsync(object sender, EventArgs args)
        {
            //if (!viewModel.New)
            //{
            //    await CheckCropYearAsync();
            //}
            
        }

        private async Task CheckCropYearAsync()
        {
            int i;
            bool edit = false;
            //var selected = Int32.TryParse(CropYear.SelectedItem.ToString(), out i) ? i : 0;
            //foreach (var yty in viewModel.Binstance.YTYDatas)
            //{
            //    if (yty.CropYear == selected)
            //    {
            //        if (viewModel.Edit)
            //        {
            //            edit = await DisplayAlert("Warning!", "Crop year already exists, edit this year?", "Yes", "No");
            //            if (edit)
            //            {
            //                viewModel.YTYData = yty;
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            viewModel.YTYData = yty;
            //        }
                    
            //    }
            //}
        }


        //private void GrainConeHeight_Changed(object sender, TextChangedEventArgs e)
        //{
        //    //CalcVolume();
        //}

        //private void GrainHopperHeight_Changed(object sender, TextChangedEventArgs e)
        //{
        //    //CalcVolume();
        //}

        //private void GrainHeight_Changed(object sender, TextChangedEventArgs e)
        //{
        //    //CalcVolume();
        //}


        //private void CalcVolume()
        //{
        //    viewModel.ViewModelMapBinType();
        //    viewModel.ViewModelMapYTY();
        //    //viewModel.YTYData.TotalVolume = VolumeMath.CalculateVolume(viewModel);
        //    //viewModel.YTYData.TotalVolume = viewModel.YTYData.TotalVolume - viewModel.YTYData.TotalDeductionVolume;
        //    //VolumeLabel.Text = "Total Grain Volume: " + viewModel.YTYData.TotalVolume.ToString("#,###.##");
        //}


        #endregion

        
    }
}