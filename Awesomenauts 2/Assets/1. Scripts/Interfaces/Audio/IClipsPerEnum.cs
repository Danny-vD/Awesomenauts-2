using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces.Audio
{
	public interface IAudioClipsPerEnum<TEnum> : IKeyValuePair<TEnum, List<AudioClip>>
		where TEnum : struct, Enum
	{
		bool ShouldLoop { get; }
	}
}