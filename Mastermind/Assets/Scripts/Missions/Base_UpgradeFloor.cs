using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Base_Upgrade Floor")]
public class Base_UpgradeFloor : MissionBase {

	public int m_upgradeLevel = 2;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();

		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		foreach (Henchmen h in a.m_henchmen) {

			t.m_henchmenIDs.Add (h.id);
		}

		if (a.m_success) {

			if (a.m_floorInFocus != null) {

				a.m_floorInFocus.m_level = m_upgradeLevel;

				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n +" + a.m_floorInFocus.m_installedUpgrade.m_name.ToUpper() + " has been upgraded to Level " + a.m_floorInFocus.m_level.ToString() + "!";
				t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
				t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			}
		
		} else {

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;


		}

		t.m_resultsText += CheckForNewTraits (a);

		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

	public override bool IsValid ()
	{
		if (GameManager.instance.currentMissionWrapper.m_floorInFocus != null && GameManager.instance.currentMissionWrapper.m_floorInFocus.m_level < m_upgradeLevel) {
			return true;
		} else {
			return false;
		}
	}
}
