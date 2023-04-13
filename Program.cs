using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var Clientbyooid =
                           DbResultDataTableHelper.GetDataTable<ClientContactInformationModel>
                           (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Clientooid.csv"));
            Console.WriteLine("Clientooid " + Clientbyooid);
        }

    }
}
