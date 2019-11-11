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
		private bool _isLimitedEvent;
		private readonly List<Event> _events = new List<Event>();
		private readonly List<Person> _participators = new List<Person>();
		private readonly Dictionary<Event, List<Person>> _participatorsOnEvent
			 = new Dictionary<Event, List<Person>>();
		public int EventsCount { get { return _events.Count; } }
		public int Count { get; set; }

		public bool IsReadOnly { get; set; }

		public bool IsSynchronized => throw new NotImplementedException();

		public object SyncRoot => throw new NotImplementedException();

		public bool IsLimitedEvent
		{
			get
			{
				return _isLimitedEvent;
			}
			set
			{
				_isLimitedEvent = value;
			}
		}

		public Controller()
		{
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
			if (title == null)
			{
				return result;
			}
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
			if (IsLimitedEvent) return check;	//bodyguard
			if (person == null || ev == null)
			{
				return check;
			}
			foreach (KeyValuePair<Event, List<Person>> item in _participatorsOnEvent)
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
				IsLimitedEvent = true;
			}
			else
			{
				IsLimitedEvent = false;
			}
			person.EventCounter++;
			check = true;
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
				person.EventCounter--;
				IsLimitedEvent = false;
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
		public List<Person> GetParticipatorsForEvent(Event ev)
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
			people = _participatorsOnEvent[ev].ToList();
			bool change;
			do   // sort alghoritmus
			{
				change = false;
				for (int i = 0; i < people.Count - 1; i++)
				{	// first - count of events
					if (!( CountEventsForPerson(people[i]) is IComparable left) || !(CountEventsForPerson(people[i + 1]) is IComparable right))
					{
						throw new Exception("Objekte sind nicht IMyCompareable");
					}
					if (left.CompareTo(right) > 0)
					{
						var tmp = people[i + 1];
						people[i + 1] = people[i];
						people[i] = tmp;
						change = true;
					}
					if (left.CompareTo(right) == 0)
					{	// then for lastname
						if (!(people[i].LastName is IComparable leftName) || !(people[i + 1].LastName is IComparable rightName))
						{
							throw new Exception("Objekte sind nicht IMyCompareable");
						}
						if (leftName.CompareTo(rightName) > 0)
						{
							var tmp = people[i + 1];
							people[i + 1] = people[i];
							people[i] = tmp;
							change = true;
						}
						if (leftName.CompareTo(rightName) == 0)
						{	// at last for a firsname
							if (!(people[i].FirstName is IComparable leftFirstName) || !(people[i + 1].FirstName is IComparable rightFirstName))
							{
								throw new Exception("Objekte sind nicht IMyCompareable");
							}
							if (leftFirstName.CompareTo(rightFirstName) > 0)
							{
								var tmp = people[i + 1];
								people[i + 1] = people[i];
								people[i] = tmp;
								change = true;
							}
						}
					}
				}
			} while (change == true);
			//var items = from pair in _participatorsOnEvent orderby pair.Value ascending select pair;
			//people.Sort();
			//MySort.Sort(people);
			return people;
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
			}	// iterate keys (events) 
			foreach (KeyValuePair<Event, List<Person>> titel in _participatorsOnEvent)
			{
				if (titel.Value.Contains(person))
				{
					newEventList.Add(titel.Key);
				}
			}
			newEventList.Sort();
			//newEventList = _participatorsOnEvent.Keys.ToList();
			//newEventList.OrderBy(i => i.MyDateTime);
			//newEventList.OrderByDescending(i => i.MyDateTime);
			return newEventList;
		}

		/// <summary>
		/// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
		/// </summary>
		/// <param name="participator"></param>
		/// <returns>Anzahl oder 0 im Fehlerfall</returns>
		public int CountEventsForPerson(Person participator)
		{
			int _countEventsForPerson = 0;
			if (participator == null)
			{
				return 0;
			}	// iterate dictionary keys
			foreach (KeyValuePair<Event, List<Person>> titel in _participatorsOnEvent)
			{	//	iterate participator lists and 
				for (int i = 0; i < titel.Value.ToList().Count; i++)
				{  // search for person
					if (titel.Value.ToList()[i].FirstName.Equals(participator.FirstName))
					{
						_countEventsForPerson++;
					}
				}  // and return counts of participator
			}
			return _countEventsForPerson;
		}
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}
		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}