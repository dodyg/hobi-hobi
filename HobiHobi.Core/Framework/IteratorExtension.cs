using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HobiHobi.Core.Framework
{
	public static class IteratorExtension
	{
		/// <summary>
		/// This extension method apply the given lambda to every single item in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="self"></param>
		/// <param name="func"></param>
		public static void ForEach<T>(this IEnumerable<T> self, Action<T> func)
		{
			foreach (var i in self)
				func(i);
		}

		
		/// <summary>
		/// Convert a ReadOnlyCollection of Type T to a List of type R.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="R"></typeparam>
		/// <param name="coll"></param>
		/// <param name="convert"></param>
		/// <returns></returns>
		public static List<R> ToList<T, R>(this ReadOnlyCollection<T> coll, Func<T,R> convert)
		{
			var newList = new List<R>();

			foreach (var i in coll)
				newList.Add(convert(i));
			
			return newList;
		}
	
	}
}
