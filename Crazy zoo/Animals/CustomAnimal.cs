using Crazy_zoo.Animals.Interfaces;

namespace Crazy_zoo.Modules
{
    public class CustomAnimal : Animal, ICrazyAction
    {
        public string CrazyText { get; set; }

        public override string MakeSound()
        {
            return "??? (mystery sound)";
        }

        public string ActCrazy()
        {
            return string.IsNullOrWhiteSpace(CrazyText)
                ? $"{Name} is doing something crazy!"
                : CrazyText;
        }
    }
}