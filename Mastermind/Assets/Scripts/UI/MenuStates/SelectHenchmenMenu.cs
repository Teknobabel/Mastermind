using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectHenchmenMenu : MenuState {
	public static SelectHenchmenMenu instance;

	public GameObject m_selectHenchmenMenu;
	public GameObject m_backButton;

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
		Debug.Log ("Starting Select Henchmen Menu");
		m_selectHenchmenMenu.gameObject.SetActive (true);
		m_backButton.gameObject.SetActive (true);

		if (GameManager.instance.currentMissionWrapper != null) {

			MissionWrapper r = GameManager.instance.currentMissionWrapper;

			GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
			h.transform.localScale = Vector3.one;
			SectionHeader sh = (SectionHeader)h.GetComponent<SectionHeader> ();
			m_listViewItems.Add (h);

			for (int i = 0; i < r.m_henchmen.Count; i++) {
				Henchmen thisHenchmen = r.m_henchmen [i];
				GameObject g = (GameObject)(Instantiate (m_henchmenListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<Henchmen_ListViewItem> ().InitializeHenchmen (thisHenchmen);

				sh.m_children.Add (g);
			}

			sh.Initialize ("AVAILABLE HENCHMEN");
		}
	}

	public override void OnHold()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_selectHenchmenMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		base.OnReturn ();

		m_selectHenchmenMenu.gameObject.SetActive (true);
	}

	public override void OnDeactivate()
	{
		if (GameManager.instance.targetMenuState != State.None) {
			GameManager.instance.currentMissionWrapper = null;
		}

		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_selectHenchmenMenu.gameObject.SetActive (false);
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

	public void SelectHenchmen (int hID)
	{
		Debug.Log ("YTFHGKHGJHKJHGJHGKHGLIUYL");
		GameManager.instance.currentMissionWrapper.m_henchmen.Clear();
		Henchmen h = GameManager.instance.game.GetHenchmenByID (hID);
		GameManager.instance.currentMissionWrapper.m_henchmen.Add (h);

		GameManager.instance.ProcessMissionWrapper ();

		GameManager.instance.PopMenuState ();
	}
}
