using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crazy_zoo.Modules
{
    public abstract class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Species { get; set; }

        public virtual string Describe()
        {
            return $"{Name} ({Species}), {Age} years old.";
        }

        public abstract string MakeSound();
    }
}
