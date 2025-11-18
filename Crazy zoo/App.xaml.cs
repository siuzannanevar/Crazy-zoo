using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Crazy_zoo.Modules;
using Crazy_zoo.Logging;
using Crazy_zoo.Data;
using Crazy_zoo.Animals;

namespace Crazy_zoo
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ILogger logger = new ConsoleLogger();
            IRepository<Animal> repository = new InMemoryRepository<Animal>();

            var viewModel = new ZooViewModel(logger, repository);

            var mainWindow = new MainWindow(viewModel);
            mainWindow.Show();
        }
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }
    }

    public class InMemoryRepository<T> : IRepository<T> where T : Animal
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);
        public void Remove(T item) => _items.Remove(item);
        public IEnumerable<T> GetAll() => _items;

        public T? Find(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);
    }
}
