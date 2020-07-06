using System.Collections.Generic;
using AwsomenautsCardGame.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace AwsomenautsCardGame.Menu.UI {
	public class MapDropdownInitializer : MonoBehaviour
	{
		private Dropdown ddMapList;
		
		public Image mapPreview;
		// Start is called before the first frame update
		private void Start()
		{
			CardNetworkManager manager = CardNetworkManager.Instance;
			if (manager == null)
			{
				return;
			}

			ddMapList = GetComponent<Dropdown>();

			List<Dropdown.OptionData> data = new List<Dropdown.OptionData>();
			ddMapList.options = data;
			for (int i = 0; i < CardNetworkManager.Instance.AvailableMaps.Length; i++)
			{
				Dropdown.OptionData od = new Dropdown.OptionData(manager.AvailableMaps[i].Prefab.name, manager.AvailableMaps[i].MapIcon);
				data.Add(od);
			}


			ddMapList.onValueChanged.AddListener(manager.SetMapToLoad);
			ddMapList.onValueChanged.AddListener(OnDDValueChanged);
		
			ddMapList.value = 0;
			ddMapList.RefreshShownValue();
			OnDDValueChanged(0);
		}

		public void OnDDValueChanged(int id)
		{
			mapPreview.sprite = ddMapList.options[id].image;
			//Debug.Log("Changing Icon: " + id);
		}

	}
}
