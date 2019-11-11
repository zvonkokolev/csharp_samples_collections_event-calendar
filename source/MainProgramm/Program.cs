using System;
using EventCalendar.Entities;
using EventCalendar.Logic;
using System.Collections.Generic;
using System.Linq;

namespace MainProgramm
{
	public class Program
	{
		static void Main(string[] args)
		{
			Controller controller = new Controller();
			Person invitor = new Person("Huber", "Max") { MailAddress = "max.huber@x.x", PhoneNumber = "1234567" };
			controller.CreateEvent(invitor, "First Event", DateTime.Now.AddDays(1));
			controller.CreateEvent(invitor, "Second Event", DateTime.Now.AddDays(2));
			Event ev1 = controller.GetEvent("First Event");
			Event ev2 = controller.GetEvent("Second Event");
			Event ev3 = controller.GetEvent("Third Event");
			Console.WriteLine($"Event <{ev1.Title}> Nr.1");
			Console.WriteLine($"Event <{ev2.Title}> Nr.2");
				if (ev3 == null)
					Console.WriteLine($"Es gibt keinen EVENT namens {nameof(ev3)}");

			Person participator1 = new Person("Part1", "Franz");
			Person participator2 = new Person("Part2", "Hans");
			Person participator3 = new Person("Part3", "Adi");
			Person participator4 = new Person("Part4", "Sellinde");
			controller.RegisterPersonForEvent(participator2, ev1);
			controller.RegisterPersonForEvent(participator1, ev1);
			controller.RegisterPersonForEvent(participator3, ev1);
			controller.RegisterPersonForEvent(participator4, ev1);
			controller.RegisterPersonForEvent(participator2, ev2);
			controller.RegisterPersonForEvent(participator3, ev2);
			var events = controller.GetEventsForPerson(participator2);
			var people = controller.GetParticipatorsForEvent(ev1);
			Dictionary<Event, List<Person>> dictionary = new Dictionary<Event, List<Person>>();
			List<Person> people1 = new List<Person>();
			dictionary.Add(ev1, people1);
			Console.WriteLine("------------------------------------------------------------");

			foreach (var item in people1)
			{
				Console.WriteLine($"LISTE {item} ");
			}
			dictionary[ev1].Add(participator4);
			dictionary[ev1].Add(participator1);
			dictionary[ev1].Add(participator2);
			dictionary[ev1].Add(participator3);

			dictionary.Add(ev2, people1);

			List<Event> eventListe = new List<Event>();
			List<Person> personal = new List<Person>();
			eventListe = dictionary.Keys.ToList();
			//personal = dictionary.Values.ToList();
			foreach (var item in eventListe)
			{
				for (int i = 0; i < dictionary[item].ToList().Count; i++)
				{
					Console.WriteLine($"{item.Title} / {item.MyDateTime} : {dictionary[item].ToArray()[i]}");
				}
			}
			Console.WriteLine("Sortiert nach Datum absteigend");
			foreach (var item in eventListe.OrderByDescending(i => i.MyDateTime))
			{
				Console.WriteLine($"{item.Title} / {item.MyDateTime}");
			}
			Console.WriteLine("-----------------UMGEKEHRT----------------------------------");

			foreach (var item in eventListe)
			{
				for (int i = dictionary[item].ToArray().Length - 1; i >= 0; i--)
				{
					Console.WriteLine($"{item.Title} / {item.MyDateTime} : {dictionary[item].ToList()[i]}");
				}
			}
			//var items = from pair in dictionary orderby pair.Value ascending select pair;
		}
	}
}
