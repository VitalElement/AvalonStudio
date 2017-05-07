using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Eval
    {
        public void NewParameterizedArray(Type type, int rank, int dims, int lowBounds)
        {
            var eval2 = QueryInterface<Eval2>();
            eval2.NewParameterizedArray(type, rank, dims, lowBounds);
        }
    }
}
