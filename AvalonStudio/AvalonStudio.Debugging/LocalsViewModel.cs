﻿namespace AvalonStudio.Debugging
{
    using AvalonStudio.Debugging;
    using System.Collections.Generic;
    using System.Linq;
    
    public class LocalsViewModel : WatchListViewModel
    {
        private List<Variable> locals;

        public void InvalidateLocals(List<Variable> variables)
        {
            var updated = new List<Variable>();
            var removed = new List<Variable>();

            for(int i = 0; i < locals.Count; i++)
            {
                var local = locals[i];

                var currentVar = variables.FirstOrDefault(v => v.Name == local.Name);

                if (currentVar == null)
                {
                    removed.Add(local);                    
                }
                else
                {
                    updated.Add(local);
                }
            }

            foreach(var variable in variables)
            {
                var currentVar = updated.FirstOrDefault(v => v.Name == variable.Name);

                if(currentVar == null)
                {
                    locals.Add(variable);
                    AddWatch(variable.Name);
                }
            }

            foreach(var removedvar in  removed)
            {
                locals.Remove(removedvar);
                RemoveWatch(Children.FirstOrDefault(w => w.Name == removedvar.Name));
            }
        }

        public override void Clear()
        {
            locals.Clear();
            base.Clear();
        }

        public LocalsViewModel()
        {
            Title = "Locals";
            locals = new List<Variable>();
        }        
    }
}
