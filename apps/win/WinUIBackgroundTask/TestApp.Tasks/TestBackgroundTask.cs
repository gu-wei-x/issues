using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TestApp.Tasks
{
    public sealed class TestBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral? taskDeferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += TaskInstance_Canceled;
            Debug.WriteLine(taskInstance.Task.Name + " Starting...");
            taskDeferral = taskInstance.GetDeferral();

            await Task.Run(() =>
            {
                Debug.WriteLine(taskInstance.Task.Name + " running...");
            });

            Debug.WriteLine("Background " + taskInstance.Task.Name + " Completed.");
            taskDeferral.Complete();
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (taskDeferral != null)
            {
                taskDeferral.Complete();
                Debug.WriteLine(sender.Task.Name + " terminated");
            }
        }
    }
}
