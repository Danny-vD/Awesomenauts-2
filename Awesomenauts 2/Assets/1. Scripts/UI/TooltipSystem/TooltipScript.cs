using System;
using System.Collections.Generic;
using System.Linq;
using VDFramework.VDUnityFramework.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.UI.TooltipSystem {
	public class TooltipScript : Singleton<TooltipScript>
	{
		[Serializable]
		public struct Tooltip
		{
			public string Name;
			public TooltipType Type;
			public string Message;
			public Sprite Background;
		}

		public List<Tooltip> Tooltips;
		public Image Background;
		private Sprite DefaultBackground;
		public Text TooltipText;


		// Start is called before the first frame update
		private void Start()
		{
			DefaultBackground = Background.sprite;
		}

		// Update is called once per frame
		private void Update()
		{

		}

		public void SetTooltip(TooltipType type)
		{
			Tooltip tip = Tooltips.FirstOrDefault(x => x.Type == type);
			Background.sprite = tip.Background == null ? DefaultBackground : tip.Background;
			TooltipText.text = tip.Message ?? "";
		}
	}
}
