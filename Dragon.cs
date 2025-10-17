namespace CrazyZoo.Animals
{
    public class Dragon: Animal, ICrazyAction
    {
        public override string Species => "Dragon";

        public override void MakeSound(Action<string> output)
        {
            output($"{Name} says: Aaargh🐉🔥!");
        }

        public void ActCrazy(IEnumerable<Animal> allAnimals, Action<string> log)
        {
            log($"{Name} is starting the fire!");
        }
    }
}