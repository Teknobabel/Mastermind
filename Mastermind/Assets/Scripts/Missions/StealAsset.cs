using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Steal Asset")]
public class StealAsset : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// find a revealed non-empty asset token
			TokenSlot at = a.m_tokenInFocus;

			if (at.m_state == TokenSlot.State.Revealed) {

				Asset asset = at.m_assetToken.m_asset;

				// remove it from region and add to player bank

				if (GameManager.instance.game.player.currentAssets.Count < GameManager.instance.game.player.maxAssets) {

					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
					t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper () + " GAINS A " + asset.m_name.ToUpper () + " ASSET.";

					GameManager.instance.game.player.AddAsset (at.m_assetToken.m_asset);
					a.m_region.RemoveAssetToken (at);
				} else {

					t.m_resultsText += "\n" + a.m_mission.m_name.ToUpper () + " mission aborted: No room for Asset in Lair";
				}

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

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_tokenInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_tokenInFocus.m_assetToken.m_name + "</size>";
		}

		return s;
	}

	public override bool IsValid ()
	{
		bool hasPreRequisites = base.IsValid ();	

		// valid if there is a revealed, non empty asset token in the region

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_tokenInFocus != null && GameManager.instance.game.player.currentAssets.Count < GameManager.instance.game.player.maxAssets) {

			if (GameManager.instance.currentMissionWrapper.m_tokenInFocus.m_state == TokenSlot.State.Revealed && GameManager.instance.currentMissionWrapper.m_tokenInFocus.m_type == TokenSlot.TokenType.Asset &&
				GameManager.instance.currentMissionWrapper.m_tokenInFocus.m_assetToken != GameManager.instance.m_intel) {
					
				if (hasPreRequisites) {
					return true;
				}
			}

		}

		return false;
	}
}
