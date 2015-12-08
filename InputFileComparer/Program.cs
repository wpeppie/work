using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystemAssembly.Helpers;

namespace InputFileComparer
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                new InputComparer(new FileSystem()).Process(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            return 0;
        }
    }
}
