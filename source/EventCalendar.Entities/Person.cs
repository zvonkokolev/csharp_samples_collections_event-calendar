using System;
using System.Collections;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
	/// <summary>
	/// Person kann sowohl zu einer Veranstaltung einladen,
	/// als auch an Veranstaltungen teilnehmen
	/// </summary>
	public class Person : IComparable
	{
		public class SortFirstNameHelper : IComparer
		{
			public int Compare(object x, object y)
			{
				Person leftPerson = (Person)x;
				Person rightPerson = (Person)y;
				return String.Compare(leftPerson.FirstName, rightPerson.FirstName);
			}
		}
		public class SortLastNameHelper : IComparer
		{
			public int Compare(object x, object y)
			{
				Person leftPerson = (Person)x;
				Person rightPerson = (Person)y;
				return String.Compare(rightPerson.FirstName, leftPerson.FirstName);
			}
		}
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
			if(lastName == null || firstName == null)
			{
				throw new NullReferenceException("Name ist nicht gueltig");
			}
			LastName = lastName;
			FirstName = firstName;
			//EventCounter++;
		}
		public Person(string lastNameX, string firstNameX, string email = "0", string phoneNumber = "0")
		{
			if(lastNameX == null || firstNameX == null)
			{
				throw new NullReferenceException("Personalien sind nicht vollstaendig");
			}
			LastName = lastNameX;
			FirstName = firstNameX;
			MailAddress = email;
			PhoneNumber = phoneNumber;
			if(MailAddress == "0" || PhoneNumber == "0")
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


		public static IComparer SortFirstName(List<Person> people)
		{
			if (people is null)
			{
				throw new ArgumentNullException(nameof(people));
			}
			return (IComparer) new SortFirstNameHelper();
		}
		public static IComparer SortLastName(Person people)
		{
			if (people is null)
			{
				throw new ArgumentNullException();
			}

			return (IComparer) new SortLastNameHelper();
		}

		public int CompareTo(object obj)
		{
			throw new NotImplementedException();
		}
		//int IComparable.CompareTo(object obj)
		//{
		//	if (obj == null || !(obj is Person))
		//	{
		//		throw new ArgumentException("Objekt ist kein Person");
		//	}
		//	Person otherPerson = (Person)obj;
		//	if (EventCounter > otherPerson.EventCounter) return -1;
		//	if (EventCounter < otherPerson.EventCounter) return 1;
		//	else return 0;
		//}
	}
}