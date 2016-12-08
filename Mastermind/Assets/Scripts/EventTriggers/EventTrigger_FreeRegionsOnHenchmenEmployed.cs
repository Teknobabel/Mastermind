using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EventTrigger/Free Regions On Henchmen Employed")]
public class EventTrigger_FreeRegionsOnHenchmenEmployed : EventTriggerBase {

	public int
	m_numHenchmenEmployed = 5,
	m_numAgents = 2;

	public override bool EvaluateTrigger ()
	{
		if (GameManager.instance.game.player.currentHenchmen.Count >= m_numHenchmenEmployed && GameManager.instance.game.player.GetNumOwnedRegions() > 0) {

			return true;
		}

		return false;
	}

	public override void ExecuteEvent ()
	{
		Debug.Log ("Event Triggered: Freeing Regions");

		// get up to numAgents number of idle agents

		for (int i = 0; i < m_numAgents; i++) {

			if (GameManager.instance.game.agentOrganization.agentsByState.ContainsKey (GameManager.instance.game.agentOrganization.agentAIState_Idle)) {

				List<AgentWrapper> validAgents = GameManager.instance.game.agentOrganization.agentsByState[GameManager.instance.game.agentOrganization.agentAIState_Idle];

				if (validAgents.Count > 0) {

					// change ai state to attack base

					AgentWrapper aw = validAgents[Random.Range(0, validAgents.Count)];
					aw.ChangeAIState (GameManager.instance.game.agentOrganization.agentAIState_FreeRegion);
				}
			}
		}
	}
}
