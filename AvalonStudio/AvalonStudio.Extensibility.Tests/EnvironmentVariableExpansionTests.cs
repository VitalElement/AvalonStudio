using AvalonStudio.Platforms;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AvalonStudio.Extensibility.Tests
{
       
    public class EnvironmentVariableExpansionTests
    {
        [Fact]
        private void Expanding_Custom_Var_Produces_Expected_Result()
        {
            var environment = new Dictionary<string, string>
            {
                { "Var1", "Variable1" },
                { "Var2", "Variable2" }
            };

            var input = "Test $(Var1) variable $(Var2) expansion.";

            var result = input.ExpandVariables(environment);

            Assert.Equal("Test Variable1 variable Variable2 expansion.", result);
        }

        [Fact]
        private void Expanding_Custom_Var_Not_In_Dictionary_Expands_to_EmptyString()
        {
            var environment = new Dictionary<string, string>
            {
                { "Var1", "Variable1" }
            };

            var input = "Test $(Var1) variable $(Var2) expansion.";

            var result = input.ExpandVariables(environment);

            Assert.Equal("Test Variable1 variable  expansion.", result);
        }

        [Fact]
        private void Expanding_Null_Value_Returns_Null()
        {
            var environment = new Dictionary<string, string>
            {
                { "Var1", "Variable1" }
            };

            string input = null;

            var result = input.ExpandVariables(environment);

            Assert.Equal(null, result);
        }

        [Fact]
        private void Expanding_Null_Variable_Inserts_Empty_String()
        {
            var environment = new Dictionary<string, string>
            {
                { "Var1", "Variable1" },
                { "Var2", null }
            };

            var input = "Test $(Var1) variable $(Var2) expansion.";

            var result = input.ExpandVariables(environment);

            Assert.Equal("Test Variable1 variable  expansion.", result);
        }
    }
}
