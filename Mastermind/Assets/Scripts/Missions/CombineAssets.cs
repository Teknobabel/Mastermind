using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Combine Assets")]
public class CombineAssets : MissionBase {

	public Asset m_requiredUpgrade;
	public List<Asset> m_sourceAssets;
	public Asset m_createdAsset;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// check if player still meets requirements

			if (a.m_region == GameManager.instance.game.player.homeRegion && GameManager.instance.game.player.currentAssets.Contains(m_requiredUpgrade))
			{
				bool hasAllSourceAssets = true;

				foreach (Asset asset in m_sourceAssets) {

					if (!GameManager.instance.game.player.currentAssets.Contains (asset)) {
						hasAllSourceAssets = false;
						break;
					}
				}

				if (hasAllSourceAssets) {

					GameManager.instance.game.player.AddAsset (m_createdAsset);

					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
					t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper () + " GAINS " + m_createdAsset.m_name.ToUpper () + " ASSET.";
					t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
					t.m_resultType = GameEvent.Henchmen_MissionCompleted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
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
		if (!base.IsValid ()) { return false;}		// Valid if region is lair and player has requiredUpgrade and all sourceAssets

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			if (r == GameManager.instance.game.player.homeRegion && GameManager.instance.game.player.currentAssets.Contains(m_requiredUpgrade))
			{

//				foreach (Asset a in m_sourceAssets) {
//					
//					if (!GameManager.instance.game.player.currentAssets.Contains (a)) {
//						return false;
//					}
//				}
					
				return true;
			}
		}
		return false;
	}
}
