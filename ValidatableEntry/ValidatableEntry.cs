namespace ValidatableEntry;

public class ValidatableEntry : Grid
{
    Entry Entry { get; set; }
    Label FloatingPlaceholder { get; set; }
    Label ValidationMessageLabel { get; set; }
    /// <summary>
    /// Is true only if validations were never run.
    /// </summary>
    public bool IsNeverValidated { get; private set; } = true;
    /// <summary>
    /// Font Size of the Validation Message
    /// </summary>
    public double ValidationMessageFontSize
    {
        get => (double)GetValue(ValidationMessageColorProperty);
        set => SetValue(ValidationMessageColorProperty, value);
    }
    /// <summary>
    /// If true, the input characters are masked.
    /// </summary>
    public bool IsPassword
    {
        get => Entry.IsPassword;
        set => Entry.IsPassword = value;
    }

    #region BindableProperties
    /// <summary>
    /// The content of the Entry
    /// </summary>
    public string Text
    {
        get
        {
            Text = Entry.Text;
            return (string)GetValue(TextProperty);
        } 
        set => SetValue(TextProperty, value);
    }
    /// <summary>
    /// Bindable property associated with the Text Property
    /// </summary>
    public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
        typeof(string),
        typeof(ValidatableEntry),
        string.Empty,
        propertyChanged: OnTextPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay
        );
    private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue == newValue) return;
        string newString = newValue as string ?? string.Empty;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.Entry == null)
            return;
        ValidatableEntry.Entry.Text = newString;
        if (ValidatableEntry.ValidateOnTextChanged)
        {
            ValidatableEntry.RunValidations();
        }

    }
    /// <summary>
    /// Will be displayed inside the Entry when no text has been entered, and on top of it
    /// after that.
    /// </summary>
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set
        {
            SetValue(PlaceholderProperty, value);
        }
    }
    /// <summary>
    /// Bindable property associated with the Placeholder Property
    /// </summary>
    public static BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder),
        typeof(string),
        typeof(ValidatableEntry),
        string.Empty,
        propertyChanged: OnPlaceholderPropertyChanged);
    private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue == newValue) return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.Entry != null)
            ValidatableEntry.Entry.Placeholder = newValue.ToString();
        if(ValidatableEntry?.FloatingPlaceholder != null)
        {
            ValidatableEntry.FloatingPlaceholder.Text = 
                string.IsNullOrEmpty(ValidatableEntry?.Entry?.Text) 
                ? string.Empty 
                : newValue.ToString();
        }
    }
    /**************************************************************************************
    *************   ValidationMessage *****************************************************
    ***************************************************************************************/
    public string ValidationMessage { 
        get => (string)GetValue(ValidationMessageProperty);
        set => SetValue(ValidationMessageProperty, value);
    }
    public static BindableProperty ValidationMessageProperty = BindableProperty.Create(
        nameof(ValidationMessage),
        typeof(string),
        typeof(ValidatableEntry),
        string.Empty,
        propertyChanged: OnValidationMessagePropertyChanged);
    private static void OnValidationMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue == newValue) return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.ValidationMessageLabel != null)
            ValidatableEntry.ValidationMessageLabel.Text = newValue.ToString();
    }
    /// <summary>
    /// Use this parameter to change type of keyboard displayed on 
    /// touch devices
    /// </summary>
    public Keyboard Keyboard { get=>(Keyboard)GetValue(KeyboardProperty); set=>SetValue(KeyboardProperty,value); }
    public static BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard),
        typeof(Keyboard),
        typeof(ValidatableEntry),
        Keyboard.Plain,
        propertyChanged:OnKeyboardPropertyChanged
        );

    private static void OnKeyboardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue == newValue) return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.Entry != null)
            ValidatableEntry.Entry.Keyboard = (Keyboard)newValue;
    }

/// <summary>
/// Is true if the text entered on the Entry has been validated, and the validations are
/// satisfactory
/// </summary>
    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set
        {
            SetValue(IsValidProperty, value);
            IsValidChanged();
        }
    }
    public static BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidatableEntry), false);
    
    

/// <summary>
/// Text Color of the Validation Message
/// </summary>
    public Color ValidationMessageColor
    {
        get => (Color)GetValue(ValidationMessageColorProperty);
        set => SetValue(ValidationMessageColorProperty, value);
    }
    public static readonly BindableProperty ValidationMessageColorProperty = BindableProperty.Create(
            nameof(ValidationMessageColor),
            typeof(Color),
            typeof(ValidatableEntry),
            Colors.Transparent,
            propertyChanged: ValidationMessageColorPropertyChanged
            );

    private static void ValidationMessageColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.ValidationMessageLabel == null)
            return;
        ValidatableEntry.ValidationMessageLabel.TextColor = (Color)newValue;
    }

