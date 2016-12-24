﻿namespace AvalonStudio.Controls
{
    using Avalonia.Media;
    using AvalonStudio.Extensibility.Languages.CompletionAssistance;
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SignatureViewModel : ViewModel<Signature>
    {
        public SignatureViewModel(Signature model) : base(model)
        {
            Parameters = new List<ParameterViewModel>();

            foreach (var parameter in model.Parameters)
            {
                Parameters.Add(new ParameterViewModel(parameter));
            }

            SelectedParameter = Parameters.FirstOrDefault();
        }

        public List<ParameterViewModel> Parameters { get; private set; }

        private ParameterViewModel selectedParameter;
        public ParameterViewModel SelectedParameter
        {
            get { return selectedParameter; }
            set
            {
                if(selectedParameter != null)
                {
                    selectedParameter.ResetFontWeight();
                }

                if (value != null)
                {
                    value.FontWeight = FontWeight.SemiBold;
                }

                this.RaiseAndSetIfChanged(ref selectedParameter, value);
                
            }
        }

        public bool HasParameters
        {
            get
            {
                return Parameters.Count > 0;
            }
        }


        public string Name
        {
            get
            {
                return Model.Name;
            }
        }

        public string Label
        {
            get
            {
                return Model.Label;
            }
        }

        public string Description
        {
            get
            {
                return Model.Description;
            }
        }

        private int parameterIndex;
        public int ParameterIndex
        {
            get { return parameterIndex; }
            set
            {
                this.RaiseAndSetIfChanged(ref parameterIndex, value);

                if (value < Parameters.Count)
                {
                    SelectedParameter = Parameters[value];
                }
                else
                {
                    SelectedParameter = null;
                }
            }
        }

        public string BuiltInReturnType
        {
            get
            {
                return Model.BuiltInReturnType;
            }
        }

        public string ReturnType
        {
            get
            {
                return Model.ReturnType;
            }
        }
    }
}
