using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectMissionMenu : MenuState {

	public static SelectMissionMenu instance;

	public GameObject m_selectMissionMenu;

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
		Debug.Log ("Starting Select Mission Menu");

		UpdateMissionList ();

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

		m_selectMissionMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	private void UpdateMissionList ()
	{
		m_selectMissionMenu.gameObject.SetActive (true);

		if (GameManager.instance.currentMissionRequest != null) {

			foreach (MissionBase m in GameManager.instance.m_missionBank) {
				if (m.IsValid ()) {

					GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					m_listViewItems.Add (g);
					g.GetComponent<Mission_ListViewItem> ().Initialize (m, GameManager.instance.currentMissionRequest.m_henchmen);

				}
			}
		}
	}


	public void SelectMission (MissionBase m)
	{
		if (GameManager.instance.game.player.currentCommandPool >= m.m_cost) {
			GameManager.instance.game.player.UseCommandPoints (m.m_cost);
			GameManager.instance.currentMissionRequest.m_mission = m;
			GameManager.instance.ProcessMissionRequest ();
			GameManager.instance.PopMenuState ();
		}
	}

}
