using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HenchmenMenu : MenuState, IObserver {
	public static HenchmenMenu instance;

	public GameObject m_henchmanForHireListViewItem;
	public GameObject m_henchmenListViewItem;
	public GameObject m_sectionHeader;
	public GameObject m_scrollView;
	public GameObject m_scrollViewContent;
	public GameObject m_sortPanelParent;

	private List<GameObject> m_listViewItems = new List<GameObject> ();
	private List<GameObject> m_activityPaneListViewItems = new List<GameObject> ();

	private bool m_activityPaneOpen = false;

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
		GameManager.instance.game.player.AddObserver (this);

		Debug.Log ("Starting Henchmen Menu");
//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
//		t.m_tabButton.ChangeState (TabButton.State.Selected);
		m_tabInfo = tabInfo;
//		Debug.Log(m_tabInfo);
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_scrollView.gameObject.SetActive (true);
		m_sortPanelParent.gameObject.SetActive (true);

		UpdateHenchmenList ();
	}

	public override void OnHold()
	{
	}

	public override void OnReturn()
	{
		UpdateHenchmenList ();
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.Organization_HenchmenDismissed:
		case GameEvent.Organization_HenchmenHired:
		case GameEvent.Organization_HenchmenFired:
			UpdateHenchmenList ();
//			Organization o = (Organization)subject;
			break;
		}
	}

	public void UpdateHenchmenList ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		Organization player = GameManager.instance.game.player;

		GameObject h2 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h2.transform.localScale = Vector3.one;
		h2.GetComponent<SectionHeader> ().Initialize ("CURRENT HENCHMEN (" + player.currentHenchmen.Count.ToString() + ")");
		m_listViewItems.Add (h2);

		if (player.currentHenchmen.Count > 0) {
			for (int i = 0; i < player.currentHenchmen.Count; i++) {
				Henchmen thisHenchmen = player.currentHenchmen [i];
				GameObject g = (GameObject)(Instantiate (m_henchmenListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<Henchmen_ListViewItem> ().Initialize (thisHenchmen);
			}
		}

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		h.GetComponent<SectionHeader> ().Initialize ("AVAILABLE FOR HIRE (" + player.availableHenchmen.Count.ToString() + ")");
		m_listViewItems.Add (h);

		for (int i = 0; i < player.availableHenchmen.Count; i++) {
			Henchmen thisHenchmen = player.availableHenchmen [i];
			GameObject g = (GameObject)(Instantiate (m_henchmanForHireListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			g.GetComponent<HenchmenForHire_ListViewItem> ().Initialize (thisHenchmen);
		}
	}

	public override void OnDeactivate()
	{
		GameManager.instance.game.player.RemoveObserver (this);
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

		if (m_activityPaneOpen) {
			HideActionPane ();
		}
	}

	public override void DisplayActionPane (){

		m_activityPaneOpen = true;

		TabMenu.instance.m_activityPane.SetActive (true);

		Organization player = GameManager.instance.game.player;
		List<TurnResultsEntry> t = new List<TurnResultsEntry> ();

		if (player.turnResultsByType.ContainsKey (GameEvent.Organization_HenchmenHired)) {
			
			t.AddRange (player.turnResultsByType [GameEvent.Organization_HenchmenHired]);
		}

		GameObject activityCell = TabMenu.instance.m_activityCell_Small;
		GameObject activityPaneContents = TabMenu.instance.m_activityPaneScrollViewContent;

		foreach (TurnResultsEntry e in t) {
			GameObject g = (GameObject)(Instantiate (activityCell, activityPaneContents.transform));
			g.transform.localScale = Vector3.one;
			m_activityPaneListViewItems.Add (g);
			g.GetComponent<ActivitySmall_ListViewItem> ().Initialize (e);
		}
	}

	public override void HideActionPane (){

		m_activityPaneOpen = true;

		while (m_activityPaneListViewItems.Count > 0) {
			GameObject g = m_activityPaneListViewItems [0];
			m_activityPaneListViewItems.RemoveAt (0);
			Destroy (g);
		}

		TabMenu.instance.m_activityPane.SetActive (false);
	}

	public override void OnUpdate()
	{

	}
}
