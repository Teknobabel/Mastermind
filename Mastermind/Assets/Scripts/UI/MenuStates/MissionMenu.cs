using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionMenu : MenuState {
	public static MissionMenu instance;

	public GameObject m_missionMenu;
	public GameObject m_backButton;

	public GameObject m_missionListViewItem;
	public GameObject m_activeMissionListViewItem;
	public GameObject m_henchmenListViewItem;
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
		Debug.Log ("Starting Select Research Menu");

		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_missionMenu.gameObject.SetActive (true);
		m_backButton.gameObject.SetActive (true);

		UpdateMissionMenu ();

	}

	public override void OnHold()
	{

	}

	public void UpdateMissionMenu ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		// display any currently active mission

		Organization player = GameManager.instance.game.player;

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		SectionHeader sh = (SectionHeader)h.GetComponent<SectionHeader> ();
		m_listViewItems.Add (h);

		foreach (MissionWrapper a in player.activeMissions) {
			GameObject g = (GameObject)(Instantiate (m_activeMissionListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			g.GetComponent<Mission_Active_ListViewItem> ().Initialize (a);

			sh.m_children.Add (g);
		}

		sh.Initialize ("CURRENTLY ACTIVE MISSIONS");


		// display all unlocked missions

		List<MissionBase> availableMissions = new List<MissionBase> ();

		Organization p = GameManager.instance.game.player;

		if (p.currentResearch.Count > 0) {

			foreach (MissionBase m in GameManager.instance.m_missionBank) {

				if (m.m_requiredResearch != null && p.currentResearch.Contains (m.m_requiredResearch)) {

					availableMissions.Add (m);
				}
			}
		}


		GameObject h2 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h2.transform.localScale = Vector3.one;
		SectionHeader sh2 = (SectionHeader)h2.GetComponent<SectionHeader> ();
		m_listViewItems.Add (h2);

		foreach (MissionBase m in availableMissions) {

			GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			Mission_ListViewItem mlv = g.GetComponent<Mission_ListViewItem> ();
			mlv.Initialize (m, m.m_maxRank);
			mlv.m_missionSuccessChance.gameObject.SetActive (false);
			mlv.m_button.gameObject.SetActive (false);

			sh2.m_children.Add (g);
		}

		sh2.Initialize ("UNLOCKED MISSIONS");


		// display linked missions for all henchmen

		GameObject h3 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h3.transform.localScale = Vector3.one;
		SectionHeader sh3 = (SectionHeader)h3.GetComponent<SectionHeader> ();
		m_listViewItems.Add (h3);

		Dictionary<int, List<MissionBase>> henchmenMissions = new Dictionary<int, List<MissionBase>> ();

		foreach (Henchmen thisH in player.currentHenchmen)
		{
			List<MissionBase> linkedMissions = new List<MissionBase> ();

			foreach (MissionBase m in GameManager.instance.m_missionBank) {

				if (thisH.HasTrait (m.m_linkedTrait)) {

					linkedMissions.Add (m);
				}
			}
				
			henchmenMissions.Add (thisH.id, linkedMissions);
		}

		foreach (KeyValuePair<int, List<MissionBase>> pair in henchmenMissions) {

			if (pair.Value.Count > 0) {

				Henchmen thisH = GameManager.instance.game.GetHenchmenByID (pair.Key);

				GameObject g = (GameObject)(Instantiate (m_henchmenListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<Henchmen_ListViewItem> ().Initialize (thisH);

				sh3.m_children.Add (g);

				foreach (MissionBase m in pair.Value) {

					GameObject g2 = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
					g2.transform.localScale = Vector3.one;
					m_listViewItems.Add (g2);
					g2.GetComponent<Mission_ListViewItem> ().Initialize (m, m.m_maxRank);

					sh3.m_children.Add (g2);
				}



				GameObject h4 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
				h4.transform.localScale = Vector3.one;
//				h4.GetComponent<SectionHeader> ().Initialize ("");
				m_listViewItems.Add (h4);

				sh3.m_children.Add (h4);
			}
		}

		sh3.Initialize ("HENCHMEN MISSIONS");
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

		m_missionMenu.gameObject.SetActive (false);
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
}
