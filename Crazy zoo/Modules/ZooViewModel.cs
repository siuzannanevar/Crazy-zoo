using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Crazy_zoo.Animals;
using Crazy_zoo.Animals.Interfaces;

namespace Crazy_zoo.Modules
{
    public class ZooViewModel : BaseViewModel
    {
        private readonly Enclosure<Animal> _enclosure;
        private readonly Enclosure<Animal> _secondEnclosure;
        private readonly DispatcherTimer _nightTimer;

        public ObservableCollection<Animal> Animals { get; } = new();
        public ObservableCollection<Animal> SecondAnimals { get; } = new();
        public ObservableCollection<string> Log { get; } = new();

        private Animal? _selectedAnyAnimal;
        public Animal? SelectedAnyAnimal
        {
            get => _selectedAnyAnimal;
            set
            {
                _selectedAnyAnimal = value;
                OnPropertyChanged();
                RemoveAnimalCommand.RaiseCanExecuteChanged();
                MakeSoundCommand.RaiseCanExecuteChanged();
                CrazyActionCommand.RaiseCanExecuteChanged();
            }
        }

        public string NewSpecies { get => _newSpecies; set { _newSpecies = value; OnPropertyChanged(); } }
        public string NewName { get => _newName; set { _newName = value; OnPropertyChanged(); } }
        public string NewAge { get => _newAge; set { _newAge = value; OnPropertyChanged(); } }
        public string NewCrazyText { get => _newCrazyText; set { _newCrazyText = value; OnPropertyChanged(); } }
        public string FoodInput { get => _foodInput; set { _foodInput = value; OnPropertyChanged(); FeedCommand.RaiseCanExecuteChanged(); } }

        private string _newSpecies = "";
        private string _newName = "";
        private string _newAge = "";
        private string _newCrazyText = "";
        private string _foodInput = "";
        private string _stats = "";

        public string Stats { get => _stats; set { _stats = value; OnPropertyChanged(); } }

        public RelayCommand AddAnimalCommand { get; }
        public RelayCommand RemoveAnimalCommand { get; }
        public AsyncRelayCommand FeedCommand { get; }
        public RelayCommand MakeSoundCommand { get; }
        public RelayCommand CrazyActionCommand { get; }
        public RelayCommand StartNightEventCommand { get; }

        public ZooViewModel()
        {
            _enclosure = new Enclosure<Animal>();
            _secondEnclosure = new Enclosure<Animal>();
            _nightTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            _nightTimer.Tick += NightEvent;

            AddAnimalCommand = new RelayCommand(AddAnimalFromInput);
            RemoveAnimalCommand = new RelayCommand(RemoveSelectedAnimal, () => SelectedAnyAnimal != null);
            FeedCommand = new AsyncRelayCommand(async () => await FeedAnimalAsync(FoodInput),
                                                () => !string.IsNullOrWhiteSpace(FoodInput));
            MakeSoundCommand = new RelayCommand(MakeSound, () => SelectedAnyAnimal != null);
            CrazyActionCommand = new RelayCommand(PerformCrazyAction, () => SelectedAnyAnimal != null);
            StartNightEventCommand = new RelayCommand(StartNightEvent);

            _enclosure.AnimalJoinedInSameEnclosure += (s, e) => Animals.Add(e.Animal);
            _secondEnclosure.AnimalJoinedInSameEnclosure += (s, e) => SecondAnimals.Add(e.Animal);

            _enclosure.Add(new Lion { Name = "Edward", Age = 17, Species = "Lion" });
            _enclosure.Add(new Sheep { Name = "Bella", Age = 16, Species = "Sheep" });
            _enclosure.Add(new Parrot { Name = "Kesha", Age = 3, Species = "Parrot" });
            _enclosure.Add(new Unicorn { Name = "Silver", Age = 212, Species = "Unicorn" });
            _enclosure.Add(new Dragon { Name = "Drakharis", Age = 156, Species = "Dragon" });
            _enclosure.Add(new Capybara { Name = "Cappy", Age = 4, Species = "Capybara" });
            _secondEnclosure.Add(new Shark { Name = "Jaws", Age = 8, Species = "Shark" });
            _secondEnclosure.Add(new Whale { Name = "Willy", Age = 20, Species = "Whale" });

            UpdateStats();
        }

        private void AddAnimalFromInput()
        {
            if (string.IsNullOrWhiteSpace(NewName))
            {
                MessageBox.Show(ErrorMessages.errorEmptyName);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewSpecies))
            {
                MessageBox.Show(ErrorMessages.errorEmptySpecies);
                return;
            }

