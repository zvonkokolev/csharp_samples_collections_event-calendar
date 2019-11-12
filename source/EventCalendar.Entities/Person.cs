using System;
using System.Collections;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
	/// <summary>
	/// Person kann sowohl zu einer Veranstaltung einladen,
	/// als auch an Veranstaltungen teilnehmen
	/// </summary>
	public class Person : IComparable<Person>
	{
		private readonly bool _isInvitor = false;
		public string LastName { get; }
		public string FirstName { get; }
		public string MailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public int EventCounter { get; set; }
		//constructor
		public Person()
		{
		}
		public Person(string lastName, string firstName)
		{
			if (lastName == null || firstName == null)
			{
				throw new NullReferenceException("Name ist nicht gueltig");
			}
			LastName = lastName;
			FirstName = firstName;
		}
		public Person(string lastNameX, string firstNameX, string email = "0", string phoneNumber = "0")
		{
			if (lastNameX == null || firstNameX == null)
			{
				throw new NullReferenceException("Personalien sind nicht vollstaendig");
			}
			LastName = lastNameX;
			FirstName = firstNameX;
			MailAddress = email;
			PhoneNumber = phoneNumber;
			if (MailAddress == "0" || PhoneNumber == "0")
			{
				_isInvitor = true;
			}
		}

		public bool IsInvitor()
		{
			return _isInvitor;
		}

		public override string ToString()
		{
			return $"{LastName} {FirstName} {MailAddress} {PhoneNumber}";
		}
		public int CompareTo(Person other)
		{
			if (other == null || !(other is IComparable))
			{
				throw new ArgumentNullException($"{nameof(other)} ist kein Person");
			}
			Person otherPeron = (Person)other;
			return otherPeron.EventCounter.CompareTo(EventCounter);
		}
		public class LastNameComparer : IComparer<Person>
		{
			public int Compare(Person x, Person y)
			{
				return x.LastName.CompareTo(y.LastName);
			}
		}
		public class FirstNameComparer : IComparer<Person>
		{
			public int Compare(Person x, Person y)
			{
				return x.FirstName.CompareTo(y.FirstName);
			}
		}
	}
}