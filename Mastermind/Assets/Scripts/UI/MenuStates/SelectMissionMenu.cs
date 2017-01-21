using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectMissionMenu : MenuState {

	public static SelectMissionMenu instance;

	public GameObject m_selectMissionMenu;
	public GameObject m_backButton;

	public GameObject m_missionListViewItem;
	public GameObject m_henchmenListViewItem;
	public GameObject m_sectionHeader;

	public GameObject m_scrollViewContent;

	public RegionHenchmenButton[] m_henchmenSlots;

	private List<GameObject> m_listViewItems = new List<GameObject> ();
	private List<MissionWrapper> m_validMissions = new List<MissionWrapper>();

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
//		UpdateHenchmenList ();

	}

	public override void OnHold()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		m_selectMissionMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		base.OnReturn ();

		m_selectMissionMenu.gameObject.SetActive (true);

		UpdateMissionList ();
//		UpdateHenchmenList ();
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

		List<MissionBase> validMissions = new List<MissionBase> ();

		if (GameManager.instance.currentMissionWrapper.m_floorInFocus.m_floorState == Base.FloorState.Empty) {

			// if floor is empty, gather list of upgrades that can be build

			GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.Floor;

			foreach (MissionBase mb in GameManager.instance.game.director.m_baseUpgrades) {

				if (mb.IsValid ()) {
					validMissions.Add (mb);
				}
			}

		} else {

			// if not empty, gather missions list gated by floor level

			foreach (BaseFloor.BaseFloorMissions bfm in GameManager.instance.currentMissionWrapper.m_floorInFocus.m_installedUpgrade.m_missions) {
	
				if (bfm.m_level <= GameManager.instance.currentMissionWrapper.m_floorInFocus.m_level) {
	
					foreach (MissionBase mb in bfm.m_availableMissions) {

						if (mb.IsValid ()) {
							validMissions.Add (mb);
						}
					}
				}
			}
		}

		// spawn mission list view items

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		SectionHeader header = (SectionHeader) h.GetComponent<SectionHeader> ();
		m_listViewItems.Add (h);

		foreach (MissionBase mb in validMissions) {

			GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			g.GetComponent<Mission_ListViewItem> ().Initialize (mb, mb.m_maxRank);
			header.m_children.Add (g);
		}

		header.Initialize ("AVAILABLE MISSIONS");

		// spawn participating henchmen list view items

		MissionWrapper r = GameManager.instance.currentMissionWrapper;

		GameObject h2 = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h2.transform.localScale = Vector3.one;
		SectionHeader sh = (SectionHeader)h2.GetComponent<SectionHeader> ();
		m_listViewItems.Add (h2);

		for (int i = 0; i < r.m_henchmen.Count; i++) {
			Henchmen thisHenchmen = r.m_henchmen [i];
			GameObject g = (GameObject)(Instantiate (m_henchmenListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			g.GetComponent<Henchmen_ListViewItem> ().InitializeHenchmen (thisHenchmen);

			sh.m_children.Add (g);
		}

		sh.Initialize ("PARTICIPATING HENCHMEN");

	}

	private void UpdateMissionList_TempDisabled ()
	{
		m_selectMissionMenu.gameObject.SetActive (true);


		foreach (MissionBase m in GameManager.instance.m_missionBank) {

			GameManager.instance.currentMissionWrapper.m_mission = m;

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Region) {

				if (m.m_targetType == MissionBase.TargetType.Region && m.IsValid ()) {

					m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

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

						m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

					}
				}

				GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.Henchmen;
				GameManager.instance.currentMissionWrapper.m_tokenInFocus = null;

				foreach (Henchmen thisH in GameManager.instance.currentMissionWrapper.m_henchmen) {

					GameManager.instance.currentMissionWrapper.m_henchmenInFocus = thisH;

					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

						m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

					}
				}

				GameManager.instance.currentMissionWrapper.m_henchmenInFocus = null;
				GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.Agent;

				foreach (AgentWrapper thisAW in GameManager.instance.currentMissionWrapper.m_agents) {

					GameManager.instance.currentMissionWrapper.m_agentInFocus = thisAW;

					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

						m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

					}
				}


				GameManager.instance.currentMissionWrapper.m_agentInFocus = null;
				GameManager.instance.currentMissionWrapper.m_scope = MissionBase.TargetType.OwnedAsset;

				if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.OwnedAsset) {

					foreach (Asset a in GameManager.instance.game.player.currentAssets) {

						GameManager.instance.currentMissionWrapper.m_assetInFocus = a;

						if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

							m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

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

						m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

					}
				}
			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Agent) {

				foreach (AgentWrapper thisAW in GameManager.instance.currentMissionWrapper.m_agents) {

					GameManager.instance.currentMissionWrapper.m_agentInFocus = thisAW;

					if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

						m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

					}
				}


			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.AssetToken || GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.EconomicControlToken || GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.MilitaryControlToken ||
				GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.PoliticalControlToken || GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.ControlToken || 
				GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.PolicyToken) {

				if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

					m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

				}
			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.BaseUpgrade) {

				if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

					m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

				}
			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Floor) {

				if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

					m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

				}

				foreach (Henchmen thisH in GameManager.instance.currentMissionWrapper.m_henchmen) {

					GameManager.instance.currentMissionWrapper.m_henchmenInFocus = thisH;

					if (m.m_targetType == MissionBase.TargetType.Henchmen && m.IsValid ()) {

						m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

					}
				}



				foreach (Region r in GameManager.instance.game.regions) {

					GameManager.instance.currentMissionWrapper.m_regionInFocus = r;

					if (m.m_targetType == MissionBase.TargetType.RemoteRegion && m.IsValid ()) {

						m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

					}
				}

			}

			if (GameManager.instance.currentMissionWrapper.m_scope == MissionBase.TargetType.Research) {

				if (m.m_targetType == GameManager.instance.currentMissionWrapper.m_scope && m.IsValid ()) {

					m_validMissions.Add ((MissionWrapper)GameManager.instance.currentMissionWrapper.Clone());

				}

			}


		}

		// spawn mission list view items

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		SectionHeader header = (SectionHeader) h.GetComponent<SectionHeader> ();
		m_listViewItems.Add (h);

		foreach (MissionWrapper m in m_validMissions) {

			GameObject g = (GameObject)(Instantiate (m_missionListViewItem, m_scrollViewContent.transform));
			g.transform.localScale = Vector3.one;
			m_listViewItems.Add (g);
			g.GetComponent<Mission_ListViewItem> ().Initialize (m);
			header.m_children.Add (g);
		}

		header.Initialize ("AVAILABLE MISSIONS");

		m_validMissions.Clear ();
	}
		
