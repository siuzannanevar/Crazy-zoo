using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Crazy_zoo.Animals
{
    public class Sheep : Animal, ICrazyAction
    {   
        public Sheep() { }
        public override string MakeSound() => "Beeee!";

        public string ActCrazy()
        {
            return $"{Name} teleported to the Moon🌘";
        }
    }
}
