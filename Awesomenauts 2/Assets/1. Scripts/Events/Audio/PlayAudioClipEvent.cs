using Structs.Audio;
using VDFramework.EventSystem;

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