//	public override void SelectMission (MissionWrapper mw)
//	{
//		if (GameManager.instance.game.player.currentCommandPool >= mw.m_mission.m_cost) {
//			GameManager.instance.currentMissionWrapper = mw;
//			GameManager.instance.currentMissionWrapper.m_organization = GameManager.instance.game.player;
//			GameManager.instance.ProcessMissionWrapper ();
//			GameManager.instance.PopMenuState ();
//		}
//	}

	public override void SelectMission (MissionBase m)
	{

		if (GameManager.instance.game.player.currentCommandPool >= m.m_cost) {
			
			GameManager.instance.currentMissionWrapper.m_mission = m;
			GameManager.instance.currentMissionWrapper.m_scope = m.m_targetType;
			GameManager.instance.currentMissionWrapper.m_organization = GameManager.instance.game.player;

			if (m.m_targetType == MissionBase.TargetType.Floor) {

				GameManager.instance.ProcessMissionWrapper ();
				GameManager.instance.PopMenuState ();
			} else if (GameManager.instance.currentMissionWrapper.m_regionInFocus == null) {

				// select region

				GameManager.instance.PushMenuState (State.SelectRegionMenu);

			} else if (GameManager.instance.currentMissionWrapper.m_regionInFocus != null) {

				// start mission

				GameManager.instance.ProcessMissionWrapper ();
				GameManager.instance.PopMenuState ();
			}
		}
	}

	public List<MissionWrapper> validMissions {get{return m_validMissions;}}
}
