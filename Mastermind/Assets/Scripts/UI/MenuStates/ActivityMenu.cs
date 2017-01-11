using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActivityMenu : MenuState, ISubject {
	public static ActivityMenu instance;

	public GameObject m_turnResultsMenu;
	public GameObject m_turnResultsListViewItem;
//	public GameObject m_missionListViewItem;
//	public GameObject m_activeMissionListViewItem;
	public GameObject m_turnResultsButton;
	public GameObject m_sectionHeader;
	public GameObject m_scrollViewContent;
	public GameObject m_sortPane;
	public GameObject m_sortPaneScrollViewContent;

	public Sprite[] m_turnResultsIcons;

	private List<GameObject> m_listViewItems = new List<GameObject> ();
	private List<TurnResultsButton> m_sortPaneListViewItems = new List<TurnResultsButton> ();

	private bool m_displayTurnResults = false;
	private int m_currentlyDisplayedTurnNumber = -1;

	private List<IObserver>
	m_observers = new List<IObserver> ();

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

		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}
			
		m_turnResultsMenu.SetActive (true);
			
//		if (m_displayTurnResults) {
//			
//			m_displayTurnResults = false;
//			int lastTurn = GameManager.instance.game.turnNumber - 1;
//			TurnResultsButtonPressed (lastTurn);
//
//		} 
//		else {
//			TurnResultsButtonPressed (GameManager.instance.game.turnNumber);
//		}

		int lastTurn = GameManager.instance.game.turnNumber - 1;
		TurnResultsButtonPressed (lastTurn);


	}

	public override void OnHold()
	{
		m_turnResultsMenu.SetActive (false);
	}

	public override void OnReturn()
	{
		base.OnReturn ();

		m_turnResultsMenu.SetActive (true);
	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		while (m_sortPaneListViewItems.Count > 0) {
			TurnResultsButton g = m_sortPaneListViewItems [0];
			g.Destroy ();
			m_sortPaneListViewItems.RemoveAt (0);
			Destroy (g.gameObject);
		}

		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

		m_turnResultsMenu.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	public void UpdateActiveTurnView (int turn)
	{
		m_currentlyDisplayedTurnNumber = turn;

		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		while (m_sortPaneListViewItems.Count > 0) {
			TurnResultsButton g = m_sortPaneListViewItems [0];
			g.Destroy ();
			m_sortPaneListViewItems.RemoveAt (0);
			Destroy (g.gameObject);
		}

		Organization player = GameManager.instance.game.player;

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		h.GetComponent<SectionHeader> ().Initialize ("TURN " + turn.ToString () + " RESULTS");
		m_listViewItems.Add (h);

		if (turn != GameManager.instance.game.turnNumber && player.turnResults.ContainsKey (turn)) {

			List<TurnResultsEntry> t = player.turnResults [turn];

			int position = 0;
			foreach (TurnResultsEntry thisT in t) {

				GameObject g = (GameObject)(Instantiate (m_turnResultsListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<TurnResults_ListViewItem> ().Initialize (thisT, position);
				position++;
			}
		} 
//		else if (turn == GameManager.instance.game.turnNumber) {
//			
//			foreach (MissionWrapper a in player.activeMissions) {
//				GameObject g = (GameObject)(Instantiate (m_activeMissionListViewItem, m_scrollViewContent.transform));
//				g.transform.localScale = Vector3.one;
//				m_listViewItems.Add (g);
//				g.GetComponent<Mission_Active_ListViewItem> ().Initialize (a);
//			}
//		}

		// create turn buttons

		foreach (KeyValuePair<int, List<TurnResultsEntry>> pair in player.turnResults) {
			if (pair.Key != GameManager.instance.game.turnNumber) {
				GameObject g = (GameObject)(Instantiate (m_turnResultsButton, m_sortPaneScrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				TurnResultsButton b = (TurnResultsButton) g.GetComponent<TurnResultsButton> ();
				b.Initialize(pair.Key);
				m_sortPaneListViewItems.Add (b);
			}
		}

//		GameObject g2 = (GameObject)(Instantiate (m_turnResultsButton, m_sortPaneScrollViewContent.transform));
//		g2.transform.localScale = Vector3.one;
//		TurnResultsButton b2 = (TurnResultsButton)g2.GetComponent<TurnResultsButton> ();
//		b2.Initialize (GameManager.instance.game.turnNumber);
//		m_sortPaneListViewItems.Add (b2);
	}

	public void TurnResultsButtonPressed (int turnNumber)
	{
		UpdateActiveTurnView (turnNumber);
		Notify (this, GameEvent.ActivityMenu_TurnResultsChanged);
	}

	public void AddObserver (IObserver observer)
	{
		if (!m_observers.Contains(observer))
		{
			m_observers.Add (observer);
		}
	}

	public void RemoveObserver (IObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (ISubject subject, GameEvent thisGameEvent)
	{
		List<IObserver> observers = new List<IObserver> (m_observers);

		for (int i=0; i < observers.Count; i++)
		{
			observers[i].OnNotify(subject, thisGameEvent);
		}
	}

	public bool displayTurnResults {get{return m_displayTurnResults;}set{m_displayTurnResults = value;}}
	public int currentlyDisplayedTurnNumber {get{return m_currentlyDisplayedTurnNumber; }}
}
