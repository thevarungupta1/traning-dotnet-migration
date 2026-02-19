using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly BindingSource _bindingSource = new();
        private readonly List<Contact> _contacts = new();
        private int _nextId = 1;
        public Form1()
        {
            InitializeComponent();
            SeedSampleData();
            BindGrid();
        }

        // Data Setup
        private void SeedSampleData()
        {
            _contacts.AddRange(new[]
            {
                new Contact { Id = _nextId++, Name = "Alice Smith", Email = "alice@example.com", Phone = "555-1234", Department = "Sales" },
                new Contact { Id = _nextId++, Name = "Bob Johnson", Email = "bob@example.com", Phone = "555-5678", Department = "Marketing" },
                new Contact { Id = _nextId++, Name = "Charlie Brown", Email = "charle@example.com", Phone = "555-9012", Department = "IT" },
                new Contact { Id = _nextId++, Name = "Diana Prince", Email = "diana@example.com", Phone = "555-3456", Department = "HR" },
            });
        }

        private void BindGrid()
        {
            _bindingSource.DataSource = new BindingList<Contact>(_contacts);
            dataGridView1.DataSource = _bindingSource;

            if(dataGridView1.Columns["Id"] is DataGridViewColumn colId)
            {
                colId.ReadOnly = true;
                colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colId.Width = 50;
            } 
        }

        private void RefreshGrid()
        {
            _bindingSource.ResetBindings(false);

        }

        private void UpdateRecordCount()
        {

        }
    }
}
