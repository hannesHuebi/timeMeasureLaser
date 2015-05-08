using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace DriverLibaryArduinoZeitmessung
{
    public class SerialToArduino : ZeitmessungArduino.ISerialToArduino
    {
        SerialPort mySerialPort;

        public event EventHandler<CustomEventArgs> RaiseCustomEvent;

        public SerialToArduino()
        {
            string comPort = "COM3";
            try
            {
                mySerialPort = new SerialPort(comPort);
                mySerialPort.BaudRate = 9600;
                mySerialPort.Parity = Parity.None;
                mySerialPort.DataBits = 8;
                mySerialPort.Handshake = Handshake.None;
                mySerialPort.NewLine = "\n";
                mySerialPort.ReadTimeout = 1000;
                mySerialPort.WriteTimeout = 1000;
                

                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                mySerialPort.Open();

            }
            catch (Exception exep)
            {

            }
        }
        ~SerialToArduino()
        {
            try
            {
                if (!mySerialPort.IsOpen) mySerialPort.Open();
                Thread sendLine = new Thread(this.SendCommand);
                sendLine.Start();
                System.Threading.Thread.Sleep(100);
                sendLine.Join();
                if (mySerialPort != null && mySerialPort.IsOpen)
                {
                    mySerialPort.ReadExisting();
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();
                    Thread closeComport = new Thread(this.CloseComPort);
                    closeComport.Start();
                    System.Threading.Thread.Sleep(100);
                    closeComport.Join();
                }
            }
            catch (Exception exep)
            {

            }
        }
        public string ComPort()
        {
            try
            {
                if (mySerialPort.IsOpen)
                {
                    return mySerialPort.PortName;
                }
                else
                {
                    return "";
                }

            }
            catch
            {
                return "";
            }
            
        }
        public bool ChangeComPort(string comPort)
        {
            try
            {
                if (mySerialPort.IsOpen)
                {
                    mySerialPort.WriteLine("blabla");
                    mySerialPort.ReadExisting();
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();

                    mySerialPort.Close();
                }

            }
            catch
            {
            }
            try
            {
                if (!mySerialPort.IsOpen)
                {
                    mySerialPort.PortName = comPort;
                    mySerialPort.Open();
                }

            }
            catch
            {

            }
            if (mySerialPort.IsOpen && mySerialPort.PortName == comPort)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        public void CloseComPort()
        {
            if (mySerialPort.IsOpen)
            {
                mySerialPort.WriteLine("blabla");
                Thread.Sleep(100);
                mySerialPort.ReadExisting();
                mySerialPort.DiscardInBuffer();
                mySerialPort.DiscardOutBuffer();

                mySerialPort.Close();
            }
            else
            {
                try
                {
                    mySerialPort.Close();
                }
                catch
                {

                }
            }
        }

        public void SendCommand(string commandString = "TestCommand")
        {
            mySerialPort.WriteLine(commandString);
        }
        public void SendCommand()
        {
            mySerialPort.WriteLine("TestCommand");
        }
        public bool IsConnected
        {
            get
            {
                if (mySerialPort != null)
                {
                    return mySerialPort.IsOpen;
                }
                else
                {
                    return false;
                }
            }
        }


        private void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            sp.ReadExisting();
            

            string[] stringArray = indata.Split(':');

            CustomEventArgs eventArgs = new CustomEventArgs();
            eventArgs.Command = stringArray[0];


            if (stringArray.Count() > 1)
            {
                decimal usedTime;
                if (decimal.TryParse(stringArray[1], out usedTime))
                {
                    eventArgs.Time = usedTime / 1000;
                }
                else
                {
                    eventArgs.Time = -1;
                }
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
    public class CustomEventArgs : EventArgs
    {
        
        private string command;
        private decimal time;

        public string Command
        {
            get { return command; }
            set { command = value; }
        }
        public decimal Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}

