using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client.Configuration
{
    public interface IConfigController
    {
        void WriteConfig(Models.Config config);
        void DoesConfigFileExists();
        Models.Config ReadConfig();
    }
}
