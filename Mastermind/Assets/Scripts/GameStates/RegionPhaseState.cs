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

					if (h.statusTrait.m_type == TraitData.TraitType.Healthy && Random.Range(0.0f, 1.0f) > 0.75f) {

						// if healthy, chance to return to world

						Region r = GameManager.instance.game.GetRandomRegion (true);

						if (r != null) {
							
							hs.RemoveHenchmen ();
							r.AddHenchmen (h);

							TurnResultsEntry t = new TurnResultsEntry ();
							t.m_resultsText = h.henchmenName.ToUpper() + " appears in " + r.regionName.ToUpper() + "!";
							t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
							GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
						}

					} else if (h.statusTrait.m_type == TraitData.TraitType.Incapacitated && Random.Range(0.0f, 1.0f) > 0.65f) {

						// if incapacitated, chance to go to critical

						h.UpdateStatusTrait (GameManager.instance.m_statusTraits [2]);

					} else if (h.statusTrait.m_type == TraitData.TraitType.Critical && Random.Range(0.0f, 1.0f) > 0.65f) {

						// if critical, chance to go to injured

						h.UpdateStatusTrait (GameManager.instance.m_statusTraits [1]);

					} else if (h.statusTrait.m_type == TraitData.TraitType.Injured && Random.Range(0.0f, 1.0f) > 0.65f) {

						// if injured, chance to go to healthy

						h.UpdateStatusTrait (GameManager.instance.m_statusTraits [0]);

					} 

				} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent && hs.m_agent.m_agent.currentState != Henchmen.state.Captured && Random.Range (0.0f, 1.0f) < 0.4f) {

					Henchmen h = hs.m_agent.m_agent;

					if (h.statusTrait.m_type == TraitData.TraitType.Healthy && Random.Range(0.0f, 1.0f) > 0.75f) {

						// if healthy, chance to return to world

						Region r = GameManager.instance.game.GetRandomRegion (true);

						if (r != null) {
							
							AgentWrapper aw = hs.m_agent;

							if (aw.m_vizState == AgentWrapper.VisibilityState.Visible) {

								aw.ChangeVisibilityState (AgentWrapper.VisibilityState.Hidden);
							}

							hs.RemoveAgent ();
							r.AddAgent (aw);

//							TurnResultsEntry t = new TurnResultsEntry ();
//							t.m_resultsText = h.henchmenName.ToUpper() + " appears in " + r.regionName.ToUpper() + "!";
//							t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
//							GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
						}

					} else if (h.statusTrait.m_type == TraitData.TraitType.Incapacitated && Random.Range(0.0f, 1.0f) > 0.65f) {

						// if incapacitated, chance to go to critical

						h.UpdateStatusTrait (GameManager.instance.m_statusTraits [2]);

					} else if (h.statusTrait.m_type == TraitData.TraitType.Critical && Random.Range(0.0f, 1.0f) > 0.65f) {

						// if critical, chance to go to injured

						h.UpdateStatusTrait (GameManager.instance.m_statusTraits [1]);

					} else if (h.statusTrait.m_type == TraitData.TraitType.Injured && Random.Range(0.0f, 1.0f) > 0.65f) {

						// if injured, chance to go to healthy

						h.UpdateStatusTrait (GameManager.instance.m_statusTraits [0]);

					} 
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
