using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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
            // ������� ���� ����� ������ ������ ��� ������� ������������
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
            eventManager = new EventManager();
        }

        [TestMethod]
        public void AddEvent_ShouldAddEventToList()
        {
            // Arrange
            var newEvent = new Event
            {
                Type = "meeting",
                Date = DateTime.Now,
                Duration = 30,
                Description = "Team meeting"
            };

            // Act
            eventManager.AddEvent(newEvent);

            // Assert
            eventManager.ViewEvents(); // ������ �������� ����� ��� ����������� �������
                                       // ����� �� ������ �������� �������������� �������� �� ���������� �����, ���� ��� ����������.
        }

        [TestMethod]
        public void EditEvent_ShouldUpdateExistingEvent()
        {
            // Arrange
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

            // Act
            eventManager.EditEvent(0, updatedEvent);

            // Assert
            var events = eventManager.ViewEvents(); // ������ ��� ��������, ��� ��� ����� ���������� ������ �������.
            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("meeting", events[0].Type);
            Assert.AreEqual("Updated meeting", events[0].Description);
        }

        [TestMethod]
        public void LoadEvents_ShouldLoadEventsFromFile()
        {
            // Arrange
            var eventsToSave = new List<Event>
    {
        new Event { Type = "birthday", Date = DateTime.Now, Duration = 120, Description = "Birthday party" },
        new Event { Type = "call", Date = DateTime.Now.AddDays(1), Duration = 30, Description = "Follow-up call" }
    };

            // ��������� ������� � ���� ��� ������������
            var json = JsonSerializer.Serialize(eventsToSave, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("events.json", json); // ���������, ��� ���� ��������� � ���������� ����������

            // Act
            var eventManager = new EventManager(); // ������� ����� ��������� EventManager, ������� ������� LoadEvents

            // Assert
            var eventsLoaded = eventManager.ViewEvents(); // �������� ����������� �������
            Assert.AreEqual(2, eventsLoaded.Count); // ��������� ���������� ����������� �������
            Assert.AreEqual("birthday", eventsLoaded[0].Type); // ��������� ������ �������
            Assert.AreEqual("call", eventsLoaded[1].Type); // ��������� ������ �������
        }


        [TestMethod]
        public void DeleteEvent_ShouldRemoveEventFromList()
        {
            // Arrange
            var newEvent = new Event
            {
                Type = "task",
                Date = DateTime.Now,
                Duration = 30,
                Description = "Complete report"
            };
            eventManager.AddEvent(newEvent);

            // Act
            eventManager.DeleteEvent(0);

            // Assert
            var events = eventManager.ViewEvents();
            Assert.AreEqual(0, events.Count);
        }


        [TestCleanup]
        public void Cleanup()
        {
            // ������� ���� ����� ���������� ������ ��� ������� ���������
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }
    }
}
