
using System.ComponentModel;
using System.Xml.Linq;
using ValidatableEntry;

namespace ValidatableEntry;
/*
 TODO:
 Default Value + Default Value string template
 Expose Keyboard Property
 AllowOnlyNumeric Property
 */
public class ValidatableEntry : Grid
{
    public Entry Entry { get; set; }
    public Label FloatingPlaceholder { get; set; }
    public Label ValidationMessageLabel { get; set; }


    public bool IsNeverValidated { get; private set; } = true;

    public double ValidationMessageFontSize
    {
        get => (double)GetValue(ValidationMessageColorProperty);
        set => SetValue(ValidationMessageColorProperty, value);
    }
    public bool IsPassword
    {
        get => Entry.IsPassword;
        set => Entry.IsPassword = value;
    }

    #region BindableProperties
    /**************************************************************************************
    *************   TextProperty ****************************************
    ***************************************************************************************/
    public string Text
    {
        get
        {
            Text = Entry.Text;
            return (string)GetValue(TextProperty);
        } 
        set => SetValue(TextProperty, value);
    }
    public static BindableProperty TextProperty = BindableProperty.Create(nameof(Text),
        typeof(string),
        typeof(ValidatableEntry),
        string.Empty,
        propertyChanged: OnTextPropertyChanged);
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
    /**************************************************************************************
    *************   PlaceholderProperty ****************************************
    ***************************************************************************************/
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set
        {
            SetValue(PlaceholderProperty, value);
        }
    }
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

    /**************************************************************************************
    *************   IsValid ***************************************************************
    ***************************************************************************************/
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
    
    

    /**************************************************************************************
    *************   ValidationMessageColorProperty ****************************************
    ***************************************************************************************/
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

    /**************************************************************************************
    *************   FontFamilyPropertyProperty ********************************************
    ***************************************************************************************/
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
    /**************************************************************************************
    *************   FloatingPlaceholderNormalColorProperty ********************************
    ***************************************************************************************/

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
    /**************************************************************************************
    *************   FloatingPlaceholderErrorColorProperty *********************************
    ***************************************************************************************/
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
    /**************************************************************************************
    *************   FontSizeProperty ******************************************************
    ***************************************************************************************/
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
    /**************************************************************************************
    *************   PlaceholderInsideColorProperty ****************************************
    ***************************************************************************************/
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
    /**************************************************************************************
    *************   FloatingPlaceholderFontsizeProperty ***********************************
    ***************************************************************************************/
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

    /**************************************************************************************
    *************   ValidationMessageFontSizeProperty *************************************
    ***************************************************************************************/

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
    #endregion

    
    


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

    public bool ValidateOnTextChanged { get; set; } = false;
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

        FloatingPlaceholder.Text =
            string.IsNullOrEmpty(Entry?.Text)
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


    }
    public static void EvaluateFloatingPlaceholderColor(ValidatableEntry e)
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