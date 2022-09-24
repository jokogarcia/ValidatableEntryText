
using System.ComponentModel;
using ValidatableEntry;

namespace ValidatableEntry;

public class ValidatableEntry : ContentView, INotifyPropertyChanged
{
    public Entry Entry { get; set; }
    public Label PlaceholderTop { get; set; }
    public Label ValidationMessages { get; set; }
    public string FontFamily
    {
        get => Entry.FontFamily;
        set
        {
            Entry.FontFamily = value;
            PlaceholderTop.FontFamily = value;
            ValidationMessages.FontFamily = value;
        }
    }

    public bool IsNeverValidated { get; set; } = true;
    public double FontSize { get => Entry.FontSize; set { Entry.FontSize = value; } }
    public Color PlaceholderInsideColor { get => Entry.PlaceholderColor; set => Entry.PlaceholderColor = value; }

    //public string PlaceholderTopFontFamily { get => PlaceholderTop.FontFamily; set => PlaceholderTop.FontFamily = value; }
    public double PlaceholderTopFontsize
    {
        get => PlaceholderTop.FontSize; set => PlaceholderTop.FontSize = value;
    }
    public Color PlaceholderTopNormalColor
    {
        get => placeholderTopNormalColor; set
        {
            placeholderTopNormalColor = value;
            if (IsValid || IsNeverValidated) PlaceholderTop.TextColor = placeholderTopNormalColor;
        }
    }
    public Color PlaceholderTopErrorColor
    {
        get => placeholderTopErrorColor; set
        {
            placeholderTopErrorColor = value;
            if(!IsValid && !IsNeverValidated) PlaceholderTop.TextColor = placeholderTopErrorColor;
        }
    }
    public Color ValidationMessageColor
    {
        get => ValidationMessages.TextColor;
        set => ValidationMessages.TextColor = value;
    }
    public double ValidationMessgesFontSize
    {
        get => ValidationMessages.FontSize;
        set => ValidationMessages.FontSize = value;
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
    private Color placeholderTopNormalColor;
    private Color placeholderTopErrorColor;
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
        return;//this is actually a readonly value
    }
    #endregion

    public string Placeholder
    {
        get => Entry.Placeholder;
        set
        {
            Entry.Placeholder = value;
            PlaceholderTop.Text = value;
        }
    }
    public String ValidationErrorMessage { get => ValidationMessages.Text; set => ValidationMessages.Text = value; }
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
                NotifyPropertyChanged();
            };
            PlaceholderTop.TextColor = isValid ? PlaceholderTopNormalColor : PlaceholderTopErrorColor;
        }
    }
    public bool ValidateOnTextChanged { get; set; } = false;
    public bool ValidateOnFocusLost { get; set; } = true;
    public event EventHandler<bool> ValidationStateChanged;


    public void Validate()
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

        PlaceholderTop.IsVisible = !string.IsNullOrEmpty(Entry?.Text);

        if (ValidateOnTextChanged && !IsNeverValidated)
            Validate();

    }
    public ValidatableEntry()
    {

        PlaceholderTop = new Label();
        PlaceholderTop.VerticalOptions = LayoutOptions.End;
        PlaceholderTop.IsVisible = false;
        PlaceholderTop.TextColor = PlaceholderTopNormalColor;
        PlaceholderTop.FontSize = PlaceholderTopFontsize;

        ValidationMessages = new Label();


        Entry = new Entry();
        Entry.TextChanged += OnEntryTextChanged;
        Entry.Unfocused += OnUnfocused;


        Grid MainGrid = new Grid()
        {
            RowDefinitions = {
                new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
            }
        };
        MainGrid.Add(PlaceholderTop, 0, 0);
        MainGrid.Add(Entry, 0, 1);
        MainGrid.Add(ValidationMessages, 0, 2);
        Content = MainGrid;

    }

    private void OnUnfocused(object sender, FocusEventArgs e)
    {
        if (ValidateOnFocusLost)
            Validate();
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}