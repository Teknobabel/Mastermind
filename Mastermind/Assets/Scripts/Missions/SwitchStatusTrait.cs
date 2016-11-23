using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Switch Status Trait")]
public class SwitchStatusTrait : MissionBase {

	public TraitData m_affectedTrait;
	public TraitData m_newTrait;

	public Asset m_requiredAsset;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		a.m_henchmenInFocus.UpdateStatusTrait (m_newTrait);

		TurnResultsEntry t = new TurnResultsEntry ();
		t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
		t.m_resultsText += "\n" + a.m_henchmenInFocus.henchmenName.ToUpper () + "'s status is now " + m_newTrait.m_name.ToUpper();
		//			t.m_resultsText += "\n" + completionChance.ToString ();
		t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
		t.m_resultType = GameEvent.Henchmen_MissionCompleted;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

	}

	public override string GetNameText ()
	{
		string s = m_name + " - ";

		if (GameManager.instance.currentMissionWrapper.m_henchmenInFocus != null) {
			s += GameManager.instance.currentMissionWrapper.m_henchmenInFocus.henchmenName;
		}

		return s;
	}

	public override bool IsValid ()
	{

		if (m_requiredAsset == null || (m_requiredAsset != null && GameManager.instance.game.player.orgBase.m_currentAssets.Contains(m_requiredAsset)) && 
			GameManager.instance.currentMissionWrapper.m_region == GameManager.instance.game.player.homeRegion)
		{
			if (GameManager.instance.currentMissionWrapper.m_henchmenInFocus != null)
			{
				if (GameManager.instance.currentMissionWrapper.m_henchmenInFocus.statusTrait == m_affectedTrait) {
					return true;
				} else {
					return false;
				}
			}
		}

		return false;
	}

}
