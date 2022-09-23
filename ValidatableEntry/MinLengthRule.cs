
namespace ValidatableEntry
{
    public class MinLengthRule : IValidationRule
    {
        public string ErrorMessage { get; set; }
        public int MinLength { get; set; }

        public bool Validate(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            return text.Length > MinLength;
        }
    }
}
