using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Organizer;
using System.Runtime.CompilerServices;

namespace EventPlanner.Tests
{
    [TestClass]
    public class EventManagerTests
    {
        private const string TestFilePath = "events.json";
        private EventManager eventManager;

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
            eventManager = new EventManager();
        }

        [TestMethod]
        public void AddEvent_ShouldAddEventToList()
        {
            var newEvent = new Event
            {
                Type = "meeting",
                Date = DateTime.Now,
                Duration = 30,
                Description = "Team meeting"
            };
            eventManager.AddEvent(newEvent);

            eventManager.ViewEvents(); 
        }

        [TestMethod]
        public void EditEvent_ShouldUpdateExistingEvent()
        {
            var initialEvent = new Event
            {
                Type = "call",
                Date = DateTime.Now,
                Duration = 15,
                Description = "Client call"
            };
            eventManager.AddEvent(initialEvent);

            var updatedEvent = new Event
            {
                Type = "meeting",
                Date = DateTime.Now.AddHours(1),
                Duration = 45,
                Description = "Updated meeting"
            };

            eventManager.EditEvent(0, updatedEvent);

            var events = eventManager.ViewEvents();
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("meeting", events[0].Type);
            Assert.AreEqual("Updated meeting", events[0].Description);
        }

        [TestMethod]
        public void LoadEvents_ShouldLoadEventsFromFile()
        {
            var eventsToSave = new List<Event>
    {
        new Event { Type = "birthday", Date = DateTime.Now, Duration = 120, Description = "Birthday party" },
        new Event { Type = "call", Date = DateTime.Now.AddDays(1), Duration = 30, Description = "Follow-up call" }
    };

            var json = JsonSerializer.Serialize(eventsToSave, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("events.json", json);

            var eventManager = new EventManager(); 

            var eventsLoaded = eventManager.ViewEvents(); 
            Assert.AreEqual(2, eventsLoaded.Count);
            Assert.AreEqual("birthday", eventsLoaded[0].Type);
            Assert.AreEqual("call", eventsLoaded[1].Type);
        }


        [TestMethod]
        public void DeleteEvent_ShouldRemoveEventFromList()
        {
            var newEvent = new Event
            {
                Type = "task",
                Date = DateTime.Now,
                Duration = 30,
                Description = "Complete report"
            };
            eventManager.AddEvent(newEvent);

            eventManager.DeleteEvent(0);

            var events = eventManager.ViewEvents();
            Assert.AreEqual(0, events.Count);
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }
    }
}
