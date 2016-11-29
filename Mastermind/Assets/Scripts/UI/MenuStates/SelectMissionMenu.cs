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

	public RegionHenchmenButton[] m_henchmenSlots;

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
		UpdateHenchmenList ();

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
			GameManager.instance.currentMissionWrapper = null;
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
		GameManager.instance.currentMissionWrapper = null;
		GameManager.instance.PopMenuState ();
	}

	private void UpdateHenchmenList ()
	{
		List<Region.HenchmenSlot> occupiedSlots = new List<Region.HenchmenSlot> ();

		foreach (Region.HenchmenSlot s in GameManager.instance.currentMissionWrapper.m_region.henchmenSlots) {

			if (s.m_state == Region.HenchmenSlot.State.Occupied) {

				occupiedSlots.Add (s);
			}
		}

		for (int i = 0; i < m_henchmenSlots.Length; i++) {

			if (i < occupiedSlots.Count) {

				Region.HenchmenSlot hs = occupiedSlots [i];

				m_henchmenSlots [i].gameObject.SetActive (true);
				m_henchmenSlots [i].Initialize (hs);

			} else {

				m_henchmenSlots [i].Deactivate ();
			}
		}
	}

	private void UpdateMissionList ()
	{
		m_selectMissionMenu.gameObject.SetActive (true);

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		h.GetComponent<SectionHeader> ().Initialize ("AVAILABLE MISSIONS");
		m_listViewItems.Add (h);

		foreach (MissionBase m in GameManager.instance.m_missionBank) {

			GameManager.instance.currentMissionWrapper.m_mission = m;

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Region) {

				if (m.m_targetType == MissionBase.TargetType.Region && m.IsValid ()) {

					GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					m_listViewItems.Add (g);
					g.GetComponent<Mission_ListViewItem> ().Initialize ();
				}

				foreach (TokenSlot t in GameManager.instance.currentMissionWrapper.m_region.allTokens) {

					GameManager.instance.currentMissionWrapper.m_tokenInFocus = t;

				switch (t.m_type) {
					case TokenSlot.TokenType.Asset:
						
						GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.AssetToken;
						break;

					case TokenSlot.TokenType.Control:

						if (t.m_controlToken.m_controlType == ControlToken.ControlType.Economic) {
							GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.EconomicControlToken;
						} else if (t.m_controlToken.m_controlType == ControlToken.ControlType.Military) {
							GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.MilitaryControlToken;
						} else if (t.m_controlToken.m_controlType == ControlToken.ControlType.Political) {
							GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.PoliticalControlToken;
						}

					break;

					case TokenSlot.TokenType.Policy:

						GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.PolicyToken;

						break;

					}

					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
						g.transform.localScale = Vector3.one;
						m_listViewItems.Add (g);
						g.GetComponent<Mission_ListViewItem> ().Initialize ();

					}
				}

				GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.Henchmen;
				GameManager.instance.currentMissionWrapper.m_tokenInFocus = null;

				foreach (Henchmen thisH in GameManager.instance.currentMissionWrapper.m_henchmen) {

					GameManager.instance.currentMissionWrapper.m_henchmenInFocus = thisH;

					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
						g.transform.localScale = Vector3.one;
						m_listViewItems.Add (g);
						g.GetComponent<Mission_ListViewItem> ().Initialize ();

					}
				}

				GameManager.instance.currentMissionWrapper.m_henchmenInFocus = null;
				GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.Agent;

				foreach (AgentWrapper thisAW in GameManager.instance.currentMissionWrapper.m_agents) {

					GameManager.instance.currentMissionWrapper.m_agentInFocus = thisAW;

					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
						g.transform.localScale = Vector3.one;
						m_listViewItems.Add (g);
						g.GetComponent<Mission_ListViewItem> ().Initialize ();

					}
				}


				GameManager.instance.currentMissionWrapper.m_agentInFocus = null;
				GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.OwnedAsset;

				if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.OwnedAsset) {

					foreach (Asset a in GameManager.instance.game.player.currentAssets) {

						GameManager.instance.currentMissionWrapper.m_assetInFocus = a;

						if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

							GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
							g.transform.localScale = Vector3.one;
							m_listViewItems.Add (g);
							g.GetComponent<Mission_ListViewItem> ().Initialize ();

						}
					}

					GameManager.instance.currentMissionWrapper.m_assetInFocus = null;
					GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.Region;
				}

			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Henchmen) {

					foreach (Henchmen thisH in GameManager.instance.currentMissionWrapper.m_henchmen) {

						GameManager.instance.currentMissionWrapper.m_henchmenInFocus = thisH;

						if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

							GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
							g.transform.localScale = Vector3.one;
							m_listViewItems.Add (g);
							g.GetComponent<Mission_ListViewItem> ().Initialize ();

						}
					}
				}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Agent) {

				foreach (AgentWrapper thisAW in GameManager.instance.currentMissionWrapper.m_agents) {

					GameManager.instance.currentMissionWrapper.m_agentInFocus = thisAW;

					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
						g.transform.localScale = Vector3.one;
						m_listViewItems.Add (g);
						g.GetComponent<Mission_ListViewItem> ().Initialize ();

					}
				}


			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.AssetToken || GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.EconomicControlToken || GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.MilitaryControlToken ||
				GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.PoliticalControlToken || GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.ControlToken || 
				GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.PolicyToken) {

				if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

					GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					m_listViewItems.Add (g);
					g.GetComponent<Mission_ListViewItem> ().Initialize ();

				}
			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.BaseUpgrade) {

				if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

					GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					m_listViewItems.Add (g);
					g.GetComponent<Mission_ListViewItem> ().Initialize ();

				}
			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Floor) {

				if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

					GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					m_listViewItems.Add (g);
					g.GetComponent<Mission_ListViewItem> ().Initialize ();

				}

				foreach (Henchmen thisH in GameManager.instance.currentMissionWrapper.m_henchmen) {

					GameManager.instance.currentMissionWrapper.m_henchmenInFocus = thisH;

					if (m.m_targetType == MissionBase.TargetType.Henchmen && m.IsValid ()) {

						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
						g.transform.localScale = Vector3.one;
						m_listViewItems.Add (g);
						g.GetComponent<Mission_ListViewItem> ().Initialize ();

					}
				}
			}


		}
	}

	public void SelectMission (MissionBase m)
	{
		
		if (GameManager.instance.game.player.currentCommandPool >= m.m_cost) {
			GameManager.instance.currentMissionWrapper.m_mission = m;
			GameManager.instance.currentMissionWrapper.m_organization = GameManager.instance.game.player;
			GameManager.instance.ProcessMissionWrapper ();
			GameManager.instance.PopMenuState ();
		}
	}

}
