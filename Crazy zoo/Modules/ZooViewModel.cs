using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Crazy_zoo.Modules;
using Crazy_zoo.Animals;
using Crazy_zoo.Logging;
using Crazy_zoo.Data;
using Crazy_zoo.Animals.Interfaces;

namespace Crazy_zoo.Modules
{
    public class ZooViewModel : BaseViewModel
    {
        private readonly DispatcherTimer _nightTimer;
        private readonly ILogger _logger;
        private readonly IRepository<Animal> _repository;

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

        public ZooViewModel(ILogger logger, IRepository<Animal> repository)
        {
            _logger = logger;
            _repository = repository;

            _logger.Log("ZooViewModel initialized.");

            _nightTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            _nightTimer.Tick += NightEvent;

            AddAnimalCommand = new RelayCommand(AddAnimalFromInput);
            RemoveAnimalCommand = new RelayCommand(RemoveSelectedAnimal, () => SelectedAnyAnimal != null);
            FeedCommand = new AsyncRelayCommand(async () => await FeedAnimalAsync(FoodInput),
                                                () => !string.IsNullOrWhiteSpace(FoodInput));
            MakeSoundCommand = new RelayCommand(MakeSound, () => SelectedAnyAnimal != null);
            CrazyActionCommand = new RelayCommand(PerformCrazyAction, () => SelectedAnyAnimal != null);
            StartNightEventCommand = new RelayCommand(StartNightEvent);

            LoadAnimals();
        }

        private void LoadAnimals()
        {
            try
            {
                var animalsFromDb = _repository.GetAll().ToList();

                if (animalsFromDb.Any())
                {
                    foreach (var animal in animalsFromDb)
                    {
                        var concrete = EnsureConcreteAnimal(animal);
                        concrete.EnclosureId ??= 1;

                        if (concrete.EnclosureId == 1)
                            Animals.Add(concrete);
                        else
                            SecondAnimals.Add(concrete);
                    }
                    _logger.Log($"Loaded {animalsFromDb.Count} animals from database.");
                }
                else
                {
                    AddDefaultAnimals();
                    _logger.Log("Database empty — added default animals.");
                }

                UpdateStats();
            }
            catch (Exception ex)
            {
                _logger.Log($"Error loading animals from database: {ex.Message}");
                MessageBox.Show(ErrorMessages.errorDataBase);
            }
        }

        private Animal EnsureConcreteAnimal(Animal animal)
        {
            string species = animal.Species.Trim();

            Animal concrete = species switch
            {
                "Lion" => new Lion(),
                "Sheep" => new Sheep(),
                "Parrot" => new Parrot(),
                "Unicorn" => new Unicorn(),
                "Dragon" => new Dragon(),
                "Capybara" => new Capybara(),
                "Dolphin" => new Dolphin(),
                "Shark" => new Shark(),
                "Whale" => new Whale(),
                _ => new CustomAnimal() { CrazyText = (animal as CustomAnimal)?.CrazyText ?? "" }
            };

            concrete.Id = animal.Id;
            concrete.Name = animal.Name.Trim();
            concrete.Species = species;
            concrete.Age = animal.Age;
            concrete.EnclosureId = animal.EnclosureId;

            return concrete;
        }

        private void AddDefaultAnimals()
        {
            var defaultAnimals = new Animal[]
            {
                new Lion { Name = "Edward", Age = 17, Species = "Lion" },
                new Sheep { Name = "Bella", Age = 16, Species = "Sheep" },
                new Parrot { Name = "Kesha", Age = 3, Species = "Parrot" },
                new Unicorn { Name = "Silver", Age = 212, Species = "Unicorn" },
                new Dragon { Name = "Drakharis", Age = 156, Species = "Dragon" },
                new Capybara { Name = "Capy", Age = 2, Species = "Capybara" },
                new Dolphin { Name = "Dolph", Age = 15, Species = "Dolphin" },
                new Shark { Name = "Jaw", Age = 8, Species = "Shark" },
                new Whale { Name = "Wally", Age = 47, Species = "Whale" }
            };

            foreach (var animal in defaultAnimals)
            {
                animal.EnclosureId = (Animals.Count + SecondAnimals.Count) < 5 ? 1 : 2;

                if (animal.EnclosureId == 1)
                    Animals.Add(animal);
                else
                    SecondAnimals.Add(animal);

                _repository.Add(animal);
            }

            UpdateStats();
        }

        private void AddAnimalFromInput()
        {
            var wnd = new AddAnimalWindow
            {
                Owner = Application.Current.MainWindow
            };

            bool? result = wnd.ShowDialog();
            if (result != true || wnd.CreatedAnimal == null)
                return;

            var animal = wnd.CreatedAnimal;

            animal.EnclosureId = (Animals.Count + SecondAnimals.Count) < 5 ? 1 : 2;

            if (animal.EnclosureId == 1)
                Animals.Add(animal);
            else
                SecondAnimals.Add(animal);

            _repository.Add(animal);
            Log.Insert(0, $"New animal added: {animal.Name} the {animal.Species}, age {animal.Age}");
            UpdateStats();
        }

        private void RemoveSelectedAnimal()
        {
            var animal = SelectedAnyAnimal;
            if (animal == null) { MessageBox.Show(ErrorMessages.errorNoOneSelected); return; }

            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Animals.Contains(animal)) Animals.Remove(animal);
                    else if (SecondAnimals.Contains(animal)) SecondAnimals.Remove(animal);

                    _repository.Remove(animal);
                    SelectedAnyAnimal = null;
                    UpdateStats();
                });
            }
            catch (Exception) { MessageBox.Show(ErrorMessages.errorRemove); }
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
            foreach (var a in all)
            {
                Log.Insert(0, $"{a.Name} the {a.Species} starts eating {food}...");
                await Task.Delay(500);
                Log.Insert(0, $"{a.Name} the {a.Species} finished eating {food}!");
            }
        }

        private void UpdateStats()
        {
            var text1 = Animals.Any() ? string.Join("\n", Animals.GroupBy(a => a.Species)
                .Select(g => $"{g.Key}: {g.Count()} (avg {g.Average(a => a.Age):F1})")) : "— empty —";

            var text2 = SecondAnimals.Any() ? string.Join("\n", SecondAnimals.GroupBy(a => a.Species)
                .Select(g => $"{g.Key}: {g.Count()} (avg {g.Average(a => a.Age):F1})")) : "— empty —";

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
