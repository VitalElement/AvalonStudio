// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Eval
    {
        public void NewParameterizedArray(Type type, uint rank, uint dims, uint lowBounds)
        {
            var eval2 = QueryInterface<Eval2>();
            eval2.NewParameterizedArray(type, rank, dims, lowBounds);
        }

        public void NewParameterizedObject(Function managedFunction, Type[] argumentTypes, Value[] arguments)
        {

            Type[] types = null;
            uint typesLength = 0;
            Value[] values = null;
            uint valuesLength = 0;
            var eval2 = QueryInterfaceOrNull<Eval2>();

            if (argumentTypes != null)
            {
                types = new Type[argumentTypes.Length];
                for (int i = 0; i < argumentTypes.Length; i++)
                    types[i] = argumentTypes[i];
                typesLength = (uint)types.Length;
            }
            if (arguments != null)
            {
                values = new Value[arguments.Length];
                for (int i = 0; i < arguments.Length; i++)
                    values[i] = arguments[i];
                valuesLength = (uint)values.Length;
            }

            eval2.NewParameterizedObject(managedFunction, typesLength, types, valuesLength, values);
        }

        public void CallParameterizedFunction(Function managedFunction, Type[] argumentTypes, Value[] arguments)
        {
            Type[] types = null;
            uint typesLength = 0;
            Value[] values = null;
            uint valuesLength = 0;

            var eval2 = QueryInterfaceOrNull<Eval2>();

            if (argumentTypes != null)
            {
                types = new Type[argumentTypes.Length];
                for (int i = 0; i < argumentTypes.Length; i++)
                    types[i] = argumentTypes[i];
                typesLength = (uint)types.Length;
            }
            if (arguments != null)
            {
                values = new Value[arguments.Length];
                for (int i = 0; i < arguments.Length; i++)
                    values[i] = arguments[i];
                valuesLength = (uint)values.Length;
            }
            eval2.CallParameterizedFunction(managedFunction, typesLength, types, valuesLength, values);
        }


        /** Rude abort the current computation. */
        public void RudeAbort()
        {
            QueryInterface<Eval2>().RudeAbort();
        }

        public Value CreateValueForType(Type type)
        {
            Value val = null;
            var eval2 = QueryInterface<Eval2>();
            eval2.CreateValueForType(type, out val);
            return val;
        }
    }
}
