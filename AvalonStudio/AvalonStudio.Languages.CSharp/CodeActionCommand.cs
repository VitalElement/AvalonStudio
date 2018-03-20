using System;
using System.Windows.Input;

namespace AvalonStudio.Languages.CSharp
{
    class CodeActionCommand : ICommand
    {
        RoslynContextActionProvider _provider;
        ICodeAction _codeAction;

        public CodeActionCommand(RoslynContextActionProvider provider, ICodeAction codeAction)
        {
            _provider = provider;
            _codeAction = codeAction;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            await _provider.ExecuteCodeActionAsync(_codeAction).ConfigureAwait(true);
        }
    }

}