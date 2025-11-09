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
    public class Lion : Animal, ICrazyAction
    {
        public Lion() { }
        public override string MakeSound() => "Raaaaw!";
        public string ActCrazy()
        {
            return $" Lion {Name} fell in love with the sheep❤️";
        }
    }
}
