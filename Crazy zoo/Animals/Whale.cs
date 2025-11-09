using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazy_zoo.Animals
{
    public class Whale : Animal, ICrazyAction
    {
        public Whale() { }
        public override string MakeSound() => $"🐳​🐳​🐳!​";
        public string ActCrazy()
        {
            return $" {Name} calls for relatives!";
        }
    }
}
