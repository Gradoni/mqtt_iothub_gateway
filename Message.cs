using System;

namespace Message
{
    class Message 
    {
        public DateTime TimestampGMT;
        public string Location;
        public double Speed;
        public double Freq;

        public Message()
        {
            TimestampGMT = new DateTime();
            Location = "Junction3";
            Speed = 0D;
            Freq = 0D;
        }
        
    }
}