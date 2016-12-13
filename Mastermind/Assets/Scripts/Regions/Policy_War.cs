using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Token/Policy - Declare War")]
public class Policy_War : PolicyToken {

	public override void UpdatePolicy (TokenSlot t)
	{
		base.UpdatePolicy (t);

		// player gains infamy each turn War is active

		int infamyGain = 10;

		GameManager.instance.game.player.GainInfamy (infamyGain);

		TurnResultsEntry tr = new TurnResultsEntry ();
		tr.m_resultsText = t.m_region.regionName.ToUpper() + " is at War with " + GameManager.instance.game.player.orgName.ToUpper();
		tr.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper() + " gains" + infamyGain.ToString() + " Infamy" ;
		tr.m_resultType = GameEvent.Henchmen_MissionCompleted;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, tr);
	}
}
