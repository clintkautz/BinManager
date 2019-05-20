using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Location;
using Esri.ArcGISRuntime.Mapping.Popups;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Security;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using BinManager.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.Geocoding;

namespace BinManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private FeatureLayer _localFeatureLayer;
        private ArcGISFeature _feature;
        private Map _webMap;

        // search functionality
        private LocatorTask _geocoder;
        private static ServiceFeatureTable _featureTable;
        FeatureLayer _featureLayer;

        private SimpleMarkerSymbol NotMovingMarkerSym = new SimpleMarkerSymbol
        {
            Color = Color.Red,
            Style = SimpleMarkerSymbolStyle.Circle,
            Size = 16
        };

        private SimpleMarkerSymbol MovingMarkerSym = new SimpleMarkerSymbol
        {
            Color = Color.Blue,
            Style = SimpleMarkerSymbolStyle.Circle,
            Size = 12
        };

        public MapPage()
        {
            InitializeComponent();
            
            _featureTable = new ServiceFeatureTable(new Uri(ArcGisService.FeatureServiceUrl));
            _featureLayer = new FeatureLayer(_featureTable);
            choice.SelectedIndex = 0;
            SetupMap();
        }

        //public MapPage(bool onBack)
        //{
        //    InitializeComponent();

        //    _featureTable = new ServiceFeatureTable(new Uri(ArcGisService.NaucFeatureServiceUrl));
        //    _featureLayer = new FeatureLayer(_featureTable);

        //    EnableBackButtonOverride = true;

        //    if (EnableBackButtonOverride)
        //    {
        //        CustomBackButtonAction = async () =>
        //        {
        //            await Navigation.PopToRootAsync();                
        //        };
                
        //    }

        //    SetupMap();
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HandleLoad();
        }

        public async void SetupMap()
        {
            MyMapView.GeoViewTapped += MapView_GeoViewTapped;
            MyMapView.DrawStatusChanged += MapView_OnDrawStatusChanged;

            _geocoder = new LocatorTask(new Uri("https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer"));
            await _geocoder.LoadAsync();

            MyMapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Navigation;
            MyMapView.LocationDisplay.DefaultSymbol = NotMovingMarkerSym;
            MyMapView.LocationDisplay.CourseSymbol = MovingMarkerSym;
            MyMapView.LocationDisplay.IsEnabled = true;
            MyMapView.LocationDisplay.ShowPingAnimationSymbol = true;
        }

        private void Refresh_Activated(object sender, EventArgs e)
        {
            OnAppearing();
        }

        private void Search_Activated(object sender, EventArgs e)
        {
            Search_Grid.IsVisible = !Search_Grid.IsVisible;
            AddressTextBox.Text = "";
            FeatureTextBox.Text = "";
            SuggestionList.ItemsSource = new List<string>();
        }

        private async void HandleLoad()
        {
            _webMap = await Map.LoadFromUriAsync(new Uri(ArcGisService.WebMapItemUrl));

            if(_webMap.LoadStatus == LoadStatus.Loaded)
            {
                MyMapView.Map = _webMap;
                if (GlobalSettings.UpdateViewpoint != null)
                {
                     MyMapView.SetViewpoint(GlobalSettings.UpdateViewpoint);
                }

                _localFeatureLayer = MyMapView.Map.OperationalLayers[0] as FeatureLayer;
            }
        }

        public async void GetAddressSuggestions(string searchText)
        {
            if (_geocoder.LocatorInfo.SupportsSuggestions)
            {
                //var currentExtent = MyMapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry).TargetGeometry;
                // search area V as well
                var suggestParams = new SuggestParameters { MaxResults = 10 };
                IReadOnlyList<SuggestResult> suggestions = await _geocoder.SuggestAsync(searchText, suggestParams);

                SuggestionList.ItemsSource = suggestions;
            }
        }

        public async void ShowAddressLocation(SuggestResult suggestion)
        {
            IReadOnlyList<GeocodeResult> matches = await _geocoder.GeocodeAsync(suggestion);
            var bestMatch = (from match in matches orderby match.Score select match).FirstOrDefault();
            if (bestMatch == null) return;

            // done to get a reference to 'matchOverlay' used to add the 'matchGraphic' later
            MyMapView.GraphicsOverlays.Add(new GraphicsOverlay());
            var matchOverlay = MyMapView.GraphicsOverlays.FirstOrDefault();
            matchOverlay.Graphics.Clear();

            var matchGraphic = new Graphic(bestMatch.DisplayLocation);
            matchGraphic.Symbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, Color.Green, 16);
            matchOverlay.Graphics.Add(matchGraphic);

            await MyMapView.SetViewpointAsync(new Viewpoint(bestMatch.DisplayLocation, 24000));
        }

        private void AddressTextChanged(object sender, TextChangedEventArgs e)
        {
            var address = AddressTextBox.Text.Trim();

            if (string.IsNullOrEmpty(address) || address.Length < 3)
            {
                SuggestionList.ItemsSource = new List<string>();
                return;
            }

            var suggestion = SuggestionList.SelectedItem as SuggestResult;
            if (suggestion != null && suggestion.Label == address) return;

            GetAddressSuggestions(address);
        }

        private void SuggestionChosen(object sender, SelectedItemChangedEventArgs e)
        {
            var suggestion = SuggestionList.SelectedItem as SuggestResult;
            if (suggestion == null) return;

            AddressTextBox.Text = suggestion.Label;

            ShowAddressLocation(suggestion);
        }

        // for feature search
        private void QSearch_Clicked(object sender, EventArgs e)
        {
            FeatureSearch();
        }

        private async void FeatureSearch()
        {
            _featureLayer.ClearSelection();
            var address = FeatureTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(address)) return;

            await QueryStateFeature(address);
        }

        private async Task QueryStateFeature(string feature)
        {
            try
            {
                string type = "";
                switch (choice.SelectedIndex)
                {
                    case -1:
                        return;
                    case 0:
                        type = "identifier";
                        break;
                    case 1:
                        type = "created_by";
                        break;
                    case 2:
                        type = "modified_by";
                        break;
                }
                await _featureTable.LoadAsync();  
                // Create a query parameters that will be used to query the feature table.
                var queryParams = new QueryParameters();

                // Construct and assign the where clause that will be used to query the feature table.
                queryParams.WhereClause = "lower("+type+") LIKE '%" + feature + "%'";

                // Query the feature table.
                var queryResult = await _featureTable.QueryFeaturesAsync(queryParams);

                if (queryResult.Any())
                {
                    // Loop over each feature from the query result.
                    foreach (Feature featur in queryResult)
                    {
                        string searched = featur.Attributes[type].ToString();
                        if (searched == feature)
                        {
                            _localFeatureLayer.SelectFeature(featur);
                            Viewpoint loco = new Viewpoint(featur.Geometry.Extent);
                            await MyMapView.SetViewpointAsync(loco);
                            break;
                        }
                    }   
                }
            }
            catch (Exception)
            {
                Console.WriteLine("query failed");
            }
        }

        private void clearMapPoint_Btn_Clicked(object sender, EventArgs e)
        {
            MyMapView.GraphicsOverlays.Clear();
        }

        private async void viewWindowBtn_Click_Event(object message)
        {
            MyMapView.DismissCallout();
            await Navigation.PushAsync(new BinPage(_feature));
        }

        /// <summary>
        /// Sets up logic to capture GeoViewTapped event and process a UI callout
        /// if the map point tapped is near a feature
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private async void MapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            var buffer = GeometryEngine.Buffer(e.Location, 10);

            var query = new QueryParameters();
            query.Geometry = buffer;
            query.SpatialRelationship = SpatialRelationship.Contains;
            // Should be = 1?
            query.MaxFeatures = 1;

            var selected = await _localFeatureLayer.SelectFeaturesAsync(query, Esri.ArcGISRuntime.Mapping.SelectionMode.New);

            if (selected.Any())
            {
                foreach(var feature in selected)
                {
                    _feature = (ArcGISFeature)feature;

                    await _feature.LoadAsync();

                    var calloutDefinition = new CalloutDefinition(_feature);
                    calloutDefinition.Text = (string)_feature.Attributes["identifier"];
                    calloutDefinition.MaxWidth = 5000;
                    calloutDefinition.Icon = null;
                    calloutDefinition.ButtonImage = new RuntimeImage(new Uri("https://cdn3.iconfinder.com/data/icons/block/32/box_edit-512.png"));

                    MyMapView.ShowCalloutForGeoElement(_feature, e.Position, calloutDefinition);
                    calloutDefinition.OnButtonClick = viewWindowBtn_Click_Event;
                }
            }
            else
            {
                MyMapView.DismissCallout();
            }
        }

        private void MapView_OnDrawStatusChanged(object sender, DrawStatusChangedEventArgs e)
        {
            if (e.Status == DrawStatus.InProgress)
            {
                GlobalSettings.UpdateViewpoint = MyMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale);
            }
        }
    }
}