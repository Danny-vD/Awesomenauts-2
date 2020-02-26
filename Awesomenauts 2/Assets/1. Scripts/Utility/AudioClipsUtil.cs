using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using VDFramework.Extensions;

namespace Utility
{
	public static class FakeDictionaryUtil
	{
		public static void PopulateEnumDictionary<TKeyValuePair, TKey, TValue>(ref List<TKeyValuePair> list)
			where TKeyValuePair : IKeyValuePair<TKey, TValue>, new()
			where TKey : struct, Enum
		{
			if (list == null)
			{
				list = new List<TKeyValuePair>();
			}

			TKey @enum = default;
			TKey[] enumValues = @enum.GetValues().ToArray();

			// Remove the keys that are no longer in the enum
			if (list.Count >= enumValues.Length)
			{
				list.RemoveRange(enumValues.Length, list.Count - enumValues.Length);
			}

			foreach (TKey enumValue in enumValues)
			{
				if (ContainsKey<TKeyValuePair, TKey, TValue>(list, enumValue))
				{
					continue;
				}

				list.Add(new TKeyValuePair {Key = enumValue});
			}
		}

		private static bool ContainsKey<TKeyValuePair, TKey, TValue>(IEnumerable<TKeyValuePair> list, TKey enumValue)
			where TKeyValuePair : IKeyValuePair<TKey, TValue>, new()
			where TKey : struct, Enum
		{
			return list.Any(pair => Equals(pair.Key, enumValue));
		}
	}
}