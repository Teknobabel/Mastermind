using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "EventTrigger/Attack Base On Intel Captured")]
public class EventTrigger_AttackBaseOnIntelCaptured : EventTriggerBase {

	public int 
	m_numCapturedIntel = 2,
	m_numAgents = 3;

	public override bool EvaluateTrigger ()
	{
		if (GameManager.instance.game.player.currentIntel >= m_numCapturedIntel) {

			return true;
		}

		return false;
	}

	public override void ExecuteEvent ()
	{

	}
}
