using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMenu : MenuState {
	public static WorldMenu instance;

	public GameObject m_regionListViewItem;
	public GameObject m_sectionHeader;
	public GameObject m_scrollView;
	public GameObject m_scrollViewContent;
	public GameObject m_sortPanelParent;

	private List<GameObject> m_listViewItems = new List<GameObject> ();

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
//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
//		t.m_tabButton.ChangeState (TabButton.State.Selected);
		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		UpdateRegionList ();
	}

	private void UpdateRegionList ()
	{
		m_scrollView.gameObject.SetActive (true);
		m_sortPanelParent.gameObject.SetActive (true);

		Dictionary<RegionData.RegionGroup, List<Region>> regionsByGroup = GameManager.instance.game.GetAllRegionsByGroup ();

		foreach (KeyValuePair<RegionData.RegionGroup, List<Region>> pair in regionsByGroup) {

			GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
			h.transform.localScale = Vector3.one;
			h.GetComponent<SectionHeader> ().Initialize (pair.Key.ToString().ToUpper());
			m_listViewItems.Add (h);

			for (int i = 0; i < pair.Value.Count; i++) {
				Region thisRegion = pair.Value [i];
				GameObject g = (GameObject)(Instantiate (m_regionListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<Region_ListViewItem> ().Initialize (thisRegion);
			}
		}
	}

	public override void OnHold()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_scrollView.gameObject.SetActive (false);
		m_sortPanelParent.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		UpdateRegionList ();
	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
//		t.m_tabButton.ChangeState (TabButton.State.Unselected);
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

		m_scrollView.gameObject.SetActive (false);
		m_sortPanelParent.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	public void SelectMissionForRegion (int regionID)
	{
		if (GameManager.instance.game.regionsByID.ContainsKey (regionID)) {
			Region r = GameManager.instance.game.regionsByID [regionID];

			if (r.currentHenchmen.Count > 0 && GameManager.instance.game.player.GetMission(r) == null) {

				MissionRequest mr = new MissionRequest ();
				mr.m_region = r;
				foreach (Henchmen h in r.currentHenchmen) {
					mr.m_henchmen.Add (h);
				}
				GameManager.instance.currentMissionRequest = mr;

				GameManager.instance.PushMenuState (State.SelectMissionMenu);
			}
		}
	}

	public void SelectHenchmenForTravel (int regionID)
	{
		// gather list of henchmen that can travel
		List<Henchmen> validHenchmen = new List<Henchmen>();

		Organization player = GameManager.instance.game.player;

		if (player.currentCommandPool >= GameManager.instance.m_travelMission.m_cost) {
			
			foreach (Henchmen h in player.currentHenchmen) {

				Organization.ActiveMission a = player.GetMissionForHenchmen (h);

				if (a == null && h.currentRegion.id != regionID) {
					validHenchmen.Add (h);
				}
			}

			if (validHenchmen.Count > 0) {
				Region region = GameManager.instance.game.regionsByID [regionID];
				MissionRequest r = new MissionRequest ();
				r.m_henchmen = validHenchmen;
				r.m_mission = GameManager.instance.m_travelMission;
				r.m_region = region;

				GameManager.instance.currentMissionRequest = r;

				GameManager.instance.PushMenuState (State.SelectHenchmenMenu);
			}
		}
	}
}
