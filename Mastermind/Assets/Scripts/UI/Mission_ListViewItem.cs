using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Mission_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_missionName;
	public TextMeshProUGUI m_missionDescription;
	public TextMeshProUGUI m_missionCost;
	public TextMeshProUGUI m_missionDuration;
	public TextMeshProUGUI m_missionSuccessChance;
	public Image m_missionPortrait;

	public TraitButton[] m_traits;

	private MissionBase m_mission = null;

	private Henchmen m_henchmen = null;

	private Region.TokenSlot m_token = null;

	public void Initialize ()
	{
		MissionWrapper mw = GameManager.instance.currentMissionWrapper;
//		Debug.Log (mw.m_mission);
		WriteBaseMissionStats (mw.m_mission);

		int turnsLeft = mw.m_mission.m_numTurns;
		string duration = turnsLeft.ToString ();
		if (turnsLeft > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		m_missionDuration.text = duration;

		CalculateTraits (mw);

		if (mw.m_mission.m_targetType == MissionBase.TargetType.Henchmen && mw.m_henchmenInFocus != null) {

			m_henchmen = mw.m_henchmenInFocus;
		} else if (mw.m_mission.m_targetType == MissionBase.TargetType.AssetToken && mw.m_tokenInFocus != null) {
			m_token = mw.m_tokenInFocus;
		}
	}

	private void WriteBaseMissionStats (MissionBase m)
	{
		m_mission = m;
		m_missionName.text = m.GetNameText().ToUpper();
		m_missionDescription.text = m.m_description;
		m_missionCost.text = m.m_cost.ToString ();
	}

	private void CalculateTraits (MissionWrapper mw)
	{
		// calculate mission rank

		int missionRank = 1;

		switch (mw.m_region.rank) {
		case RegionData.Rank.Two:
			missionRank += 1;
			break;
		case RegionData.Rank.One:
			missionRank += 2;
			break;
		}

		if (mw.m_mission.m_targetType == MissionBase.TargetType.AssetToken && mw.m_tokenInFocus.m_assetToken != null) {

			AssetToken a = (AssetToken)mw.m_tokenInFocus.m_assetToken;

			if (a.m_asset.m_rank == Asset.Rank.Three) {
				missionRank++;
			} else if (a.m_asset.m_rank == Asset.Rank.Four) {
				missionRank += 2;
			}
		}

		if (mw.m_tokenInFocus != null && mw.m_tokenInFocus.m_effects.Contains (Region.TokenSlot.Status.Protected)) {

			foreach (Region.TokenSlot.Status s in mw.m_tokenInFocus.m_effects) {
				if (s == Region.TokenSlot.Status.Protected) {
					missionRank += 1;
				}
			}

		}

		if (mw.m_tokenInFocus != null && mw.m_tokenInFocus.m_effects.Contains (Region.TokenSlot.Status.Vulnerable)) {

			foreach (Region.TokenSlot.Status s in mw.m_tokenInFocus.m_effects) {
				if (s == Region.TokenSlot.Status.Vulnerable) {
					missionRank = Mathf.Clamp (missionRank - 1, 1, 5);
				}
			}

		}


		int successChance = 0;

		// gather traits from all henchmen

		List<TraitData> combinedTraitList = new List<TraitData> ();

		foreach (Henchmen thisH in mw.m_henchmen) {
			List<TraitData> t = thisH.GetAllTraits();
			foreach (TraitData thisT in t) {
				if (!combinedTraitList.Contains (thisT)) {
					combinedTraitList.Add (thisT);
				}
			}
		}

		MissionBase.MissionTrait[] traits = mw.m_mission.GetTraitList (missionRank);

		for (int i = 0; i < m_traits.Length; i++) {
			
			TraitButton t = m_traits [i];

			if (i < traits.Length) {
				
				MissionBase.MissionTrait mT = traits [i];
				bool hasTrait = false;
				bool hasAsset = false;

				if (mT.m_trait != null) {
					hasTrait = combinedTraitList.Contains (mT.m_trait);
					t.Initialize (mT.m_trait, hasTrait);
				}

				if (!hasTrait && mT.m_asset != null) {
					hasAsset = GameManager.instance.game.player.currentAssets.Contains (mT.m_asset);
					t.Initialize (mT.m_asset, hasAsset);
				}

				if (hasAsset || hasTrait) {
					successChance = Mathf.Clamp (successChance + mT.m_percentageContribution, 0, 100);
				}

			} else {
				t.Deactivate ();
			}
		}

		m_missionSuccessChance.text = successChance.ToString() + "% SUCCESS";
	}

	public void StartMissionButtonPressed ()
	{
		if (m_mission != null && m_mission.m_cost <= GameManager.instance.game.player.currentCommandPool) {
//			CallHenchmenMenu.instance.Startmission (m_mission);
			if (GameManager.instance.currentMenuState == MenuState.State.SelectMissionMenu) {

				if (m_henchmen != null) {
					GameManager.instance.currentMissionWrapper.m_henchmenInFocus = m_henchmen;
				}
				if (m_token != null) {
					GameManager.instance.currentMissionWrapper.m_tokenInFocus = m_token;
				}

				SelectMissionMenu.instance.SelectMission (m_mission);
			}
		}
	}

}
