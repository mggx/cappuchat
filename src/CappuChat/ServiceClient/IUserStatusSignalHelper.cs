using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CappuChat.ServiceClient
{
    public interface IUserStatusSignalHelper
    {
        Task SetUserStatus(SessionSwitchReason switchReason);
    }
}
