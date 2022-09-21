using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ValidatableEntry
{
    public class RegexRule : IValidationRule
    {
        public string ErrorMessage { get; set; }
        public string RegexRuleStr
        {
            get => regexRuleStr;
            set
            {
                regexRuleStr = value;
                _rule = new Regex(value);
            }
        }
        private Regex _rule;
        private string regexRuleStr;

        public bool Validate(string text) => _rule.IsMatch(text);
    }
}
