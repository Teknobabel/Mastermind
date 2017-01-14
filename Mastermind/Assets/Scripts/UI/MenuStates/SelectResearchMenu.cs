using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectResearchMenu : MenuState {
	public static SelectResearchMenu instance;

	public GameObject m_selectResearchMenu;
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

		m_selectResearchMenu.gameObject.SetActive (true);
		m_backButton.gameObject.SetActive (true);

		UpdateResearchList ();

	}

	public override void OnHold()
	{

	}

	private void UpdateResearchList ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		// show research mission

		ResearchButton rb = GameManager.instance.currentMissionWrapper.m_researchButtonInFocus;

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		SectionHeader sh = (SectionHeader)h.GetComponent<SectionHeader> ();

		m_listViewItems.Add (h);


		GameManager.instance.currentMissionWrapper.m_mission = rb.researchObject.m_researchMission;
		GameObject g2 = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
		g2.transform.localScale = Vector3.one;
		m_listViewItems.Add (g2);
		g2.GetComponent<Mission_ListViewItem> ().Initialize ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());
		sh.m_children.Add (g2);

		sh.Initialize ("RESEARCH");



		// show missions to be unlocked

		List<MissionBase> unlockedMissions = new List<MissionBase> ();

		foreach (MissionBase m in GameManager.instance.m_missionBank) {

			if (rb.researchObject.m_researchGained.Contains(m.m_requiredResearch)) {

				unlockedMissions.Add (m);
			}
		}

		GameObject h2 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h2.transform.localScale = Vector3.one;
		SectionHeader sh2 = (SectionHeader)h2.GetComponent<SectionHeader> ();
		m_listViewItems.Add (h2);

		foreach (MissionBase m in unlockedMissions) {

			GameManager.instance.currentMissionWrapper.m_mission = m;

			GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			g.GetComponent<Mission_ListViewItem> ().Initialize (GameManager.instance.currentMissionWrapper);
//			m_missionsInList.Add (m);
			sh2.m_children.Add (g);
		}

		sh2.Initialize ("NEW MISSIONS");

		GameManager.instance.currentMissionWrapper.m_mission = null;
	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_selectResearchMenu.gameObject.SetActive (false);
		m_backButton.gameObject.SetActive (false);

	}

	public override void SelectMission (MissionWrapper mw)
	{
		if (GameManager.instance.game.player.currentCommandPool >= mw.m_mission.m_cost) {
			GameManager.instance.currentMissionWrapper = mw;
			GameManager.instance.currentMissionWrapper.m_organization = GameManager.instance.game.player;
			GameManager.instance.ProcessMissionWrapper ();
			GameManager.instance.PopMenuState ();
		}
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
