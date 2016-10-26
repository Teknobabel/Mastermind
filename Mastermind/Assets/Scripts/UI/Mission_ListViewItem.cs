﻿using UnityEngine;
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

	public void Initialize (MissionBase m, MissionRequest mr)
	{
		WriteBaseMissionStats (m);

		int turnsLeft = m.m_numTurns;
		string duration = turnsLeft.ToString ();
		if (turnsLeft > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		CalculateTraits (m, mr.m_region, mr.m_henchmen);
	}

	public void Initialize (Organization.ActiveMission a)
	{
		WriteBaseMissionStats (a.m_mission);

		int turnsLeft = a.m_mission.m_numTurns - a.m_turnsPassed;
		string duration = turnsLeft.ToString ();
		if (turnsLeft > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		m_missionDuration.text = duration;

		CalculateTraits (a.m_mission, a.m_region, a.m_henchmen);
	}

	private void WriteBaseMissionStats (MissionBase m)
	{
		m_mission = m;
		m_missionName.text = m.m_name.ToUpper();
		m_missionDescription.text = m.m_description;
		m_missionCost.text = m.m_cost.ToString ();
	}

	private void CalculateTraits (MissionBase m, Region r, List<Henchmen> hList)
	{
		// calculate mission rank

		int missionRank = 1;

		switch (r.rank) {
		case RegionData.Rank.Two:
			missionRank += 1;
			break;
		case RegionData.Rank.One:
			missionRank += 2;
			break;
		}

		int successChance = 0;

		// gather traits from all henchmen

		List<TraitData> combinedTraitList = new List<TraitData> ();

		foreach (Henchmen thisH in hList) {
			List<TraitData> t = thisH.GetAllTraits();
			foreach (TraitData thisT in t) {
				if (!combinedTraitList.Contains (thisT)) {
					combinedTraitList.Add (thisT);
				}
			}
		}

		MissionBase.MissionTrait[] traits = m.GetTraitList (missionRank);

		for (int i = 0; i < m_traits.Length; i++) {
			TraitButton t = m_traits [i];
			if (i < traits.Length) {
				MissionBase.MissionTrait mT = traits [i];
				if (mT.m_trait != null)
				{
					bool hasTrait = combinedTraitList.Contains (mT.m_trait);
					t.Initialize (mT.m_trait, hasTrait);

					if (hasTrait) {
						successChance = Mathf.Clamp(successChance + mT.m_percentageContribution, 0, 100);
					}
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
				SelectMissionMenu.instance.SelectMission (m_mission);
			}
		}
	}

}
