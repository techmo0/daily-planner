using EventPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Organizer
{
    public class EventManager
    {
        private const string FilePath = "events.json";
        private List<Event> events;

        public EventManager()
        {
            events = LoadEvents();
        }

        public void AddEvent(Event newEvent)
        {
            events.Add(newEvent);
            SaveEvents();
        }

        public void EditEvent(int index, Event updatedEvent)
        {
            if (index >= 0 && index < events.Count)
            {
                events[index] = updatedEvent;
                SaveEvents();
            }
            else
            {
                Console.WriteLine("Invalid event index.");
            }
        }

        public void DeleteEvent(int index)
        {
            if (index >= 0 && index < events.Count)
            {
                events.RemoveAt(index);
                SaveEvents();
            }
            else
            {
                Console.WriteLine("Invalid event index.");
            }
        }

        public List<Event> ViewEvents()
        {
            for (int i = 0; i < events.Count; i++)
            {
                Console.WriteLine($"{i}: {events[i]}");
            }
            return events;
        }

        public List<Event> LoadEvents()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Event>>(json) ?? new List<Event>();
            }
            return new List<Event>();
        }

        private void SaveEvents()
        {
            var json = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
