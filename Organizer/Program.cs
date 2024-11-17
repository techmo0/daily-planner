using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EventPlanner
{
    public class Event
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; } // Duration in minutes
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Type} on {Date.ToShortDateString()} at {Date.ToShortTimeString()} for {Duration} minutes: {Description}";
        }
    }

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

    class Program
    {
        static void Main(string[] args)
        {
            var eventManager = new EventManager();

            while (true)
            {
                Console.WriteLine("\nEvent Planner");
                Console.WriteLine("1. Add Event");
                Console.WriteLine("2. View Events");
                Console.WriteLine("3. Edit Event");
                Console.WriteLine("4. Delete Event");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var newEvent = CreateEvent();
                        eventManager.AddEvent(newEvent);
                        break;
                    case "2":
                        eventManager.ViewEvents();
                        break;
                    case "3":
                        Console.Write("Enter event index to edit: ");
                        if (int.TryParse(Console.ReadLine(), out int editIndex))
                        {
                            var updatedEvent = CreateEvent();
                            eventManager.EditEvent(editIndex, updatedEvent);
                        }
                        break;
                    case "4":
                        Console.Write("Enter event index to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteIndex))
                        {
                            eventManager.DeleteEvent(deleteIndex);
                        }
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static Event CreateEvent()
        {
            Console.Write("Enter event type (meeting, call, birthday, task): ");
            string type = Console.ReadLine();

            Console.Write("Enter event date and time (yyyy-mm-dd hh:mm): ");
            DateTime date = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter duration in minutes (minimum 15): ");
            int duration = int.Parse(Console.ReadLine());

            if (duration < 15)
                throw new ArgumentException("Duration must be at least 15 minutes.");

            Console.Write("Enter description: ");
            string description = Console.ReadLine();

            return new Event { Type = type, Date = date, Duration = duration, Description = description };
        }
    }
}
