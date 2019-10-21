using Newtonsoft.Json;
using Clientdisplay.Incoming_messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Clientdisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageObserver
    {
        private BikeSession bikeSession;
        private bool simulationRunning;
        private StationaryBike stationaryBike;

        public MainWindow(int age , double weight, string sex)
        {
            InitializeComponent();

            CurrentlyConnectedLabel.Content = "Not connected to a bike";

            bycicleBox.Items.Add("01140");
            bycicleBox.Items.Add("00457");
            bycicleBox.Items.Add("24517");
            bycicleBox.Items.Add("00438");

            //StationaryBike stationaryBike = new StationaryBike();
            bikeSession = new BikeSession();
            stationaryBike = new StationaryBike(this, bikeSession);
            simulationRunning = false;


        }
        public static string getPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Startsplit = startupPath.Substring(0, startupPath.LastIndexOf("bin"));
            string split = Startsplit.Replace(@"\", "/");
            return split;
        }

        public void ChangeValues(Message message)
        {
            if (message == null)
                return;

            //Allow other threads to work on the UI thread.
            Application.Current.Dispatcher.Invoke(new Action(() => {
                switch (message)
                {
                    case GeneralDataMessage generalMessage:
                        lblSpeed.Content = string.Format("Snelheid: {0} km/u", bikeSession.Speed.ToString());
                        lblDistance.Content = string.Format("Afstand afgelegd: {0} meter", bikeSession.MetersTravelled.ToString());
                        lblTime.Content = string.Format("Tijd sinds begin: {0} seconden", bikeSession.GetTimeSinceStart().ToString());
                        break;
                    case StationaryDataMessage stationaryMessage:
                        lblVoltage.Content = string.Format("Voltage: {0} Watt", bikeSession.Voltage.ToString());
                        break;
                    case HearthDataMessage hearthDataMessage:
                        lblHearthRate.Content = string.Format("Hartslag {0} bpm", bikeSession.HearthBeats.ToString());
                        break;
                }
            }));
        }

        public void Log(string message)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                log.Content += message + "\n";
            }));
        }

        private void BtnSimulate_Click(object sender, RoutedEventArgs e)
        {
            simulationRunning = !simulationRunning;
            String simulatedData = System.IO.File.ReadAllText(getPath() + "Resources/simulatie.txt");
            List<byte[]> bytes = new List<byte[]>();
            string[] stringBytes = simulatedData.Split('#');

            foreach (string stringBytesLine in stringBytes)
            {
                byte[] tempBytes = new byte[13];
                string[] tempAllBytes = stringBytesLine.Split('-');
                int counter = 0;

                if (tempAllBytes.Length == 13)
                {
                    foreach (string stringByte in tempAllBytes)
                    {
                        tempBytes[counter] = byte.Parse(stringByte);
                        counter++;
                    }

                    bytes.Add(tempBytes);
                }
            }

            Thread t = new Thread(() =>
            {
                int counter = 0;
                while (simulationRunning)
                {

                    if (counter == bytes.Count)
                    {
                        simulationRunning = false;
                        return;
                    }

                    byte[] data = bytes[counter];
                    Message message = null;

                    switch (data[4])
                    {
                        case 16:
                            message = new GeneralDataMessage(data, bikeSession);
                            break;
                        case 25:
                            message = new StationaryDataMessage(data, bikeSession);
                            break;
                        default:
                            break;
                    }

                    ChangeValues(message);

                    counter++;
                    Thread.Sleep(250);
                }
                return;
            });

            if (simulationRunning)
            {
                t.Start();
                btnSimulate.Content = "Stop simulatie";
            }
            else
            {
                btnSimulate.Content = "Start simulatie";
            }

        }



        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            stationaryBike.StartConnection();
            if (bycicleBox.SelectedItem == null)
            {
                this.stationaryBike.ConnectionNumber = "01140";
                CurrentlyConnectedLabel.Content = "Using default: 01140";
            }
            else
            {
                CurrentlyConnectedLabel.Content = string.Format("Using: {0}", bycicleBox.SelectedItem.ToString());
            }
        }

        private void BycicleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.stationaryBike.ConnectionNumber = bycicleBox.SelectedItem.ToString();
        }
    }
}
