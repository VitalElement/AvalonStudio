using Avalonia.Threading;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace AvalonStudio.Shell
{
    public class WorkspaceTaskRunner : IWorkspaceTaskRunner
    {
        private Subject<Task> onTaskChangedSubject;

        public WorkspaceTaskRunner()
        {
            onTaskChangedSubject = new Subject<Task>();            
        }

        public Task CurrentTask { get; private set; }

        public IObservable<Task> OnTaskChanged => onTaskChangedSubject;

        public Task RunTask(Action action)
        {
            if(CurrentTask == null)
            {
                CurrentTask = Task.Run(action).ContinueWith(_ =>
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        CurrentTask = null;
                        onTaskChangedSubject.OnNext(CurrentTask);
                    });
                });

                onTaskChangedSubject.OnNext(CurrentTask);
            }

            return CurrentTask;
        }
    }
}
