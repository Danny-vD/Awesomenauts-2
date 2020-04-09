using Structs.Audio;
using VDFramework.SharedClasses.EventSystem;

namespace Events.Audio
{
	public class PlayAudioClipEvent : VDEvent
	{
		public readonly AudioClipData ClipData;
		
		public PlayAudioClipEvent(AudioClipData clipData)
		{
			ClipData = clipData;
		}
	}
}