using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatableEntry
{
    public class AlwaysPassRule : IValidationRule
    {
        public string ErrorMessage { get; set; }

        public bool Validate(string text)
        {
            return true;
        }
    }
}
