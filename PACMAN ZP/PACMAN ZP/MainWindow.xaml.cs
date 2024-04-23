using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PACMAN_ZP
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {

        }
        Page1 page1 = new Page1();
        //Page2 page2 = new Page2();
  
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Obrázky/menu.jpg")));

        }
        private void page1_button_Click(object sender, RoutedEventArgs e)
            {
                MainFrame.Content = page1;
                Button1.Visibility = Visibility.Collapsed;
                //Button2.Visibility = Visibility.Collapsed;
                Pacman.Visibility = Visibility.Collapsed;
        }
        /*private void page2_button_Click(object sender, RoutedEventArgs e)
            {
                MainFrame.Content = page2;
                Button1.Visibility = Visibility.Collapsed;
                Button2.Visibility = Visibility.Collapsed;
                Pacman.Visibility = Visibility.Collapsed;
        }*/




    }
}
