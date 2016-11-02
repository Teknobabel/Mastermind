using UnityEngine;
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
	
	public string m_name = "Null";
	public string m_description = "Lorem Ipsum";
	public int m_cost = 1;
	public int m_numTurns = 1;
	public int m_maxRank = 5;
	public int m_infamyGain = 0;
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

	public virtual void InitializeMission (Organization.ActiveMission a)
	{

	}

	public virtual void CompleteMission (Organization.ActiveMission a)
	{
		if (m_infamyGain > 0) {
			GameManager.instance.game.player.GainInfamy (m_infamyGain);
		}
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

	public int CalculateCompletionPercentage (MissionBase m, Region r, List<Henchmen> h)
	{
		int missionRank = 1;

		switch (r.rank) {
		case RegionData.Rank.Two:
			missionRank += 1;
			break;
		case RegionData.Rank.One:
			missionRank += 2;
			break;
		}
			
		int completionPercentage = 0;

		List<TraitData> combinedTraitList = new List<TraitData> ();

		foreach (Henchmen thisH in h) {
			List<TraitData> t = thisH.GetAllTraits();
			foreach (TraitData thisT in t) {
				if (!combinedTraitList.Contains (thisT)) {
					combinedTraitList.Add (thisT);
				}
			}
		}

		MissionBase.MissionTrait[] traits = m.GetTraitList (missionRank);

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
			
		return completionPercentage;
	}
}
