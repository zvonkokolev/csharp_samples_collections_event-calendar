using System;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
	public class Event
	{
		//fields
		DateTime _dateTime;
		Person _person;
		int _maxParticipators;
		string _title;
		//constructor
		public Event(Person person, string title, DateTime dateTime, int maxParticipators = 0)
		{
			if(title != null && dateTime != null && person != null)
			{
				Title = title;
				DateTime = dateTime;
				Person = person;
				MaxParticipators = maxParticipators;
			}
			else
			{
				throw new NullReferenceException("Person, Title oder Datum stimmen nicht");
			}
		}

		//method
		public string Title { get => _title; set => _title = value; }
		public DateTime DateTime { get => _dateTime; set => _dateTime = value; }
		public Person Person { get => _person; set => _person = value; }
		public int MaxParticipators { get => _maxParticipators; set => _maxParticipators = value; }
	}
}
