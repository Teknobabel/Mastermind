using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Unlock Omega Plan")]
public class UnlockOmegaPlan : MissionBase {

	public Asset m_requiredBaseUpgrade;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {

				if (op.state == OmegaPlan.State.Hidden) {

					op.ChangeState (OmegaPlan.State.Revealed);

					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
					t.m_resultsText += "\nOMEGA PLAN: " + op.opName.ToUpper() + " is now unlocked!";
					t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
					t.m_resultType = GameEvent.Henchmen_MissionCompleted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

					break;
				}
			}

		} else {


			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		// check if required base upgrade is present

		if (m_requiredBaseUpgrade != null && !GameManager.instance.game.player.orgBase.m_currentAssets.Contains (m_requiredBaseUpgrade)) {

			return false;
		}

		// valid if there is a locked Omega Plan and the current region is the Lair

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;
			MissionWrapper mw = GameManager.instance.currentMissionWrapper;

			if (mw.m_scope == TargetType.Floor && mw.m_floorInFocus.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_AdvancedResearch && r == GameManager.instance.game.player.homeRegion) {

				foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {

					if (op.state == OmegaPlan.State.Hidden) {
						return true;
					}
				}
			}
		}

		return false;
	}
}
