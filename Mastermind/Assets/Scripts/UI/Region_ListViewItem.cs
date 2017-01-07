﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Region_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_regionName;
	public TextMeshProUGUI m_regionRank;
	public RawImage m_regionPortrait;

	private List<TokenButton> m_policies = new List<TokenButton> ();
	private List<TokenButton> m_assets = new List<TokenButton> ();
	private List<TokenButton> m_controlPoints = new List<TokenButton> ();

	public RegionMissionButton m_missionButton;

	public RegionHenchmenButton[] m_henchmenTokens;

	public Transform m_policyTokenPanel;
	public Transform m_assetTokenPanel;
	public Transform m_controlTokenPanel;
	public Transform m_henchmenSlotPanel;

	public GameObject m_tokenButton;
	public GameObject m_henchmenSlot;

	public int m_regionID = -1;

	// Use this for initialization
	void Start () {
	
	}

	public void Initialize (Region r)
	{
		m_regionID = r.id;

		// start text crawl for region name
		TextCrawl tc = (TextCrawl) m_regionName.transform.GetComponent<TextCrawl>();
		if (tc != null) {

			tc.Initialize(r.regionName.ToUpper());
		}

		if (r.portrait != null) {
			m_regionPortrait.texture = r.portrait.texture;
		}


		List<TokenSlot> p = r.policyTokens;
		List<TokenSlot> a = r.assetTokens;
		List<TokenSlot> c = r.controlTokens;

		foreach (TokenSlot ts in p) {

			GameObject thisP = (GameObject)(Instantiate (m_tokenButton, m_policyTokenPanel));
			thisP.transform.localScale = Vector3.one;
			TokenButton tb = (TokenButton)thisP.GetComponent<TokenButton> ();
			tb.Initialize (ts);

			// set up onclick event

			Button b = tb.GetComponent<Button> ();
			b.onClick.AddListener (() => {
				TokenButtonClicked (tb);
			});
		}

		foreach (TokenSlot ts in a) {

			GameObject thisA = (GameObject)(Instantiate (m_tokenButton, m_assetTokenPanel));
			thisA.transform.localScale = Vector3.one;
			TokenButton tb = (TokenButton)thisA.GetComponent<TokenButton> ();
			tb.Initialize (ts);

			// set up onclick event

			Button b = tb.GetComponent<Button> ();
			b.onClick.AddListener (() => {
				TokenButtonClicked (tb);
			});
		}

		foreach (TokenSlot ts in c) {

			GameObject thisC = (GameObject)(Instantiate (m_tokenButton, m_controlTokenPanel));
			thisC.transform.localScale = Vector3.one;
			TokenButton tb = (TokenButton)thisC.GetComponent<TokenButton> ();
			tb.Initialize (ts);

			// set up onclick event

			Button b = tb.GetComponent<Button> ();
			b.onClick.AddListener (() => {
				TokenButtonClicked (tb);
			});
		}

		for (int i = 0; i < r.henchmenSlots.Count; i++) {

			GameObject h = (GameObject)(Instantiate (m_henchmenSlot, m_henchmenSlotPanel));
			h.transform.localScale = Vector3.one;
			RegionHenchmenButton rhb = (RegionHenchmenButton)h.GetComponent<RegionHenchmenButton> ();

			Region.HenchmenSlot hSlot = r.henchmenSlots[i];
			rhb.Initialize (hSlot);
			rhb.m_parent = this;
		}

		m_regionRank.text = "R" + r.rank.ToString();

		m_missionButton.Initialize (GameManager.instance.game.player.GetMission (r));
	}

	public void EmptyHenchmenButtonClicked (Region.HenchmenSlot clickedSlot)
	{
		if (GameManager.instance.currentMenuState == MenuState.State.WorldMenu) {
			WorldMenu.instance.SelectHenchmenForTravel (m_regionID, clickedSlot);
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

//		if (tb.tokenSlot.m_state == TokenSlot.State.Revealed) {
			WorldMenu.instance.SelectMissionForToken (tb.tokenSlot);
//		}
	}

}
