using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidatableEntry
{
    public class IsIntRule : IValidationRule
    {
        public string ErrorMessage { get; set; }
        public int MinValue { get; set; } = int.MinValue;
        public int MaxValue { get; set; } = int.MaxValue;
        public bool Validate(string text)
        {
            if(int.TryParse(text,out int value))
            {
                if(value < MinValue)
                    return false;
                if (value > MaxValue)
                    return false;
                return true;
            }
            else
                return false;
        }
    }
}
