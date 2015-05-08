using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace DriverLibaryArduinoZeitmessung
{
    public class SerialToArduinoRandom : ZeitmessungArduino.ISerialToArduino
    {

        public event EventHandler<CustomEventArgs> RaiseCustomEvent;
        private bool sending = false;
        Random randomNumber = new Random();
        private int timeToEnd = 0;
        private string privateCommandString;
        public bool IsConnected{get; set;}
        

        public SerialToArduinoRandom()
        {
            string comPort = "TEST";
            try
            {
                IsConnected = true;
                //mySerialPort = new SerialPort(comPort);
                //mySerialPort.BaudRate = 9600;
                //mySerialPort.Parity = Parity.None;
                //mySerialPort.DataBits = 8;
                //mySerialPort.Handshake = Handshake.None;
                //mySerialPort.NewLine = "\n";

                //mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                //mySerialPort.Open();

            }
            catch (Exception exep)
            {

            }
        }
        ~SerialToArduinoRandom()
        {
            try
            {
                //mySerialPort.Close();
            }
            catch (Exception exep)
            {

            }
        }

        public string ComPort()
        {
            return "TEST";
        }

        public bool ChangeComPort(string comPort)
        {
            if (comPort == "TEST")
            {
                IsConnected = true;
                return true;
            }
            else
            {
                IsConnected = false;
                return false;
            }
        }
        public void CloseComPort()
        {
            
        }

        public void SendCommand(string commandString)
        {
            privateCommandString = commandString;
            sending = true;
            timeToEnd = randomNumber.Next(6000);
            System.Threading.Thread.Sleep(timeToEnd);
            timeToEnd = randomNumber.Next(10000);
            System.Timers.Timer timerToEnd = new System.Timers.Timer(timeToEnd);
            timerToEnd.AutoReset = false;
            timerToEnd.Interval = timeToEnd;
            timerToEnd.Elapsed += timerToEnd_Elapsed;
            timerToEnd.Start();
            int counter = 0;
            while (sending)
            {

                if (privateCommandString == "time1")
                {
                    DataReceivedHandler("time1:" + (counter * 200).ToString());
                }
                else if (privateCommandString == "time2")
                {
                    DataReceivedHandler("time2:" + (counter * 200).ToString());
                }
                System.Threading.Thread.Sleep(200);
                counter++;
            }


        }
        public void SendCommand()
        {
            return;
        }

        void timerToEnd_Elapsed(object sender, ElapsedEventArgs e)
        {
            sending = false;
            if (privateCommandString == "time1")
            {
                DataReceivedHandler("timeend1:" + timeToEnd.ToString());
            }
            else if (privateCommandString == "time2")
            {
                DataReceivedHandler("timeend2:" + timeToEnd.ToString());
            }
        }


        private void DataReceivedHandler(
                    string indata)
        {
            //SerialPort sp = (SerialPort)sender;
            //string indata = sp.ReadLine();
            //sp.ReadExisting();
            

            string[] stringArray = indata.Split(':');

            CustomEventArgs eventArgs = new CustomEventArgs();
            eventArgs.Command = stringArray[0];

            decimal usedTime;
            if (decimal.TryParse(stringArray[1], out usedTime))
            {
                eventArgs.Time = usedTime / 1000;
            }
            else
            {
                eventArgs.Time = -1;
            }
            OnRaiseCustomEvent(eventArgs);
        }

        protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = RaiseCustomEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                //e.Message += e;

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }






    }
    //public class CustomEventArgs : EventArgs
    //{
        
    //    private string command;
    //    private decimal time;

    //    public string Command
    //    {
    //        get { return command; }
    //        set { command = value; }
    //    }
    //    public decimal Time
    //    {
    //        get { return time; }
    //        set { time = value; }
    //    }
    //}
}


