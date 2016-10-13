using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMenu : MenuState {

	public GameObject m_regionListViewItem;
	public GameObject m_sectionHeader;
	public GameObject m_scrollView;
	public GameObject m_scrollViewContent;

	private List<GameObject> m_listViewItems = new List<GameObject> ();

	public override void OnActivate()
	{
		Debug.Log ("Starting Lair Menu");
		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		t.m_tabButton.ChangeState (TabButton.State.Selected);

		m_scrollView.gameObject.SetActive (true);

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
	}

	public override void OnReturn()
	{
	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		t.m_tabButton.ChangeState (TabButton.State.Unselected);
	}

	public override void OnUpdate()
	{

	}
}
