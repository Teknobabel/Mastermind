﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldMenu : MenuState, IObserver {
	public static WorldMenu instance;

	public enum SortType
	{
		None,
		RegionGroup,
		Occupied,
		RegionRank,
		Name,
	}

	public enum SortDirection
	{
		Normal,
		Reverse,
	}

	public GameObject m_regionListViewItem;
	public GameObject m_sectionHeader;
	public GameObject m_worldMenu;
	public GameObject m_scrollView;
	public GameObject m_scrollViewContent;
	public GameObject m_sortPanelParent;

	public SortModeButton[] m_sortModeButtons;
	public SortDirectionButton m_sortDirectionButton;

	private List<GameObject> m_listViewItems = new List<GameObject> ();

	private SortType m_sortType = SortType.RegionGroup;

	private SortDirection m_sortDirection = SortDirection.Normal;

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
		Debug.Log ("Starting World Menu");

		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_worldMenu.SetActive (true);

		ChangeSortType (m_sortType);

		UpdateRegionList ();
	}

	public void UpdateRegionList ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		Dictionary<string, List<Region>> sortedRegionList = GetRegionList (m_sortType);

		if (m_sortDirection == SortDirection.Normal) {
			
			foreach (KeyValuePair<string, List<Region>> pair in sortedRegionList) {

				GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
				h.transform.localScale = Vector3.one;
				SectionHeader sh = (SectionHeader)h.GetComponent<SectionHeader> ();

//				sh.AddObserver (this);

				m_listViewItems.Add (h);

//				if (sh.minimizeState == SectionHeader.MinimizeState.Normal) {
					
					for (int i = 0; i < pair.Value.Count; i++) {
						Region thisRegion = pair.Value [i];
						GameObject g = (GameObject)(Instantiate (m_regionListViewItem, m_scrollViewContent.transform));
						g.transform.localScale = Vector3.one;
						m_listViewItems.Add (g);
						g.GetComponent<Region_ListViewItem> ().Initialize (thisRegion);
					sh.m_children.Add (g);
					}
//				}

				sh.Initialize (pair.Key.ToUpper ());
			}
		} else {

//			if (sortedRegionList.Count == 1) {
//
//
//
//			} else if (sortedRegionList.Count > 1) {

				foreach (KeyValuePair<string, List<Region>> pair in sortedRegionList.Reverse()) {

					GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
					h.transform.localScale = Vector3.one;
				SectionHeader sh = (SectionHeader)h.GetComponent<SectionHeader> ();
					
//				sh.AddObserver (this);
					m_listViewItems.Add (h);

				if (sortedRegionList.Count == 1) {

//					if (sh.minimizeState == SectionHeader.MinimizeState.Normal) {
						
						List<Region> regions = new List<Region> (pair.Value);

						for (int i = regions.Count - 1; i >= 0; i--) {
							Region thisRegion = pair.Value [i];
							GameObject g = (GameObject)(Instantiate (m_regionListViewItem, m_scrollViewContent.transform));
							g.transform.localScale = Vector3.one;
							m_listViewItems.Add (g);
							g.GetComponent<Region_ListViewItem> ().Initialize (thisRegion);
						sh.m_children.Add (g);
						}
//					}

				} else {

//					if (sh.minimizeState == SectionHeader.MinimizeState.Normal) {
						
						for (int i = 0; i < pair.Value.Count; i++) {
							Region thisRegion = pair.Value [i];
							GameObject g = (GameObject)(Instantiate (m_regionListViewItem, m_scrollViewContent.transform));
							g.transform.localScale = Vector3.one;
							m_listViewItems.Add (g);
							g.GetComponent<Region_ListViewItem> ().Initialize (thisRegion);
						sh.m_children.Add (g);
						}
//					}
				}

				sh.Initialize (pair.Key.ToUpper ());

				}
//			}
		}
	}

	private Dictionary<string, List<Region>> GetRegionList (SortType sortType)
	{
		Dictionary<string, List<Region>> regionList = new Dictionary<string, List<Region>> ();

		switch (sortType) {
		case SortType.RegionGroup:

			Dictionary<RegionData.RegionGroup, List<Region>> regionsByGroup = GameManager.instance.game.GetAllRegionsByGroup ();
	
			foreach (KeyValuePair<RegionData.RegionGroup, List<Region>> pair in regionsByGroup) {
	
				List<Region> l = new List<Region> ();
	
				for (int i = 0; i < pair.Value.Count; i++) {
					Region thisRegion = pair.Value [i];
					l.Add (thisRegion);
				}

				regionList.Add (pair.Key.ToString (), l);
			}

			break;
		case SortType.Occupied:

			List<Region> occupiedList = new List<Region> ();
			List<Region> unoccupiedList = new List<Region> ();

			foreach (Region r in GameManager.instance.game.regions) {

				bool occupied = false;

				foreach (Region.HenchmenSlot hs in r.henchmenSlots)
				{
					if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player || (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent && hs.m_agent.m_vizState != AgentWrapper.VisibilityState.Hidden)) {

						occupied = true;
						break;
					}
				}
					
				if (!occupied) {
					unoccupiedList.Add (r);
				} else {
					occupiedList.Add (r);
				}

			}

			regionList.Add ("OCCUPIED REGIONS", occupiedList);
			regionList.Add ("UNOCCUPIED REGIONS", unoccupiedList);

			break;
		case SortType.RegionRank:

			List<Region> rank1 = new List<Region> ();
			List<Region> rank2 = new List<Region> ();
			List<Region> rank3 = new List<Region> ();

			foreach (Region r in GameManager.instance.game.regions) {

				switch (r.rank) {
				case 1:
					rank1.Add (r);
					break;
				case 2:
					rank2.Add (r);
					break;
				case 3:
					rank3.Add (r);
					break;
				}

			}
				
			regionList.Add ("RANK THREE", rank3);
			regionList.Add ("RANK TWO", rank2);
			regionList.Add ("RANK ONE", rank1);

			break;
		case SortType.Name:

			List<Region> alphaList = new List<Region> ();

			foreach (Region r in GameManager.instance.game.regions) {

				alphaList.Add (r);
			}

			alphaList.Sort (
				delegate(Region i1, Region i2) { 
					return i1.regionName.CompareTo (i2.regionName); 
				}
			);

			regionList.Add ("Alpha", alphaList);
			break;
		}


		return regionList;
	}

	public override void OnHold()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_worldMenu.SetActive (false);
	}

	public override void OnReturn()
	{
		Debug.Log ("Returning to World Menu");
		base.OnReturn ();

		m_worldMenu.SetActive (true);

		ChangeSortType (m_sortType);

		UpdateRegionList ();
	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

		m_worldMenu.SetActive (false);
	}

	public override void OnUpdate()
	{
		if (Input.GetKeyUp (KeyCode.Alpha1)) {

			SortDirectionButtonclicked ();

		}

	}

	public void SelectMissionForRegion (int regionID)
	{
		Debug.Log("Select Mission For Region");

		if (GameManager.instance.game.regionsByID.ContainsKey (regionID)) {
			Region r = GameManager.instance.game.regionsByID [regionID];

			if (r.currentHenchmen.Count > 0 && GameManager.instance.game.player.GetMission(r) == null) {

				MissionWrapper mr = new MissionWrapper ();
				mr.m_region = r;

				foreach (Region.HenchmenSlot hs in r.henchmenSlots) {

					if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

						mr.m_henchmen.Add (hs.m_henchmen);

					} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

						mr.m_agents.Add (hs.m_agent);
					}
				}

				GameManager.instance.currentMissionWrapper = mr;
				GameManager.instance.PushMenuState (State.SelectMissionMenu);
			}
		}
	}

	public void SelectMissionForToken (TokenSlot ts)
	{
		Debug.Log("Select Mission For Token");

		MissionWrapper mr = new MissionWrapper ();
		mr.m_region = ts.m_region;
		foreach (Henchmen h in mr.m_region.currentHenchmen) {
			mr.m_henchmen.Add (h);
		}

		switch (ts.m_type) {
		case TokenSlot.TokenType.Asset:
			mr.m_scope = MissionBase.TargetType.AssetToken;
			break;
		case TokenSlot.TokenType.Policy:
			mr.m_scope = MissionBase.TargetType.PolicyToken;
			break;
		case TokenSlot.TokenType.Control:

			switch (ts.m_controlToken.m_controlType) {
			case ControlToken.ControlType.Economic:
				mr.m_scope = MissionBase.TargetType.EconomicControlToken;
				break;
			case ControlToken.ControlType.Military:
				mr.m_scope = MissionBase.TargetType.MilitaryControlToken;
				break;
			case ControlToken.ControlType.Political:
				mr.m_scope = MissionBase.TargetType.PoliticalControlToken;
				break;
			}

			break;
		}
		mr.m_tokenInFocus = ts;
		GameManager.instance.currentMissionWrapper = mr;
		GameManager.instance.PushMenuState (State.SelectMissionMenu);

	}

	public void SortDirectionButtonclicked ()
	{
		if (m_sortDirection == SortDirection.Normal) {

			m_sortDirection = SortDirection.Reverse;

		} else {

			m_sortDirection = SortDirection.Normal;
		}

		m_sortDirectionButton.UpdateState (m_sortDirection);

		UpdateRegionList ();
	}

	public void SortButtonClicked (int sortType)
	{
		if (m_sortDirection == SortDirection.Reverse) {

			m_sortDirection = SortDirection.Normal;
			m_sortDirectionButton.UpdateState (m_sortDirection);
		}

		switch (sortType) {
		case 0:
			ChangeSortType( SortType.RegionGroup);
			break;
		case 1:
			ChangeSortType( m_sortType = SortType.Occupied);
			break;
		case 2:
			ChangeSortType( m_sortType = SortType.RegionRank);
			break;
		case 3:
			ChangeSortType( m_sortType = SortType.Name);
			break;
		}


	}

	private void ChangeSortType (SortType sortType)
	{

		m_sortType = sortType;

		UpdateRegionList ();

		foreach (SortModeButton b in m_sortModeButtons) {

			if (b.m_sortType == m_sortType) {
				b.ChangeState (SortModeButton.State.Selected);
			} else if (b.m_sortType != m_sortType) {
				b.ChangeState (SortModeButton.State.Unselected);
			}
		}
	}

	public void SelectHenchmenForTravel (int regionID, Region.HenchmenSlot clickedSlot)
	{
		// gather list of henchmen that can travel
		List<Henchmen> validHenchmen = new List<Henchmen>();

		Organization player = GameManager.instance.game.player;

		if (player.currentCommandPool >= GameManager.instance.m_travelMission.m_cost) {
			
			foreach (Henchmen h in player.currentHenchmen) {

				MissionWrapper a = player.GetMission (h);

				if (a == null && h.currentRegion.id != regionID && h.currentRegion.id != GameManager.instance.game.limbo.id) {
					validHenchmen.Add (h);
				}
			}

			if (validHenchmen.Count > 0) {
				Region region = GameManager.instance.game.regionsByID [regionID];
				MissionWrapper r = new MissionWrapper ();
				r.m_henchmen = validHenchmen;
				r.m_mission = GameManager.instance.m_travelMission;
				r.m_organization = GameManager.instance.game.player;
				r.m_region = region;
				r.m_henchmenSlotInFocus = clickedSlot;

				GameManager.instance.currentMissionWrapper = r;

				GameManager.instance.PushMenuState (State.SelectHenchmenMenu);
			}
		}
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
//		switch (thisGameEvent) {
//		case GameEvent.UI_SectionHeader_MinimizeButtonClicked:
//			
//			UpdateRegionList ();
//			break;
//		}
	}
}
