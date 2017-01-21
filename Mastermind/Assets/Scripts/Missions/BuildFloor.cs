using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Build Floor")]
public class BuildFloor : MissionBase {
	
	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// add floor to base

			GameManager.instance.game.player.orgBase.AddNewFloor (GameManager.instance.game.player.orgBase.m_floors.Count+1);

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n A new floor has been added to the Base!";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
//
		} else {

//			a.m_floorInFocus.m_floorState = Base.FloorState.Empty;

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		Debug.Log ("D;SLKFJS;LKFJLAKSDJFLSKFJ;SLFD");
//		if (!base.IsValid ()) { return false;}		

		Organization player = GameManager.instance.game.player;
//		MissionWrapper mw = GameManager.instance.currentMissionWrapper;

//		if (mw.m_scope == m_targetType && mw.m_floorInFocus.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_Workshop  && player.orgBase.m_floors.Count < player.orgBase.maxFloors)
//		{
//			return true;
//		}

		if (player.orgBase.m_floors.Count < player.orgBase.maxFloors)
		{
			return true;
		}

		return false;
	}

}
