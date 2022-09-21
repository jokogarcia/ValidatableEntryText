using System.Text.RegularExpressions;

namespace ValidatableEntryText
{
    public class IsValidUrlRule : IValidationRule
    {
       private bool isValidIP(string s)
        {
            var parts = s.Split('.');

            bool isValid = parts.Length == 4
                           && !parts.Any(
                               x =>
                               {
                                   int y;
                                   return Int32.TryParse(x, out y) && y > 255 || y < 1;
                               });
            return isValid;
        }

        public string ErrorMessage { get; set; }

        public bool Validate(string text)
        {
            var regex = new Regex(@"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)");
            if (!regex.IsMatch(text)) {
                return isValidIP(text);
            }
            return true;
        }
    }
}
