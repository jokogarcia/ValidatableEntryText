namespace ValidatableEntry
{
    public class MaxLengthRule : IValidationRule
    {
        public string ErrorMessage { get; set; }
        public int MaxLength { get; set; }

        public bool Validate(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return true;
            return text.Length < MaxLength;
        }
    }
}
