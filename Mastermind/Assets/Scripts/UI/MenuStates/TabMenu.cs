using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TabMenu : MenuState, IObserver {
	public static TabMenu instance;

//	public TabButton[] m_tabButtons;

	public GameObject m_tabButton;

	public GameObject m_tabButtonScrollViewContent;

	public TextMeshProUGUI
		m_currentCommandPoints,
		m_commandPoolHeader,
		m_costPerTurn,
		m_infamy,
		m_wantedLevel,
		m_intel,
		m_turn;

	public RawImage[]
		m_IntelInPlaySprites;

	public Texture
		m_intelEmpty,
		m_intelFull;

	public GameObject
		m_activityPane,
		m_activityPaneScrollViewContent,
		m_activityCell_Small,
		m_infamyMeterPip;

	public Transform
		m_infamyMeterScrollViewContent;

	public List<RawImage> m_infamyMeter = new List<RawImage> ();

	private List<TabButton> m_tabButtonList = new List<TabButton> ();

	void Awake ()
	{
		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public override void OnActivate(MenuTab tabInfo)
	{
		
		// set up tabs

		Dictionary<int, MenuTab> tabDict = GameManager.instance.game.player.menuTabs;
		List<MenuTab> tabList = new List<MenuTab> ();
		foreach (KeyValuePair<int, MenuTab> pair in tabDict) {
			tabList.Add (pair.Value);
		}

		for (int i=0; i < tabList.Count; i++)
		{

			MenuTab m = tabList [i];

			bool drawTab = true;

			if (m.m_menuState == MenuState.State.OmegaPlanMenu && m.objectID != -1) { // don't draw tabs for hidden omega plans

				OmegaPlan op = GameManager.instance.game.player.omegaPlansByID [m.objectID];

				if (op.state == OmegaPlan.State.Hidden) {

					drawTab = false;
				}
			} else if (m.m_menuState == MenuState.State.AgentsMenu) { // don't draw agent tab if no visible agents in world

				drawTab = false;

				foreach (AgentWrapper aw in GameManager.instance.game.agentOrganization.currentAgents) {

					if (aw.m_vizState == AgentWrapper.VisibilityState.Visible) {

						drawTab = true;
						break;
					}
				}

				// if no visible henchmen, need to listen to agent organization for first appearence of henchmen to show tab

				if (!drawTab) {

					GameManager.instance.game.agentOrganization.AddObserver (this);
				}
			}

			if (drawTab) {
				
				GameObject g = (GameObject)(Instantiate (m_tabButton, m_tabButtonScrollViewContent.transform));
				g.transform.localScale = Vector3.one;
				TabButton tb = g.GetComponent<TabButton> ();
				m_tabButtonList.Add (tb);

				tb.Initialize (m);
				tb.ChangeState (TabButton.State.Unselected);
			}

		}

		// set up infamy meter

		for (int i = 0; i < 50; i++) {

			GameObject g = (GameObject)(Instantiate (m_infamyMeterPip, m_infamyMeterScrollViewContent));
			g.transform.localScale = Vector3.one;
			m_infamyMeter.Add ((RawImage)g.GetComponent<RawImage> ());
		}

		GameManager.instance.game.AddObserver (this);

//		// activate henchmen tab
//
//		if (tabList.Count > 0) {
//			MenuTab firstTab = tabList [0];
//			GameManager.instance.PushMenuState(firstTab);
//		}
	}

	public override void OnHold()
	{
	}

	public override void OnReturn()
	{
		// pop this state if it is not the target state (continue back through the stack)

		if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState != m_state)
		{
			GameManager.instance.PopMenuState();
			return;
		} else if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState == m_state)
		{
			GameManager.instance.targetMenuState = MenuState.State.None;
		}
	}

	public override void OnDeactivate()
	{
	}

	public override void OnUpdate()
	{
	}

	private void UpdateInfamyMeter ()
	{
		Organization player = GameManager.instance.game.player;

		for (int i = 0; i < m_infamyMeter.Count; i++) {

			RawImage image = m_infamyMeter [i];

			if (i <= player.currentInfamy/2) {

				image.color = ColorManager.instance.GetColor (ColorManager.UIElement.InfamyMeterPip_Full);
			} else {
				image.color = ColorManager.instance.GetColor (ColorManager.UIElement.InfamyMeterPip_Empty);
			}
		}
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.Organization_CommandPoolChanged:
		case GameEvent.Organization_HenchmenHired:
		case GameEvent.Organization_HenchmenFired:
		case GameEvent.Organization_Initialized:
			Organization player = (Organization)subject;

			// update command points
//			m_currentCommandPoints.text = player.currentCommandPool.ToString ();
			NumberCrawl nc = (NumberCrawl)m_currentCommandPoints.transform.GetComponent<NumberCrawl> ();
			if (nc != null) {

				nc.Initialize (player.currentCommandPool);
			}

			m_commandPoolHeader.text = "COMMAND POOL: " + player.commandPool.ToString ();
			m_costPerTurn.text = "-" + player.costPerTurn.ToString () + " / TURN";
			UpdateInfamyMeter ();
//			m_infamy.text = player.currentInfamy.ToString () + "<size=24>/" + player.maxInfamy.ToString() + "</size>";
			m_intel.text = player.currentIntel.ToString () + "<size=24>/" + player.maxIntel.ToString () + "</size>";
			m_wantedLevel.text = player.currentWantedLevel.ToString () + "<size=24>/" + player.maxWantedLevel.ToString () + "</size>";
			break;
		case GameEvent.GameState_TurnNumberChanged:
			m_turn.text = GameManager.instance.game.turnNumber.ToString();
			break;
		case GameEvent.Organization_InfamyChanged:
			UpdateInfamyMeter ();
//			m_infamy.text = ((Organization)subject).currentInfamy.ToString () + "<size=24>/" + ((Organization)subject).maxInfamy.ToString() + "</size>";
			break;
		case GameEvent.Organization_IntelSpawned:
		case GameEvent.Organization_IntelCaptured:

			m_intel.text = GameManager.instance.game.player.currentIntel.ToString () + "<size=24>/" + GameManager.instance.game.player.maxIntel.ToString () + "</size>";

			for (int i=0; i < m_IntelInPlaySprites.Length; i++)
			{
				if (i < GameManager.instance.game.intelInPlay.Count) {
					
					m_IntelInPlaySprites [i].texture = m_intelFull;

				} else {
					
					m_IntelInPlaySprites [i].texture = m_intelEmpty;
				}
			}
			break;
		case GameEvent.Agent_BecameVisible:

			// enable the Agents menu when the first agent becomes visible

			Dictionary<int, MenuTab> tabDict = GameManager.instance.game.player.menuTabs;
			List<MenuTab> tabList = new List<MenuTab> ();
			foreach (KeyValuePair<int, MenuTab> pair in tabDict) {
				tabList.Add (pair.Value);
			}

			for (int i = 0; i < tabList.Count; i++) {

				MenuTab m = tabList [i];

				if (m.m_menuState == MenuState.State.AgentsMenu) {

					GameObject g = (GameObject)(Instantiate (m_tabButton, m_tabButtonScrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					TabButton tb = g.GetComponent<TabButton> ();
					m_tabButtonList.Add (tb);

					tb.Initialize (m);

					break;
				}
			}


			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = "AGENTS Menu has beeen added to the Viewscreen.";
			t.m_resultType = GameEvent.Agent_BecameVisible;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

			GameManager.instance.game.agentOrganization.RemoveObserver (this);

			break;
		}
	}

	public void ExecuteTurnButtonClicked ()
	{
		GameManager.instance.PushMenuState(State.AlertView_ExecuteTurn);
//		GameManager.instance.ChangeGameState (GameManager.instance.missionPhase);

	}

	public void PauseButtonClicked ()
	{
		GameManager.instance.PushMenuState(State.PauseMenu);
	}

	public override void BackButtonPressed ()
	{
		if (GameManager.instance.currentMenu != null) {
			GameManager.instance.currentMenu.BackButtonPressed ();
		}
	}

	public void EnableActivityPane ()
	{
		if (GameManager.instance.currentMenu != null) {
			GameManager.instance.currentMenu.DisplayActionPane ();

			// gather related results
			Organization player = GameManager.instance.game.player;

			List<TurnResultsEntry> results = new List<TurnResultsEntry>();

			if (player.turnResultsByType.ContainsKey (GameEvent.Organization_HenchmenHired)) {
				results.AddRange (player.turnResultsByType [GameEvent.Organization_HenchmenHired]);
			}

			foreach (TurnResultsEntry t in results) {

			}
		}
	}

	public void DisableActivityPane ()
	{
		if (GameManager.instance.currentMenu != null) {
			GameManager.instance.currentMenu.HideActionPane ();
		}
	}
}
