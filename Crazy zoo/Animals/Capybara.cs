using Crazy_zoo.Animals.Interfaces;
using Crazy_zoo.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazy_zoo.Animals
{
    public class Capybara : Animal, ICrazyAction
    {
        public Capybara() { }
        public override string MakeSound() => $"Capybara, capybara, capybara, ca-py-ba-ra!";
        public string ActCrazy()
        {
            return $" {Name} scretcjing the belly!";
        }
    }
}
