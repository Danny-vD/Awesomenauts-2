using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TransportProtocolDropdownHelper : MonoBehaviour
{
	private Dropdown dd = null;
	// Start is called before the first frame update
	private void Start()
	{
		dd = GetComponent<Dropdown>();
		if (dd != null)
		{
			Dropdown.OptionData[] names = Enum.GetNames(typeof(CardNetworkManager.TransportType))
				.Select(x => new Dropdown.OptionData(x)).ToArray();
			dd.options.Clear();
			dd.options.AddRange(names);
			dd.SetValueWithoutNotify((int)CardNetworkManager.Instance.TransportInfoData.TransportType);
			dd.onValueChanged.AddListener(ProtocolChanged);
		}
	}


	public void ProtocolChanged(int index)
	{
		//CardNetworkManager.Instance.ChangeTransport((CardNetworkManager.TransportType)index);
	}

}
