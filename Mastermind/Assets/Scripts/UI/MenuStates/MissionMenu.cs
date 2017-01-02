using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionMenu : MenuState {
	public static MissionMenu instance;

	public GameObject m_missionMenu;
	public GameObject m_backButton;

	public GameObject m_missionListViewItem;
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

	public override void OnReturn()
	{
	}

	private void UpdateMissionMenu ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		List<MissionBase> availableMissions = new List<MissionBase> ();

		Organization p = GameManager.instance.game.player;

		if (p.currentResearch.Count > 0) {

			foreach (MissionBase m in GameManager.instance.m_missionBank) {

				if (m.m_requiredResearch != null && p.currentResearch.Contains (m.m_requiredResearch)) {

					availableMissions.Add (m);
				}
			}
		}


		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		h.GetComponent<SectionHeader> ().Initialize ("AVAILABLE MISSIONS");
		m_listViewItems.Add (h);

		foreach (MissionBase m in availableMissions) {

			GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			Mission_ListViewItem mlv = g.GetComponent<Mission_ListViewItem> ();
			mlv.Initialize (m, m.m_maxRank);
			mlv.m_missionSuccessChance.gameObject.SetActive (false);
			mlv.m_button.gameObject.SetActive (false);
			//			m_missionsInList.Add (m);
		}
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
