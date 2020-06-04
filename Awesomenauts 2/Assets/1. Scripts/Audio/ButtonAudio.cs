using UnityEngine;
using VDFramework;
using EventType = Enums.Audio.EventType;

namespace Audio
{
	public class ButtonAudio : BetterMonoBehaviour
	{
		public void PlaySound()
		{
			AudioPlayer.PlaySound(EventType.SFX_CARDS_CardDeath, Camera.main.gameObject);
		}
	}
}