﻿
namespace BinManager.Views
{
    #region imports
    using Plugin.Permissions;
    using Plugin.Permissions.Abstractions;
    using cam = Plugin.Media.CrossMedia;
    using camOptions = Plugin.Media.Abstractions;
    using System;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using BinManager.Settings;
    using BinManager.Utilities.Enums;
    using Plugin.Media;
    #endregion

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        public MainMenu()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            ConnectToArcGIS();
        }

        public async void ConnectToArcGIS()
        {
            var loginResult = await ArcGisService.Authenticate("clint.kautz", "Demopassword12");

            // if login successful, load main menu
            if (loginResult == ArcCrudEnum.Success)
            {
                GlobalSettings.EmployeeNum = "123";
            }
            // if login unsuccessful, set info message
            else if (loginResult == ArcCrudEnum.Failure)
            {
                
            }
        }

        public async void NewEntryBtn_Clicked(object sender, EventArgs args)
        {
            // Check camera permissions
            if (DeviceInfo.DeviceType == DeviceType.Physical)
            {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                if (cameraStatus == PermissionStatus.Denied
                    || cameraStatus == PermissionStatus.Disabled
                    || cameraStatus == PermissionStatus.Restricted)
                {
                    Device.OpenUri(new Uri("app-settings:"));
                    return;
                }

                // Take photo, used in creating new entry. Metadata taken from here
                await cam.Current.Initialize();
                var photo = await CrossMedia.Current.TakePhotoAsync(new camOptions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = "binPic.png"
                });

                //var photo = await cam.Current.TakePhotoAsync(new camOptions.StoreCameraMediaOptions() { });

                if (photo != null)
                {
                    // Navigate to next page, pass photo
                    await Navigation.PushAsync(new BinPage(photo)
                    {
                        Title = GlobalSettings.NewEntryPageTitle
                    });
                }
                else
                {
                    await DisplayAlert("Error", "Photo is required", "Ok");
                }
            }
            else
            {
                await Navigation.PushAsync(new BinPage());
            }

            
        }

        public void MapViewBtn_Clicked(object sender, EventArgs args)
        {
            //Navigation.PushAsync(_mapPage);
            Navigation.PushAsync(new MapPage()
            {
                Title = "Map"
            });
        }
    }
}