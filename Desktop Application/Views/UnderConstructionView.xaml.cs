using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Desktop_Application.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnderConstructionView : Page
    {
        public UnderConstructionView()
        {
            this.InitializeComponent();

            Random random = new Random();

            int num = random.Next();

            if (num % 2 == 0)
            {
                Construction_Img.Source = new BitmapImage(new Uri("https://publicdomainvectors.org/photos/under-construction_geek_man_01.png"));
            }
            if (num % 9 == 0)
            {
                Construction_Img.Source = new BitmapImage(new Uri("https://media.tenor.com/EsReJqH9JkcAAAAC/dog-sad.gif"));
            }
            else
            {
                Construction_Img.Source = new BitmapImage(new Uri("https://publicdomainvectors.org/photos/under-construction-woman_at_work-o-f-daisy.png"));
            }
        }
    }
}
