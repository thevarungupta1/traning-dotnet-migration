using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinFormsApp2
{
    public class ContactFormViewModel : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _department = string.Empty;
        private string _validationMessage = string.Empty;
        private bool _isSaved;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ContactFormViewModel(Contact? existing = null)
        {
            IsEditMode = existing != null;

            if (existing != null)
            {
                _name = existing.Name;
                _email = existing.Email;
                _phone = existing.Phone;
                _department = existing.Department;
            }
            SaveCommand = new RelayCommand(ExecuteSave, CanExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
            ClearCommand = new RelayCommand(ExecuteClear);

        }
        public event Action<bool>? RequestClose;
        public bool IsEditMode { get; }
        public string FormTitle => IsEditMode ? "Edit Contact" : "Add New Contact";

        // bindable properties
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChange(); RaiseSaveCanExecute();
            }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChange(); RaiseSaveCanExecute(); }
        }
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChange(); RaiseSaveCanExecute(); }
        }

        public string Department
        {
            get => _department;
            set { _department = value; OnPropertyChange(); RaiseSaveCanExecute(); }
        }

        public string ValidationMessage
        {
            get => _validationMessage;
            private set { _validationMessage = value; OnPropertyChange(); }
        }

        public bool IsSaved
        {
            get => _isSaved;
            private set { _isSaved = value; OnPropertyChange(); }
        }

        public static string[] strings = new[] { "HR", "IT", "Sales", "Marketing" };

        // commands (bound to button command - no click handlers)
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ClearCommand { get; }

        // command logic
        private bool CanExecuteSave(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   !string.IsNullOrWhiteSpace(Department);
        }

        private void ExecuteSave(object? parameter)
        {
            if (!Email.Contains("@"))
            {
                ValidationMessage = "Please enter a valid email address.";
                return;
            }
            ValidationMessage = string.Empty;
            IsSaved = true;
            RequestClose?.Invoke(true);


        }
        private void ExecuteCancel(object? parameter)
        {
            IsSaved = false;
            RequestClose?.Invoke(false);
        }

        private void ExecuteClear(object? parameter)
        {
            Name = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Department = string.Empty;
            ValidationMessage = string.Empty;
        }

        public void ApplyTo(Contact contact)
        {
            contact.Name = Name;
            contact.Email = Email;
            contact.Phone = Phone;
            contact.Department = Department;
        }

        private void RaiseSaveCanExecute()
        {

        }
        private void OnPropertyChange([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
