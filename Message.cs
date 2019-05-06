using System;

namespace Message_custom
{
    class Message_read 
    {
        public DateTime TimestampGMT;
        public string Location;
        public double Speed;
        public double Freq;

        public Message_read()
        {
            TimestampGMT = new DateTime();
            Location = "Junction3";
            Speed = 0D;
            Freq = 0D;
        }
        
    }
}