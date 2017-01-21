using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectRegionMenu : MenuState {
	public static SelectRegionMenu instance;

	public GameObject m_selectRegionMenu;
	public GameObject m_backButton;

	public GameObject m_regionListViewItem;
	public GameObject m_sectionHeader;

	public GameObject m_scrollViewContent;

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
		Debug.Log ("Starting Select Region Menu");

		m_selectRegionMenu.gameObject.SetActive (true);
		m_backButton.gameObject.SetActive (true);

		UpdateRegionList ();

	}

	public override void OnHold()
	{

	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_selectRegionMenu.gameObject.SetActive (false);
		m_backButton.gameObject.SetActive (false);

	}

	public override void OnUpdate()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {
			BackButtonPressed ();
		}
	}

	public override void BackButtonPressed ()
	{
		GameManager.instance.currentMissionWrapper = null;
		GameManager.instance.PopMenuState ();
	}

	public void UpdateRegionList ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		Dictionary<string, List<Region>> sortedRegionList = WorldMenu.instance.GetRegionList (WorldMenu.SortType.RegionGroup);

//		if (m_sortDirection == SortDirection.Normal) {

			foreach (KeyValuePair<string, List<Region>> pair in sortedRegionList) {

				GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
				h.transform.localScale = Vector3.one;
				SectionHeader sh = (SectionHeader)h.GetComponent<SectionHeader> ();

				m_listViewItems.Add (h);

				for (int i = 0; i < pair.Value.Count; i++) {
					Region thisRegion = pair.Value [i];
					GameObject g = (GameObject)(Instantiate (m_regionListViewItem, m_scrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					m_listViewItems.Add (g);
					g.GetComponent<Region_ListViewItem> ().Initialize (thisRegion);
					sh.m_children.Add (g);
				}

				sh.Initialize (pair.Key.ToUpper ());
			}
//		} else {
//
//
//			foreach (KeyValuePair<string, List<Region>> pair in sortedRegionList.Reverse()) {
//
//				GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
//				h.transform.localScale = Vector3.one;
//				SectionHeader sh = (SectionHeader)h.GetComponent<SectionHeader> ();
//
//				m_listViewItems.Add (h);
//
//				if (sortedRegionList.Count == 1) {
//
//					List<Region> regions = new List<Region> (pair.Value);
//
//					for (int i = regions.Count - 1; i >= 0; i--) {
//						Region thisRegion = pair.Value [i];
//						GameObject g = (GameObject)(Instantiate (m_regionListViewItem, m_scrollViewContent.transform));
//						g.transform.localScale = Vector3.one;
//						m_listViewItems.Add (g);
//						g.GetComponent<Region_ListViewItem> ().Initialize (thisRegion);
//						sh.m_children.Add (g);
//					}
//
//				} else {
//
//					for (int i = 0; i < pair.Value.Count; i++) {
//						Region thisRegion = pair.Value [i];
//						GameObject g = (GameObject)(Instantiate (m_regionListViewItem, m_scrollViewContent.transform));
//						g.transform.localScale = Vector3.one;
//						m_listViewItems.Add (g);
//						g.GetComponent<Region_ListViewItem> ().Initialize (thisRegion);
//						sh.m_children.Add (g);
//					}
//				}
//
//				sh.Initialize (pair.Key.ToUpper ());
//
//			}
//		}
	}
}
