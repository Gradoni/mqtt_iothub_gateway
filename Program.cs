using System;
using System.Threading.Tasks;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;
using Message;
using Microsoft.Azure.Devices.Client;

namespace mqtt_reader
{
    class Program
    {

        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Subscribed to the topic");
        }

        static void client_MqttMsgPublishRecieved(object sender, MqttMsgPublishEventArgs e)
        {
            string utfString = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
            Message.Message msg = JsonConvert.DeserializeObject<Message.Message>(utfString);
            Console.WriteLine(msg.TimestampGMT);
            Console.WriteLine(msg.Location);
            Console.WriteLine(msg.Speed);
            Console.WriteLine(msg.Freq);
        }

        static void Main(string[] args)
        {
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
