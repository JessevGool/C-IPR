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
            Sex.Items.Add("Man");
            Sex.Items.Add("Woman");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!IsDigitsOnly(Age.Text))
            {
                if (Int32.Parse(Age.Text) < 0 || Int32.Parse(Age.Text) > 100 || Age.Text == "")
                {
                    Age.Background = Brushes.Red;
                }   
            }
            else
            {
                Age.Background = Brushes.White;
            }
            if (!IsDigitsOnly(Weight.Text) || Weight.Text == "")
            {
                Weight.Background = Brushes.Red;
            }
            else
            {
                Weight.Background = Brushes.White;
            }
            if (Age.Text != "" && Weight.Text != "" && Sex.Text != "" && Name.Text != "") {
                if (IsDigitsOnly(Weight.Text) && IsDigitsOnly(Age.Text) && Int32.Parse(Age.Text) >= 0 && Int32.Parse(Age.Text) <= 100)
                {
                    this.Hide();
                    MainWindow mainWindow = new MainWindow(Name.Text, Int32.Parse(Age.Text), double.Parse(Weight.Text), Sex.Text);
                    mainWindow.Closed += (s, args) => this.Close();
                    mainWindow.Show();

                }
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

      
    }
}
