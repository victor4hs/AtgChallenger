using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using QuickFix;
using QuickFix.FIX44;

namespace OrderAccumulator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SessionSettings settings = new("./orderaccumulator.cfg");
                IApplication executorApp = new OrderAccumulator();
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
                ILogFactory logFactory = new FileLogFactory(settings);
                ThreadedSocketAcceptor acceptor = new(executorApp, storeFactory, settings, logFactory);
                acceptor.Start();
                Console.WriteLine("press <enter> to quit");
                Console.Read();

                acceptor.Stop();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("==FATAL ERROR==");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
