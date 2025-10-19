using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;

namespace Crazy_zoo.Animals
{
    public class Unicorn : Animal, ICrazyAction
    {
        public override string MakeSound() => $"says: 🦄🦄🦄!";
        public string ActCrazy()
        {
            return $"{Name} teleports in time!";
        }
    }
}