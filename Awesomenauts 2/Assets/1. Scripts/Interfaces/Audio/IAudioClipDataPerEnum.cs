using System;
using Structs.Audio;

namespace Interfaces.Audio
{
	public interface IAudioClipDataPerSoundType<TEnum> : IKeyValuePair<TEnum, AudioClipData>
		where TEnum : struct, Enum { }
}