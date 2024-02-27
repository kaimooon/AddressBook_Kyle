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
                using (StreamReader reader = new StreamReader("contacts.csv"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] contactInfo = line.Split(',');
                        contacts.Add(contactInfo);
                    }
                }
                foreach (var contact in contacts)
                {
                    ListofNames.Items.Add(contact[0]);
                    ListofAddress.Items.Add(contact[1]);
                    ListofPhoneNumber.Items.Add(contact[2]);
                    ListofEmail.Items.Add(contact[3]);
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
                ListofNames.Items.Add(newContact[0]); // Only add to ListofNames
                ListofAddress.Items.Add(newContact[1]);
                ListofPhoneNumber.Items.Add(newContact[2]);
                ListofEmail.Items.Add(newContact[3]);
                SaveContacts(); // Save contacts after adding
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to AddUpdateContactWindow for updating an existing contact
            if (ListofNames.SelectedIndex != -1)
            {
                string[] selectedContact = contacts[ListofNames.SelectedIndex];

                AddUpdateContactWindow addUpdateContactWindow = new AddUpdateContactWindow(selectedContact[0], selectedContact[1], selectedContact[2], selectedContact[3]);
                if (addUpdateContactWindow.ShowDialog() == true)
                {
                    selectedContact[0] = addUpdateContactWindow.Name;
                    selectedContact[1] = addUpdateContactWindow.Address;
                    selectedContact[2] = addUpdateContactWindow.PhoneNumber;
                    selectedContact[3] = addUpdateContactWindow.Email;
                    // Update only the corresponding item in ListofNames
                    ListofNames.Items[ListofNames.SelectedIndex] = selectedContact[0];
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
            ListofNames.Items.Clear(); // Clear existing items

            foreach (var contact in contacts)
            {
                // Check if the Name contains the search term
                if (contact[0].ToLower().Contains(searchTerm))
                {
                    ListofNames.Items.Add(contact[0]);
                }
            }
        }
    }
}
