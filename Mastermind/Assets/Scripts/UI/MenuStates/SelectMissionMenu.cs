using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectMissionMenu : MenuState {

	public static SelectMissionMenu instance;

	public GameObject m_selectMissionMenu;
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
		Debug.Log ("Starting Select Mission Menu");

		m_selectMissionMenu.gameObject.SetActive (true);
		m_backButton.gameObject.SetActive (true);

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
		if (GameManager.instance.targetMenuState != State.None) {
			GameManager.instance.currentMissionRequest = null;
		}

		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_selectMissionMenu.gameObject.SetActive (false);
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
		GameManager.instance.currentMissionRequest = null;
		GameManager.instance.PopMenuState ();
	}

	private void UpdateMissionList ()
	{
		m_selectMissionMenu.gameObject.SetActive (true);

		if (GameManager.instance.currentMissionRequest != null) {

			GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
			h.transform.localScale = Vector3.one;
			h.GetComponent<SectionHeader> ().Initialize ("AVAILABLE MISSIONS");
			m_listViewItems.Add (h);

			foreach (MissionBase m in GameManager.instance.m_missionBank) {

				switch (m.m_targetType) {

				case MissionBase.TargetType.Region:

					if (m.IsValid ()) {

						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
						g.transform.localScale = Vector3.one;
						m_listViewItems.Add (g);
						g.GetComponent<Mission_ListViewItem> ().Initialize (m, GameManager.instance.currentMissionRequest);

					}

					break;

				case MissionBase.TargetType.Henchmen:

					foreach (Henchmen thisH in GameManager.instance.currentMissionRequest.m_henchmen) {

						GameManager.instance.currentMissionRequest.m_henchmenInFocus = thisH;

						if (m.IsValid ()) {

							GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
							g.transform.localScale = Vector3.one;
							m_listViewItems.Add (g);
							g.GetComponent<Mission_ListViewItem> ().Initialize (m, GameManager.instance.currentMissionRequest);

						}
					}

					break;

				case MissionBase.TargetType.AssetToken:

					foreach (Region.TokenSlot ts in GameManager.instance.currentMissionRequest.m_region.assetTokens) {

						GameManager.instance.currentMissionRequest.m_tokenInFocus = ts;

						if (m.IsValid ()) {

							GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
							g.transform.localScale = Vector3.one;
							m_listViewItems.Add (g);
							g.GetComponent<Mission_ListViewItem> ().Initialize (m, GameManager.instance.currentMissionRequest);

						}
					}

					break;
				}


			}
		}
	}


	public void SelectMission (MissionBase m)
	{
		if (GameManager.instance.game.player.currentCommandPool >= m.m_cost) {
			GameManager.instance.currentMissionRequest.m_mission = m;
			GameManager.instance.ProcessMissionRequest ();
			GameManager.instance.PopMenuState ();
		}
	}

}
