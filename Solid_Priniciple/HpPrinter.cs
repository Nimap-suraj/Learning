using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid_Priniciple
{
    internal class HpPrinter : IPrintScanner, IFaxContent, IDuplex
    {
        public void DuplexContent(string message)
        {
            Console.WriteLine(message);
        }

        public void FaxContent(string message)
        {
            throw new NotImplementedException();
        }

        public void PhotoCopyContent(string message)
        {
            throw new NotImplementedException();
        }

        public void PrintContent(string message)
        {
            throw new NotImplementedException();
        }

        public void ScanContent(string message)
        {
            throw new NotImplementedException();
        }
    }
}
