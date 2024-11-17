using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Organizer;


namespace EventPlanner
{
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
