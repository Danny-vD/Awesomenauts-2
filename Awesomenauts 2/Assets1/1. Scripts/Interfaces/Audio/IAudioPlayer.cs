using Events.Audio;
using Structs.Audio;

namespace Interfaces.Audio
{
	/// <summary>
	/// Facade interface for playing audio
	/// </summary>
	public interface IAudioPlayer
	{
		void OnPlayAudioClip(PlayAudioClipEvent playAudioClipEvent);

		void PlayAudio(AudioClipData clip);
	}
}