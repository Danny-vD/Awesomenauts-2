using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DebugPanel {
	public class SolarDisplay : MonoBehaviour
	{
		public Text SolarText;
		public List<Image> SolarImages;
		public Sprite SolarActive;
		public Sprite SolarInactive;

		public void SetSolar(int amount)
		{
			SolarText.text = $"{(int)amount}/10";
			SolarImages.ForEach(x=>x.sprite = SolarInactive);
			SolarImages.Take(amount).ToList().ForEach(x=>x.sprite = SolarActive);
		}
	}
}