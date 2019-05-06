using System;
using System.Threading.Tasks;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;
using Message_custom;
using Microsoft.Azure.Devices.Client;

namespace mqtt_reader
{
    class Program
    {
        private static DeviceClient s_deviceClient;
        // connection string for created iot_hub device
        private readonly static string s_connectionString = "HostName=aune-iot-hub.azure-devices.net;DeviceId=mqtt_gateway;SharedAccessKey=ngpqe31NoLOmehO5g7NyTHxHgRbeHicZiU/RDMrK4Bc=";
        // event extension for MqttMsgSubscribed, used for logging subscription results
        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Subscribed to the topic");
        }
        // called every time new message from the broker is obtained, resends the message to iot_hub
        static void client_MqttMsgPublishRecieved(object sender, MqttMsgPublishEventArgs e)
        {
            string utfString = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            // Message_read msg = JsonConvert.DeserializeObject<Message_read>(utfString);
            SendDeviceToCloudMessagesAsync(utfString);           
        }

        private static async void SendDeviceToCloudMessagesAsync(string msg)
        {
            var message = new Message(Encoding.ASCII.GetBytes(msg));
            Console.WriteLine(msg); 
            await s_deviceClient.SendEventAsync(message);
            Console.WriteLine("Message sent to iot_hub");
        }

        static void Main(string[] args)
        {
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Amqp);
            MqttClient client = new MqttClient("iot.research.hamk.fi");
            client.Connect(Guid.NewGuid().ToString());
            string[] topic = {"hamk/ICT_R/Traffic/simulatedTraffic"};
            byte[] qosLevel = {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE};
            client.Subscribe(topic, qosLevel);
            client.MqttMsgPublishReceived += client_MqttMsgPublishRecieved;
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
        }


        
    }
}
