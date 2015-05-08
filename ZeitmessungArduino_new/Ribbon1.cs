using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using DriverLibaryArduinoZeitmessung;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace ZeitmessungArduino
{
    [ComVisible(true)]
    public interface IRibbon1
    {
        void Measure1();
        void Measure2();
        void Connect(string comPort = "");
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class Ribbon1 : IRibbon1
    {
        ISerialToArduino serialArduino;
        //SerialToArduinoRandom serialArduino;
        delegate void SetTextCallback(string text);
        delegate void SetDGVCallback(CustomEventArgs eventargs);
        int refreshTime = 100;
        static bool loopStopWatch = false;
        Stopwatch stopWatch = new Stopwatch();
        Thread loopThread;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            if (comboBox1.Text.Contains("TEST"))
            {
                serialArduino = new SerialToArduinoRandom();
                serialArduino.RaiseCustomEvent += serialArduino_RaiseCustomEvent;
                serialArduino.ChangeComPort(comboBox1.Text);
            }
            else if (comboBox1.Text.Contains("COM"))
            {
                serialArduino = new SerialToArduino();
                serialArduino.RaiseCustomEvent += serialArduino_RaiseCustomEvent;
                serialArduino.ChangeComPort(comboBox1.Text);
            }
        }
        ~Ribbon1()
        {
            loopStopWatch = false;
            if (loopThread != null && loopThread.ThreadState == System.Threading.ThreadState.Running)
            {
                loopThread.Join(refreshTime + 10);
                loopThread = null;
            }
            if (serialArduino != null)
            {
                try
                {
                    Thread closeComport = new Thread(this.serialArduino.CloseComPort);
                    closeComport.Start();
                    System.Threading.Thread.Sleep(100);
                    closeComport.Join();
                }
                catch (Exception exep)
                {

                }
            }
        }

        public void loopUntilStop()
        {
            while (loopStopWatch)
            {
                try
                {
                    System.Threading.Thread.Sleep(refreshTime);
                    Globals.ThisAddIn.Application.Cells[29, 11] = (decimal)stopWatch.ElapsedMilliseconds / 1000;
                }
                catch
                {

                }

            }
        }

        private void Button_Click(object sender, RibbonControlEventArgs e)
        {
            Worksheet worksheet = Globals.Factory.GetVstoObject(
                Globals.ThisAddIn.Application.ActiveWorkbook.Worksheets[1]);


            string buttonName = "MyButton";

            if (((RibbonCheckBox)sender).Checked)
            {
                Microsoft.Office.Interop.Excel.Range selection = Globals.ThisAddIn.Application.Selection as Microsoft.Office.Interop.Excel.Range;
                if (selection != null)
                {
                    //Microsoft.Office.Tools.Excel.Controls.Button button =
                    //    new Microsoft.Office.Tools.Excel.Controls.Button();
                    //worksheet.Controls.AddControl(button, selection, buttonName);
                    worksheet.Cells[5, 5] = 53.5;
                }
            }
            else
            {
                worksheet.Controls.Remove(buttonName);
            }
        }

        private void comboBox1_TextChanged(object sender, RibbonControlEventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, RibbonControlEventArgs e)
        {
            Connect(comboBox1.Text);
        }
        public void Connect(string comPort = "")
        {

            Excel.Worksheet activeWorksheet = Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
            //string comPort = "";
            if (activeWorksheet != null)
            {
                if (!comPort.Contains("COM") && !comPort.Contains("TEST"))
                {
                    Excel.Range range1 = activeWorksheet.get_Range("B52", System.Type.Missing);
                    comPort = range1.Value2;
                    if (comPort == null) comPort = "";
                }
                Excel.Range range2 = activeWorksheet.get_Range("B55", System.Type.Missing);
                if (range2.Value2 != null)
                {
                    int.TryParse(range2.Value2.ToString(), out refreshTime);
                    if (refreshTime == 0) refreshTime = 100;
                }

            }
            if (serialArduino != null)
            {
                
                serialArduino.CloseComPort();
            }
            if (loopThread != null && loopThread.ThreadState == System.Threading.ThreadState.Running)
            {
                loopThread.Join();
                loopThread.Abort();
                loopThread = null;
                
            }
            loopThread = new Thread(this.loopUntilStop);
            serialArduino = null;
            if (comPort.Contains("TEST"))
            {
                serialArduino = new SerialToArduinoRandom();
            }
            else
            {
                serialArduino = new SerialToArduino();
            }
            serialArduino.RaiseCustomEvent += serialArduino_RaiseCustomEvent;
            
            if (serialArduino.IsConnected)
            {
                Globals.ThisAddIn.Application.Cells[53, 2] = "Connected";
                Globals.ThisAddIn.Application.Cells[52, 2] = serialArduino.ComPort();
                Globals.ThisAddIn.Application.Cells[55, 2] = refreshTime;
            }
            else
            {
                Globals.ThisAddIn.Application.Cells[53, 2] = "Not Connected";
                Globals.ThisAddIn.Application.Cells[52, 2] = serialArduino.ComPort();
                Globals.ThisAddIn.Application.Cells[55, 2] = refreshTime;
            }

        }

        private void serialArduino_RaiseCustomEvent(object sender, CustomEventArgs e)
        {
            if (e.Command.Contains("timestart1") || e.Command.Contains("timestart2"))
            {
                stopWatch.Restart();
                loopStopWatch = true;
                loopThread = null;
                if (loopThread == null)
                {
                    loopThread = new Thread(this.loopUntilStop);
                }
                loopThread.Start();

            }
            else if (e.Command == "timeend2" || e.Command == "timeend1")
            {
                stopWatch.Stop();
                loopStopWatch = false;
                System.Threading.Thread.Sleep(20);
                loopThread.Join(refreshTime + 10);
                loopThread = null;
                this.SetText(e);
                this.RunMacroFromWorkbook(e);
            }
            else
            {
                this.SetText(e);
            }
        }


        private void SetText(CustomEventArgs e)
        {
            Globals.ThisAddIn.Application.Cells[29, 11] = e.Time;
        }
        private void RunMacroFromWorkbook(CustomEventArgs e)
        {
            string macroName = (Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet).get_Range("B54", System.Type.Missing).Value2;
            if (macroName != null && macroName != "")
            {
                try
                {
                    Globals.ThisAddIn.Application.Run(macroName);
                }
                catch
                {

                }
            }
            
        }

        public void buttonMeasure1_Click(object sender, RibbonControlEventArgs e)
        {
            Measure1();
        }
        public void Measure1()
        {
            if (serialArduino!= null && serialArduino.IsConnected)
            {
                if (loopThread != null && loopThread.ThreadState != System.Threading.ThreadState.Unstarted)
                {

                    if (loopStopWatch == true) loopStopWatch = false;
                    System.Threading.Thread.Sleep(20);
                    loopThread.Join(refreshTime + 10);
                    loopThread = null;
                }
                CustomEventArgs bla = new CustomEventArgs();
                bla.Time = 0;
                SetText(bla);
                serialArduino.SendCommand("time1");
            }
            else
            {
                MessageBox.Show("Not connected to time measuring device", "Not Connected");
            }
        }

        public void Measure2()
        {
            if (serialArduino != null && serialArduino.IsConnected)
            {
                if (loopThread != null && loopThread.ThreadState != System.Threading.ThreadState.Unstarted)
                {
                    loopStopWatch = false;
                    System.Threading.Thread.Sleep(20);
                    loopThread.Join();
                    loopThread = null;
                }
                CustomEventArgs bla = new CustomEventArgs();
                bla.Time = 0;
                SetText(bla);
                serialArduino.SendCommand("time2");
            }
            else
            {
                MessageBox.Show("Not connected to time measuring device", "Not Connected");
            }
        }

        private void buttonMeasure2_Click(object sender, RibbonControlEventArgs e)
        {
            Measure2();
        }


    }
}
