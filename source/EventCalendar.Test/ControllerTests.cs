using System;
using EventCalendar.Entities;
using EventCalendar.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventCalendar.Test
{
    [TestClass()]
    public class ControllerTests
    {
        [TestMethod()]
        public void Constructor_NoEvent_ShouldReturnCountZero()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            // Assert
            Assert.AreEqual(0, controller.EventsCount);
        }

        [TestMethod()]
        public void CreateEvent_AllOk_ShouldReturnTrue()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            // Act
            bool ok = controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Assert
            Assert.IsTrue(ok);
        }

        [TestMethod()]
        public void CreateEvent_PersonNullOrEmpty_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            bool ok = controller.CreateEvent(null, "First Event", DateTime.Now.AddDays(1));
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void CreateEvent_TitleNullOrEmpty_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            // Act
            bool ok = controller.CreateEvent(invitor, "", DateTime.Now.AddDays(1));
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void CreateEvent_DateInThePast_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            // Act
            bool ok = controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(-1));
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void CreateEvent_TitleNotUnique_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            // Act
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            bool ok = controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void EventsCount_FirstEventWithoutParticipators_ShouldReturnOneEvent()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Act
            // Assert
            Assert.AreEqual(1, controller.EventsCount);
        }

        [TestMethod()]
        public void GetEvent_TitleNull_ShouldReturnNull()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            var ev = controller.GetEvent(null);
            // Assert
            Assert.IsNull(ev);
        }

        [TestMethod()]
        public void GetEvent_TitleEmpty_ShouldReturnNull()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            var ev = controller.GetEvent("");
            // Assert
            Assert.IsNull(ev);
        }

        [TestMethod()]
        public void GetEvent_TitleFound_ShouldReturnNull()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Act
            var ev = controller.GetEvent("Second Event");
            // Assert
            Assert.IsNull(ev);
        }

        [TestMethod()]
        public void GetEvent_Title_ShouldReturnEvent()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            // Act
            var ev = controller.GetEvent("First Event");
            // Assert
            Assert.IsNotNull(ev);
            Assert.AreEqual("First Event", ev.Title);
        }

        [TestMethod()]
        public void CountEventsForPerson_TwoEventsRegistered_ShouldReturnTwo()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            // Act
            int eventsCounterParticipatoricipator1 = controller.CountEventsForPerson(participator1);
            // Assert
            Assert.AreEqual(2, eventsCounterParticipatoricipator1);
        }

        [TestMethod()]
        public void CountEventsForPerson_OneEventsRegisteredTwice_ShouldReturnOne()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            int eventsCounterParticipator1 = controller.CountEventsForPerson(participator1);
            // Assert
            Assert.AreEqual(1, eventsCounterParticipator1);
        }

        [TestMethod()]
        public void CountEventsForPerson_OneEventsRegisteredAndUnregistered_ShouldReturnZero()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.UnregisterPersonForEvent(participator1, ev1);
            // Act
            int eventsCounterParticipator1 = controller.CountEventsForPerson(participator1);
            // Assert
            Assert.AreEqual(0, eventsCounterParticipator1);
        }

        [TestMethod()]
        public void CountEventsForPerson_ZeroRegisteredAndUnregistered_ShouldReturnZero()
        {
            // Arrange
            Controller controller = new Controller();
            Person participator1 = new Person("Part1", "Hans");
            // Act
            int eventsCounterParticipator1 = controller.CountEventsForPerson(participator1);
            // Assert
            Assert.AreEqual(0, eventsCounterParticipator1);
        }

        [TestMethod()]
        public void CountEventsForPerson_PersonNull_ShouldReturnZero()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            int eventsCounterParticipator1 = controller.CountEventsForPerson(null);
            // Assert
            Assert.AreEqual(0, eventsCounterParticipator1);
        }

        [TestMethod()]
        public void GetParticipatorsForEvent_FirstEventWithoutParticipators_ShouldReturnEmptyList()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            // Act
            var participators = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreEqual(0, participators.Count);
        }

        [TestMethod()]
        public void GetParticipatorsForEvent_PersonNotRegistered_ShouldReturnNull()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            var people = controller.GetParticipatorsForEvent(null);
            // Assert
            Assert.IsNull(people);
        }


        [TestMethod()]
        public void GetParticipatorsForEvent_TwoParticipatorsDifferentEventCounterCorrectOrder_ShouldReturnSameOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator2, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod()]
        public void GetParticipatorsForEvent_TwoParticipatorsDifferentEventCounterRegisteredWrongOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator2, ev1);
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod()]
        public void GetParticipatorsForEvent_TwoParticipatorsSameEventCounterRegisteredWrongOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part", "Franz");
            Person participator2 = new Person("Part", "Hans");
            controller.RegisterPersonForEvent(participator2, ev1);
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod()]
        public void GetParticipatorsForEvent_TwoParticipatorsSameEventCounterRegisteredCorrectOrder_ShouldReturnCorrectOrder()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part", "Franz");
            Person participator2 = new Person("Part", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator2, ev1);
            // Act
            var people = controller.GetParticipatorsForEvent(ev1);
            // Assert
            Assert.AreSame(participator1, people[0]);
            Assert.AreSame(participator2, people[1]);
        }

        [TestMethod()]
        public void GetEventsForPerson_TwoEventsRegisteredInCorrectOrder_ShouldReturnTwoEvents()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(2));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev2);
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            var events = controller.GetEventsForPerson(participator1);
            // Assert
            Assert.AreSame(ev1, events[0]);
            Assert.AreSame(ev2, events[1]);
        }

        [TestMethod()]
        public void GetEventsForPerson_TwoEventsRegisteredInWrongOrder_ShouldReturnTwoEvents()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(2));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            // Act
            var events = controller.GetEventsForPerson(participator1);
            // Assert
            Assert.AreSame(ev1, events[0]);
            Assert.AreSame(ev2, events[1]);
        }

        [TestMethod()]
        public void GetEventsForPerson_PersonNull_ShouldReturnNull()
        {
            // Arrange
            Controller controller = new Controller();
            // Act
            var events = controller.GetEventsForPerson(null);
            // Assert
            Assert.IsNull(events);
        }


        [TestMethod()]
        public void UnregisterPersonFromEvent_UnregisterOneOfTwo_ShouldReturnTrue()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            // Act
            bool ok = controller.UnregisterPersonForEvent(participator1, ev1);
            // Assert
            Assert.IsTrue((ok));
        }

        [TestMethod()]
        public void UnregisterPersonFromEvent_PersonNull_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            // Act
            bool ok = controller.UnregisterPersonForEvent(null, ev1);
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void UnregisterPersonFromEvent_EventNull_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            Person participator1 = new Person("Part1", "Hans");
            // Act
            bool ok = controller.UnregisterPersonForEvent(participator1, null);
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void UnregisterPersonFromEvent_PersonNotRegistered_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Event ev2 = controller.GetEvent("Second Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);
            controller.RegisterPersonForEvent(participator1, ev2);
            controller.RegisterPersonForEvent(participator2, ev1);
            // Act
            bool ok = controller.UnregisterPersonForEvent(participator2, ev2);
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void RegisterForLimitedEvent_SecondOfTwoParticipators_ShouldReturnTrue()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "Limited Event", DateTime.Now.AddDays(1), 2);
            Event ev1 = controller.GetEvent("Limited Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            bool ok = controller.RegisterPersonForEvent(participator2, ev1);
            // Assert
            Assert.IsTrue(ok);
        }

        [TestMethod()]
        public void RegisterPersonForEvent_FirstRegistration_ShouldReturnTrue()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");
            // Act
            bool ok = controller.RegisterPersonForEvent(participator1, ev1);
            // Assert
            Assert.IsTrue(ok);
        }

        [TestMethod()]
        public void RegisterPersonForEvent_RegistrationTwice_ShouldReturnTrue()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            Person participator1 = new Person("Part1", "Hans");
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            bool ok = controller.RegisterPersonForEvent(participator1, ev1);
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void RegisterPersonForEvent_PersonNull_ShouldReturnTrue()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
            Event ev1 = controller.GetEvent("First Event");
            // Act
            bool ok = controller.RegisterPersonForEvent(null, ev1);
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void RegisterPersonForEvent_EventNull_ShouldReturnTrue()
        {
            // Arrange
            Controller controller = new Controller();
            Person participator1 = new Person("Part1", "Hans");
            // Act
            bool ok = controller.RegisterPersonForEvent(participator1, null);
            // Assert
            Assert.IsFalse(ok);
        }

        [TestMethod()]
        public void RegisterForLimitedEvent_SecondParticipatorByLimitOne_ShouldReturnFalse()
        {
            // Arrange
            Controller controller = new Controller();
            Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
            controller.CreateEvent(invitor, "Limited Event", DateTime.Now.AddDays(1), 1);
            Event ev1 = controller.GetEvent("Limited Event");
            Person participator1 = new Person("Part1", "Hans");
            Person participator2 = new Person("Part2", "Franz");
            controller.RegisterPersonForEvent(participator1, ev1);
            // Act
            bool ok = controller.RegisterPersonForEvent(participator2, ev1);
            // Assert
            Assert.IsFalse(ok);
        }



    }
}