using System;

namespace Interfaces.Audio
{
	public interface IAudioSetPerEnum<TEnum, TAudioSet> : IKeyValuePair<TEnum, TAudioSet>
		where TEnum : struct, Enum
		where TAudioSet : IAudioSet { }
}