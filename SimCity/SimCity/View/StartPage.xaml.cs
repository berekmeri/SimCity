using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimCity.View
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Window
    {
        private string _cityname="";
        public String GetCityName { get { return _cityname; } }
        public StartPage()
        {
            InitializeComponent();   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            ((App)Application.Current).OpenGameWindow();
            this.Close();
            
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).CloseGame();
            this.Close();
        }


    }
}
