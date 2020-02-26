using UnityEngine;
using VDFramework.EventSystem;

namespace Events.Audio
{
	public class PlayAudioClipEvent : VDEvent
	{
		public readonly AudioClip ClipToPlay;
		
		public PlayAudioClipEvent(AudioClip clipToPlay)
		{
			ClipToPlay = clipToPlay;
		}
	}
}