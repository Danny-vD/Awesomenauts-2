using Events.Audio;
using UnityEngine;

namespace Interfaces.Audio
{
	/// <summary>
	/// Facade interface for playing audio
	/// </summary>
	public interface IAudioPlayer
	{
		void OnPlayAudioClip(PlayAudioClipEvent playAudioClipEvent);

		void PlayAudio(AudioClip clip);
	}
}