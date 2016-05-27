namespace AvalonStudio.Controls
{
    using AvalonStudio.Languages;
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SymbolViewModel : ViewModel<Symbol>
    {
        private void SetCursorDescriptionToFunction()
        {
            string arguments = string.Empty;

            foreach (var argument in Model.Arguments)
            {
                Arguments.Add(new ParameterSymbolViewModel(argument));
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

                Spelling = Model.Name;
            }
        }


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

        private string scopeDescription;
        public string ScopeDescription
        {
            get { return scopeDescription; }
            set { this.RaiseAndSetIfChanged(ref scopeDescription, value); }
        }

        private string builtInTypeDescription;
        public string BuiltInTypeDescription
        {
            get { return builtInTypeDescription; }
            set { this.RaiseAndSetIfChanged(ref builtInTypeDescription, value); }
        }

        private string enumTypeDescription;
        public string EnumTypeDescription
        {
            get { return enumTypeDescription; }
            set { this.RaiseAndSetIfChanged(ref enumTypeDescription, value); }
        }

        private string enumValue;
        public string EnumValue
        {
            get { return enumValue; }
            set { this.RaiseAndSetIfChanged(ref enumValue, value); }
        }

        private string typeDescription;
        public string TypeDescription
        {
            get { return typeDescription; }
            set { this.RaiseAndSetIfChanged(ref typeDescription, value); }
        }

        private string classDescription;
        public string ClassDescription
        {
            get { return classDescription; }
            set { this.RaiseAndSetIfChanged(ref classDescription, value); }
        }

        private string spelling;
        public string Spelling
        {
            get { return spelling; }
            set { this.RaiseAndSetIfChanged(ref spelling, value); }
        }

        private ObservableCollection<ParameterSymbolViewModel> arguments;
        public ObservableCollection<ParameterSymbolViewModel> Arguments
        {
            get { return arguments; }
            set { this.RaiseAndSetIfChanged(ref arguments, value); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { this.RaiseAndSetIfChanged(ref description, value); this.RaisePropertyChanged(() => DescriptionVisibility); }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { this.RaiseAndSetIfChanged(ref isVisible, value); }
        }

        public bool DescriptionVisibility
        {
            get
            {
                return (!string.IsNullOrEmpty(description));
            }
        }

        public bool ArgumentsVisibility
        {
            get
            {
                return Arguments.Count != 0;
            }
        }
    }
}
