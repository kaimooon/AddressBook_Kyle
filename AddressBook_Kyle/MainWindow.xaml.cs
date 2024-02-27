using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace AddressBook_Kyle
{
    public partial class MainWindow : Window
    {
        private List<string[]> contacts;

        public MainWindow()
        {
            InitializeComponent();
            contacts = new List<string[]>();
            LoadContacts();
            
        }

        private void LoadContacts()
        {
            try
            {
                // Read contacts from CSV file
                using (StreamReader reader = new StreamReader("contacts.csv"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] contactInfo = line.Split(',');
                        contacts.Add(contactInfo);
                    }
                }

                // Populate list box with loaded contacts
                foreach (var contact in contacts)
                {
                    ListofContacts.Items.Add($"{contact[0]}, {contact[1]}, {contact[2]}, {contact[3]}");
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Contacts file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveContacts()
        {
            try
            {
                // Write contacts to CSV file
                using (StreamWriter writer = new StreamWriter("contacts.csv"))
                {
                    foreach (var contact in contacts)
                    {
                        writer.WriteLine($"{contact[0]},{contact[1]},{contact[2]},{contact[3]}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving contacts: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to AddUpdateContactWindow for adding a new contact
            AddUpdateContactWindow addUpdateContactWindow = new AddUpdateContactWindow();
            if (addUpdateContactWindow.ShowDialog() == true)
            {
                string[] newContact = { addUpdateContactWindow.Name, addUpdateContactWindow.Address, addUpdateContactWindow.PhoneNumber, addUpdateContactWindow.Email };
                contacts.Add(newContact);
                ListofContacts.Items.Add($"{newContact[0]}, {newContact[1]}, {newContact[2]}, {newContact[3]}");
                SaveContacts(); // Save contacts after adding
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to AddUpdateContactWindow for updating an existing contact
            if (ListofContacts.SelectedIndex != -1)
            {
                string[] selectedContact = contacts[ListofContacts.SelectedIndex];

                AddUpdateContactWindow addUpdateContactWindow = new AddUpdateContactWindow(selectedContact[0], selectedContact[1], selectedContact[2], selectedContact[3]);
                if (addUpdateContactWindow.ShowDialog() == true)
                {
                    selectedContact[0] = addUpdateContactWindow.Name;
                    selectedContact[1] = addUpdateContactWindow.Address;
                    selectedContact[2] = addUpdateContactWindow.PhoneNumber;
                    selectedContact[3] = addUpdateContactWindow.Email;
                    ListofContacts.Items[ListofContacts.SelectedIndex] = $"{selectedContact[0]}, {selectedContact[1]}, {selectedContact[2]}, {selectedContact[3]}";
                    SaveContacts(); // Save contacts after updating
                }
            }
            else
            {
                MessageBox.Show("Please select a contact to update.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void searchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.ToLower();
            ListofContacts.Items.Clear(); // Clear existing items

            foreach (var contact in contacts)
            {
                // Concatenate contact fields for searching
                string contactInfo = string.Join(",", contact).ToLower();

                // Check if any field contains the search term
                if (contactInfo.Contains(searchTerm))
                {
                    ListofContacts.Items.Add($"{contact[0]}, {contact[1]}, {contact[2]}, {contact[3]}");
                }
            }
        }
    }
}
