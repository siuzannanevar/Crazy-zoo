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
    public class Parrot : Animal, IFlyable, ICrazyAction
    {
        public Parrot() { }
        private bool IsFlying = false;

        public override string MakeSound() => "Chirp!";

        public void Fly()
        {
            IsFlying = !IsFlying;
        }

        public string ActCrazy()
        {
            Fly();
            return IsFlying
                ? $"{Name} is flying and singing the song🎶"
                : $"{Name} can't fly";
        }
    }
}
