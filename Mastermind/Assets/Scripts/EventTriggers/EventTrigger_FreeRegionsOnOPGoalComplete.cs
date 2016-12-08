using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EventTrigger/Free Regions On OP Goal Complete")]
public class EventTrigger_FreeRegionsOnOPGoalComplete : EventTriggerBase {

	public int
	m_numOPGoalsCompleted = 5,
	m_numAgents = 2;

	public override bool EvaluateTrigger ()
	{
		if (GameManager.instance.game.player.GetNumOwnedRegions() > 0) {

			foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {

				if (op.GetNumGoalsCompleted () >= m_numOPGoalsCompleted) {
					return true;
				}
			}
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
