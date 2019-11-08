using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using EventCalendar.Entities;
using static System.String;

namespace EventCalendar.Logic
{
	public class Controller //: ICollection<T>
	{
		//fields
		private readonly ICollection<Event> _events;
		private IList<Person> _participators;
		private Dictionary<IList<Person>, Event> _participatorsOnEvent;
		public int EventsCount { get { return _events.Count; } }

		public int Count => throw new NotImplementedException();

		public bool IsReadOnly => throw new NotImplementedException();

		public Controller()
		{
			_events = new List<Event>();
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
			if (invitor == null || title == null || title == "" || isEqualTitel || dateTime == null || timeSpan > 0)
			{
				//throw new NullReferenceException("Daten sind nicht gueltig");
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
			if(person == null || ev == null)
			{
				return check;
			}
			_participators = new List<Person>
			{
				person
			};
			_participatorsOnEvent = new Dictionary<IList<Person>, Event>
			{
				{ _participators, ev }
			};
			check = true;
			foreach (Person item in _participators)
			{
				if(item.FirstName.ToLower().Equals(person.FirstName.ToLower())
					&& item.LastName.ToLower().Equals(person.LastName.ToLower()))
				{
					check = false;
					break;
				}
			}
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
			bool check = false;
			//foreach (Person item in _participators)
			//{
			//	if (item.FirstName.ToLower().Equals(person.FirstName.ToLower())
			//		&& item.LastName.ToLower().Equals(person.LastName.ToLower()))
			//	{
			//		check = true;
			//		break;
			//	}
			//}
			if (person == null || ev == null || !check)
			{
				return false;
			}
			_participatorsOnEvent.Remove(_participators);
			_participators.Remove(person);
			_participatorsOnEvent.Add(_participators, ev);
			check = true;
			return check;
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Liefert alle Veranstaltungen der Person nach Datum (aufsteigend) sortiert.
		/// </summary>
		/// <param name="person"></param>
		/// <returns>Liste der Veranstaltungen oder null im Fehlerfall</returns>
		public List<Event> GetEventsForPerson(Person person)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Liefert die Anzahl der Veranstaltungen, für die die Person registriert ist.
		/// </summary>
		/// <param name="participator"></param>
		/// <returns>Anzahl oder 0 im Fehlerfall</returns>
		public int CountEventsForPerson(Person participator)
		{
			throw new NotImplementedException();
		}
		// ICollections begin----------------------------
		//public void Add(T item)
		//{
		//	throw new NotImplementedException();
		//}

		//public void Clear()
		//{
		//	throw new NotImplementedException();
		//}

		//public bool Contains(T item)
		//{
		//	throw new NotImplementedException();
		//}

		//public void CopyTo(T[] array, int arrayIndex)
		//{
		//	throw new NotImplementedException();
		//}

		//public bool Remove(T item)
		//{
		//	throw new NotImplementedException();
		//}

		//public IEnumerator<T> GetEnumerator()
		//{
		//	throw new NotImplementedException();
		//}

		//IEnumerator IEnumerable.GetEnumerator()
		//{
		//	throw new NotImplementedException();
		//}
		//--------------------------------------
		//public void Add(Event item)
		//{
		//	throw new NotImplementedException();
		//}

		//public bool Contains(Event item)
		//{
		//	throw new NotImplementedException();
		//}

		//public void CopyTo(Event[] array, int arrayIndex)
		//{
		//	throw new NotImplementedException();
		//}

		//public bool Remove(Event item)
		//{
		//	throw new NotImplementedException();
		//}

		//IEnumerator<Event> IEnumerable<Event>.GetEnumerator()
		//{
		//	throw new NotImplementedException();
		//}

		//public void Clear()
		//{
		//	throw new NotImplementedException();
		//}

		//public IEnumerator GetEnumerator()
		//{
		//	throw new NotImplementedException();
		//}
		//ICollection end------------------------
	}
}
