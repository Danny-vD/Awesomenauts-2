using System;
using VDFramework.EventSystem;

namespace Events.Audio
{
	/// <typeparam name="TAudioType">e.g. NautSoundType.attack</typeparam>
	/// <typeparam name="TAudioSet">e.g. Awesomenaut.Voltar</typeparam>
	public class PlayAudioTypeEvent<TAudioType, TAudioSet> : VDEvent
		where TAudioType : struct, Enum
		where TAudioSet : struct, Enum
	{
		public readonly TAudioType SoundTypeToPlay;
		public readonly TAudioSet SetToUse;

		public PlayAudioTypeEvent(TAudioType soundTypeToPlay, TAudioSet setToUse)
		{
			SoundTypeToPlay = soundTypeToPlay;
			SetToUse = setToUse;
		}
	}
}