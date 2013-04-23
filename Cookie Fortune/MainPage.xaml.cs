using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;

namespace Cookie_Fortune
{
    public sealed partial class MainPage : Page
    {
        private List<Control> _layoutAwareControls;

        Random rnd = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));

        List<string> fortunes = new List<string>();

        StringBuilder output = new StringBuilder();

        bool isFirstTap = true;
        
        public MainPage()
        {
            this.InitializeComponent();
            Grid.Tapped += new TappedEventHandler(Grid_Tapped);
            Loaded += MainPage_Loaded;

        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterForShare();

            var control = sender as Control;

            if (control == null) return;

            // Set the initial visual state of the control

            VisualStateManager.GoToState(control, ApplicationView.Value.ToString(), false);

            if (this._layoutAwareControls == null)
            {

                this._layoutAwareControls = new List<Control>();

            }

            this._layoutAwareControls.Add(control);

            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void RegisterForShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.ShareTextHandler);
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Cookie Fortune";
            request.Data.Properties.Description = "Share your fortune with your friends.";
            request.Data.SetText(FortuneDisplay.Text);
        }


        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            FortuneDisplay.Visibility = Visibility.Collapsed;

            string visualState = ApplicationView.Value.ToString();

            if (this._layoutAwareControls != null)
            {
                foreach (var layoutAwareControl in this._layoutAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string fortuneString = null;

            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create("Assets//Fortunes.xml"))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                     
                        case XmlNodeType.Text:
                            fortuneString = reader.Value;
                            fortunes.Add(fortuneString);
                            break;
                       
                    }
                }   
            }
        }

        private void Grid_Tapped(object sender, RoutedEventArgs e)
        {
            if (isFirstTap)
            {
                if (ApplicationView.Value != ApplicationViewState.Snapped)
                {
                    FortuneDisplay.Text = fortunes[rnd.Next(0, fortunes.Count)];
                    FortuneDisplay.Visibility = Visibility.Visible;
                    CookieWhole.Visibility = Visibility.Collapsed;
                    CookieBrokenLeft.Visibility = Visibility.Visible;
                    CookieBrokenRight.Visibility = Visibility.Visible;
                    isFirstTap = false;
                }
            }            
            else
            {
                FortuneDisplay.Visibility = Visibility.Collapsed;
                CookieWhole.Visibility = Visibility.Visible;
                CookieBrokenLeft.Visibility = Visibility.Collapsed;
                CookieBrokenRight.Visibility = Visibility.Collapsed;
                isFirstTap = true;
            }
        }

        
    }
}
