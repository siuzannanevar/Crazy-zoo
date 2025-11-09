using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazy_zoo.Animals
{
    public class Shark : Animal, ICrazyAction
    {
        public Shark() { }
        public override string MakeSound() => $"says: 🦈🦈🦈!";
        public string ActCrazy()
        {
            return $" {Name} circling in the water!";
        }
    }
}
