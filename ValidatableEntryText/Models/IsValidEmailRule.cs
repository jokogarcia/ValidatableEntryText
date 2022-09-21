namespace ValidatableEntryText
{
    public class IsValidEmailRule : RegexRule
    {
        public IsValidEmailRule()
        {
            RegexRuleStr = "(\\w+)(\\.|_)?(\\w*)@(\\w+)(\\.(\\w+))+";
                //@"/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g";
        }
    }
}
