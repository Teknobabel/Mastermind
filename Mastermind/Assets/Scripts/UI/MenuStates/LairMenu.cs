﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LairMenu : MenuState {
	public static LairMenu instance;

	public GameObject m_lairMenu;

	public TextMeshProUGUI m_lairText;

	public Transform 
	m_assetsPanel,
	m_basePanel,
	m_researchPanel_Influence,
	m_researchPanel_Force,
	m_researchPanel_Tech,
	m_researchPanel_Lair;

	public GameObject 
		m_tokenButton,
		m_researchButton,
		m_baseFloor;

	private List<GameObject> m_assetGO = new List<GameObject>();
	private List<GameObject> m_baseFloorGO = new List<GameObject>();
	private List<GameObject> m_researchGO = new List<GameObject>();

	void Awake ()
	{
		if (!instance) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}

	public override void OnActivate(MenuTab tabInfo)
	{
		Debug.Log ("Starting Lair Menu");

		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_lairMenu.gameObject.SetActive (true);
//		m_sortPanelParent.gameObject.SetActive (false);

		UpdateText ();
		UpdateBase ();
		UpdateAssets ();
		UpdateResearch ();
	}

	public override void OnHold()
	{

		m_lairMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		// continue going back up the stack if this is not the target menu

		if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState != m_state)
		{
			GameManager.instance.PopMenuState();
			return;
		} else if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState == m_state)
		{
			GameManager.instance.targetMenuState = MenuState.State.None;
		}

		m_lairMenu.gameObject.SetActive (true);
//		m_sortPanelParent.gameObject.SetActive (false);

		UpdateText ();
		UpdateBase ();
		UpdateAssets ();
		UpdateResearch ();
	}

	public override void OnDeactivate()
	{
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

		m_lairMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	private void UpdateResearch ()
	{
		while (m_researchGO.Count > 0) {

			GameObject go = m_researchGO [0];
			m_researchGO.RemoveAt (0);
			Destroy (go);
		}

		TechTree techTree = GameManager.instance.m_techTree;

		foreach (TechTree.ResearchBranch b in techTree.m_branches) {

			foreach (ResearchObject r in b.m_researchObjects)
			{
				Transform t = null;

				switch (b.m_branchType) {

				case TechTree.BranchType.Influence:
					t = m_researchPanel_Influence;
					break;
				case TechTree.BranchType.Force:
					t = m_researchPanel_Force;
					break;
				case TechTree.BranchType.Tech:
					t = m_researchPanel_Tech;
					break;
				case TechTree.BranchType.Lair:
					t = m_researchPanel_Lair;
					break;

				}

				if (t != null) {

					GameObject thisR = (GameObject)(Instantiate (m_researchButton, t));
					thisR.transform.localScale = Vector3.one;
					m_researchGO.Add (thisR);
					ResearchButton rb = (ResearchButton)thisR.GetComponent<ResearchButton> ();

					rb.Initialize (r, b);
				}
			}

		}

//		foreach (ResearchButton rb in m_researchButtons) {
//
//			rb.Initialize ();
//		}
	}

	private void UpdateBase ()
	{

		while (m_baseFloorGO.Count > 0) {

			GameObject go = m_baseFloorGO [0];
			m_baseFloorGO.RemoveAt (0);
			Destroy (go);
		}

		Base b = GameManager.instance.game.player.orgBase;

		for (int i = 0; i < b.m_floors.Count; i++) {

			GameObject thisF = (GameObject)(Instantiate (m_baseFloor, m_basePanel));
			thisF.transform.localScale = Vector3.one;
			m_baseFloorGO.Add (thisF);
			BaseFloorButton bfb = (BaseFloorButton)thisF.GetComponent<BaseFloorButton> ();

			bfb.Initialize (b.m_floors [i]);
		}

		if (b.m_lair != null) {

			GameObject thisF = (GameObject)(Instantiate (m_baseFloor, m_basePanel));
			thisF.transform.localScale = Vector3.one;
			m_baseFloorGO.Add (thisF);
			BaseFloorButton bfb = (BaseFloorButton)thisF.GetComponent<BaseFloorButton> ();
			bfb.Initialize (b.m_lair);
		}
	} 

	private void UpdateAssets ()
	{
		while (m_assetGO.Count > 0) {

			GameObject go = m_assetGO [0];
			m_assetGO.RemoveAt (0);
			Destroy (go);
		}

		Organization player = GameManager.instance.game.player;

		for (int i = 0; i < player.maxAssets; i++) {

			GameObject thisA = (GameObject)(Instantiate (m_tokenButton, m_assetsPanel));
			thisA.transform.localScale = Vector3.one;
			m_assetGO.Add (thisA);
			TokenButton tb = (TokenButton)thisA.GetComponent<TokenButton> ();
			tb.m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_Normal);
			tb.m_tokenTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Asset);


			if (i < player.currentAssets.Count) {

				Asset a = player.currentAssets [i];

				tb.m_tokenText.text = a.m_name.ToUpper ();
			} else {

				tb.m_tokenText.gameObject.SetActive (false);
			}
		}
	}

	private void UpdateText ()
	{
		Organization player = GameManager.instance.game.player;

		string s = player.orgName;
//		s += "\n UPGRADES:";
//		s += "\n ASSETS:";
//
//		foreach (Asset a in player.currentAssets) {
//			s += "\n  - " + a.m_name.ToUpper();
//		}

		m_lairText.text = s;
	}

	public void SelectUpgradeForFloor (Base.Floor targetFloor)
	{
		MissionWrapper mr = new MissionWrapper ();
		mr.m_scope = MissionBase.TargetType.BaseUpgrade;
		mr.m_floorInFocus = targetFloor;
		mr.m_region = GameManager.instance.game.player.homeRegion;

		foreach (Henchmen h in mr.m_region.currentHenchmen) {
			mr.m_henchmen.Add (h);
		}

		GameManager.instance.currentMissionWrapper = mr;
		GameManager.instance.PushMenuState (State.SelectMissionMenu);
	}

	public void SelectMissionForFloor (Base.Floor targetFloor)
	{
		MissionWrapper mr = new MissionWrapper ();
		mr.m_scope = MissionBase.TargetType.Floor;
		mr.m_floorInFocus = targetFloor;
		mr.m_region = GameManager.instance.game.player.homeRegion;

		foreach (Henchmen h in mr.m_region.currentHenchmen) {
			mr.m_henchmen.Add (h);
		}

		GameManager.instance.currentMissionWrapper = mr;
		GameManager.instance.PushMenuState (State.SelectMissionMenu);
	}

	public void SelectMissionForResearchButton (ResearchButton r)
	{
		if (r.researchState == ResearchButton.ResearchState.Available) {

			MissionWrapper mr = new MissionWrapper ();
			mr.m_scope = MissionBase.TargetType.Research;
			mr.m_researchButtonInFocus = r;
			mr.m_region = GameManager.instance.game.player.homeRegion;

			foreach (Henchmen h in mr.m_region.currentHenchmen) {
				mr.m_henchmen.Add (h);
			}

			GameManager.instance.currentMissionWrapper = mr;
			GameManager.instance.PushMenuState (State.SelectResearchMenu);

		}
	}
}
