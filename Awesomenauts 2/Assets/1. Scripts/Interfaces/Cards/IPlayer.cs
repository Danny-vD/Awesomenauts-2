using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
	Transform CameraTransform { get; }
	Transform HandAnchorPoint { get; }
	IHand Hand { get; set; }
	IDeck Deck { get; set; }
	void ToggleInteractions(bool active);
	void Initialize(GameSettingsObject settings, IHand hand, IDeck deck);
}
