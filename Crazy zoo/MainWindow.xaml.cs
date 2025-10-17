using Crazy_zoo.Animals;
using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;
using System.Collections.ObjectModel;
using System.Windows;

namespace Crazy_zoo
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Animal> Animals { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            AnimalList.ItemsSource = Animals;

            Animals.Add(new Lion { Name = "Edward", Age = 17, Species = "Lion" });
            Animals.Add(new Sheep { Name = "Bella", Age = 16, Species = "Sheep" });
            Animals.Add(new Parrot { Name = "Kesha", Age = 3, Species = "Parrot" });
        }

        private void AnimalList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal animal)
            {
                AnimalDetails.Text = animal.Describe();
            }
        }

        private void MakeSound_Click(object sender, RoutedEventArgs e)
        {
            if(AnimalList.SelectedItem is not Animal animal)
            {
                MessageBox.Show(ErrorMessages.errorNoOneSelected);
                return;
            }

            LogBox.Items.Add($"{animal.Name}: {animal.MakeSound()}");
        }

        private void Feed_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is not Animal animal)
            {
                MessageBox.Show(ErrorMessages.errorNoOneSelected);
                return;
            }

            if (string.IsNullOrWhiteSpace(FoodInput.Text))
            {
                MessageBox.Show(ErrorMessages.errorFeedEmpty);
                return;
            }

            LogBox.Items.Add($"{animal.Name} ate {FoodInput.Text}");
            FoodInput.Clear();
        }

        private void Crazy_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is not Animal animal)
            {
                MessageBox.Show(ErrorMessages.errorNoOneSelected);
                return;
            }

            if (animal is ICrazyAction crazy)
                LogBox.Items.Add(crazy.ActCrazy());
            else
                MessageBox.Show(ErrorMessages.errorCrazyAction);

        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SpeciesBox.Text))
            {
                MessageBox.Show(ErrorMessages.errorEmptySpecies);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewNameBox.Text))
            {
                MessageBox.Show(ErrorMessages.errorEmptyName);
                return;
            }

            if (!int.TryParse(NewAgeBox.Text, out int age))
            {
                MessageBox.Show(ErrorMessages.errorInvalidAge);
                return;
            }

            var animal = new CustomAnimal
            {
                Species = SpeciesBox.Text,
                Name = NewNameBox.Text,
                Age = age,
                CrazyText = NewActionBox.Text
            };

            Animals.Add(animal);
            LogBox.Items.Add($"Added new animal: {animal.Species} name is {animal.Name}");

            SpeciesBox.Clear();
            NewNameBox.Clear();
            NewAgeBox.Clear();
            NewActionBox.Clear();
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalList.SelectedItem is Animal animal)
            {
                Animals.Remove(animal);
                LogBox.Items.Add($"Removed: {animal.Name}");
            }
        }
    }
}