/// <summary>
/// FontFamily of the Entry and also for the Placeholder and ValidationMessages
/// </summary>
    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }
    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            nameof(FontFamily),
            typeof(string),
            typeof(ValidatableEntry),
            null,
            propertyChanged: FontFamilyPropertyChanged
            );

    private static void FontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if(ValidatableEntry != null)
        {
            if (ValidatableEntry.Entry != null)
                ValidatableEntry.Entry.FontFamily = (string)newValue;
            if (ValidatableEntry.FloatingPlaceholder != null) 
                ValidatableEntry.FloatingPlaceholder.FontFamily = (string)newValue;
            if (ValidatableEntry.ValidationMessageLabel != null) 
                ValidatableEntry.ValidationMessageLabel.FontFamily = (string)newValue;
        }
        
    }
/// <summary>
/// Text Color of the FloatingPlaceholder when the validations are succesfull (or have never been run)
/// </summary>
    public Color FloatingPlaceholderNormalColor
    {
        get => (Color)GetValue(FloatingPlaceholderNormalColorProperty);
        set => SetValue(FloatingPlaceholderNormalColorProperty, value);
    }
    public static readonly BindableProperty FloatingPlaceholderNormalColorProperty = BindableProperty.Create(
        nameof(FloatingPlaceholderNormalColor),
        typeof(Color),
        typeof(ValidatableEntry),
        Colors.Transparent,
        propertyChanged: FloatingPlaceholderNormalColorPropertyChanged
        );

    private static void FloatingPlaceholderNormalColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        EvaluateFloatingPlaceholderColor(ValidatableEntry);
    }
    /// <summary>
    /// Text Color of the FloatingPlaceholder when the validations fail.
    /// </summary>
    public Color FloatingPlaceholderErrorColor
    {
        get => (Color)GetValue(FloatingPlaceholderErrorColorProperty);
        set => SetValue(FloatingPlaceholderErrorColorProperty, value);
    }
    public static readonly BindableProperty FloatingPlaceholderErrorColorProperty = BindableProperty.Create(
        nameof(FloatingPlaceholderErrorColor),
        typeof(Color),
        typeof(ValidatableEntry),
        Colors.Transparent,
        propertyChanged: FloatingPlaceholderErrorColorPropertyChanged
        );

    private static void FloatingPlaceholderErrorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        EvaluateFloatingPlaceholderColor(ValidatableEntry);
    }
/// <summary>
/// Font Size for the Entry.
/// </summary>
    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }
    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(ValidatableEntry),
        10.0,
        propertyChanged: FontSizePropertyChanged
        );

    private static void FontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.Entry == null) 
            return;
        ValidatableEntry.Entry.FontSize = (double)newValue;
    }
/// <summary>
/// Font Color of the Placeholder text when it is shown inside the Entry.
/// </summary>
    public Color PlaceholderInsideColor
    {
        get => (Color)GetValue(PlaceholderInsideColorProperty);
        set => SetValue(PlaceholderInsideColorProperty, value);
    }
    public static readonly BindableProperty PlaceholderInsideColorProperty = BindableProperty.Create(
            nameof(PlaceholderInsideColor),
            typeof(Color),
            typeof(ValidatableEntry),
            Colors.Transparent,
            propertyChanged: PlaceholderInsideColorPropertyChanged
            );

    private static void PlaceholderInsideColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.Entry == null)
            return;
        ValidatableEntry.Entry.PlaceholderColor = (Color)newValue;
    }
/// <summary>
/// Font size of the Placeholder text when it is shown over the Entry
/// </summary>
    public double FloatingPlaceholderFontsize
    {
        get => (double)GetValue(FloatingPlaceholderFontsizeProperty);
        set => SetValue(FloatingPlaceholderFontsizeProperty, value);
    }
    public static readonly BindableProperty FloatingPlaceholderFontsizeProperty = BindableProperty.Create(
            nameof(FloatingPlaceholderFontsize),
            typeof(double),
            typeof(ValidatableEntry),
            10.0,
            propertyChanged: FloatingPlaceholderFontsizePropertyChanged
            );

    private static void FloatingPlaceholderFontsizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.FloatingPlaceholder == null)
            return;
        ValidatableEntry.FloatingPlaceholder.FontSize = (double)newValue;
    }

