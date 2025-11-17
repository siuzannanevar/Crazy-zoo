// AddAnimalWindow.xaml.cs
using System;
using System.Windows;
using Crazy_zoo.Modules;
using Crazy_zoo.Animals;

namespace Crazy_zoo.Modules
{
    public partial class AddAnimalWindow : Window
    {
        public Animal? CreatedAnimal { get; private set; }

        public AddAnimalWindow()
        {
            InitializeComponent();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnAddClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) || string.IsNullOrWhiteSpace(SpeciesBox.Text) || !int.TryParse(AgeBox.Text, out int age))
            {
                MessageBox.Show("Please fill Name, Species and valid Age.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string species = SpeciesBox.Text.Trim();

            Animal animal = species.Trim().ToLower() switch
            {
                "lion" => new Lion(),
                "sheep" => new Sheep(),
                "parrot" => new Parrot(),
                "unicorn" => new Unicorn(),
                "dragon" => new Dragon(),
                "capybara" => new Capybara(),
                "dolphin" => new Dolphin(),
                "shark" => new Shark(),
                "whale" => new Whale(),
                _ => new CustomAnimal { CrazyText = CrazyTextBox.Text.Trim() }
            };

            animal.Name = NameBox.Text.Trim();
            animal.Species = char.ToUpper(species[0]) + species.Substring(1).ToLower(); 
            animal.Age = age;

            CreatedAnimal = animal;
            DialogResult = true;
            Close();
        }
    }
}
