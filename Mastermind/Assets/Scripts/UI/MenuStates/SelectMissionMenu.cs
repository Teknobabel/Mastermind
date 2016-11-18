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
		for (int i = 0; i < GameManager.instance.currentMissionWrapper.m_region.henchmenSlots.Count; i++) {

			Region.HenchmenSlot hs = GameManager.instance.currentMissionWrapper.m_region.henchmenSlots [i];

			if (i < m_henchmenSlots.Length) {

				m_henchmenSlots [i].gameObject.SetActive (true);
				m_henchmenSlots [i].Initialize (hs);
			} 
//			else {
//				m_henchmenSlots [i].Deactivate ();
//			}
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

				foreach (Region.TokenSlot t in GameManager.instance.currentMissionWrapper.m_region.allTokens) {

					GameManager.instance.currentMissionWrapper.m_tokenInFocus = t;

				switch (t.m_type) {
					case Region.TokenSlot.TokenType.Asset:
						
						GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.AssetToken;
						break;

					case Region.TokenSlot.TokenType.Control:

						if (t.m_controlToken.m_controlType == ControlToken.ControlType.Economic) {
							GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.EconomicControlToken;
						} else if (t.m_controlToken.m_controlType == ControlToken.ControlType.Military) {
							GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.MilitaryControlToken;
						} else if (t.m_controlToken.m_controlType == ControlToken.ControlType.Political) {
							GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.PoliticalControlToken;
						}

					break;

					case Region.TokenSlot.TokenType.Policy:

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

				GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.Region;
				GameManager.instance.currentMissionWrapper.m_henchmenInFocus = null;

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

//	private void UpdateMissionList2 ()
//	{
//		m_selectMissionMenu.gameObject.SetActive (true);
//
//		if (GameManager.instance.currentMissionWrapper != null) {
//
//			GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
//			h.transform.localScale = Vector3.one;
//			h.GetComponent<SectionHeader> ().Initialize ("AVAILABLE MISSIONS");
//			m_listViewItems.Add (h);
//
//			Debug.Log ("<color=red>" + GameManager.instance.currentMissionWrapper.m_scope + "</color>");
//			switch (GameManager.instance.currentMissionWrapper.m_scope) {
//			case MissionBase.TargetType.Region:
//			case MissionBase.TargetType.None:
//				
//				foreach (MissionBase m in GameManager.instance.m_missionBank) {
//
//					GameManager.instance.currentMissionWrapper.m_mission = m;
//
//					switch (m.m_targetType) {
//
//					case MissionBase.TargetType.Region:
//
//						if (m.IsValid ()) {
//
//							GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
//							g.transform.localScale = Vector3.one;
//							m_listViewItems.Add (g);
//							g.GetComponent<Mission_ListViewItem> ().Initialize ();
//
//						}
//
//						break;
//
//					case MissionBase.TargetType.Henchmen:
//
//						foreach (Henchmen thisH in GameManager.instance.currentMissionWrapper.m_henchmen) {
//
//							GameManager.instance.currentMissionWrapper.m_henchmenInFocus = thisH;
//
//							if (m.IsValid ()) {
//
//								GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
//								g.transform.localScale = Vector3.one;
//								m_listViewItems.Add (g);
//								g.GetComponent<Mission_ListViewItem> ().Initialize ();
//
//							}
//						}
//
//						break;
//
//					case MissionBase.TargetType.AssetToken:
//
//						foreach (Region.TokenSlot ts in GameManager.instance.currentMissionWrapper.m_region.assetTokens) {
//
//							GameManager.instance.currentMissionWrapper.m_tokenInFocus = ts;
//
//							if (m.IsValid ()) {
//
//								GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
//								g.transform.localScale = Vector3.one;
//								m_listViewItems.Add (g);
//								g.GetComponent<Mission_ListViewItem> ().Initialize ();
//
//							}
//						}
//
//						break;
//					}
//				}
//				break;
//			case MissionBase.TargetType.ControlToken:
//			case MissionBase.TargetType.AssetToken:
//			case MissionBase.TargetType.EconomicControlToken:
//			case MissionBase.TargetType.PoliticalControlToken:
//			case MissionBase.TargetType.MilitaryControlToken:
////				Debug.Log ("<color=red>A;SLKDFJSLKDJFSLKDJF</color>");
//				foreach (MissionBase m in GameManager.instance.m_missionBank) {
//
//					GameManager.instance.currentMissionWrapper.m_mission = m;
//
//					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {
//						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
//						g.transform.localScale = Vector3.one;
//						m_listViewItems.Add (g);
//						g.GetComponent<Mission_ListViewItem> ().Initialize ();
//					}
//				}
//				break;
////			case MissionBase.TargetType.ControlToken:
////
////				foreach (MissionBase m in GameManager.instance.m_missionBank) {
////
////					GameManager.instance.currentMissionWrapper.m_mission = m;
////
////					if (m.m_targetType == MissionBase.TargetType.ControlToken && m.IsValid ()) {
////						GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
////						g.transform.localScale = Vector3.one;
////						m_listViewItems.Add (g);
////						g.GetComponent<Mission_ListViewItem> ().Initialize ();
////					}
////				}
////				break;
//			}
//		}
//	}


	public void SelectMission (MissionBase m)
	{
		
		if (GameManager.instance.game.player.currentCommandPool >= m.m_cost) {
			GameManager.instance.currentMissionWrapper.m_mission = m;
			GameManager.instance.ProcessMissionWrapper ();
			GameManager.instance.PopMenuState ();
		}
	}

}
