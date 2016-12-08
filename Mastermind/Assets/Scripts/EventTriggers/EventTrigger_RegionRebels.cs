using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EventTrigger/Regions Rebel")]
public class EventTrigger_RegionRebels : EventTriggerBase {

	public int 
	m_numPlayerOwnedRegions = 2,
	m_numToRebel = 1;

	public override bool EvaluateTrigger ()
	{

		if (GameManager.instance.game.player.GetNumOwnedRegions() >= m_numPlayerOwnedRegions) {

			return true;
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

		List<Region> ownedRegions = new List<Region> ();

		foreach (Region r in GameManager.instance.game.regions) {

			bool playerOwned = true;

			foreach (TokenSlot ts in r.controlTokens) {

				if (ts.owner == Region.Owner.AI) {

					playerOwned = false;
					break;
				}
			}

			if (playerOwned) {

				ownedRegions.Add (r);
			}
		}

		for (int i = 0; i < m_numToRebel; i++) {

			if (ownedRegions.Count > 0) {

				Region rebel = ownedRegions[Random.Range(0, ownedRegions.Count)];

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = rebel.regionName.ToUpper () + " has rebelled!";
//				t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper () + " has lost all Control Tokens in " + rebel.regionName.ToUpper () + "!";


				int tokensLost = 0;

				foreach (TokenSlot ts in rebel.controlTokens) {

					if (ts.owner == Region.Owner.Player && (Random.Range(0.0f, 1.0f) > 0.35f) || tokensLost == 0) {

						ts.ChangeOwner (Region.Owner.AI);
						tokensLost++;
					}
				}

				t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper () + " lost " + tokensLost.ToString() + " Control Tokens in " + rebel.regionName.ToUpper () + "!";
				t.m_resultType = GameEvent.None;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
		}
	}
}
