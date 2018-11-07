using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSCodeGenerator
{
    public partial class ParameterSetDictionary : ParameterSetDefinition.BaseParameterSetDictionary
    {
        public override FunctionDefinition Function { get; }

        public ParameterSetDictionary(FunctionDefinition function) { Function = function; }
    }
}