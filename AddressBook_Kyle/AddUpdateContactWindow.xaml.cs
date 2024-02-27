using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AddressBook_Kyle
{
    /// <summary>
    /// Interaction logic for AddUpdateContactWindow.xaml
    /// </summary>
    public partial class AddUpdateContactWindow : Window
    {
        public new string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public AddUpdateContactWindow(string name = "", string address = "", string phoneNumber = "", string email = "")
        {
            InitializeComponent();
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            nameTextBox.Text = Name;
            addressTextBox.Text = Address;
            phoneTextBox.Text = PhoneNumber;
            emailTextBox.Text = Email;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Name = nameTextBox.Text;
            Address = addressTextBox.Text;
            PhoneNumber = phoneTextBox.Text;
            Email = emailTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}