using UnityEngine;
using System.Collections;

public class RegionPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Entering Region Phase");

		// check for any henchmen and agents in Limbo

		foreach (Region.HenchmenSlot hs in GameManager.instance.game.limbo.henchmenSlots) {

			if (hs.m_state != Region.HenchmenSlot.State.Empty) {

				// remove agent or henchmen from limbo and place in random region

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player && hs.m_henchmen.currentState != Henchmen.state.Captured && Random.Range (0.0f, 1.0f) < 0.4f) {

					Henchmen h = hs.m_henchmen;
					hs.RemoveHenchmen ();
					Region r = GameManager.instance.game.GetRandomRegion (true);
					r.AddHenchmen (h);

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = h.henchmenName.ToUpper() + " appears in " + r.regionName.ToUpper() + "!";
					t.m_resultType = GameEvent.Henchmen_ArriveInRegion;

				} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent && hs.m_agent.m_agent.currentState != Henchmen.state.Captured && Random.Range (0.0f, 1.0f) < 0.4f) {

					AgentWrapper aw = hs.m_agent;
					hs.RemoveAgent ();
					Region r = GameManager.instance.game.GetRandomRegion (true);
					r.AddAgent (aw);

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = aw.m_agent.henchmenName.ToUpper() + " appears in " + r.regionName.ToUpper() + "!";
					t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
				}
			}
		}

		GameManager.instance.ChangeGameState (GameManager.instance.agentPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Region Phase");
	}
}
