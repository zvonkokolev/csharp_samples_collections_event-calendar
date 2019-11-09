﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
	public class Event : IComparable
	{
		private class SortOnTitleAscendingHelper : IComparer
		{
			public int Compare(object x, object y)
			{
				Event a = (Event)x;
				Event b = (Event)y;
				return String.Compare(a.Title, b.Title);
			}
		}
		private class SortOnTitleDescendingHelper : IComparer
		{
			public int Compare(object x, object y)
			{
				Event a = (Event)x;
				Event b = (Event)y;
				return String.Compare(b.Title, a.Title);
			}
		}
		private class SortOnDatumDescendingHelper : IComparer
		{
			public int Compare(object x, object y)
			{
				Event a = (Event)x;
				Event b = (Event)y;
				return DateTime.Compare(b.MyDateTime, a.MyDateTime);
			}
		}
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
				MyDateTime = dateTime;
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
		public DateTime MyDateTime { get => _dateTime; set => _dateTime = value; }
		public Person Person { get => _person; set => _person = value; }
		public int MaxParticipators { get => _maxParticipators; set => _maxParticipators = value; }

		public int CompareTo(object obj)
		{
			if (obj == null || !(obj is Event))
			{
				throw new Exception("Das ist kein Event");
			}
			Event other = (Event)obj;
			return DateTime.Compare(MyDateTime, other.MyDateTime);
		}
		public static IComparer SortTitleAscending()
		{
			return (IComparer)new SortOnTitleAscendingHelper();
		}
		public static IComparer SortTitleDescending()
		{
			return (IComparer)new SortOnTitleDescendingHelper();
		}
		public static IComparer SortDatumDescending(List<Event> events)
		{
			return (IComparer)new SortOnDatumDescendingHelper();
		}
	}
}
