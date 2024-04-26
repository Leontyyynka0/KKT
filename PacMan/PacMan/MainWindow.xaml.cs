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

//TODO: :(((  VYPNUTÍ OSTATNÍCH OKEN (seká se mi aplikace), KOLIZE, POWERUPY, nejlepší čas ukládat do souboru

namespace PacMan
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {

        }
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Obrázky/menu.jpg")));

        }
        private void window1_button_Click(object sender, RoutedEventArgs e)
        {
            Window1 popup = new Window1();
            popup.ShowDialog();
        }
        private void window2_button_Click(object sender, RoutedEventArgs e)
        {
            Window2 popup = new Window2();
            popup.ShowDialog();
        }




    }
}