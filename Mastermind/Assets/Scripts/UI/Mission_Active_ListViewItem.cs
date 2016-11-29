using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Mission_Active_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_missionName;
	public TextMeshProUGUI m_missionDescription;
	public TextMeshProUGUI m_missionDuration;
	public TextMeshProUGUI m_missionSuccessChance;
	public Image m_missionPortrait;

	public TraitButton[] m_traits;
	public RegionHenchmenButton[] m_henchmenTokens;

	public void Initialize (MissionWrapper a)
	{
		m_missionName.text = a.m_mission.m_name.ToUpper();
		m_missionDescription.text = a.m_mission.m_description;

		int turnsLeft = a.m_mission.m_numTurns - a.m_turnsPassed;
		string duration = turnsLeft.ToString ();
		if (turnsLeft > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		m_missionDuration.text = duration;

		CalculateTraits (a);

		Region r = a.m_region;

		for (int i = 0; i < m_henchmenTokens.Length; i++) {
			RegionHenchmenButton tB = m_henchmenTokens [i];
			if (i < r.henchmenSlots.Count) {
				Region.HenchmenSlot hSlot = r.henchmenSlots[i];

				if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Player) {
					tB.Initialize (hSlot);
				} else {
					tB.Deactivate ();
				}
			} else if (i >= r.henchmenSlots.Count) {
				tB.Deactivate ();
			}
		}
	}

	private void CalculateTraits (MissionWrapper mw)
	{
		// calculate mission rank

		int missionRank = mw.m_region.rank;

		if (mw.m_mission.m_targetType == MissionBase.TargetType.AssetToken && mw.m_tokenInFocus.m_assetToken != null) {

			AssetToken a = (AssetToken)mw.m_tokenInFocus.m_assetToken;

			if (a.m_asset.m_rank == Asset.Rank.Three) {
				missionRank++;
			} else if (a.m_asset.m_rank == Asset.Rank.Four) {
				missionRank += 2;
			}

		}

		if (mw.m_tokenInFocus != null && mw.m_tokenInFocus.m_effects.Contains (TokenSlot.Status.Protected)) {

			foreach (TokenSlot.Status s in mw.m_tokenInFocus.m_effects) {
				if (s == TokenSlot.Status.Protected) {
					missionRank += 1;
				}
			}

		}

		if (mw.m_tokenInFocus != null && mw.m_tokenInFocus.m_effects.Contains (TokenSlot.Status.Vulnerable)) {

			foreach (TokenSlot.Status s in mw.m_tokenInFocus.m_effects) {
				if (s == TokenSlot.Status.Vulnerable) {
					missionRank = Mathf.Clamp (missionRank - 1, 1, 5);
				}
			}

		}

		if (mw.m_mission.m_targetType == MissionBase.TargetType.Agent && mw.m_agentInFocus != null) {

			if (mw.m_agentInFocus.m_agent.rank == 2) {
				missionRank += 1;
			} else if (mw.m_agentInFocus.m_agent.rank == 3) {
				missionRank += 2;
			}
		}

		int successChance = 0;

		// gather traits from all henchmen

		List<TraitData> combinedTraitList = new List<TraitData> ();

		foreach (Henchmen thisH in mw.m_henchmen) {

			if (thisH.statusTrait.m_type != TraitData.TraitType.Incapacitated) {
				
				List<TraitData> t = thisH.GetAllTraits ();
				foreach (TraitData thisT in t) {
					if (!combinedTraitList.Contains (thisT)) {
						combinedTraitList.Add (thisT);
					}
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

				if (mT.m_asset != null) {
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

		// check status of each henchmen for penalties

		foreach (Henchmen h in mw.m_henchmen) {

			switch (h.statusTrait.m_type) {

			case TraitData.TraitType.Injured:
				successChance = Mathf.Clamp (successChance - GameManager.instance.game.director.m_injuredStatusPenalty, 0, 100);
				break;
			case TraitData.TraitType.Critical:
				successChance = Mathf.Clamp (successChance - GameManager.instance.game.director.m_criticalStatusPenalty, 0, 100);
				break;
			}
		}

		m_missionSuccessChance.text = successChance.ToString() + "% SUCCESS";
	}
}
