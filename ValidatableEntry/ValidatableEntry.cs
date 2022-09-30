
using System.ComponentModel;
using System.Xml.Linq;
using ValidatableEntry;

namespace ValidatableEntry;

public class ValidatableEntry : Grid
{
    public Entry Entry { get; set; }
    public Label FloatingPlaceholder { get; set; }
    public Label validationMessage { get; set; }
    public string FontFamily
    {
        get => Entry.FontFamily;
        set
        {
            Entry.FontFamily = value;
            FloatingPlaceholder.FontFamily = value;
            validationMessage.FontFamily = value;
        }
    }

    public bool IsNeverValidated { get; private set; } = true;
    public double FontSize { get => Entry.FontSize; set { Entry.FontSize = value; } }
    public Color PlaceholderInsideColor { get => Entry.PlaceholderColor; set => Entry.PlaceholderColor = value; }

    //public string FloatingPlaceholderFontFamily { get => FloatingPlaceholder.FontFamily; set => FloatingPlaceholder.FontFamily = value; }
    public double FloatingPlaceholderFontsize
    {
        get => FloatingPlaceholder.FontSize; set => FloatingPlaceholder.FontSize = value;
    }
    public Color FloatingPlaceholderNormalColor
    {
        get => floatingPlaceholderNormalColor; set
        {
            floatingPlaceholderNormalColor = value;
            if (IsValid || IsNeverValidated) FloatingPlaceholder.TextColor = FloatingPlaceholderNormalColor;
        }
    }
    public Color FloatingPlaceholderErrorColor
    {
        get => floatingPlaceholderErrorColor; set
        {
            floatingPlaceholderErrorColor = value;
            if(!IsValid && !IsNeverValidated) FloatingPlaceholder.TextColor = FloatingPlaceholderErrorColor;
        }
    }
    public Color ValidationMessageColor
    {
        get => validationMessage.TextColor;
        set => validationMessage.TextColor = value;
    }
    public double ValidationMessageFontSize
    {
        get => validationMessage.FontSize;
        set => validationMessage.FontSize = value;
    }
    public bool IsPassword
    {
        get => Entry.IsPassword;
        set => Entry.IsPassword = value;
    }

    #region BindableProperties
    public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ValidatableEntry), string.Empty, propertyChanged: OnTextPropertyChanged);
    public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ValidatableEntry), string.Empty, propertyChanged: OnPlaceholderPropertyChanged);
    public static BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidatableEntry), false, propertyChanged: OnIsValidPropertyChanged);
    private bool isValid;
    private Color floatingPlaceholderNormalColor;
    private Color floatingPlaceholderErrorColor;
    #endregion
    #region BindableProperties_ChangeMethos
    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue == newValue) return;
        string newString = newValue as string ?? string.Empty;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        ValidatableEntry.Text = newString;

    }
    private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue == newValue) return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        ValidatableEntry.Placeholder = newValue.ToString();
    }
    private static void OnIsValidPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue == newValue) return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        ValidatableEntry.IsValid = (bool)newValue;
    }
    #endregion

    public string Placeholder
    {
        get => Entry.Placeholder;
        set
        {
            Entry.Placeholder = value;
            FloatingPlaceholder.Text = value;
        }
    }
    public string ValidationErrorMessage { get => validationMessage.Text; set => validationMessage.Text = value; }
    public string Text
    {
        get => Entry.Text;
        set => Entry.Text = value;
    }

    public List<IValidationRule> ValidationRules { get; set; } = new();
    public bool IsValid
    {
        get => isValid;
        set
        {
            if (isValid != value)
            {
                isValid = value;
                OnValidationStateChanged(isValid);
                OnPropertyChanged();
            };
            FloatingPlaceholder.TextColor = isValid ? FloatingPlaceholderNormalColor : FloatingPlaceholderErrorColor;
        }
    }
    public bool ValidateOnTextChanged { get; set; } = false;
    public bool ValidateOnFocusLost { get; set; } = true;
    public event EventHandler<bool> ValidationStateChanged;


    public void RunValidations()
    {
        IsNeverValidated = false;
        ValidationErrorMessage = string.Empty;
        IsValid = true;
        if (ValidationRules is null || ValidationRules.Count == 0)
            return;
        foreach (var item in ValidationRules)
        {
            if (!item.Validate(Text))
            {
                IsValid = false;
                ValidationErrorMessage = item.ErrorMessage;
                return;
            }
        }

    }
    private void OnValidationStateChanged(bool isValid)
    {
        // Make a temporary copy of the event to avoid possibility of
        // a race condition if the last subscriber unsubscribes
        // immediately after the null check and before the event is raised.
        var e = ValidationStateChanged;
        if (e != null)
        {
            e(this, isValid);
        }
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {

        FloatingPlaceholder.IsVisible = !string.IsNullOrEmpty(Entry?.Text);

        if (ValidateOnTextChanged && !IsNeverValidated)
            RunValidations();

    }
    public ValidatableEntry()
    {

        FloatingPlaceholder = new Label();
        FloatingPlaceholder.VerticalOptions = LayoutOptions.End;
        FloatingPlaceholder.IsVisible = false;
        FloatingPlaceholder.TextColor = FloatingPlaceholderNormalColor;
        FloatingPlaceholder.FontSize = FloatingPlaceholderFontsize;

        validationMessage = new Label();


        Entry = new Entry();
        Entry.TextChanged += OnEntryTextChanged;
        Entry.Unfocused += OnUnfocused;
        RowDefinitions = new RowDefinitionCollection{
            new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
            };


       
        this.Add(FloatingPlaceholder, 0, 0);
        this.Add(Entry, 0, 1);
        this.Add(validationMessage, 0, 2);
        

}

    private void OnUnfocused(object sender, FocusEventArgs e)
    {
        if (ValidateOnFocusLost)
            RunValidations();
    }
   
    
}