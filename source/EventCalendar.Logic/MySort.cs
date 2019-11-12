using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EventCalendar.Entities;
namespace EventCalendar.Logic
{
	public class MySort
	{
		//SORT methods
		public static void Sort<T>(List<Person> people)
		{
			Controller controller = new Controller();
			bool change;
			do   // sort alghoritmus
			{
				change = false;
				for (int i = 0; i < people.Count - 1; i++)
				{  // first - count of events
					if (!(controller.CountEventsForPerson(people[i]) is IComparable left) || !(controller.CountEventsForPerson(people[i + 1]) is IComparable right))
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
					{  // then for lastname
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
						{  // at last for a firsname
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
		}
		public static void Sort<T>(List<T> array, IComparer comparer)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			if (comparer == null)
				throw new ArgumentNullException(nameof(comparer));
			bool changed;
			do
			{
				changed = false;
				for (int i = 0; i < array.Count - 1; i++)
				{
					if (comparer.Compare(array[i], array[i + 1]) > 0)
					{
						T tmp = array[i + 1];
						array[i + 1] = array[i];
						array[i] = tmp;
						changed = true;
					}
				}
			} while (changed == true);
		}
	}
}
