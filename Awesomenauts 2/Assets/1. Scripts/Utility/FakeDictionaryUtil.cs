using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using VDFramework.Extensions;

namespace Utility
{
	public static class FakeDictionaryUtil
	{
		/// <summary>
		/// Will add a KeyValuePair for every enumValue to the list
		/// </summary>
		public static void PopulateEnumDictionary<TKeyValuePair, TEnum, TValue>(List<TKeyValuePair> list)
			where TKeyValuePair : IKeyValuePair<TEnum, TValue>, new()
			where TEnum : struct, Enum
		{
			if (list == null)
			{
				list = new List<TKeyValuePair>();
			}

			TEnum @enum = default;
			TEnum[] enumValues = @enum.GetValues().ToArray();

			// Remove the keys that are no longer in the enum
			if (list.Count >= enumValues.Length)
			{
				list.RemoveRange(enumValues.Length, list.Count - enumValues.Length);
			}

			foreach (TEnum enumValue in enumValues)
			{
				if (ContainsKey<TKeyValuePair, TEnum, TValue>(list, enumValue))
				{
					continue;
				}

				list.Add(new TKeyValuePair {Key = enumValue});
			}
		}

		private static bool ContainsKey<TKeyValuePair, TKey, TValue>(IEnumerable<TKeyValuePair> collection, TKey key)
			where TKeyValuePair : IKeyValuePair<TKey, TValue>
		{
			return collection.Any(pair => Equals(pair.Key, key));
		}
	}
}