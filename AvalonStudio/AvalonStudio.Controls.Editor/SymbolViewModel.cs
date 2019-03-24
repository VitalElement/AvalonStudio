using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.Collections.ObjectModel;
using System;

namespace AvalonStudio.Controls.Editor
{
    public class SymbolViewModel : ViewModel<Symbol>
    {
        private ObservableCollection<ParameterSymbolViewModel> arguments;

        private string builtInTypeDescription;

        private string classDescription;

        private string description;

        private string enumTypeDescription;

        private string enumValue;

        private bool isVisible;

        private string scopeDescription;

        private string spelling;

        private string typeDescription;

        public SymbolViewModel(Symbol model) : base(model)
        {
            Arguments = new ObservableCollection<ParameterSymbolViewModel>();
            IsVisible = true;

            switch (model.Kind)
            {
                case CursorKind.FunctionDeclaration:
                case CursorKind.CXXMethod:
                case CursorKind.Constructor:
                case CursorKind.Destructor:
                    SetCursorDescriptionToFunction();
                    break;

                case CursorKind.ClassDeclaration:
                case CursorKind.CXXThisExpression:
                    BuiltInTypeDescription = "class ";
                    TypeDescription = model.TypeDescription;
                    break;

                case CursorKind.Namespace:
                    BuiltInTypeDescription = "namespace ";
                    break;

                case CursorKind.TypedefDeclaration:
                    BuiltInTypeDescription = "typedef ";
                    TypeDescription = model.TypeDescription;
                    break;

                case CursorKind.EnumDeclaration:
                    BuiltInTypeDescription = "enum ";
                    EnumTypeDescription = model.TypeDescription;
                    break;

                case CursorKind.StructDeclaration:
                    BuiltInTypeDescription = "struct ";
                    TypeDescription = model.TypeDescription;
                    break;

                case CursorKind.EnumConstantDeclaration:
                    EnumTypeDescription = model.TypeDescription;
                    Spelling = model.Name + " = ";
                    EnumValue = model.EnumDescription;
                    break;

                case CursorKind.ClassTemplate:
                case CursorKind.TemplateReference:
                    BuiltInTypeDescription = "class ";
                    TypeDescription = model.Definition;
                    break;

                case CursorKind.VarDeclaration:
                    switch (model.Linkage)
                    {
                        case LinkageKind.NoLinkage:
                            ScopeDescription = "(local variable) ";
                            break;

                        case LinkageKind.Internal:
                            ScopeDescription = "(static variable) ";
                            break;

                        case LinkageKind.External:
                            ScopeDescription = "(global variable) ";
                            break;
                    }                    

                    if (model.IsBuiltInType)
                    {
                        BuiltInTypeDescription = model.SymbolType;
                    }
                    else
                    {
                        TypeDescription = model.SymbolType;
                    }

                    Spelling = model.Name;
                    break;

                case CursorKind.FieldDeclaration:
                    ScopeDescription = "(field) ";

                    if (model.IsBuiltInType)
                    {
                        BuiltInTypeDescription = model.SymbolType;
                    }
                    else
                    {
                        TypeDescription = model.SymbolType;
                    }

                    Spelling = model.Name;
                    break;

                case CursorKind.NonTypeTemplateParameter:
                case CursorKind.ParmDeclaration:
                    ScopeDescription = "(parameter) ";

                    if (model.IsBuiltInType)
                    {
                        BuiltInTypeDescription = model.SymbolType;
                    }
                    else
                    {
                        TypeDescription = model.SymbolType;
                    }

                    Spelling = model.Name;
                    break;

                default:
                    // dontShow = true;
                    //DebugData = cursor.Kind.ToString().Replace(" &", "&").Replace(" *", "*");
                    Spelling = model.Name;
                    break;
            }

            Description = Model.BriefComment;
        }

        public string ScopeDescription
        {
            get { return scopeDescription; }
            set { this.RaiseAndSetIfChanged(ref scopeDescription, value); }
        }

        public string BuiltInTypeDescription
        {
            get { return builtInTypeDescription; }
            set { this.RaiseAndSetIfChanged(ref builtInTypeDescription, value); }
        }

        public string EnumTypeDescription
        {
            get { return enumTypeDescription; }
            set { this.RaiseAndSetIfChanged(ref enumTypeDescription, value); }
        }

        public string EnumValue
        {
            get { return enumValue; }
            set { this.RaiseAndSetIfChanged(ref enumValue, value); }
        }

        public string TypeDescription
        {
            get { return typeDescription; }
            set { this.RaiseAndSetIfChanged(ref typeDescription, value); }
        }

        public string ClassDescription
        {
            get { return classDescription; }
            set { this.RaiseAndSetIfChanged(ref classDescription, value); }
        }

        public string Spelling
        {
            get { return spelling; }
            set { this.RaiseAndSetIfChanged(ref spelling, value); }
        }

        public ObservableCollection<ParameterSymbolViewModel> Arguments
        {
            get { return arguments; }
            set { this.RaiseAndSetIfChanged(ref arguments, value); }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref description, value);
                this.RaisePropertyChanged(nameof(DescriptionVisibility));
            }
        }

        private int argumentIndex;

        public int ArgumentIndex
        {
            get
            {
                return argumentIndex;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref argumentIndex, value);

                foreach (var argument in Arguments)
                {
                    argument.ResetFontWeight();
                }

                if (argumentIndex >= Arguments.Count && Model.IsVariadic)
                {
                    argumentIndex = Arguments.Count - 1;
                }

                if (argumentIndex < Arguments.Count)
                {
                    string comment = Arguments[argumentIndex].Comment;

                    if (comment != null)
                    {
                        comment = comment.Trim();

                        if (comment.StartsWith("-"))
                        {
                            comment = comment.Substring(1).Trim();
                        }
                    }

                    Description = comment;

                    Arguments[argumentIndex].FontWeight = Avalonia.Media.FontWeight.SemiBold;
                }
            }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set { this.RaiseAndSetIfChanged(ref isVisible, value); }
        }

        public bool DescriptionVisibility
        {
            get { return !string.IsNullOrEmpty(description); }
        }

        public bool ArgumentsVisibility
        {
            get { return Arguments.Count != 0; }
        }

        private void SetCursorDescriptionToFunction()
        {
            var arguments = string.Empty;

            if (Model.Arguments != null)
            {
                foreach (var argument in Model.Arguments)
                {
                    Arguments.Add(new ParameterSymbolViewModel(this, argument));
                }
            }

            if (Model.ResultType != null && Model.Name != null)
            {
                switch (Model.Kind)
                {
                    case CursorKind.Constructor:
                    case CursorKind.Destructor:
                        break;

                    default:
                        if (Model.IsBuiltInType)
                        {
                            BuiltInTypeDescription = Model.ResultType + " ";
                        }
                        else
                        {
                            TypeDescription = Model.ResultType + " ";
                        }
                        break;
                }
            }

            if(Model.IsAsync)
            {
                ScopeDescription = "(awaitable) ";
            }

            Spelling = Model.Name;
        }
    }

}