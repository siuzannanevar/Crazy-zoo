using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;
using System.Xml.Linq;

namespace Crazy_zoo.Animals
{
    public class Dragon : Animal, ICrazyAction
    {
        public override string MakeSound() => $"says: Aaargh🐉🔥!";
        public string ActCrazy()
        {
            return $"{Name} is starting the fire!";
        }
    }
}