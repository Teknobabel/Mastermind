using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HenchmenDetailMenu : MenuState {
	public static HenchmenDetailMenu instance;

	public GameObject m_henchmenDetailMenu;
	public GameObject m_backButton;

	public GameObject m_henchmenListViewItem;
	public GameObject m_missionListViewItem;
	public GameObject m_activeMissionListViewItem;
	public GameObject m_turnResultsListViewItem;

	public GameObject m_sectionHeader;

	public GameObject m_scrollViewContent;

	private List<GameObject> m_listViewItems = new List<GameObject> ();

	private int m_henchmenID = -1;

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
		Debug.Log ("Starting Henchmen Detail Menu");

		m_henchmenDetailMenu.gameObject.SetActive (true);
		m_backButton.gameObject.SetActive (true);

		UpdateListView ();

	}

	public void UpdateListView ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}


		// show henchmen

		if (m_henchmenID != -1) {

			GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
			h.transform.localScale = Vector3.one;
			h.GetComponent<SectionHeader> ().Initialize ("HENCHMEN DETAIL");
			m_listViewItems.Add (h);

			Henchmen thisHenchmen = GameManager.instance.game.GetHenchmenByID (m_henchmenID);
			GameObject g = (GameObject)(Instantiate (m_henchmenListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			g.GetComponent<Henchmen_ListViewItem> ().Initialize (thisHenchmen);





			// show any currently active missions

			List<MissionWrapper> currentMissions = new List<MissionWrapper> ();

			Organization player = GameManager.instance.game.player;

			foreach (MissionWrapper a in player.activeMissions) {

				if ((a.m_henchmenInFocus != null && a.m_henchmenInFocus == thisHenchmen) || a.m_henchmen.Contains (thisHenchmen)) {

					currentMissions.Add (a);
				}
			}

			if (currentMissions.Count > 0) {

				GameObject h3 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
				h3.transform.localScale = Vector3.one;
				h3.GetComponent<SectionHeader> ().Initialize ("CURRENTLY ACTIVE MISSIONS");
				m_listViewItems.Add (h3);

				foreach (MissionWrapper a in currentMissions) {

					GameObject g4 = (GameObject)(Instantiate (m_activeMissionListViewItem, m_scrollViewContent.transform));
					g4.transform.localScale = Vector3.one;
					m_listViewItems.Add (g4);
					g4.GetComponent<Mission_Active_ListViewItem> ().Initialize (a);

				}
			}



			// show linked missions

			GameObject h2 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
			h2.transform.localScale = Vector3.one;
			h2.GetComponent<SectionHeader> ().Initialize ("MISSIONS");
			m_listViewItems.Add (h2);

			List<MissionBase> validMissions = new List<MissionBase> ();

			foreach (MissionBase m in GameManager.instance.m_missionBank) {

				if (thisHenchmen.HasTrait (m.m_linkedTrait)) {

					validMissions.Add (m);
				}
			}

			if (validMissions.Count > 0) {

				foreach (MissionBase m in validMissions) {

					GameObject g2 = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
					g2.transform.localScale = Vector3.one;
					m_listViewItems.Add (g2);
					g2.GetComponent<Mission_ListViewItem> ().Initialize (m, m.m_maxRank);
				}
			}

			// show turn results history

			GameObject h4 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
			h4.transform.localScale = Vector3.one;
			h4.GetComponent<SectionHeader> ().Initialize ("HISTORY");
			m_listViewItems.Add (h4);

			List<TurnResultsEntry> validEntries = new List<TurnResultsEntry> ();

			foreach (KeyValuePair<int, List<TurnResultsEntry>> pair in GameManager.instance.game.player.turnResults) {

				if (pair.Value.Count > 0) {

					foreach (TurnResultsEntry t in pair.Value) {
						
						if (t.m_henchmenIDs.Contains (thisHenchmen.id)) {

							validEntries.Add (t);
						}
					}
				}
			}

			int position = 0;
			foreach (TurnResultsEntry thisT in validEntries) {

				GameObject g3 = (GameObject)(Instantiate (m_turnResultsListViewItem, m_scrollViewContent.transform));
				g3.transform.localScale = Vector3.one;
				m_listViewItems.Add (g3);
				g3.GetComponent<TurnResults_ListViewItem> ().Initialize (thisT, position);
				position++;
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
		m_henchmenID = -1;

		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_henchmenDetailMenu.SetActive (false);
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

	public int henchmenID {get{ return m_henchmenID; }set{m_henchmenID = value;}}
}
