using System;
using UnityEngine;

namespace Interfaces.Audio
{
	public interface IAudioSetPerEnum<TEnum> : IEquatable<IAudioSetPerEnum<TEnum>>
		where TEnum : struct, Enum
	{
		TEnum Key { get; }
		ScriptableObject Value { get; }
	}
}