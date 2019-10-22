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
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Clientdisplay
{
    /// <summary>
    /// Interaction logic for StartupScreen.xaml
    /// </summary>
    public partial class StartupScreen : Window
    {
        public StartupScreen()
        {
            InitializeComponent();
            List<string> genders = new List<string>();
            genders.Add("man");
            genders.Add("women");
            Sex.ItemsSource = genders;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDigitsOnly(Age.Text))
            {
                Age.Background = Brushes.Red;
            }
            else
            {
                Age.Background = Brushes.White;
            }
            if (Int32.Parse(Age.Text) < 0 || Int32.Parse(Age.Text) > 100)
            {
                Age.Background = Brushes.Red;
            }
            else
            {
                Age.Background = Brushes.White;
            }
            if (!IsDigitsOnly(Weight.Text))
            {
                Weight.Background = Brushes.Red;
            }
            else
            {
                Weight.Background = Brushes.White;
            }
            if (!(Sex.Text.ToLower().Equals("man") || Sex.Text.ToLower().Equals("women")))
            {
                Sex.Background = Brushes.Red;
            }
            else
            {
                Sex.Background = Brushes.White;
            }
            if ((Sex.Text.ToLower().Equals("man") || Sex.Text.ToLower().Equals("women")) && IsDigitsOnly(Weight.Text) && IsDigitsOnly(Age.Text) && Int32.Parse(Age.Text) >= 0 && Int32.Parse(Age.Text) <= 100)
                if ((Sex.Text.ToLower().Equals("man") || Sex.Text.ToLower().Equals("women")) && IsDigitsOnly(Weight.Text) && IsDigitsOnly(Age.Text) && Name.Text != null)
                {
                    this.Hide();
                    MainWindow mainWindow = new MainWindow(Int32.Parse(Age.Text), double.Parse(Weight.Text), Sex.Text, Name.Text);
                    mainWindow.Closed += (s, args) => this.Close();
                    mainWindow.Show();

                }
        }

        static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
