using System;
using UnityEngine;

namespace Interfaces.Audio
{
	public interface IAudioSetPerEnum<TEnum> : IKeyValuePair<TEnum, ScriptableObject>
		where TEnum : struct, Enum { }
}