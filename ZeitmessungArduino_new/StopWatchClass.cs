using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ZeitmessungArduino
{
    class StopWatchClass
    {


        bool loopStopWatch = false;
        Stopwatch stopWatch = new Stopwatch();

        public void loopUntilStop()
        {
            while (loopStopWatch)
            {
                System.Threading.Thread.Sleep(200);
                Globals.ThisAddIn.Application.Cells[29, 11] = (decimal)stopWatch.ElapsedMilliseconds / 1000;
            }
        }
        public void Start()
        {
            stopWatch.Restart();
            loopStopWatch = true;
            loopUntilStop();
        }

        public void Stop()
        {
            stopWatch.Stop();
            loopStopWatch = false;
        }

    }
}
