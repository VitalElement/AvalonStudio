using AvalonStudio.MVVM;
using ReactiveUI;
using System;
namespace AvalonStudio.Extensibility.MVVM
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    
    public interface ITool<T> : ITool, IViewFor<T> where T : class
    {
        
    }

    [InheritedExport(typeof(ITool))]
    public interface ITool : IViewFor
    {

    }

    [InheritedExport(typeof(IToolMetaData))]
    public interface IToolMetaData
    {
        Func<object> Factory { get; }
        Type ViewModelType { get; }
    }
    
}
