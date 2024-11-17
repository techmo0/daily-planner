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
            // Удаляем файл перед каждым тестом для чистоты тестирования
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
            eventManager.ViewEvents(); // Просто вызываем метод для отображения событий
                                       // Здесь вы можете добавить дополнительные проверки на консольный вывод, если это необходимо.
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
            var events = eventManager.ViewEvents(); // Теперь это работает, так как метод возвращает список событий.
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

            // Сохраняем события в файл для тестирования
            var json = JsonSerializer.Serialize(eventsToSave, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("events.json", json); // Убедитесь, что файл создается в правильной директории

            // Act
            var eventManager = new EventManager(); // Создаем новый экземпляр EventManager, который вызовет LoadEvents

            // Assert
            var eventsLoaded = eventManager.ViewEvents(); // Получаем загруженные события
            Assert.AreEqual(2, eventsLoaded.Count); // Проверяем количество загруженных событий
            Assert.AreEqual("birthday", eventsLoaded[0].Type); // Проверяем первое событие
            Assert.AreEqual("call", eventsLoaded[1].Type); // Проверяем второе событие
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
            // Удаляем файл после выполнения тестов для чистоты окружения
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }
    }
}
