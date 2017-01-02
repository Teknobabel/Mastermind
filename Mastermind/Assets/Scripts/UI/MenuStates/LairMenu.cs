using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LairMenu : MenuState {
	public static LairMenu instance;

	public GameObject m_lairMenu;
//	public GameObject m_sortPanelParent;

	public BaseFloorButton[] m_floors;

	public TextMeshProUGUI m_lairText;

	public TokenButton[] m_assets;

	public ResearchButton[] m_researchButtons;

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
		Debug.Log ("Starting Lair Menu");

		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_lairMenu.gameObject.SetActive (true);
//		m_sortPanelParent.gameObject.SetActive (false);

		UpdateText ();
		UpdateBase ();
		UpdateAssets ();
		UpdateResearch ();
	}

	public override void OnHold()
	{

		m_lairMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		// continue going back up the stack if this is not the target menu

		if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState != m_state)
		{
			GameManager.instance.PopMenuState();
			return;
		} else if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState == m_state)
		{
			GameManager.instance.targetMenuState = MenuState.State.None;
		}

		m_lairMenu.gameObject.SetActive (true);
//		m_sortPanelParent.gameObject.SetActive (false);

		UpdateText ();
		UpdateBase ();
		UpdateAssets ();
		UpdateResearch ();
	}

	public override void OnDeactivate()
	{
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

		m_lairMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	private void UpdateResearch ()
	{

		foreach (ResearchButton rb in m_researchButtons) {

			rb.Initialize ();
		}
	}

	private void UpdateBase ()
	{
		Base b = GameManager.instance.game.player.orgBase;

		for (int i = 0; i < m_floors.Length; i++) {

			BaseFloorButton fb = m_floors [i];

			fb.gameObject.SetActive (true);

			if (i < b.m_floors.Count) {

				Base.Floor f = b.m_floors [i];
				m_floors [i].Initialize (f);

			} else {

				fb.gameObject.SetActive (false);

			}
		}

		if (b.m_lair != null && b.m_lair.m_floorNumber < m_floors.Length) {

			BaseFloorButton fb = m_floors [b.m_lair.m_floorNumber-1];
			fb.gameObject.SetActive (true);
			fb.Initialize (b.m_lair);

		}
	} 

	private void UpdateAssets ()
	{
		Organization player = GameManager.instance.game.player;

		for (int i = 0; i < LairMenu.instance.m_assets.Length; i++) {

			TokenButton tb = LairMenu.instance.m_assets [i];

			if (i < player.maxAssets) {

				if (i < player.currentAssets.Count) {
					Asset a = player.currentAssets [i];
					tb.gameObject.SetActive (true);
					tb.m_tokenText.gameObject.SetActive (true);
					tb.m_tokenText.text = a.m_name.ToUpper();
				} else {

					tb.gameObject.SetActive (true);
					tb.m_tokenText.gameObject.SetActive (false);
				}

			} else {

				tb.Deactivate ();
			}
		}
	}

	private void UpdateText ()
	{
		Organization player = GameManager.instance.game.player;

		string s = player.orgName;
//		s += "\n UPGRADES:";
//		s += "\n ASSETS:";
//
//		foreach (Asset a in player.currentAssets) {
//			s += "\n  - " + a.m_name.ToUpper();
//		}

		m_lairText.text = s;
	}

	public void SelectUpgradeForFloor (Base.Floor targetFloor)
	{
		MissionWrapper mr = new MissionWrapper ();
		mr.m_scope = MissionBase.TargetType.BaseUpgrade;
		mr.m_floorInFocus = targetFloor;
		mr.m_region = GameManager.instance.game.player.homeRegion;

		foreach (Henchmen h in mr.m_region.currentHenchmen) {
			mr.m_henchmen.Add (h);
		}

		GameManager.instance.currentMissionWrapper = mr;
		GameManager.instance.PushMenuState (State.SelectMissionMenu);
	}

	public void SelectMissionForFloor (Base.Floor targetFloor)
	{
		MissionWrapper mr = new MissionWrapper ();
		mr.m_scope = MissionBase.TargetType.Floor;
		mr.m_floorInFocus = targetFloor;
		mr.m_region = GameManager.instance.game.player.homeRegion;

		foreach (Henchmen h in mr.m_region.currentHenchmen) {
			mr.m_henchmen.Add (h);
		}

		GameManager.instance.currentMissionWrapper = mr;
		GameManager.instance.PushMenuState (State.SelectMissionMenu);
	}

	public void SelectMissionForResearchButton (ResearchButton r)
	{
		if (r.researchState == ResearchButton.ResearchState.Available) {

			MissionWrapper mr = new MissionWrapper ();
			mr.m_scope = MissionBase.TargetType.Research;
			mr.m_researchButtonInFocus = r;
			mr.m_region = GameManager.instance.game.player.homeRegion;

			foreach (Henchmen h in mr.m_region.currentHenchmen) {
				mr.m_henchmen.Add (h);
			}

			GameManager.instance.currentMissionWrapper = mr;
			GameManager.instance.PushMenuState (State.SelectResearchMenu);

		}
	}
}
