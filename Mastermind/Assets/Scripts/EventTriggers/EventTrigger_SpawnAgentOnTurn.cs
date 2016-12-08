using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EventTrigger/Spawn Agent On Turn")]
public class EventTrigger_SpawnAgentOnTurn : EventTriggerBase {

	public int 
	m_turnNumber = 3,
	m_numberOfAgents = 1,
	m_maxAgentRank = 3;

	public override bool EvaluateTrigger ()
	{
		if (GameManager.instance.game.turnNumber >= m_turnNumber) {

			return true;
		}

		return false;
	}

	public override void ExecuteEvent ()
	{
		base.ExecuteEvent ();

		Debug.Log ("Event Triggered: Spawning " + m_numberOfAgents + " Agents");

		for (int i = 0; i < m_numberOfAgents; i++) {

			// gather list of valid agents by max rank

			List<Henchmen> agentBank = new List<Henchmen> ();
			List<Henchmen> currentHenchmen = new List<Henchmen>();

			// sort out currently active henchmen from bank list

			foreach (AgentWrapper aw in GameManager.instance.game.agentOrganization.currentAgents) {
				currentHenchmen.Add (aw.m_agent);
			}

			foreach (Henchmen a in GameManager.instance.game.agents) {

				if (!currentHenchmen.Contains (a) && a.rank <= m_maxAgentRank) {

					agentBank.Add (a);
				}
			}

			// choose agent from list and spawn

			if (agentBank.Count > 0)
			{
				Henchmen agentToSpawn = agentBank[Random.Range(0, agentBank.Count)];

				GameManager.instance.game.agentOrganization.SpawnAgentInWorld (agentToSpawn);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = agentToSpawn.henchmenName.ToUpper () + " is now searching for you ";
				t.m_resultType = GameEvent.None;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
		}
	}
}
