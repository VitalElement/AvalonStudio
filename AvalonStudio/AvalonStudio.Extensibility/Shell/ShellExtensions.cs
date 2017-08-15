using System;
using System.Reactive.Linq;

namespace AvalonStudio.Shell
{
    public static class ShellExtensions
    {
        public static IObservable<bool> OnSolutionLoaded(this IShell shell) => shell.OnSolutionChanged.Select(s => s != null).StartWith(false);

        public static IObservable<bool> OnCurrentTaskChanged(this IShell shell) => shell.TaskRunner.OnTaskChanged.Select(t => t != null).StartWith(false);

        public static IObservable<bool> CanRunTask (this IShell shell) => shell.OnSolutionLoaded().CombineLatest(shell.OnCurrentTaskChanged(), (loaded, running) => loaded & !running);
    }
}
