using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthcare.Incoming_messages
{
    public interface IMessageObserver
    {
        void ChangeValues(Message message);
        void Log(string message);
    }
}
