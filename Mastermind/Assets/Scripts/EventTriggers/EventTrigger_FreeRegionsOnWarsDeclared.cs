using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EventTrigger/Free Regions On Wars Declared")]
public class EventTrigger_FreeRegionsOnWarsDeclared : EventTriggerBase {

	public int
	m_numWarsDeclared = 3,
	m_numAgents = 2;

	public override bool EvaluateTrigger ()
	{
		int warsDeclared = 0;

		foreach (Region r in GameManager.instance.game.regions) {

			foreach (TokenSlot ts in r.policyTokens) {

				if (ts.m_policyToken != null && ts.m_policyToken == GameManager.instance.m_declareWar) {

					warsDeclared++;

					if (warsDeclared >= m_numWarsDeclared) {

						return true;
					}
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
