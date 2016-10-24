﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectHenchmenMenu : MenuState {
	public static SelectHenchmenMenu instance;

	public GameObject m_selectHenchmenMenu;

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

		if (GameManager.instance.currentMissionRequest != null) {

			MissionRequest r = GameManager.instance.currentMissionRequest;

			GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
			h.transform.localScale = Vector3.one;
			h.GetComponent<SectionHeader> ().Initialize ("AVAILABLE HENCHMEN");
			m_listViewItems.Add (h);

			for (int i = 0; i < r.m_henchmen.Count; i++) {
				Henchmen thisHenchmen = r.m_henchmen [i];
				GameObject g = (GameObject)(Instantiate (m_henchmenListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<Henchmen_ListViewItem> ().Initialize (thisHenchmen);
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

		m_selectHenchmenMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	public void SelectHenchmen (int hID)
	{
		GameManager.instance.currentMissionRequest.m_henchmen.Clear();
		Henchmen h = GameManager.instance.game.GetHenchmenByID (hID);
		GameManager.instance.currentMissionRequest.m_henchmen.Add (h);

		GameManager.instance.ProcessMissionRequest ();

		GameManager.instance.PopMenuState ();
	}
}
