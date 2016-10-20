using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityMenu : MenuState {
	public static ActivityMenu instance;

	public GameObject m_turnResultsMenu;
	public GameObject m_turnResultsListViewItem;
	public GameObject m_turnResultsButton;
	public GameObject m_sectionHeader;
	public GameObject m_scrollViewContent;
	public GameObject m_sortPane;
	public GameObject m_sortPaneScrollViewContent;

	private List<GameObject> m_listViewItems = new List<GameObject> ();
	private List<GameObject> m_sortPaneListViewItems = new List<GameObject> ();

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
		Debug.Log ("Starting Activity Menu");
		//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		//		t.m_tabButton.ChangeState (TabButton.State.Selected);
		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}
			
		int lastTurn = GameManager.instance.game.turnNumber - 1;
//		Debug.Log (lastTurn);
		if (lastTurn > 0) {
			UpdateActiveTurnView (lastTurn);
		}

		m_sortPane.SetActive (true);
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

		while (m_sortPaneListViewItems.Count > 0) {
			GameObject g = m_sortPaneListViewItems [0];
			m_sortPaneListViewItems.RemoveAt (0);
			Destroy (g);
		}

		//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		//		t.m_tabButton.ChangeState (TabButton.State.Unselected);
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

	}

	public override void OnUpdate()
	{

	}

	public void UpdateActiveTurnView (int turn)
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		while (m_sortPaneListViewItems.Count > 0) {
			GameObject g = m_sortPaneListViewItems [0];
			m_sortPaneListViewItems.RemoveAt (0);
			Destroy (g);
		}

		Organization player = GameManager.instance.game.player;

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		h.GetComponent<SectionHeader> ().Initialize ("TURN " + turn.ToString () + " RESULTS");
		m_listViewItems.Add (h);

		if (player.turnResults.ContainsKey (turn)) {

			List<TurnResultsEntry> t = player.turnResults [turn];



			foreach (TurnResultsEntry thisT in t) {

				GameObject g = (GameObject)(Instantiate (m_turnResultsListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<TurnResults_ListViewItem> ().Initialize (thisT);
			}
		}

		foreach (KeyValuePair<int, List<TurnResultsEntry>> pair in player.turnResults) {
			GameObject g = (GameObject)(Instantiate (m_turnResultsButton, m_sortPaneScrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_sortPaneListViewItems.Add (g);
			g.GetComponent<TurnResultsButton> ().Initialize (pair.Key);
		}
	}
}
