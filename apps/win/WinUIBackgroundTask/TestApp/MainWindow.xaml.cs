using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TestApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private const string TestBackgroundTaskName = "TestBackgroundTask";
        private const string TestBackgroundTaskEntryPoint = "TestApp.Tasks.TestBackgroundTask";
        private const int TestBackgroundInterval = 60;

        public MainWindow()
        {
            this.InitializeComponent();
            this.mainPage.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.DeniedBySystemPolicy
                || status == BackgroundAccessStatus.DeniedByUser
                || status == BackgroundAccessStatus.Unspecified)
            {
                return;
            }

            var canRegister = true;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == TestBackgroundTaskName)
                {
                    canRegister = false;
                    break;
                }
            }

            this.registerButton.IsEnabled = canRegister;
            this.unRegisterButton.IsEnabled = !canRegister;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.registerButton == (Button)sender)
            {
                this.RegisterBackgoundTasks();
            }
            else
            {
                this.UnRegisterBackgoundTasks();
            }
        }

        private void RegisterBackgoundTasks()
        {
            var configTaskBuilder = new BackgroundTaskBuilder()
            {
                Name = TestBackgroundTaskName,
                TaskEntryPoint = TestBackgroundTaskEntryPoint
            };

            configTaskBuilder.SetTrigger(new TimeTrigger(TestBackgroundInterval, false));
            configTaskBuilder.Register();
            this.registerButton.IsEnabled = false;
            this.unRegisterButton.IsEnabled = true;
        }

        private void UnRegisterBackgoundTasks()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == TestBackgroundTaskName)
                {
                    task.Value.Unregister(true);
                }
            }

            this.registerButton.IsEnabled = true;
            this.unRegisterButton.IsEnabled = false;
        }
    }
}
