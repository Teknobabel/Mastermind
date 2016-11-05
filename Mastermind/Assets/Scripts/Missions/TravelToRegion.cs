using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Travel To Region")]
public class TravelToRegion : MissionBase {

	public override void InitializeMission (Organization.ActiveMission a)
	{
		Debug.Log ("Initializing Mission: " + m_name);

		if (a.m_region != null && a.m_henchmen.Count > 0) {
			
			foreach (Henchmen h in a.m_henchmen) {

				a.m_region.ReserveSlot (h);
			}
		}

	}

	public override void CompleteMission (Organization.ActiveMission a)
	{
		Debug.Log ("Completing Mission: " + m_name);

		string s = null;
		foreach (Henchmen h in a.m_henchmen) {
			
			a.m_region.AddHenchmen (h);
			s += h.henchmenName.ToUpper () + " ";
		}

//		Henchmen h = a.m_henchmen[0];
//
//		h.SetRegion (a.m_region);
//
		TurnResultsEntry t = new TurnResultsEntry ();
		t.m_resultsText = s + " arrives in " + a.m_region.regionName.ToUpper();
		t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

	public override bool IsValid ()
	{
		return true;
	}

}
