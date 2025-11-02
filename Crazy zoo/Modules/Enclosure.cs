using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crazy_zoo.Animals;

namespace Crazy_zoo.Modules
{
    public class Enclosure<T> where T : Animal
    {
        private readonly List<T> _animals = new();

        public event EventHandler<AnimalEventArgs>? AnimalJoinedInSameEnclosure;
        public event EventHandler<FoodEventArgs>? FoodDropped;

        public void Add(T animal)
        {
            if (animal == null) return;
            _animals.Add(animal);
            AnimalJoinedInSameEnclosure?.Invoke(this, new AnimalEventArgs(animal));
        }

        public void Remove(T animal)
        {
            if (animal == null) return;
            _animals.Remove(animal);
        }

        public IEnumerable<T> GetAll() => _animals;

        public async Task DropFoodSequentiallyAsync(string food, Func<T, string, Task> feedAction)
        {
            if (string.IsNullOrWhiteSpace(food) || !_animals.Any()) return;

            FoodDropped?.Invoke(this, new FoodEventArgs(food));

            foreach (var animal in _animals)
            {
                await feedAction(animal, food);
            }
        }
    }

    public class AnimalEventArgs : EventArgs
    {
        public Animal Animal { get; }
        public AnimalEventArgs(Animal animal) => Animal = animal;
    }

    public class FoodEventArgs : EventArgs
    {
        public string Food { get; }
        public FoodEventArgs(string food) => Food = food;
    }
}
