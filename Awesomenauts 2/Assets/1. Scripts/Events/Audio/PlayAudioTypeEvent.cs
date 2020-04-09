using System;
using VDFramework.EventSystem;

namespace Events.Audio
{
	/// <typeparam name="TEnum">e.g. Awesomenaut.Scoop</typeparam>
	/// <typeparam name="TSoundToPlay">e.g. NautSound.Spawn</typeparam>
	public class PlayAudioTypeEvent<TEnum, TSoundToPlay> : VDEvent
		where TEnum : struct, Enum
		where TSoundToPlay : struct, Enum
	{
		public readonly TEnum EnumName;
		public readonly TSoundToPlay SoundTypeToPlay;

		public PlayAudioTypeEvent(TEnum enumName, TSoundToPlay soundTypeToPlay)
		{
			SoundTypeToPlay = soundTypeToPlay;
			EnumName = enumName;
		}
	}
}