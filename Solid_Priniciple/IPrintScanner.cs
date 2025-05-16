using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid_Priniciple
{
    interface IPrintScanner
    {
        void PrintContent(string message);
        void PhotoCopyContent(string message);
        void ScanContent(string message);

    }

    interface IFaxContent
    {
        void FaxContent(string message);
    }
    interface IDuplex
    {
        void DuplexContent(string message);
    }
}
