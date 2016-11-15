﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Region_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_regionName;
	public TextMeshProUGUI m_regionRank;
	public Image m_regionPortrait;

	public TokenButton[] m_policyTokens;
	public TokenButton[] m_assetTokens;
	public TokenButton[] m_controlTokens;

	public RegionMissionButton m_missionButton;

	public RegionHenchmenButton[] m_henchmenTokens;

	public int m_regionID = -1;

	// Use this for initialization
	void Start () {
	
	}

	public void Initialize (Region r)
	{
		m_regionName.text = r.regionName;
		m_regionID = r.id;
		m_regionPortrait.sprite = r.portrait;

		List<Region.TokenSlot> p = r.policyTokens;
		List<Region.TokenSlot> a = r.assetTokens;
		List<Region.TokenSlot> c = r.controlTokens;

		for (int i = 0; i < m_policyTokens.Length; i++) {
			TokenButton tB = m_policyTokens [i];
			if (i < p.Count) {
				tB.Initialize(p[i]);
			} else {
				tB.Deactivate ();
			}
		}

		for (int i = 0; i < m_assetTokens.Length; i++) {
			TokenButton tB = m_assetTokens [i];
			if (i < a.Count) {
				tB.Initialize(a[i]);
			} else {
				tB.Deactivate ();
			}
		}

		for (int i = 0; i < m_controlTokens.Length; i++) {
			TokenButton tB = m_controlTokens [i];
			if (i < c.Count) {
				tB.Initialize(c[i]);
			} else {
				tB.Deactivate ();
			}
		}

		for (int i = 0; i < m_henchmenTokens.Length; i++) {
			RegionHenchmenButton tB = m_henchmenTokens [i];
			if (i < r.henchmenSlots.Count) {
//				Henchmen h = r.currentHenchmen [i];
//				tB.Initialize (h);
				Region.HenchmenSlot hSlot = r.henchmenSlots[i];
				tB.Initialize (hSlot);
			} else if (i >= r.henchmenSlots.Count) {
				tB.Deactivate ();
			}
		}

		string s = "Null";
		switch (r.rank) {
		case RegionData.Rank.One:
			s = "1";
			break;
		case RegionData.Rank.Two:
			s = "2";
			break;
		case RegionData.Rank.Three:
			s = "3";
			break;
		}

		m_regionRank.text = "R" + s;

		m_missionButton.Initialize (GameManager.instance.game.player.GetMission (r));
	}

	public void EmptyHenchmenButtonClicked ()
	{
		if (GameManager.instance.currentMenuState == MenuState.State.WorldMenu) {
			WorldMenu.instance.SelectHenchmenForTravel (m_regionID);
		}
	}

	public void MissionButtonClicked ()
	{
		if (m_regionID != -1) {
			WorldMenu.instance.SelectMissionForRegion (m_regionID);
		}
	}

	public void TokenButtonClicked (TokenButton tb)
	{
		Debug.Log ("Token Button Clicked");

//		if (tb.tokenSlot.m_state == Region.TokenSlot.State.Revealed) {
			WorldMenu.instance.SelectMissionForToken (tb.tokenSlot);
//		}
	}

}
