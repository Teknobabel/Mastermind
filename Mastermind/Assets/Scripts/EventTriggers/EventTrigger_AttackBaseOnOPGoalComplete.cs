using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EventTrigger/Attack Base On OP Goal Complete")]
public class EventTrigger_AttackBaseOnOPGoalComplete : EventTriggerBase {

	public int 
	m_numGoalsComplete = 3,
	m_numAgents = 2,
	m_numTurns = 3;

	public override bool EvaluateTrigger ()
	{
		foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {

			if (op.GetNumGoalsCompleted () >= m_numGoalsComplete) {

				return true;
			}
		}

		// debug
//		if (GameManager.instance.game.turnNumber == 3) {
//
//			return true;
//		}

		return false;
	}

	public override void ExecuteEvent ()
	{
		base.ExecuteEvent ();

		Debug.Log ("Event Triggered: Attacking Base");

		// get up to numAgents number of idle agents

		for (int i = 0; i < m_numAgents; i++) {

			if (GameManager.instance.game.agentOrganization.agentsByState.ContainsKey (GameManager.instance.game.agentOrganization.agentAIState_Idle)) {

				List<AgentWrapper> validAgents = GameManager.instance.game.agentOrganization.agentsByState[GameManager.instance.game.agentOrganization.agentAIState_Idle];

				if (validAgents.Count > 0) {

					// change ai state to attack base

					AgentWrapper aw = validAgents[Random.Range(0, validAgents.Count)];
					aw.ChangeAIState (GameManager.instance.game.agentOrganization.agentAIState_AttackBase);
				}
			}
		}
	}
}
