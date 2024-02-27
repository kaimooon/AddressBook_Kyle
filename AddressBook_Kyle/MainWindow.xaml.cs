using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                        Name_Box.Items.Add(contactInfo[0]); // Add name to ComboBox
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Contacts file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Name_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = Name_Box.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < contacts.Count)
            {
                string[] selectedContact = contacts[selectedIndex];
                // Display corresponding address, phone number, and email in text boxes
                Address.Text = selectedContact[1];
                Phone_Number.Text = selectedContact[2];
                Email.Text = selectedContact[3];
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
                Name_Box.Items.Add(newContact[0]); // Add new contact name to ComboBox
                SaveContacts(); // Save contacts after adding
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to AddUpdateContactWindow for updating an existing contact
            int selectedIndex = Name_Box.SelectedIndex;
            if (selectedIndex != -1)
            {
                string[] selectedContact = contacts[selectedIndex];

                AddUpdateContactWindow addUpdateContactWindow = new AddUpdateContactWindow(selectedContact[0], selectedContact[1], selectedContact[2], selectedContact[3]);
                if (addUpdateContactWindow.ShowDialog() == true)
                {
                    selectedContact[0] = addUpdateContactWindow.Name;
                    selectedContact[1] = addUpdateContactWindow.Address;
                    selectedContact[2] = addUpdateContactWindow.PhoneNumber;
                    selectedContact[3] = addUpdateContactWindow.Email;

                    // Update the ComboBox item (name) for the selected contact
                    Name_Box.Items[selectedIndex] = selectedContact[0];

                    // If the selected index has changed, update the details displayed
                    if (selectedIndex == Name_Box.SelectedIndex)
                    {
                        Address.Text = selectedContact[1];
                        Phone_Number.Text = selectedContact[2];
                        Email.Text = selectedContact[3];
                    }

                    SaveContacts(); // Save contacts after updating
                }
            }
            else
            {
                MessageBox.Show("Please select a contact to update.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.ToLower();
            Name_Box.Items.Clear(); // Clear existing items

            foreach (var contact in contacts)
            {
                // Check if the Name contains the search term
                if (contact[0].ToLower().Contains(searchTerm))
                {
                    Name_Box.Items.Add(contact[0]); // Add matching contact name to ComboBox
                }
            }

            // Automatically open the ComboBox
            Name_Box.IsDropDownOpen = true;
        }

        private void Name_Box_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = Name_Box.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < contacts.Count)
            {
                string[] selectedContact = contacts[selectedIndex];
                // Display corresponding address, phone number, and email in text boxes
                Address.Text = selectedContact[1]; // Index 1 is the address
                Phone_Number.Text = selectedContact[2]; // Index 2 is the phone number
                Email.Text = selectedContact[3]; // Index 3 is the email
            }
        }
    }
}
