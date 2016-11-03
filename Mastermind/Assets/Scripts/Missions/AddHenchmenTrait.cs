using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class AddHenchmenTrait : MissionBase {

	public TraitData m_newTrait;

	public override void CompleteMission (Organization.ActiveMission a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a.m_mission, a.m_region, a.m_henchmen);

		bool success = WasMissionSuccessful (completionChance);

		if (success) {

			// check if player still meets requirements

//			if (a.m_region == GameManager.instance.game.player.homeRegion && GameManager.instance.game.player.currentAssets.Contains(m_requiredUpgrade))
//			{
//				if (m_sourceAsset == null || GameManager.instance.game.player.currentAssets.Contains (m_sourceAsset)) {
//
//					if (m_sourceAsset != null) {
//						GameManager.instance.game.player.RemoveAsset (m_sourceAsset);
//					}
//
//					GameManager.instance.game.player.AddAsset (m_createdAsset);
//
//					TurnResultsEntry t = new TurnResultsEntry ();
//					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
//
//					if (m_sourceAsset != null) {
//						t.m_resultsText += "\n" + m_sourceAsset.m_name.ToUpper () + " IS REMOVED.";
//					}
//
//					t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper () + " GAINS " + m_createdAsset.m_name.ToUpper () + " ASSET.";
//					t.m_resultsText += "\n" + completionChance.ToString ();
//					t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
//					t.m_resultType = GameEvent.Henchmen_MissionCompleted;
//					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
//				}
//			}

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
//		// Valid if region is lair and player has requiredUpgrade and sourceAsset
//		if (GameManager.instance.currentMissionRequest != null && GameManager.instance.currentMissionRequest.m_region != null) {
//
//			Region r = GameManager.instance.currentMissionRequest.m_region;
//
//			if (r == GameManager.instance.game.player.homeRegion && GameManager.instance.game.player.currentAssets.Contains(m_requiredUpgrade))
//			{
//				if (m_sourceAsset == null || GameManager.instance.game.player.currentAssets.Contains (m_sourceAsset)) {
//					return true;
//				}
//			}
//		}
		return false;
	}
}
