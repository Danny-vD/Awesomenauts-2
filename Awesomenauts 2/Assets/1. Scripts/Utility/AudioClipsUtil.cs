using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.Audio;
using VDFramework.Extensions;

namespace Utility
{
	public static class AudioClipsUtil
	{
		public static void PopulateDictionary<TEnum, TStruct>(ref List<TStruct> list)
			where TEnum : struct, Enum
			where TStruct : IAudioClipsPerEnum<TEnum>, new()
		{
			if (list == null)
			{
				list = new List<TStruct>();
			}

			TEnum @enum = default;
			TEnum[] enumValues = @enum.GetValues().ToArray();

			//Remove the keys that are no longer in the enum
			if (list.Count >= enumValues.Length)
			{
				list.RemoveRange(enumValues.Length, list.Count - enumValues.Length);
			}

			foreach (TEnum soundType in enumValues)
			{
				if (ContainsKey(list, soundType))
				{
					continue;
				}

				list.Add(new TStruct {Key = soundType});
			}
		}

		private static bool ContainsKey<TEnum, TStruct>(IEnumerable<TStruct> list, TEnum soundType)
			where TEnum : struct, Enum
			where TStruct : IAudioClipsPerEnum<TEnum>, new()
		{
			return list.Any(pair => Equals(pair.Key, soundType));
		}
	}
}