using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Logic
{
	public class MySort
	{
		//SORT methods
		public static void Sort<T>(List<T> array)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array));
			}
			bool change;
			do
			{
				change = false;
				for (int i = 0; i < array.Count - 1; i++)
				{
					if (!(array[i] is IComparable left) || !(array[i + 1] is IComparable right))
					{
						throw new Exception("Objekte sind nicht IMyCompareable");
					}
					if (left.CompareTo(right) > 0)
					{
						T tmp = array[i + 1];
						array[i + 1] = array[i];
						array[i] = tmp;
						change = true;
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
