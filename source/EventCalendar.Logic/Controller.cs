using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventCalendar.Entities;

namespace EventCalendar.Logic
{
	public class Controller : ICollection
	{
		//fields
		private static int _countEventsForPerson;
		private readonly ICollection<Event> _events = new List<Event>();
		private readonly List<Person> _participators = new List<Person>();
		private readonly Dictionary<Event, IList<Person>> _participatorsOnEvent
			 = new Dictionary<Event, IList<Person>>();
		public int EventsCount { get { return _events.Count; } }
		public int Count { get; set; }

		public bool IsReadOnly { get; set; }

		public bool IsSynchronized => throw new NotImplementedException();

		public object SyncRoot => throw new NotImplementedException();

		public Controller()
		{
			_countEventsForPerson = 0;
		}
		/// <summary>
		/// Ein Event mit dem angegebenen Titel und dem Termin wird für den Einlader angelegt.
		/// Der Titel muss innerhalb der Veranstaltungen eindeutig sein und das Datum darf nicht
		/// in der Vergangenheit liegen.
		/// Mit dem optionalen Parameter maxParticipators kann eine Obergrenze für die Teilnehmer festgelegt
		/// werden.
		/// </summary>
		/// <param name="invitor"></param>
		/// <param name="title"></param>
		/// <param name="dateTime"></param>
		/// <param name="maxParticipators"></param>
		/// <returns>Wurde die Veranstaltung angelegt</returns>
		public bool CreateEvent(Person invitor, string title, DateTime dateTime, int maxParticipators = 0)
		{
			bool check = false;
			bool isEqualTitel = false;
			int timeSpan = DateTime.Now.CompareTo(dateTime);
			foreach (Event item in _events)
			{
				if (item.Title.Equals(title))
				{
					isEqualTitel = true;
				}
			}
			if (invitor == null || title == null || title == "" 
				|| isEqualTitel || dateTime == null || timeSpan > 0)
			{
				return check;
			}
			Person personInvitor = new Person(invitor.LastName, invitor.FirstName, invitor.MailAddress, invitor.PhoneNumber);
			Event newEvent = new Event(personInvitor, title, dateTime, maxParticipators);
			_events.Add(newEvent);
			check = true;

			return check;
		}
		/// <summary>
		/// Liefert die Veranstaltung mit dem Titel
		/// </summary>
		/// <param name="title"></param>
		/// <returns>Event oder null, falls es keine Veranstaltung mit dem Titel gibt</returns>
		public Event GetEvent(string title)
		{
			Event result = null;
			foreach (Event ev in _events)
			{
				if (ev.Title.Equals(title))
				{
					result = ev;
					break;
				}
			}
			return result;
		}

		/// <summary>
		/// Person registriert sich für Veranstaltung.
		/// Eine Person kann sich zu einer Veranstaltung nur einmal registrieren.
		/// </summary>
		/// <param name="person"></param>
		/// <param name="ev">Veranstaltung</param>
		/// <returns>War die Registrierung erfolgreich?</returns>
		public bool RegisterPersonForEvent(Person person, Event ev)
		{
			bool check = false;
			if (IsReadOnly) return check;	//bodyguard
			if (person == null || ev == null)
			{
				return check;
			}
			foreach (KeyValuePair<Event, IList<Person>> item in _participatorsOnEvent)
			{	//check if same person two times for one event
				if (item.Key.Title == ev.Title && item.Value.Contains(person))
				{
					return check;
				}
			}
			_participators.Add(person);
			List<Person> temporary;
			if (!_participatorsOnEvent.ContainsKey(ev))
			{
				temporary = new List<Person>();
				_participatorsOnEvent.Add(ev, temporary);
			}
			temporary = _participatorsOnEvent[ev].ToList();
			temporary.Add(person);
			_participatorsOnEvent.Remove(ev);
			_participatorsOnEvent.Add(ev, temporary);
			Count++;
			if (ev.MaxParticipators > 0 && ev.MaxParticipators <= Count)
			{  //check if limited
				IsReadOnly = true;
			}
			else
			{
				IsReadOnly = false;
			}
			check = true;
			person.EventCounter = _countEventsForPerson; //<----
			return check;
		}
		/// <summary>
		/// Person meldet sich von Veranstaltung ab
		/// </summary>
		/// <param name="person"></param>
		/// <param name="ev">Veranstaltung</param>
		/// <returns>War die Abmeldung erfolgreich?</returns>
		public bool UnregisterPersonForEvent(Person person, Event ev)
		{
			if (person == null || ev == null || !_participatorsOnEvent[ev].Contains(person))
			{
				return false;
			}
			else
			{
				_participators.Remove(person);
				_participatorsOnEvent[ev].Remove(person);
				Count--;
				IsReadOnly = false;
				return  true;
			}
		}

		/// <summary>
		/// Liefert alle Teilnehmer an der Veranstaltung.
		/// Sortierung absteigend nach der Anzahl der Events der Personen.
		/// Bei gleicher Anzahl nach dem Namen der Person (aufsteigend).
		/// </summary>
		/// <param name="ev"></param>
		/// <returns>Liste der Teilnehmer oder null im Fehlerfall</returns>
		public IList<Person> GetParticipatorsForEvent(Event ev)
		{
			List<Person> people = new List<Person>();
			if (ev == null || !(ev is Event))
			{
				return null;
			}
			if(_participatorsOnEvent.Count == 0)
			{
				return people;
			}
			if(_participatorsOnEvent[ev].ToList<Person>().Count == 0)
			{
				return people;
			}
			people = _participatorsOnEvent[ev].ToList<Person>();
			people.Sort();
			Person.SortFirstName(people);
			return people; // as IList<Person>;
		}
		/// <summary>
		/// Liefert alle Veranstaltungen der Person nach Datum (aufsteigend) sortiert.
		/// </summary>
		/// <param name="person"></param>
		/// <returns>Liste der Veranstaltungen oder null im Fehlerfall</returns>
		public List<Event> GetEventsForPerson(Person person)
		{
			List<Event> newEventList = new List<Event>();
			if (person == null)
			{
				return null;
			}
			foreach (KeyValuePair<Event, IList<Person>> titel in _participatorsOnEvent)
			{
				if (titel.Value.Contains(person))
				{
					newEventList.Add(titel.Key);
				}
			}
			newEventList.Sort();
			return newEventList;
		}

		/// <summary>
		/// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
		/// </summary>
		/// <param name="participator"></param>
		/// <returns>Anzahl oder 0 im Fehlerfall</returns>
		public int CountEventsForPerson(Person participator)
		{
			if (participator == null)
			{
				return 0;
			}
			foreach (KeyValuePair<Event, IList<Person>> titel in _participatorsOnEvent)
			{
				if (titel.Value.Contains(participator))
				{
					_countEventsForPerson++;
				}
			}
			return _countEventsForPerson;
		}
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)GetEnumerator();
		}
		public MyEnumerator GetEnumerator()
		{
			return new MyEnumerator(_participators);
		}
	}
}
