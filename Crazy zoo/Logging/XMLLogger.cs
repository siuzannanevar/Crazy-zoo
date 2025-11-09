using System;
using System.IO;
using System.Xml.Linq;

namespace Crazy_zoo.Logging
{
    public class XMLLogger : ILogger
    {
        private readonly string _filePath = "log.xml";

        public void Log(string message)
        {
            var logEntry = new XElement("Log",
                new XElement("Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("Message", message)
            );

            XDocument doc;
            if (File.Exists(_filePath))
            {
                doc = XDocument.Load(_filePath);
                doc.Root.Add(logEntry);
            }
            else
            {
                doc = new XDocument(new XElement("Logs", logEntry));
            }
            doc.Save(_filePath);
        }
    }
}
