using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinFormsApp1
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _statusMessage = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            ClearCommand = new RelayCommand(ExecuteClear);
        }

        // properties
        public string Username
        {
            get => _username;
            set
            {
                if(_username == value) return;
                _username = value;
                OnPropertyChange();
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChange();
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage == value) return;
                _statusMessage = value;
                OnPropertyChange();
            }
        }

        // Commands (ICommand - bound to Button.Command in .net 8)
        public ICommand LoginCommand { get; }
        public ICommand ClearCommand { get; }

        // Command implementation
        private bool CanExecuteLogin(object?  parameter)
        {
            return !string.IsNullOrWhiteSpace(Username)
                 && !string.IsNullOrWhiteSpace(Password);
        }
        private void ExecuteLogin(object? parameter)
        {
            if (Username == "admin" && Password == "123")
                StatusMessage = $"Welcome, {Username}! Login Successful";
            else
                StatusMessage = "Invalid Crediantials, Try Again";
        }
        private void ExecuteClear(object? parameter)
        {
            Username = string.Empty;
            Password = string.Empty;
            StatusMessage = string.Empty;
        }

        private void OnPropertyChange([CallerMemberName] string? name = null) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
