using JeniusApps.Common.Tools.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WindowsMediaPlayer player;

        public MainPage()
        {
            this.InitializeComponent();
            player = new WindowsMediaPlayer();
            player.SetUriSource(new Uri("ms-appx:///Assets/tone.wav"), true);
        }

        private void OnPlayClicked(object sender, RoutedEventArgs e)
        {
            player.Play();
        }

        private void OnPlayWithFadeClicked(object sender, RoutedEventArgs e)
        {
            player.Play(1, 3000);
        }

        private void OnPauseClicked(object sender, RoutedEventArgs e)
        {
            player.Pause();
        }
    }
}
