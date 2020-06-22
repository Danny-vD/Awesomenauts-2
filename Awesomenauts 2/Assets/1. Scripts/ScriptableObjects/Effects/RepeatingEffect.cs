using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._1._Scripts.ScriptableObjects.Effects;
using Maps;
using Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Effects/RepeatingEffect")]
public class RepeatingEffect : AEffect
{
	public override EffectTrigger Trigger => EffectTrigger.OnRoundStart;
	public AEffect Effect;

	public override IEnumerator TriggerEffect(Card c, CardSocket containingSocket, CardSocket targetSocket)
	{

		//Either BlueSide and A or B
		//Or RedSide and A or B
		SocketSide turretSide = containingSocket.SocketSide &
		                        (SocketSide.BlueSide | SocketSide.RedSide | SocketSide.SideA | SocketSide.SideB);

		CardSocket[] sockets = MapTransformInfo.Instance.SocketManager.GetSocketsOnSide(turretSide);

		int id = c.Statistics.GetValue<int>(CardPlayerStatType.TeamID);

		CardSocket next = sockets
			.OrderBy(x => x.transform.position.x)
			.FirstOrDefault(x => x.HasCard && x.DockedCard.Statistics.GetValue<int>(CardPlayerStatType.TeamID) != id);

		if (next != null)
		{
			Effect._TriggerEffect(c, containingSocket, next);
		}

		yield return null;
		//base.TriggerEffect(c, containingSocket, targetSocket);
	}

}
