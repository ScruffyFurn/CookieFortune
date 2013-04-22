using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Cookie_Fortune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Random rnd = new Random((int)(DateTime.Now.Ticks & 0x0000FFFF));

        List<string> fortunes = new List<string>();

        StringBuilder output = new StringBuilder();

        bool isFirstTap = true;

        String xmlString =
            @"<List>

            <fortune>Fortune goes here!</fortune>
            <fortune>Fortune goes here2!</fortune>
            <fortune>Fortune goes here3!</fortune>
            <fortune>Fortune goes here4!</fortune>
            <fortune>Fortune goes here5!</fortune>
            <fortune>Fortune goes here6!</fortune>

            </List>";
        
        public MainPage()
        {
            this.InitializeComponent();
            Grid.Tapped += new TappedEventHandler(Grid_Tapped);
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
                FortuneDisplay.Text = fortunes[rnd.Next(0, fortunes.Count)];
                FortuneDisplay.Visibility = Visibility.Visible;
                CookieWhole.Visibility = Visibility.Collapsed;
                CookieBrokenLeft.Visibility = Visibility.Visible;
                CookieBrokenRight.Visibility = Visibility.Visible;
                isFirstTap = false;
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
