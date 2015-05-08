using System;
namespace ZeitmessungArduino
{
    interface ISerialToArduino
    {
        bool ChangeComPort(string comPort);
        string ComPort();
        void CloseComPort();
        bool IsConnected { get; }
        event EventHandler<DriverLibaryArduinoZeitmessung.CustomEventArgs> RaiseCustomEvent;
        void SendCommand(string commandString = "TestCommand");

        void SendCommand();
    }
}
