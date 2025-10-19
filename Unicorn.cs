namespace Crazy_zoo.Animals
{
    public class Unicorn: Animal, ICrazyAction
    {
        public override string Species => "Unicorn";

        public override void MakeSound(Action<string> output)
        {
            output($"{Name} says: 🦄🦄🦄!");
        }

        public void ActCrazy(IEnumerable<Animal> allAnimals, Action<string> log)
        {
            log($"{Name} teleports in time!");
        }
    }

}
