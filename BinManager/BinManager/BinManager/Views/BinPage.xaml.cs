﻿using BinManager.Models;
using BinManager.Utilities.Enums;
using BinManager.Utilities;
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

            //CropYear.ItemsSource = viewModel.DateRange;
            //SetCropYear();

            YearCollected.Text = DateTime.Now.ToString("yyyy");
            YearCollected.IsEnabled = false;
            viewModel.Binstance.YearCollected = YearCollected.Text;

            BinPicFrame.IsVisible = false;
            CreatedByLabel.IsVisible = false;
            CreatedByLabel.IsEnabled = false;
            CreatedBy.IsVisible = false;
            CreatedBy.IsEnabled = false;

            //HasHopperHopperDisable();
            //HideCapacityFields();
            //HideContentsFields();

        }

        //View New Bin
        public BinPage(ArcGISFeature feature)
        {
            InitializeComponent();

            BindingContext = viewModel = new BinViewModel(feature);
            
            //CropYear.ItemsSource = viewModel.DateRange;
            //SetCropYear();
            Save.Text = "Edit";

            BinPicFrame.IsVisible = false;
            //SetImage();
            ////CalcVolume();
            ////disable fields
            //GeneralStack.IsEnabled = false;
            //CapcaityStack.IsEnabled = false;
            //ContentStack.IsEnabled = false;
        }

        #endregion

        #region Save
        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (viewModel.New)
            {
                RemoveErrors();

                if (await LocationEnabledAsync())
                {
                    if (await viewModel.ValdiateAsync())
                    {
                        await SaveAsync();
                    }
                    else
                    {
                        DisplayErrors();
                    }
                }
            }
            else if (viewModel.Edit)
            {
                RemoveErrors();
                if (await viewModel.ValdiateAsync())
                {
                    //await EditAsync();
                }
                else
                {
                    DisplayErrors();
                }
            }
            else
            {//Edit Clicked for the first time since last save
                Save.Text = "Save";
                viewModel.Edit = true;
                //enable stacks
                //add cancel button
            }
        }        

        private async Task SaveAsync()
        {
            //ArcCrudEnum addResult = await ArcGisService.AddBin(viewModel);
            ////bool goToMap = false;

            //switch (addResult)
            //{
            //    case ArcCrudEnum.Success:
            //        //activityIndicator_process.Off();
            //        await DisplayAlert("Success!", "New bin successfully added", "Ok");
            //        viewModel.New = false;
            //        viewModel.Edit = false;
            //        Save.Text = "Edit";
            //        //disable fields
            //        break;
            //    case ArcCrudEnum.Failure:
            //        //activityIndicator_process.Off();
            //        await DisplayAlert("Error", "Error occurred, try again", "Ok");
            //        break;
            //    case ArcCrudEnum.Exception:
            //        //activityIndicator_process.Off();
            //        await DisplayAlert("Error", "Failed to connect to online services. Please try again", "Ok");
            //        break;
            //}
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
                //ChuteLengthLabel.Text = DefaultLabels[3];
                //ChuteLengthLabel.TextColor = Color.Black;
                //HopperHeightLabel.Text = DefaultLabels[4];
                //HopperHeightLabel.TextColor = Color.Black;
                //RectangleHeightLabel.Text = DefaultLabels[5];
                //RectangleHeightLabel.TextColor = Color.Black;
                //RectangleLengthLabel.Text = DefaultLabels[6];
                //RectangleLengthLabel.TextColor = Color.Black;
                //RectangleWidthLabel.Text = DefaultLabels[7];
                //RectangleWidthLabel.TextColor = Color.Black;

                ////polygon bin required capacity fields
                //SideHeightLabel.Text = DefaultLabels[8];
                //SideHeightLabel.TextColor = Color.Black;
                //SideWidthLabel.Text = DefaultLabels[9];
                //SideWidthLabel.TextColor = Color.Black;
                //NumberOfSidesLabel.Text = DefaultLabels[10];
                //NumberOfSidesLabel.TextColor = Color.Black;

                ////round bin required capacity fields
                //RadiusLabel.Text = DefaultLabels[11];
                //RadiusLabel.TextColor = Color.Black;
                //RoofHeightLabel.Text = DefaultLabels[12];
                //RoofHeightLabel.TextColor = Color.Black;
                //WallHeightLabel.Text = DefaultLabels[13];
                //WallHeightLabel.TextColor = Color.Black;
                //RoundHopperHeightLabel.Text = DefaultLabels[14];
                //RoundHopperHeightLabel.TextColor = Color.Black;

                ////flat bin required capacity fields
                //CribLengthLabel.Text = DefaultLabels[15];
                //CribLengthLabel.TextColor = Color.Black;
                //CribWidthLabel.Text = DefaultLabels[16];
                //CribWidthLabel.TextColor = Color.Black;

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

        private void DisplayErrors()
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
                    //case 3:
                    //    ChuteLengthLabel.Text = viewModel.Errors[3];
                    //    ChuteLengthLabel.TextColor = Color.Red;
                    //    break;
                    //case 4:
                    //    HopperHeightLabel.Text = viewModel.Errors[4];
                    //    HopperHeightLabel.TextColor = Color.Red;
                    //    break;
                    //case 5:
                    //    RectangleHeightLabel.Text = viewModel.Errors[5];
                    //    RectangleHeightLabel.TextColor = Color.Red;
                    //    break;
                    //case 6:
                    //    RectangleLengthLabel.Text = viewModel.Errors[6];
                    //    RectangleLengthLabel.TextColor = Color.Red;
                    //    break;
                    //case 7:
                    //    RectangleWidthLabel.Text = viewModel.Errors[7];
                    //    RectangleWidthLabel.TextColor = Color.Red;
                    //    break;
                    
                    ////polygon bin required capacity fields
                    //case 8:
                    //    SideHeightLabel.Text = viewModel.Errors[8];
                    //    SideHeightLabel.TextColor = Color.Red;
                    //    break;
                    //case 9:
                    //    SideWidthLabel.Text = viewModel.Errors[9];
                    //    SideWidthLabel.TextColor = Color.Red;
                    //    break;
                    //case 10:
                    //    NumberOfSidesLabel.Text = viewModel.Errors[10];
                    //    NumberOfSidesLabel.TextColor = Color.Red;
                    //    break;
                    
                    ////round bin required capacity fields
                    //case 11:
                    //    RadiusLabel.Text = viewModel.Errors[11];
                    //    RadiusLabel.TextColor = Color.Red;
                    //    break;
                    //case 12:
                    //    RoofHeightLabel.Text = viewModel.Errors[12];
                    //    RoofHeightLabel.TextColor = Color.Red;
                    //    break;
                    //case 13:
                    //    WallHeightLabel.Text = viewModel.Errors[13];
                    //    WallHeightLabel.TextColor = Color.Red;
                    //    break;
                    //case 14:
                    //    RoundHopperHeightLabel.Text = viewModel.Errors[14];
                    //    RoundHopperHeightLabel.TextColor = Color.Red;
                    //    break;
                    
                    ////flat bin required capacity fields
                    //case 15:
                    //    CribLengthLabel.Text = viewModel.Errors[15];
                    //    CribLengthLabel.TextColor = Color.Red;
                    //    break;
                    //case 16:
                    //    CribWidthLabel.Text = viewModel.Errors[16];
                    //    CribWidthLabel.TextColor = Color.Red;
                    //    break;

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
        //private void SetCapcityPage()
        //{
        //    switch (viewModel.BinType)
        //    {
        //        case -1:
        //            HideCapacityFields();
        //            break;
        //        case 0:
        //            SetFlatCapacity();
        //            break;
        //        case 1:
        //            SetGravityCapacity();
                    
        //            break;
        //        case 2:
        //            SetPolygonCapacity();
        //            break;
        //        case 3:
        //            SetRoundCapacity();
        //            break;
        //    }
        //}

        //private void SetPolygonCapacity()
        //{
        //    HideCapacityFields();

        //    TypeLable.Text = "Polygon Bin Type Capacity";
        //    TypeLable.TextColor = Color.Black;

        //    //polygon bin fields
        //    SideHeightLabel.IsEnabled = true;
        //    SideHeightLabel.IsVisible = true;
        //    SideHeight.IsEnabled = true;
        //    SideHeight.IsVisible = true;
        //    SideWidthLabel.IsEnabled = true;
        //    SideWidthLabel.IsVisible = true;
        //    SideWidth.IsEnabled = true;
        //    SideWidth.IsVisible = true;
        //    NumberOfSidesLabel.IsEnabled = true;
        //    NumberOfSidesLabel.IsVisible = true;
        //    NumberOfSides.IsEnabled = true;
        //    NumberOfSides.IsVisible = true;
        //}

        //private void SetGravityCapacity()
        //{
        //    HideCapacityFields();

        //    TypeLable.Text = "Gravity Bin Type Capacity";
        //    TypeLable.TextColor = Color.Black;

        //    //garvity bin fields
        //    RectangleHeightLabel.IsEnabled = true;
        //    RectangleHeightLabel.IsVisible = true;
        //    RectangleHeight.IsEnabled = true;
        //    RectangleHeight.IsVisible = true;
        //    RectangleWidthLabel.IsEnabled = true;
        //    RectangleWidthLabel.IsVisible = true;
        //    RectangleWidth.IsEnabled = true;
        //    RectangleWidth.IsVisible = true;
        //    RectangleLengthLabel.IsEnabled = true;
        //    RectangleLengthLabel.IsVisible = true;
        //    RectangleLength.IsEnabled = true;
        //    RectangleLength.IsVisible = true;
        //    ChuteLengthLabel.IsEnabled = true;
        //    ChuteLengthLabel.IsVisible = true;
        //    ChuteLength.IsEnabled = true;
        //    ChuteLength.IsVisible = true;
        //    HopperHeightLabel.IsEnabled = true;
        //    HopperHeightLabel.IsVisible = true;
        //    HopperHeight.IsEnabled = true;
        //    HopperHeight.IsVisible = true;
        //}

        //private void SetRoundCapacity()
        //{
        //    HideCapacityFields();

        //    TypeLable.Text = "Round Bin Type Capacity";
        //    TypeLable.TextColor = Color.Black;

        //    //round bin fields
        //    RadiusLabel.IsEnabled = true;
        //    RadiusLabel.IsVisible = true;
        //    Radius.IsEnabled = true;
        //    Radius.IsVisible = true;
        //    WallHeightLabel.IsEnabled = true;
        //    WallHeightLabel.IsVisible = true;
        //    WallHeight.IsEnabled = true;
        //    WallHeight.IsVisible = true;
        //    RoofHeightLabel.IsEnabled = true;
        //    RoofHeightLabel.IsVisible = true;
        //    RoofHeight.IsEnabled = true;
        //    RoofHeight.IsVisible = true;
        //    RoundHopperHeightLabel.IsEnabled = true;
        //    RoundHopperHeightLabel.IsVisible = true;
        //    RoundHopperHeight.IsEnabled = true;
        //    RoundHopperHeight.IsVisible = true;
        //}

        //private void SetFlatCapacity()
        //{
        //    HideCapacityFields();

        //    TypeLable.Text = "Falt Bin Type Capacity";
        //    TypeLable.TextColor = Color.Black;

        //    //flat bin fields
        //    CribWidthLabel.IsEnabled = true;
        //    CribWidthLabel.IsVisible = true;
        //    CribWidth.IsEnabled = true;
        //    CribWidth.IsVisible = true;
        //    CribLengthLabel.IsEnabled = true;
        //    CribLengthLabel.IsVisible = true;
        //    CribLength.IsEnabled = true;
        //    CribLength.IsVisible = true;
        //}

        //private void HideCapacityFields()
        //{
        //    TypeLable.Text = "Bin Type is required";
        //    TypeLable.TextColor = Color.Red;

        //    //flat bin fields
        //    CribWidthLabel.IsEnabled = false;
        //    CribWidthLabel.IsVisible = false;
        //    CribWidth.IsEnabled = false;
        //    CribWidth.IsVisible = false;
        //    CribLengthLabel.IsEnabled = false;
        //    CribLengthLabel.IsVisible = false;
        //    CribLength.IsEnabled = false;
        //    CribLength.IsVisible = false;

        //    //garvity bin fields
        //    RectangleHeightLabel.IsEnabled = false;
        //    RectangleHeightLabel.IsVisible = false;
        //    RectangleHeight.IsEnabled = false;
        //    RectangleHeight.IsVisible = false;
        //    RectangleWidthLabel.IsEnabled = false;
        //    RectangleWidthLabel.IsVisible = false;
        //    RectangleWidth.IsEnabled = false;
        //    RectangleWidth.IsVisible = false;
        //    RectangleLengthLabel.IsEnabled = false;
        //    RectangleLengthLabel.IsVisible = false;
        //    RectangleLength.IsEnabled = false;
        //    RectangleLength.IsVisible = false;
        //    ChuteLengthLabel.IsEnabled = false;
        //    ChuteLengthLabel.IsVisible = false;
        //    ChuteLength.IsEnabled = false;
        //    ChuteLength.IsVisible = false;
        //    HopperHeightLabel.IsEnabled = false;
        //    HopperHeightLabel.IsVisible = false;
        //    HopperHeight.IsEnabled = false;
        //    HopperHeight.IsVisible = false;

        //    //polygon bin fields
        //    SideHeightLabel.IsEnabled = false;
        //    SideHeightLabel.IsVisible = false;
        //    SideHeight.IsEnabled = false;
        //    SideHeight.IsVisible = false;
        //    SideWidthLabel.IsEnabled = false;
        //    SideWidthLabel.IsVisible = false;
        //    SideWidth.IsEnabled = false;
        //    SideWidth.IsVisible = false;
        //    NumberOfSidesLabel.IsEnabled = false;
        //    NumberOfSidesLabel.IsVisible = false;
        //    NumberOfSides.IsEnabled = false;
        //    NumberOfSides.IsVisible = false;

        //    //round bin fields
        //    RadiusLabel.IsEnabled = false;
        //    RadiusLabel.IsVisible = false;
        //    Radius.IsEnabled = false;
        //    Radius.IsVisible = false;
        //    WallHeightLabel.IsEnabled = false;
        //    WallHeightLabel.IsVisible = false;
        //    WallHeight.IsEnabled = false;
        //    WallHeight.IsVisible = false;
        //    RoofHeightLabel.IsEnabled = false;
        //    RoofHeightLabel.IsVisible = false;
        //    RoofHeight.IsEnabled = false;
        //    RoofHeight.IsVisible = false;
        //    RoundHopperHeightLabel.IsEnabled = false;
        //    RoundHopperHeightLabel.IsVisible = false;
        //    RoundHopperHeight.IsEnabled = false;
        //    RoundHopperHeight.IsVisible = false;
        //}
        #endregion

        #region General Field Events

        //private void EmployeeNumTxtBox_Changed(object sender, TextChangedEventArgs e)
        //{
        //    //new entry
        //    if (viewModel.New)
        //    {
        //        viewModel.Binstance.CreatedBy = EmployeeNumTxtBox.Text;
        //    }

        //    //edit entry
        //    if (viewModel.Edit)
        //    {
        //        viewModel.Binstance.ModifiedBy = EmployeeNumTxtBox.Text;
        //    }
        //}

        //private void BinType_Changed(object sender, EventArgs e)
        //{
        //    //switch (BinType.SelectedIndex)
        //    //{
        //    //    case 0:
        //    //        HasHopperHopperDisable();
        //    //        break;
        //    //    case 1:
        //    //        HasHopperHopperDisable();
        //    //        break;
        //    //    case 2:
        //    //        HasHopperHopperDisable();
        //    //        break;
        //    //    case 3:
        //    //        HasHopperHopperEnable();
        //    //        break;
        //    //    case -1:
        //    //    default:
        //    //        break;
        //    //}

        //    //SetCapcityPage();
        //    //SetContentsPage();

        //}

        //private void HasHopperHopperEnable()
        //{
        //    HasHopperLabel.IsEnabled = true;
        //    HasHopperLabel.IsVisible = true;
        //    HasHopper.IsEnabled = true;
        //    HasHopper.IsVisible = true;
        //}

        //private void HasHopperHopperDisable()
        //{
        //    HasHopperLabel.IsEnabled = false;
        //    HasHopperLabel.IsVisible = false;
        //    HasHopper.IsEnabled = false;
        //    HasHopper.IsVisible = false;
        //}

        #endregion

        #region Contents Field Events

        private async void SetImage()
        {
            //await viewModel.ArcGISFeature.LoadAsync();

            //var attachments = await viewModel.ArcGISFeature.GetAttachmentsAsync();

            //foreach (var attach in attachments)
            //{
            //    if (attach.ContentType.Contains(@"image/"))
            //    {
            //        using (var stream = await attach.GetDataAsync())
            //        {
            //            using (var memoryStream = new MemoryStream())
            //            {
            //                stream.CopyTo(memoryStream);
            //                var asByte = memoryStream.ToArray();

            //                BinPic.Source = ImageSource.FromStream(() => new MemoryStream(asByte));
            //                BinPicFrame.IsVisible = true;
            //            }
            //        }
            //    }
            //}

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