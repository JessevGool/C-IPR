using Avans.TI.BLE;
using ClientApplication.Incoming_messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApplication
{
    class StationaryBike
    {
        private IMessageObserver observer;
        private BikeSession bikeSession;
        public bool isConnected = false;
        private BLE currentConnectedBike;

        private string connectionNumber = string.Empty;
        public string ConnectionNumber { get => connectionNumber; set => connectionNumber = value; }

        public StationaryBike(IMessageObserver observer, BikeSession bikeSession)
        {
            this.observer = observer;
            this.bikeSession = bikeSession;
        }

        public void StartConnection()
        {
            Task.Run(() => this.Connect());
        }

        private async Task Connect()
        {

            if (isConnected)
            {
                currentConnectedBike.CloseDevice();
            }

            int errorCode = 0;
            BLE bleBike = new BLE();
            BLE bleHeart = new BLE();
            this.currentConnectedBike = bleBike;
            Thread.Sleep(1000); // We need some time to list available devices

            // List available devices
            List<String> bleBikeList = bleBike.ListDevices();
            this.observer.Log("Devices found: ");
            foreach (var name in bleBikeList)
            {
                if(name.ToLower().Contains("tacx flux"))
                {
                    this.observer.Log($"Device: {name}");
                }
            }

            // Connecting
            errorCode = errorCode = await bleBike.OpenDevice(string.Format("Tacx Flux {0}", connectionNumber));
            this.observer.Log(string.Format("Connecting with: Tacx Flux {0}", connectionNumber));
            if (errorCode == 1)
            {
                this.observer.Log("No connection");
            }

            var services = bleBike.GetServices;
            foreach (var service in services)
            {
                this.observer.Log($"Service: {service}");
            }

            // Set service
            errorCode = await bleBike.SetService("6e40fec1-b5a3-f393-e0a9-e50e24dcca9e");
            // __TODO__ error check

            // Subscribe
            bleBike.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            errorCode = await bleBike.SubscribeToCharacteristic("6e40fec2-b5a3-f393-e0a9-e50e24dcca9e");
            this.isConnected = true;

            // Heart rate
            errorCode = await bleHeart.OpenDevice("Decathlon Dual HR");

            await bleHeart.SetService("HeartRate");

            bleHeart.SubscriptionValueChanged += BleBike_SubscriptionValueChanged;
            await bleHeart.SubscribeToCharacteristic("HeartRateMeasurement");
        }

        private void BleBike_SubscriptionValueChanged(object sender, BLESubscriptionValueChangedEventArgs e)
        {
            Message message = null;

            if (e.Data.Length < 13)
            {
                message = new HearthDataMessage(e.Data, bikeSession);
            }
            else
            {
                switch (e.Data[4])
                {
                    case 16:
                        message = new GeneralDataMessage(e.Data, bikeSession);
                        break;
                    case 25:
                        message = new StationaryDataMessage(e.Data, bikeSession);
                        break;
                    default:
                        break;
                }
            }

            this.observer.ChangeValues(message);
        }
    }
}
