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
using System.Diagnostics;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.IO;

using ServerApp;

using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Net;

namespace Clientdisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageObserver
    {

        private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private string _prefix = "client##";

        private BikeSession bikeSession;
        private bool simulationRunning;
        private StationaryBike stationaryBike;
        private int age;
        private double weight;
        private string sex;
        private string name;
        private double voltage;
        public string time;

        public List<double> BPM { get; set; } = new List<double>();
        public List<double> Speed { get; set; } = new List<double>();
        public List<double> RPM { get; set; } = new List<double>();

        public long HeartRate;
        public int spin;
        public int speedcycle;
        public List<double> Watt { get; set; } = new List<double>();
        private ChartValues<ObservableValue> ChartMetersTravelled { get; set; }
        private ChartValues<ObservableValue> ChartSpeedValues { get; set; }
       private ChartValues<ObservableValue> ChartSpeedValues { get; set; }
        private ChartValues<ObservableValue> ChartRPM { get; set; }
        private ChartValues<ObservableValue> ChartBeat { get; set; }

        private Stopwatch AstrandWatch = new Stopwatch();
        private DispatcherTimer AstrandTimer = new DispatcherTimer();
        private DispatcherTimer DataTimer = new DispatcherTimer();
        string currentTime = string.Empty;
        string timestart;

        public MainWindow(int age, double weight, string sex, string name)
        {
            InitializeComponent();
            LoopConnect();

            SendFilebtn.IsEnabled = false;
            this.age = age;
            this.weight = weight;
            this.sex = sex;
            this.name = name;
            this.HeartRate = 0;
            this.spin = 0;
            AstrandTimer.Tick += new EventHandler(dt_Tick);
            DataTimer.Tick += new EventHandler(dt_tick2);
            AstrandTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            DataTimer.Interval = new TimeSpan(0, 0, 1);
            CurrentlyConnectedLabel.Content = "Not connected to a bike";
            string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            bycicleBox.Items.Add("01140");
            bycicleBox.Items.Add("00457");
            bycicleBox.Items.Add("24517");
            bycicleBox.Items.Add("00438");
            bycicleBox.Items.Add("00472");

            //StationaryBike stationaryBike = new StationaryBike();
            bikeSession = new BikeSession();
            stationaryBike = new StationaryBike(this, bikeSession);
            simulationRunning = false;

            CartesianChart ch = new CartesianChart();

            ChartSpeedValues = new ChartValues<ObservableValue>();
            ChartRPM = new ChartValues<ObservableValue>();
            ChartBeat =new ChartValues<ObservableValue>();

            ch.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Snelheid",
                    Values = ChartSpeedValues
                },
                new LineSeries
                {
                    Title = "RPM",
                    Values = ChartRPM
                },
                new LineSeries
                {
                    Title = "Heartbeat",
                    Values = ChartBeat
                }




            };
            chartGrid.Children.Add(ch);



        }
        private void ReceiveMessage()
        {

            Thread receivemessages = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        byte[] receivedBuffer = new byte[1024];
                        int rec = _clientSocket.Receive(receivedBuffer);
                        byte[] data = new byte[rec];

                        Array.Copy(receivedBuffer, data, rec);
                        string serverresponse = Encoding.ASCII.GetString(data);
                        string[] prefixwithmessage = serverresponse.Split(new[] { "//" }, StringSplitOptions.None);
                        if (prefixwithmessage[0] == "message")
                        {
                            incomingmessages.Content = $"Received: {prefixwithmessage[1]}";
                        }
                        else if (prefixwithmessage[1] == "file")
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        incomingmessages.Content = e.Message;
                        Console.WriteLine(e.Message);
                    }
                }
            });
            receivemessages.Start();

        }
        private void LoopConnect()
        {
            int attempts = 0;
            while (!_clientSocket.Connected)
            {


                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException e)
                {
                    ConnectionServerlbl.Content = $"Connection attempts: {attempts}";

                }
            }

            ConnectionServerlbl.Content = "Connected";

            string connected = "client##connected";
            byte[] buffer = Encoding.ASCII.GetBytes(connected);
            _clientSocket.Send(buffer);

        }
        void dt_tick2(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            byte[] buffer = Encoding.ASCII.GetBytes("client##" + new MeasurementData(timestart, time, this.name, this.sex, this.age, this.weight, this.spin, this.speedcycle, this.HeartRate, this.voltage).DataTostring());
            _clientSocket.Send(buffer);
        }
        void dt_Tick(object sender, EventArgs e)
        {
            if (AstrandWatch.IsRunning)
            {
                TimeSpan ts = AstrandWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}",
                ts.Minutes, ts.Seconds);
                timelbl.Content = $"Session Time: {currentTime} ";


                int elapsedTime = (int)AstrandWatch.Elapsed.TotalSeconds;
                if (elapsedTime < 120)
                {
                    Statuslbl.Content = $"Warming up: {120 - elapsedTime}";
                }
                else if (elapsedTime > 120 && elapsedTime < 360)
                {
                    if (this.spin < 50)
                    {
                        Statuslbl.Content = $"You have to go Faster!!";
                    }
                    else if (this.spin > 60)
                    {
                        Statuslbl.Content = $"Slowdown!!";
                    }
                    else
                    {
                        Statuslbl.Content = $"Your doing great!";
                    }
                }
                else if (elapsedTime > 360 && elapsedTime < 420)
                {
                    MeasurementData data = new MeasurementData(timestart, time, this.name, this.sex, this.age, this.weight, this.spin, this.speedcycle, this.HeartRate, this.voltage);
                    if (this.sex.Equals("man"))
                    {
                        Statuslbl.Content = "Your vo2 score is: " + data.maleVo2();
                    }
                    else if (this.sex.Equals("women"))
                    {
                        Statuslbl.Content = "Your vo2 score is: " + data.femaleVo2();
                    }

                    SendFilebtn.IsEnabled = true;
                    writeLog(new MeasurementData(timestart, time, this.name, this.sex, this.age, this.weight, this.spin, this.speedcycle, this.HeartRate, this.voltage));
                    Statuslbl.Content = $"Cool down: {420 - elapsedTime}";
                }
                else
                {
                    Statuslbl.Content = "You can stop now";
                }
            }
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
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                switch (message)
                {
                    case GeneralDataMessage generalMessage:
                        lblSpeed.Content = string.Format("Snelheid: {0} km/u", bikeSession.Speed.ToString());
                        Speed.Add(bikeSession.Speed);
                        speedcycle = (int)bikeSession.Speed;
                        RPM.Add(bikeSession.CycleRPM);
                        spin = (int)bikeSession.CycleRPM;
                        lblDistance.Content = string.Format("Afstand afgelegd: {0} meter", bikeSession.GetMetersTravelled().ToString());
                        lblRPM.Content = string.Format("RPM: {0}", bikeSession.CycleRPM);  
                        
                        ChartSpeedValues.Add(new ObservableValue(bikeSession.GetSpeed()));
                        ChartRPM.Add(new ObservableValue(bikeSession.CycleRPM));
                    /*    if (ChartSpeedValues.Count == 30)
                        {
                            ChartSpeedValues.RemoveAt(0);
                        }
                        else if(ChartRPM.Count == 30)
                        {
                            ChartRPM.RemoveAt(0);
                        }*/
                        break;
                    case StationaryDataMessage stationaryMessage:
                        lblVoltage.Content = string.Format("Voltage: {0} Watt", bikeSession.Voltage.ToString());
                        this.voltage = bikeSession.Voltage;
                        break;
                    case HearthDataMessage hearthDataMessage:
                        lblHearthRate.Content = string.Format("Hartslag {0} bpm", bikeSession.GetHearthBeats().ToString());
                        BPM.Add(bikeSession.GetHearthBeats());
                        HeartRate = bikeSession.GetHearthBeats();
                        ChartBeat.Add(new ObservableValue(bikeSession.GetHearthBeats()));
                       /* if(ChartBeat.Count == 30)
                        {
                            ChartBeat.RemoveAt(0);
                        }*/
                        break;
                }

            }));
        }

        public void Log(string message)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
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

        private void Startbtn_Click(object sender, RoutedEventArgs e)
        {

            AstrandWatch.Start();
            AstrandTimer.Start();
            DataTimer.Start();
            timestart = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            SendFilebtn.IsEnabled = false;
        }


        private void Stopbtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (AstrandWatch.IsRunning)
            {
                AstrandWatch.Stop();
                AstrandWatch.Reset();
            }
            DataTimer.Stop();
            SendFilebtn.IsEnabled = true;
            TimeSpan ts = AstrandWatch.Elapsed;
            currentTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            timelbl.Content = $"Session Time: {currentTime}";
            int elapsedTime = (int)AstrandWatch.Elapsed.TotalSeconds;
            Statuslbl.Content = $"Warming up: {120 - elapsedTime}";
            writeLog(new MeasurementData(timestart, time, this.name, this.sex, this.age, this.weight, this.spin, this.speedcycle, this.HeartRate, this.voltage));
        }

        private void writeLog(MeasurementData md)

        {

            Writer writer = new Writer();

            writer.clearFile();

            JObject o = (JObject)JToken.FromObject(md);

            writer.writeData(o);



        }



        private void Button_Click(object sender, RoutedEventArgs e)

        {

            writeLog(new MeasurementData(timestart, time, this.name, this.sex, this.age, this.weight, this.spin, this.speedcycle, this.HeartRate, this.voltage));

        }

        private void SendFilebtn_Click(object sender, RoutedEventArgs e)
        {
            incomingmessages.Content = "JOe";
            string realfilename = $"{getPath()}Data/Data.txt";
            string filetext = "client##" + System.IO.File.ReadAllText(realfilename);
            byte[] buffer = Encoding.ASCII.GetBytes(filetext);
            _clientSocket.Send(buffer);
        }
    }

    public class MeasurementData

    {
        public string timestart;
        public string time;
        public string name;
        public List<double> Voltages;

        public string gender;

        public int age;

        public double weight;

        public int rpm;

        public int speed;

        public long bpm;

        public double vo2;

        public MeasurementData(string timestart, string time, string name, string gender, int age, double weight, int rpm, int speed, long bpm, double voltage)
        {
            this.timestart = timestart;
            this.time = time;
            this.name = name;
            this.vo2 = -1;
            this.gender = gender;

            this.age = age;

            this.weight = weight;

            this.rpm = rpm;

            this.speed = speed;

            this.bpm = bpm;
            Voltages.Add(voltage);

        }

        public JObject DataTostring()
        {
            JObject o = (JObject)JToken.FromObject(this);
            return o;
        }

        public double femaleVo2()
        {
            double workload = 0;
            foreach (double voltage in Voltages)
            {
                workload += voltage;
            }
            workload = workload / Voltages.Count();
            workload = workload * 6.12;
            return ((0.00193 * workload + 0.326) / (0.769 * this.bpm - 56.1) * 100) * correction(this.age);
        }

        public double maleVo2()
        {
            double workload = 0;
            foreach (double voltage in Voltages)
            {
                workload += voltage;
            }
            workload = workload / Voltages.Count();
            workload = workload * 6.12;
            return ((0.00212 * workload + 0.299) / (0.769 * this.bpm - 48.5) * 100) * correction(this.age);
        }

        private double correction(int age)
        {
            if (age <= 15)
            {
                return 1.1 + (15 - age) * 0.01;
            }
            else if (age <= 25)
            {
                return 1.0 + (25 - age) * 0.01;
            }
            else if (age <= 35)
            {
                return 0.87 + (35 - age) * (0.013);
            }
            else if (age <= 40)
            {
                return 0.83 + (40 - age) * 0.008;
            }
            else if (age <= 45)
            {
                return 0.78 + (45 - age) * 0.01;
            }
            else if (age <= 50)
            {
                return 0.75 + (50 - age) * 0.006;
            }
            else if (age <= 55)
            {
                return 0.71 + (55 - age) * 0.008;
            }
            else if (age <= 60)
            {
                return 0.68 + (60 - age) * 0.006;
            }
            else if (age <= 65)
            {
                return 0.65 + (65 - age) * 0.006;
            }
            else
            {
                return 0.65 - (age - 65) * 0.006;
            }
        }

    }
}
