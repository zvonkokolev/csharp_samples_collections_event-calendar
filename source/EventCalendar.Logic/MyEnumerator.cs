using EventCalendar.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Logic
{
	public class MyEnumerator : IEnumerator
	{
		public List<Person> _people;
		int pos = -1;
		public MyEnumerator(List<Person> people)
		{
			_people = people;
		}


		public bool MoveNext()
		{
			pos++;
			return (pos < _people.Count);
		}

		public void Reset()
		{
			pos = -1;
		}
		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}
		public Person Current
		{
			get
			{
				try
				{
					return _people[pos];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}
	}
}
