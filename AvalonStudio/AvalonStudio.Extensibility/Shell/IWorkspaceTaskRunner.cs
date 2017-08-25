using System;
using System.Threading.Tasks;

namespace AvalonStudio.Shell
{
    public interface IWorkspaceTaskRunner
    {
        Task RunTask(Action action);

        Task CurrentTask { get; }

        IObservable<Task> OnTaskChanged { get; }
    }
}