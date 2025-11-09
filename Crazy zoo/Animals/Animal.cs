using Crazy_zoo.Animals.Interfaces;

namespace Crazy_zoo.Modules
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Species { get; set; } = "";
        public int Age { get; set; }
        public int? EnclosureId { get; set; }

        public virtual string Describe() => $"{Name} ({Species}), {Age} years old.";
        public virtual string MakeSound() => "";
    }
}
