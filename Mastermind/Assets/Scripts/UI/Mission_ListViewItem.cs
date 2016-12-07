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
	private Henchmen m_henchmen = null;
	private AgentWrapper m_agent = null;
	private TokenSlot m_token = null;
	private Asset m_asset = null;
	private Region m_region = null;

	public void Initialize ()
	{
		MissionWrapper mw = GameManager.instance.currentMissionWrapper;

		m_mission = mw.m_mission;
		m_missionName.text = mw.m_mission.GetNameText().ToUpper();
		m_missionDescription.text = mw.m_mission.m_description;
		m_missionCost.text = mw.m_mission.m_cost.ToString ();
		m_missionSuccessChance.text = m_mission.CalculateCompletionPercentage(mw).ToString() + "% SUCCESS";

		int turnsLeft = mw.m_mission.m_numTurns;
		string duration = turnsLeft.ToString ();
		if (turnsLeft > 1) {
			duration += " TURNS";
		} else {
			duration += " TURN";
		}

		m_missionDuration.text = duration;

		DrawTraits (mw);

		if (mw.m_mission.m_targetType == MissionBase.TargetType.Henchmen && mw.m_henchmenInFocus != null) {

			m_henchmen = mw.m_henchmenInFocus;
		} else if (mw.m_mission.m_targetType == MissionBase.TargetType.AssetToken && mw.m_tokenInFocus != null) {
			m_token = mw.m_tokenInFocus;
		} 
		else if (mw.m_mission.m_targetType == MissionBase.TargetType.Agent && mw.m_agentInFocus != null) {
			m_agent = mw.m_agentInFocus;
		}
		else if (mw.m_mission.m_targetType == MissionBase.TargetType.OwnedAsset && mw.m_assetInFocus != null) {
			m_asset = mw.m_assetInFocus;
		}
		else if (mw.m_mission.m_targetType == MissionBase.TargetType.RemoteRegion && mw.m_regionInFocus != null) {
			m_region = mw.m_regionInFocus;
		}
	}

	private void DrawTraits (MissionWrapper mw)
	{
		MissionBase.MissionTrait[] traits = mw.m_mission.GetTraitList (mw.m_mission.GetMissionRank(mw));
		List<TraitData> combinedTraitList = mw.m_mission.GetCombinedTraitList (mw);

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

			} else {
				t.Deactivate ();
			}
		}
	}
	public void StartMissionButtonPressed ()
	{
		if (m_mission != null && m_mission.m_cost <= GameManager.instance.game.player.currentCommandPool) {

			if (GameManager.instance.currentMenuState == MenuState.State.SelectMissionMenu) {

				if (m_henchmen != null) {
					GameManager.instance.currentMissionWrapper.m_henchmenInFocus = m_henchmen;
				}
				if (m_token != null) {
					GameManager.instance.currentMissionWrapper.m_tokenInFocus = m_token;
				} 
				if (m_agent != null) {
					GameManager.instance.currentMissionWrapper.m_agentInFocus = m_agent;
				}
				if (m_asset != null) {
					GameManager.instance.currentMissionWrapper.m_assetInFocus = m_asset;
				}
				if (m_region != null) {
					GameManager.instance.currentMissionWrapper.m_regionInFocus = m_region;
				}

				SelectMissionMenu.instance.SelectMission (m_mission);
			}
		}
	}

}
