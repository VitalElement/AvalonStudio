using AvalonStudio.Extensibility.Studio;
using System;
using System.Reactive.Linq;

namespace AvalonStudio.Shell
{
    public static class ShellExtensions
    {
        public static IObservable<bool> OnSolutionLoaded(this IStudio studio) => studio.OnSolutionChanged.Select(s => s != null).StartWith(false);

        public static IObservable<bool> OnCurrentTaskChanged(this IStudio studio) => studio.TaskRunner.OnTaskChanged.Select(t => t != null).StartWith(false);

        public static IObservable<bool> CanRunTask (this IStudio studio) => studio.OnSolutionLoaded().CombineLatest(studio.OnCurrentTaskChanged(), (loaded, running) => loaded & !running);
    }
}
