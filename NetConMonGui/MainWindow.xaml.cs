using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.ServiceProcess;
using NetConMon;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Controls.DataVisualization.Charting;

namespace NetConMonGui
{

    public partial class MainWindow : Window
    {

        private Timer serviceStatusTimer;
        private ServiceController serviceCtrl;
        private EventLog eventLog;

        public MainWindow()
        {
            InitializeComponent();

            serviceStatusTimer = new Timer();
            serviceStatusTimer.Interval = 1000;
            serviceStatusTimer.Elapsed += ServiceStatusTimer_Elapsed;

            eventLog = new EventLog(SharedValues.EV_LOG);

            serviceCtrl = new ServiceController(SharedValues.SVC_NAME);
        }

        #region Window

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            serviceStatusTimer.Start();
            ReloadMenuButton_Click(this, null);
        }

        private void ExitMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SettingsMenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReloadMenuButton_Click(object sender, RoutedEventArgs e)
        {
            (new Task(() => {
                List<KeyValuePair<DateTime, int>> dataPoints = eventLog.Entries.Cast<EventLogEntry>().Where(p => p.InstanceId == 1).Select(
                        p => new KeyValuePair<DateTime, int>(p.TimeWritten, (int)p.InstanceId)).ToList();
                chartTimeout.Dispatcher.Invoke(() => {
                    ((LineSeries)chartTimeout.Series[0]).ItemsSource = dataPoints;
                    chartTimeout.Width = dataPoints.Count * 5;
                });
            })).Start();
        }

        #endregion


        #region Service handling

        private void RestartServiceMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if(serviceCtrl.Status == ServiceControllerStatus.Running)
            {
                serviceCtrl.Stop();
                serviceCtrl.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(5));
            }
            serviceCtrl.Start();
        }

        private void StopServiceMenuButton_Click(object sender, RoutedEventArgs e)
        {
            serviceCtrl.Stop();
        }

        private void ServiceStatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ServiceStatusTextBlock.Dispatcher.Invoke(() => {
                serviceCtrl.Refresh();
                ServiceStatusTextBlock.Text = (serviceCtrl.Status == ServiceControllerStatus.Running) ? "running" : "not running";
            });
        }

        #endregion

    }

}
