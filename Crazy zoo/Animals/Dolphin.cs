using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;

namespace Crazy_zoo.Animals
{
    public class Dolphin : Animal, ICrazyAction
    {
        public override string MakeSound() => $"says: 🐬🐬🐬!";
        public string ActCrazy()
        {
            return $" {Name} jumps from the water!";
        }
    }
}