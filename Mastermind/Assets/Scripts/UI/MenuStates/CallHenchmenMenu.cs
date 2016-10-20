using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class CallHenchmenMenu : MenuState {
	public static CallHenchmenMenu instance;

	public GameObject m_callHenchmenMenu;
	public GameObject m_missionListViewItem;
	public GameObject m_scrollViewContent;
	public Henchmen_ListViewItem m_henchmenCell;

	private int m_henchmenID = -1;

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
		Debug.Log ("Starting Call Henchmen Menu");
		m_callHenchmenMenu.gameObject.SetActive (true);

		if (m_henchmenID != -1) {
			Henchmen h = GameManager.instance.game.GetHenchmenByID (m_henchmenID);
			m_henchmenCell.Initialize (h);
			UpdateMissionList (h);
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

		m_henchmenID = -1;
		m_callHenchmenMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	private void UpdateMissionList (Henchmen h)
	{
		foreach (MissionBase m in GameManager.instance.m_missionBank) {
			
			if (((IMission)m).IsValid ()) {
				
				GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				m_listViewItems.Add (g);
				g.GetComponent<Mission_ListViewItem> ().Initialize (m, h);
			}
		}
	}

	public void CloseButtonClicked ()
	{
		GameManager.instance.PopMenuState ();
	}

	public void Startmission (MissionBase m)
	{
		if (m_henchmenID != -1) {
			GameManager.instance.game.player.UseCommandPoints (m.m_cost);
			Henchmen h = GameManager.instance.game.GetHenchmenByID (m_henchmenID);
			List<Henchmen> l = new List<Henchmen> ();
			l.Add (h);
			GameManager.instance.game.player.AddMission (m, l);

			GameManager.instance.PopMenuState ();
		}
	}

	public int henchmenID {get{ return m_henchmenID; }set{m_henchmenID = value; }}

}
