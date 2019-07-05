using System;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessProcess
{
    public class BusinessService : IBusinessService
    {
        public string DoSomeLongRunningStuff(string message)
        {
            Thread.Sleep(250000);

            if (message == "error")
                throw new Exception("Something went wrong in to the end.");

            return "Archivo Guardado";
        }
    }
}
