using AwsomenautsCardGame.Audio;
using AwsomenautsCardGame.Enums.Audio;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour
{
	public Slider slider;
	// Start is called before the first frame update
	void Start()
	{
		if (!AudioManager.IsInitialized) return;

		float v = AudioManager.Instance.GetVolume(BusType.Master);
		v *= v;
		slider.value = Mathf.SmoothStep(0, 1, v);
	}


	public void SetVolume(float volume)
	{
		if (!AudioManager.IsInitialized) return;
		AudioManager.Instance.SetVolume(BusType.Master, volume * volume);
	}

}
