using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace NetConMon
{

    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {

        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(SharedValues.REG_KEY))
            {
                key.SetValue(SharedValues.REG_PARAM_PING_IP, SharedValues.DEFAULT_PING_IP, RegistryValueKind.String);
                key.SetValue(SharedValues.REG_PARAM_PING_TIMEOUT, SharedValues.DEFAULT_PING_TIMEOUT, RegistryValueKind.DWord);
                key.SetValue(SharedValues.REG_PARAM_PING_INTERVAL, SharedValues.DEFAULT_PING_INTERVAL, RegistryValueKind.DWord);
            }
            base.OnAfterInstall(savedState);
        }

    }

}
