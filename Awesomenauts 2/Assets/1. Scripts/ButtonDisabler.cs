using Maps;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour
{
	private bool init = false;

	public Button Button;
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (!init && BoardLogic.Logic != null)
		{
			init = true;
			BoardLogic.Logic.OnEndTurn += OnEndTurn;
			BoardLogic.Logic.OnStartTurn += OnStartTurn;
		}
	}

	private void OnStartTurn()
	{
		Button.interactable = true;
	}

	private void OnEndTurn()
	{
		Button.interactable = false;
	}
}
