﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionBase : ScriptableObject, IMission {

	[System.Serializable]
	public struct MissionTrait
	{
		public TraitData m_trait;
		public Asset m_asset;
		public int m_percentageContribution;
	}

	public enum TargetType
	{
		None,
		Region,
		Henchmen,
		AssetToken,
		ControlToken,
		PolicyToken,
		EconomicControlToken,
		PoliticalControlToken,
		MilitaryControlToken,
		BaseUpgrade,
		Floor,
		Agent,
		OwnedAsset,
		RemoteRegion,
	}
	
	public string m_name = "Null";
	public string m_description = "Lorem Ipsum";
	public TargetType m_targetType = TargetType.Region;
	public int m_cost = 1;
	public int m_numTurns = 1;
	public int m_maxRank = 5;
	public int m_infamyGain = 0;
	public int m_missionFailInfamyGain = 0;
	public MissionTrait[] m_rank1Traits;
	public MissionTrait[] m_rank2Traits;
	public MissionTrait[] m_rank3Traits;
	public MissionTrait[] m_rank4Traits;
	public MissionTrait[] m_rank5Traits;

	public MissionTrait[] GetTraitList (int rank)
	{
		int r = Mathf.Clamp (rank, 1, m_maxRank);

		switch (r) {
		case 1:
			return m_rank1Traits;
		case 2:
			return m_rank2Traits;
		case 3:
			return m_rank3Traits;
		case 4:
			return m_rank4Traits;
		case 5:
			return m_rank5Traits;
		}

		return null;
	}

	public virtual void InitializeMission (MissionWrapper a)
	{

	}

	public virtual void CompleteMission (MissionWrapper a)
	{
		int completionChance = CalculateCompletionPercentage (a);

		a.m_success = WasMissionSuccessful (completionChance);

	}

	public virtual string GetNameText ()
	{
		return m_name;
	}

	public virtual bool IsValid ()
	{
		return false;
	}

	public bool WasMissionSuccessful (int successChance)
	{
		bool missionSuccess = false;

		if (Random.Range (0, 101) <= successChance) {
			missionSuccess = true;
		}

		return missionSuccess;
	}

	public int CalculateCompletionPercentage (MissionWrapper mw)
	{
		int missionRank = mw.m_region.rank;

		if (mw.m_mission.m_targetType == MissionBase.TargetType.RemoteRegion) {

			missionRank = mw.m_regionInFocus.rank + 2;
		}

		if (mw.m_mission.m_targetType == TargetType.AssetToken && mw.m_tokenInFocus.m_assetToken != null) {

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

		if (mw.m_mission.m_targetType == TargetType.Agent && mw.m_agentInFocus != null) {

			if (mw.m_agentInFocus.m_agent.rank == 2) {
				missionRank += 1;
			} else if (mw.m_agentInFocus.m_agent.rank == 3) {
				missionRank += 2;
			}
		}
			
		int completionPercentage = 0;

		List<TraitData> combinedTraitList = new List<TraitData> ();

		List<Henchmen> participatingHenchmen = new List<Henchmen> ();

		if (mw.m_organization != null && mw.m_organization == GameManager.instance.game.agentOrganization) {
			
			foreach (AgentWrapper aw in mw.m_agents) {

				participatingHenchmen.Add (aw.m_agent);
			}

			if (mw.m_agentInFocus != null && !mw.m_agents.Contains (mw.m_agentInFocus)) {

				participatingHenchmen.Add (mw.m_agentInFocus.m_agent);
			}

		} else {

			foreach (Henchmen thisH in mw.m_henchmen) {

				participatingHenchmen.Add (thisH);
			}

			if (mw.m_henchmenInFocus != null && !mw.m_henchmen.Contains(mw.m_henchmenInFocus)) {

				participatingHenchmen.Add (mw.m_henchmenInFocus);
			}
		}
			
		foreach (Henchmen thisH in participatingHenchmen) {

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

		foreach (MissionBase.MissionTrait mt in traits) {

			bool hasTrait = false;
			bool hasAsset = false;

			if (mt.m_trait != null) {
				hasTrait = combinedTraitList.Contains (mt.m_trait);
			}

			if (mt.m_asset != null) {
				hasAsset = GameManager.instance.game.player.currentAssets.Contains (mt.m_asset);
			}

			if (hasTrait || hasAsset) {
				completionPercentage += mt.m_percentageContribution;
			}
		}

		// check status of each henchmen for penalties

		foreach (Henchmen h in participatingHenchmen) {

			switch (h.statusTrait.m_type) {

			case TraitData.TraitType.Injured:
				completionPercentage = Mathf.Clamp (completionPercentage - GameManager.instance.game.director.m_injuredStatusPenalty, 0, 100);
				break;
			case TraitData.TraitType.Critical:
				completionPercentage = Mathf.Clamp (completionPercentage - GameManager.instance.game.director.m_criticalStatusPenalty, 0, 100);
				break;
			}
		}

		return completionPercentage;
	}
}
