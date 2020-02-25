using System;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces.Audio
{
	public interface IAudioClipsPerEnum<TEnum>
		where TEnum : struct, Enum
	{
		TEnum Key { get; set; }
		
		List<AudioClip> Value { get; }
		
		bool ShouldLoop { get; }
	}
}