            if (!int.TryParse(NewAge, out int age))
            {
                MessageBox.Show(ErrorMessages.errorInvalidAge);
                return;
            }

            if (age < 0)
            {
                MessageBox.Show(ErrorMessages.errorWrongAge);
                return;
            }

            var animal = new CustomAnimal
            {
                Name = NewName,
                Species = NewSpecies,
                Age = age,
                CrazyText = NewCrazyText
            };

            if (Animals.Count < 5)
                _enclosure.Add(animal);
            else
                _secondEnclosure.Add(animal);

            Log.Insert(0, $"New animal added: {animal.Name} the {animal.Species}, age {animal.Age}");
            NewName = NewSpecies = NewCrazyText = NewAge = "";
            UpdateStats();
        }

        private void RemoveSelectedAnimal()
        {
            var animal = SelectedAnyAnimal;
            if (animal == null)
            {
                MessageBox.Show(ErrorMessages.errorNoOneSelected);
                return;
            }

            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Animals.Contains(animal))
                    {
                        Animals.Remove(animal);
                        _enclosure.Remove(animal);
                    }
                    else if (SecondAnimals.Contains(animal))
                    {
                        SecondAnimals.Remove(animal);
                        _secondEnclosure.Remove(animal);
                    }

                    Log.Insert(0, $"{animal.Name} the {animal.Species} was removed from the zoo!");
                    SelectedAnyAnimal = null;
                    UpdateStats();
                });
            }
            catch (Exception)
            {
                MessageBox.Show(ErrorMessages.errorRemove);
            }
        }

        private void MakeSound()
        {
            if (SelectedAnyAnimal != null)
                Log.Insert(0, $"{SelectedAnyAnimal.Name} the {SelectedAnyAnimal.Species} says: {SelectedAnyAnimal.MakeSound()}");
        }

        private void PerformCrazyAction()
        {
            if (SelectedAnyAnimal is ICrazyAction crazy)
                Log.Insert(0, $"{SelectedAnyAnimal.Name} the {SelectedAnyAnimal.Species} {crazy.ActCrazy()}");
            else
                MessageBox.Show(ErrorMessages.errorCrazyAction);
        }

        public async Task FeedAnimalAsync(string food)
        {
            if (string.IsNullOrWhiteSpace(food)) return;

            var all = Animals.Concat(SecondAnimals).ToList();
            Log.Insert(0, $"Feeding started with {food}");

            foreach (var a in all)
            {
                Log.Insert(0, $"{a.Name} the {a.Species} starts eating {food}...");
                await Task.Delay(500);
                Log.Insert(0, $"{a.Name} the {a.Species} finished eating {food}!");
            }

            Log.Insert(0, $"All animals have eaten their {food}");
        }

        private void UpdateStats()
        {
            var text1 = Animals.Any()
                ? string.Join("\n", Animals.GroupBy(a => a.Species)
                    .Select(g => $"{g.Key}: {g.Count()} (avg {g.Average(a => a.Age):F1})"))
                : "— empty —";

            var text2 = SecondAnimals.Any()
                ? string.Join("\n", SecondAnimals.GroupBy(a => a.Species)
                    .Select(g => $"{g.Key}: {g.Count()} (avg {g.Average(a => a.Age):F1})"))
                : "— empty —";

            Stats = $"Enclosure 1:\n{text1}\n\nEnclosure 2:\n{text2}";
        }

        private void StartNightEvent()
        {
            if (!_nightTimer.IsEnabled)
            {
                _nightTimer.Start();
                Log.Insert(0, $"Night event will start soon ({DateTime.Now:T})...");
            }
        }

        private async void NightEvent(object? sender, EventArgs e)
        {
            _nightTimer.Stop();
            Log.Insert(0, $"Night event started at {DateTime.Now:T}");

            var allAnimals = Animals.Concat(SecondAnimals).Distinct().ToList();
            foreach (var animal in allAnimals)
            {
                if (animal is ICrazyAction crazy)
                {
                    Log.Insert(0, $"{animal.Name} the {animal.Species} {crazy.ActCrazy()}");
                    await Task.Delay(300);
                }
                else
                {
                    Log.Insert(0, $"{animal.Name} the {animal.Species} is sleeping peacefully");
                    await Task.Delay(200);
                }
            }

            Log.Insert(0, $"Night event finished at {DateTime.Now:T} — animals are waking up!");
        }
    }
}
