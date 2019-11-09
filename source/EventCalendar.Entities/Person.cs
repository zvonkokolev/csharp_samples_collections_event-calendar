using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{
	/// <summary>
	/// Person kann sowohl zu einer Veranstaltung einladen,
	/// als auch an Veranstaltungen teilnehmen
	/// </summary>
	public class Person : IComparable
	{
		private class SortFirstNameHelper : IComparer
		{
			public int Compare(object x, object y)
			{
				Person leftPerson = (Person)x;
				Person rightPerson = (Person)y;
				return String.Compare(leftPerson.FirstName, rightPerson.FirstName);
			}
		}
		private class SortLastNameHelper : IComparer
		{
			public int Compare(object x, object y)
			{
				Person leftPerson = (Person)x;
				Person rightPerson = (Person)y;
				return String.Compare(leftPerson.LastName, rightPerson.LastName);
			}
		}
		private readonly bool _isInvitor = false;
		private readonly List<int> _personCounter; 
		public string LastName { get; }
		public string FirstName { get; }
		public string MailAddress { get; set; }
		public string PhoneNumber { get; set; }

		//constructor
		public Person()
		{
			_personCounter = new List<int>();
		}
		public Person(string lastName, string firstName)
		{
			if(lastName == null || firstName == null)
			{
				throw new NullReferenceException("Name ist nicht gueltig");
			}
			LastName = lastName;
			FirstName = firstName;
			_personCounter = new List<int>();
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

		public int CompareTo(object obj)
		{
			Person otherPerson = (Person)obj;
			if(this._personCounter.Count < otherPerson._personCounter.Count)
			{
				return -1;
			}
			if (this._personCounter.Count < otherPerson._personCounter.Count)
			{
				return 1;
			}
			else return 0;
		}
		public static IComparer SortFirstName(List<Person> people)
		{
			return (IComparer) new SortFirstNameHelper();
		}
		public static IComparer SortLastName(List<Person> people)
		{
			return (IComparer)new SortLastNameHelper();
		}
	}
}