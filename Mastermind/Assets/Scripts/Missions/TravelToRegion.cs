using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Travel To Region")]
public class TravelToRegion : MissionBase {

	public override void InitializeMission (MissionWrapper a)
	{
		Debug.Log ("Initializing Mission: " + m_name);

		if (a.m_region != null && a.m_henchmen.Count > 0) {
			
			foreach (Henchmen h in a.m_henchmen) {

				a.m_region.ReserveSlot (h);
			}
		}

	}

	public override void CompleteMission (MissionWrapper a)
	{
		Debug.Log ("Completing Mission: " + m_name);

		string s = null;
		foreach (Henchmen h in a.m_henchmen) {
			
			a.m_region.AddHenchmen (h);
			s += h.henchmenName.ToUpper () + " ";
		}

		TurnResultsEntry t = new TurnResultsEntry ();
		t.m_resultsText = s + " arrives in " + a.m_region.regionName.ToUpper();
		t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
		a.m_organization.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

	public override bool IsValid ()
	{
		return true;
	}

}
