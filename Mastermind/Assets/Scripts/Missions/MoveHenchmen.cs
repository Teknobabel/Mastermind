using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Move Henchmen")]
public class MoveHenchmen : MissionBase {

	public override void InitializeMission (MissionWrapper a)
	{
		Debug.Log ("Initializing Mission: " + m_name);

		// moving to a floor is instant, remove mission from list

		for (int i=0; i < GameManager.instance.game.player.activeMissions.Count; i++)
		{
			MissionWrapper thisMW = GameManager.instance.game.player.activeMissions [i];

			if (thisMW == a) {

				GameManager.instance.game.player.activeMissions.RemoveAt (i);
				break;
			}
		}

		if (a.m_henchmenInFocus != null || a.m_henchmen.Count > 0) {

			a.m_henchmenInFocus = a.m_henchmen [0];
		}

		a.m_floorInFocus.m_region.AddHenchmen (a.m_henchmenInFocus);
	}

	public override void CompleteMission (MissionWrapper a)
	{

	}

	public override bool IsValid ()
	{
//		if (!base.IsValid ()) { return false;}		return true;
		return false;
	}

	public override void CancelMission (MissionWrapper a)
	{
		base.CancelMission (a);
	}
}
