using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class TravelToRegion : MissionBase {

	public override void CompleteMission (Organization.ActiveMission a)
	{
		Debug.Log ("Completing Mission: " + m_name);

		Henchmen h = a.m_henchmen[0];

		h.SetRegion (a.m_region);

		TurnResultsEntry t = new TurnResultsEntry ();
		t.m_resultsText = h.henchmenName.ToUpper() + " arrives in " + a.m_region.regionName.ToUpper();
		t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

	public override bool IsValid ()
	{
		return true;
	}

}
