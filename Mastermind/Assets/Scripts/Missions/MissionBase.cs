using UnityEngine;
using System.Collections;

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
			break;
		case 2:
			return m_rank2Traits;
			break;
		case 3:
			return m_rank3Traits;
			break;
		case 4:
			return m_rank4Traits;
			break;
		case 5:
			return m_rank5Traits;
			break;
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
}
