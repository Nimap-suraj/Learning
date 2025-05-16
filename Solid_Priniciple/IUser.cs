
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid_Priniciple
{
    interface IUser
    {
        bool Login(string username,string password);
        bool Registered(string username,string email,string password);
    }
    interface ILogger
    {
        void Log(string message);
    }
    interface ISendMail
    {
        void Send(string message);
    }
}
