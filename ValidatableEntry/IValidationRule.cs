using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatableEntry
{
    public interface IValidationRule
    {
        bool Validate(string text);
        string ErrorMessage { get; set; }
    }
}
