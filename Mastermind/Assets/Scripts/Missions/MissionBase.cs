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
		Research,
	}
	
	public string m_name = "Null";
	public string m_description = "Lorem Ipsum";
	public TargetType m_targetType = TargetType.Region;
	public int m_cost = 1;
	public int m_numTurns = 1;
	public int m_maxRank = 5;
	public int m_infamyGain = 0;
	public int m_missionFailInfamyGain = 0;
	public Asset m_requiredResearch;
	public TraitData.TraitType m_linkedTrait;
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
		if (m_requiredResearch == null || GameManager.instance.game.player.currentResearch.Contains (m_requiredResearch)) {

			return true;
		}

		if (m_linkedTrait != TraitData.TraitType.None && GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (Henchmen h in r.currentHenchmen) {

				if (h.HasTrait (m_linkedTrait)) {

					return true;
				}
			}
		}
		return false;
	}

	public bool WasMissionSuccessful (int successChance)
	{
		if (GameManager.instance.m_forceMissionSuccess) {

			return true;
		}

		bool missionSuccess = false;

		if (Random.Range (0, 101) <= successChance) {
			missionSuccess = true;
		}

		return missionSuccess;
	}

	public virtual void CancelMission (MissionWrapper a)
	{
		// return command points if the mission was started this turn

		if (a.m_turnsPassed == 0) {

			GameManager.instance.game.player.GainCommandPoints (a.m_mission.m_cost);
		}
	}

	public int GetMissionRank (MissionWrapper mw)
	{
		int missionRank = mw.m_region.rank;

		if (mw.m_mission.m_targetType == MissionBase.TargetType.RemoteRegion) {

			missionRank = mw.m_regionInFocus.rank + 2;
		}

		if (mw.m_mission.m_targetType == TargetType.AssetToken && mw.m_tokenInFocus != null && mw.m_tokenInFocus.m_assetToken != null) {

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

		return missionRank;
	}

	public List<TraitData> GetCombinedTraitList (MissionWrapper mw)
	{
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
				combinedTraitList.Add (thisH.statusTrait);
			}
		}

		return combinedTraitList;
	}

	public int CalculateCompletionPercentage (MissionWrapper mw)
	{
		int missionRank = GetMissionRank (mw);
			
		int completionPercentage = 0;

		List<TraitData> combinedTraitList = GetCombinedTraitList (mw);

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

		foreach (TraitData t in combinedTraitList) {

			switch (t.m_type) {

			case TraitData.TraitType.Injured:
				completionPercentage = Mathf.Clamp (completionPercentage - GameManager.instance.game.director.m_injuredStatusPenalty, 0, 100);
				break;
			case TraitData.TraitType.Critical:
				completionPercentage = Mathf.Clamp (completionPercentage - GameManager.instance.game.director.m_criticalStatusPenalty, 0, 100);
				break;
			}
		}

		// check for dynamic traits
		int dynamicTraits = CalculateDynamicTraitBonus(combinedTraitList, mw);

		completionPercentage = Mathf.Clamp (completionPercentage + dynamicTraits, 0, 100);

		return completionPercentage;
	}

	public int CalculateDynamicTraitBonus (List<TraitData> traitList, MissionWrapper mw)
	{
		int bonus = 0;

		// check for allies & rivals

		foreach (TraitData td in traitList) {

			if (td.m_class == TraitData.TraitClass.Dynamic) {

				DynamicTrait dt = (DynamicTrait)td;

				if (dt.m_linkType == DynamicTrait.LinkType.Ally || dt.m_linkType == DynamicTrait.LinkType.Rival) {

					if (dt.m_henchmen != null && mw.m_henchmen.Contains (dt.m_henchmen)) {

						if (dt.m_linkType == DynamicTrait.LinkType.Ally) {

							bonus += 10;

						} else if (dt.m_linkType == DynamicTrait.LinkType.Rival) {

							bonus -= 10;
						}

					}
				} else if (dt.m_linkType == DynamicTrait.LinkType.Wanted || dt.m_linkType == DynamicTrait.LinkType.Citizen) {

					if (mw.m_region != null && dt.m_region != null && mw.m_region == dt.m_region) {

						if (dt.m_linkType == DynamicTrait.LinkType.Citizen) {

							bonus += 10;

						} else if (dt.m_linkType == DynamicTrait.LinkType.Wanted) {

							bonus -= 10;
						}
					}
				}
			}
		}

		return bonus;
	}

	public string CheckForNewTraits (MissionWrapper mw)
	{
		string s = "";

		float baseRegionTraitChance = 0.1f;
		float baseHenchmenTraitChance = 0.1f;
		float baseTeachTraitChance = 0.1f;

		if (Random.Range (0.0f, 1.0f) <= baseRegionTraitChance) {

			List<Henchmen> regionValid = new List<Henchmen> ();

			foreach (Henchmen h in mw.m_henchmen) {

				List<TraitData> tdata = h.GetAllTraits ();

				bool isRegionValid = true;

				foreach (TraitData td in tdata) {

					if (td.m_class == TraitData.TraitClass.Dynamic) {

						DynamicTrait dt = (DynamicTrait)td;

						if ((dt.m_linkType == DynamicTrait.LinkType.Wanted || dt.m_linkType == DynamicTrait.LinkType.Citizen) && dt.m_region == mw.m_region &&
							mw.m_region.id != GameManager.instance.game.player.homeRegion.id)
						{
							
							isRegionValid = false;
							break;
						} else if (tdata.Count >= 8) // traits are full
						{
							isRegionValid = false;
							break;
						}
					}
				}

				if (isRegionValid) {

					regionValid.Add (h);
				}
			}

			if (regionValid.Count > 0) {

				Henchmen affectedHM = regionValid[Random.Range(0, regionValid.Count)];

				float r = 0.5f;

				if (mw.m_success) {

					r += 0.2f;
				} else {
					r -= 0.2f;
				}

				if (Random.Range(0.0f, 1.0f) <= r) {

					DynamicTrait d = DynamicTrait.CreateInstance<DynamicTrait>();
					d.m_class = TraitData.TraitClass.Dynamic;
					d.m_linkType = DynamicTrait.LinkType.Citizen;
					d.m_region = mw.m_region;
					affectedHM.AddTrait (d);

					s += "\n" + affectedHM.henchmenName.ToUpper () + " is now a CITIZEN of " + d.m_region.regionName.ToUpper ();

				} else {

					DynamicTrait d = DynamicTrait.CreateInstance<DynamicTrait>();
					d.m_class = TraitData.TraitClass.Dynamic;
					d.m_linkType = DynamicTrait.LinkType.Wanted;
					d.m_region = mw.m_region;
					affectedHM.AddTrait (d);

					s += "\n" + affectedHM.henchmenName.ToUpper () + " is now WANTED in " + d.m_region.regionName.ToUpper ();
				}
			}
		}

		if (Random.Range (0.0f, 1.0f) <= baseHenchmenTraitChance && mw.m_henchmen.Count > 1) {

			List<Henchmen> pool = new List<Henchmen> ();

			foreach (Henchmen h in mw.m_henchmen) {

				pool.Add (h);
			}

			int r1 = Random.Range (0, pool.Count);
			Henchmen h1 = pool [r1];
			pool.RemoveAt (r1);
			int r2 = Random.Range (0, pool.Count);
			Henchmen h2 = pool [r2];

			// check if a link is already present

			bool linkPresent = false;

			List<TraitData> tdl = h1.GetAllTraits ();

			if (tdl.Count < 8) {
				foreach (TraitData td in tdl) {

					if (td.m_class == TraitData.TraitClass.Dynamic) {

						DynamicTrait dt = (DynamicTrait)td;

						if ((dt.m_linkType == DynamicTrait.LinkType.Ally || dt.m_linkType == DynamicTrait.LinkType.Rival) && dt.m_henchmen != null &&
						    dt.m_henchmen == h2) {
							linkPresent = true;
							break;
						}
					}
				}
			} else {

				linkPresent = true;
			}

			if (!linkPresent) {

				float r = 0.5f;

				if (mw.m_success) {

					r += 0.3f;
				} else {
					r -= 0.3f;
				}

				if (Random.Range(0.0f, 1.0f) <= r) {

					DynamicTrait d = DynamicTrait.CreateInstance<DynamicTrait>();
					d.m_class = TraitData.TraitClass.Dynamic;
					d.m_linkType = DynamicTrait.LinkType.Ally;
					d.m_henchmen = h2;
					h1.AddTrait (d);

					s += "\n" + h1.henchmenName.ToUpper () + " now considers " + h2.henchmenName.ToUpper () + " an ALLY.";

				} else {

					DynamicTrait d = DynamicTrait.CreateInstance<DynamicTrait>();
					d.m_class = TraitData.TraitClass.Dynamic;
					d.m_linkType = DynamicTrait.LinkType.Rival;
					d.m_henchmen = h2;
					h1.AddTrait (d);

					s += "\n" + h1.henchmenName.ToUpper () + " now considers " + h2.henchmenName.ToUpper () + " a RIVAL.";
				}
			}
		}

		if (Random.Range (0.0f, 1.0f) <= baseTeachTraitChance && mw.m_henchmen.Count > 1) {

			List<Henchmen> pool = new List<Henchmen> ();

			foreach (Henchmen h in mw.m_henchmen) {

				pool.Add (h);
			}

			int r1 = Random.Range (0, pool.Count);
			Henchmen h1 = pool [r1];
			pool.RemoveAt (r1);
			int r2 = Random.Range (0, pool.Count);
			Henchmen h2 = pool [r2];

			List<TraitData> tdl = h2.GetAllTraits ();

			if (tdl.Count < 8) {

				List<TraitData> validTraits = new List<TraitData> ();

				foreach (TraitData td in tdl) {

					if (td.m_class != TraitData.TraitClass.Dynamic && td.m_class != TraitData.TraitClass.Status && td.m_class != TraitData.TraitClass.Flaw && !h1.HasTrait (td)) {

						validTraits.Add (td);

						if (td.m_class != TraitData.TraitClass.Skill) {

							validTraits.Add (td);
						}
					}
				}

				if (validTraits.Count > 0) {

					TraitData thisTD = validTraits[Random.Range(0, validTraits.Count)];
					h1.AddTrait (thisTD);

					s += "\n" + h2.henchmenName.ToUpper () + " teaches " + h1.henchmenName.ToUpper () + " the Trait " + thisTD.m_name.ToUpper() + ".";
				}
			}

		}

		return s;
	}
}
