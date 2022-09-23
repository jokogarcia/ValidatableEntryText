
using ValidatableEntry;

namespace ValidatableEntry;

public class ValidatableEntry : ContentView
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

    public double FontSize { get => Entry.FontSize; set { Entry.FontSize = value; } }
    public Color PlaceholderInsideColor { get => Entry.PlaceholderColor; set => Entry.PlaceholderColor = value; }

    //public string PlaceholderTopFontFamily { get => PlaceholderTop.FontFamily; set => PlaceholderTop.FontFamily = value; }
    public double PlaceholderTopFontsize
    {
        get => PlaceholderTop.FontSize; set => PlaceholderTop.FontSize = value;
    }
    public Color PlaceholderTopNormalColor { get; set; }
    public Color PlaceholderTopErrorColor { get; set; }
    public Color ErrorMessageColor
    {
        get => ValidationMessages.TextColor;
        set => ValidationMessages.TextColor = value;
    }
    public double ValidationMessgesFontSize
    {
        get => ValidationMessages.FontSize;
        set => ValidationMessages.FontSize = value;
    }

    #region BindableProperties
    public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ValidatableEntry), string.Empty, propertyChanged: OnTextPropertyChanged);
    public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ValidatableEntry), string.Empty, propertyChanged: OnPlaceholderPropertyChanged);
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
    public bool IsValid { get; set; }
    public void Validate()
    {
        ValidationErrorMessage = String.Empty;
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


    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        Validate();
        PlaceholderTop.IsVisible = !string.IsNullOrEmpty(Entry?.Text);
        
    }
    public ValidatableEntry()
	{
		
        PlaceholderTop = new Label();
        PlaceholderTop.VerticalOptions = LayoutOptions.End;
        PlaceholderTop.IsVisible = false;
        Entry = new Entry();
        ValidationMessages = new Label();
        //PlaceholderTop.BackgroundColor = Colors.Yellow;
        //ValidationMessages.BackgroundColor = Colors.Aqua;

        Entry.TextChanged += OnEntryTextChanged;

        Grid MainGrid = new Grid()
        {
            RowDefinitions = {
                new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
            }
        };
        MainGrid.Add(PlaceholderTop,0,0);
        MainGrid.Add(Entry, 0, 1);
        MainGrid.Add(ValidationMessages, 0, 2);
        Content = MainGrid;

    }
}