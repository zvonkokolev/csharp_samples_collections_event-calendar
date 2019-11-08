using System;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{
	/// <summary>
	/// Person kann sowohl zu einer Veranstaltung einladen,
	/// als auch an Veranstaltungen teilnehmen
	/// </summary>
	public class Person
	{
		private readonly bool _isInvitor = true;
		public string LastName { get; }
		public string FirstName { get; }
		public string MailAddress { get; set; }
		public string PhoneNumber { get; set; }

		//constructor
		public Person(string lastName, string firstName)
		{
			if(lastName == null || firstName == null)
			{
				throw new NullReferenceException("Name ist nicht gueltig");
			}
			LastName = lastName;
			FirstName = firstName;
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
				_isInvitor = false;
			}
		}
		//methods
		public bool IsInvitor()
		{
			return _isInvitor;
		}
		public override string ToString()
		{
			return $"{LastName} {FirstName} {MailAddress} {PhoneNumber}";
		}
	}
}