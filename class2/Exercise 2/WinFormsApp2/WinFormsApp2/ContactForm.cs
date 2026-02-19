using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class ContactForm : Form
    {
        private readonly ContactFormViewModel _viewModel;
        public ContactForm(Contact? existingContact = null)
        {
            InitializeComponent();
            _viewModel = new ContactFormViewModel(existingContact);
        }

        public bool Saved => _viewModel.IsSaved;

        public void ApplyTo(Contact contact) => _viewModel.ApplyTo(contact);

        // MVVM writing - Button.Command 
        private void SetupCommandBindings()
        {
            button1.Command = _viewModel.SaveCommand;
            button2.Command = _viewModel.ClearCommand;
            button3.Command = _viewModel.CancelCommand;

            _viewModel.RequestClose += saved =>
            {
                DialogResult = saved ? DialogResult.OK : DialogResult.Cancel;
                Close();
            };
        }

        // data binding - view model + control
        private void SetupDataBindings()
        {
            textBox1.DataBindings.Add(
                nameof(TextBox.Text), _viewModel, nameof(_viewModel.Name), 
                false, DataSourceUpdateMode.OnPropertyChanged);

            textBox2.DataBindings.Add(
                nameof(TextBox.Text), _viewModel, nameof(_viewModel.Email),
                false, DataSourceUpdateMode.OnPropertyChanged);

            textBox3.DataBindings.Add(
                nameof(TextBox.Text), _viewModel, nameof(_viewModel.Phone),
                false, DataSourceUpdateMode.OnPropertyChanged);


        }
       
    }
}
