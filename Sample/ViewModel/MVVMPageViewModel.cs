using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ViewModel
{
    internal class MVVMPageViewModel : INotifyPropertyChanged
    {
        private Command<bool> uRL_ValidationChangedCommand;
        private Command buttonCommand;
        private string uRL;
        private string labelText;
        private bool isURLValid;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string URL
        {
            get => uRL;
            set
            {
                uRL = value; NotifyPropertyChanged();
            }
        }
        public string LabelText
        {
            get => labelText;
            set
            {
                labelText = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsURLValid
        {
            get => isURLValid;
            set
            {
                isURLValid = value;
                NotifyPropertyChanged();
                ButtonCommand.ChangeCanExecute();
            }
        }
        public Command<bool> URL_ValidationChangedCommand { get => uRL_ValidationChangedCommand ?? (uRL_ValidationChangedCommand = new Command<bool>(URL_ValidationChangedCommand_Execute)); }
        public Command ButtonCommand { get => buttonCommand ?? (buttonCommand = new Command(Button_Command_Execute, parameter => this.IsURLValid)); }

        private void Button_Command_Execute(object obj)
        {
            LabelText = URL;
        }

        private void URL_ValidationChangedCommand_Execute(bool isValid)
        {

            LabelText = "The URL is " + (isValid ? "valid" : "invalid");

        }
        public MVVMPageViewModel()
        {
            LabelText = "Enter a URL";
        }
       
    }
}