/// <summary>
/// Font size of the Validation Message
/// </summary>

    public static readonly BindableProperty ValidationMessageFontSizeProperty = BindableProperty.Create(
            nameof(ValidationMessageFontSize),
            typeof(double),
            typeof(ValidatableEntry),
            10.0,
            propertyChanged: ValidationMessageFontSizePropertyChanged
            );

    private static void ValidationMessageFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        if (ValidatableEntry?.ValidationMessageLabel == null)
            return;
        ValidatableEntry.ValidationMessageLabel.FontSize = (double)newValue;
    }
    public static readonly BindableProperty ValidationChangedCommandProperty = BindableProperty.Create(
        nameof(ValidationChangedCommand),
        typeof(Command<bool>),
        typeof(ValidatableEntry),
        null,
        propertyChanged: OnValidationChangedCommandChanged
        );
    private Command<bool> _validationChangedCommand;
    /// <summary>
    /// Command that is executed when the state of the validation changes.
    /// </summary>
    /// <remarks>
    /// Be sure to define a Command with one Bool parameter, so it can receive
    /// the status of the validation. Otherwise, an exception will be thrown
    /// </remarks>
    public Command<bool> ValidationChangedCommand
    {
        get => _validationChangedCommand;
        set => _validationChangedCommand = value;
    }
    private static void OnValidationChangedCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue == oldValue)
            return;
        ValidatableEntry ValidatableEntry = bindable as ValidatableEntry;
        try
        {
            Command<bool> newCommand = (Command<bool>)newValue;
            ValidatableEntry.ValidationChangedCommand = newCommand;
        }
        catch (InvalidCastException ex)
        {
            throw new InvalidCastException("This command needs to be of type Command<bool>", ex);
        }
    }

    /// <summary>
    /// A string containing all the characters allowed to be used in the Entry.
    /// If left blank, it is ignored and all characters are allowed.
    /// </summary>
    public string AllowedCharactersSet { 
        get => (string)GetValue(AllowedCharactersSetProperty);
        set => SetValue(AllowedCharactersSetProperty, value);
    }
    public static BindableProperty AllowedCharactersSetProperty = BindableProperty.Create(
        nameof(AllowedCharactersSet),
        typeof(string),
        typeof(ValidatableEntry)
        );
    #endregion
/// <summary>
/// List of ValidationRules
/// </summary>
    public List<IValidationRule> ValidationRules { get; set; } = new();
    private bool previousIsValidValue;
    private bool validateOnFocusLost = true;

    

    private void IsValidChanged()
    {
        var isValid = IsValid;
        if (isValid != previousIsValidValue)
        {
            OnValidationStateChanged(isValid);
            OnPropertyChanged();
            EvaluateFloatingPlaceholderColor(this);
        }
        previousIsValidValue = isValid;
    }
    /// <summary>
    /// If true, the validations will be run after every character entered, changed or deleted
    /// </summary>
    public bool ValidateOnTextChanged { get; set; } = false;
    /// <summary>
    /// If true, validations will be run every time the Entry looses focus
    /// </summary>
    public bool ValidateOnFocusLost
    {
        get => validateOnFocusLost;
        set
        {
            validateOnFocusLost = value;
            if (value)
            {
                Entry.Unfocused += this.OnUnfocused;
            }
            else
            {
                Entry.Unfocused -= this.OnUnfocused;
            }
        }
    }
    public event EventHandler<bool> ValidationStateChanged;

    /// <summary>
    /// Causes validations to be run agains the contents of Entry
    /// As soon as one validation fails, the corresponding message is loaded
    /// into ValidationMessage and the method ends.
    /// </summary>
    public void RunValidations()
    {
        IsNeverValidated = false;
        ValidationMessage = string.Empty;
        IsValid = true;
        if (ValidationRules is null || ValidationRules.Count == 0)
            return;
        foreach (var item in ValidationRules)
        {
            if (!item.Validate(Text))
            {
                IsValid = false;
                ValidationMessage = item.ErrorMessage;
                return;
            }
        }

    }
    private void OnValidationStateChanged(bool isValid)
    {
        ValidationChangedCommand?.Execute(isValid);
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
        if (!string.IsNullOrEmpty(AllowedCharactersSet))
        {
            foreach(char c in e.NewTextValue ?? string.Empty)
            {
                if (!AllowedCharactersSet.Contains(c))
                {
                    ((Entry)sender).Text = e.OldTextValue;
                    return;
                }
            }
        }
        FloatingPlaceholder.Text =
            string.IsNullOrEmpty(e.NewTextValue)
                ? string.Empty
                : this.Placeholder;
        if (ValidateOnTextChanged && !IsNeverValidated)
            RunValidations();
    }
    public ValidatableEntry()
    {
        FloatingPlaceholder = new Label();
        FloatingPlaceholder.VerticalOptions = LayoutOptions.End;
        FloatingPlaceholder.TextColor = FloatingPlaceholderNormalColor;
        FloatingPlaceholder.FontSize = FloatingPlaceholderFontsize;

        ValidationMessageLabel = new Label();


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
        this.Add(ValidationMessageLabel, 0, 2);
        // This is to prevent a bug that causes the TextProperty not to update
        // when no ValidationRule is defined
        this.ValidationRules.Add(new AlwaysPassRule());


    }
    static void EvaluateFloatingPlaceholderColor(ValidatableEntry e)
    {
        if (e?.FloatingPlaceholder == null)
            return;
        e.FloatingPlaceholder.TextColor =
            e.IsValid || e.IsNeverValidated ? e.FloatingPlaceholderNormalColor
            : e.FloatingPlaceholderErrorColor;
    }
    private void OnUnfocused(object sender, FocusEventArgs e)
    {
        RunValidations();
    }


}