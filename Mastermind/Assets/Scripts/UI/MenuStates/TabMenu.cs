using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TabMenu : MenuState, IObserver {
	public static TabMenu instance;

	public TabButton[] m_tabButtons;

	public TextMeshProUGUI
		m_currentCommandPoints,
		m_commandPoolHeader,
		m_costPerTurn,
		m_infamy,
		m_wantedLevel,
		m_intel,
		m_turn;

	public GameObject
		m_activityPane,
		m_activityPaneScrollViewContent,
		m_activityCell_Small;

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

		for (int i=0; i < m_tabButtons.Length; i++)
		{
			TabButton b = m_tabButtons [i];

			if (i < tabList.Count) {
				MenuTab m = tabList [i];
				b.Initialize (m);

			} else {
				b.Deactivate ();
			}
		}

		if (tabList.Count > 0) {
			MenuTab firstTab = tabList [0];
			GameManager.instance.PushMenuState(firstTab);
		}

		GameManager.instance.game.AddObserver (this);
	}

	public override void OnHold()
	{
	}

	public override void OnReturn()
	{
	}

	public override void OnDeactivate()
	{
	}

	public override void OnUpdate()
	{
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.Organization_CommandPoolChanged:
		case GameEvent.Organization_HenchmenHired:
		case GameEvent.Organization_Initialized:
			Organization player = (Organization)subject;
			m_currentCommandPoints.text = player.currentCommandPool.ToString ();
			m_commandPoolHeader.text = "COMMAND POOL: " + player.commandPool.ToString ();
			m_costPerTurn.text = "-" + player.costPerTurn.ToString () + " / TURN";
			m_infamy.text = player.currentInfamy.ToString () + "<size=24>/" + player.maxInfamy.ToString() + "</size>";
			m_intel.text = player.currentIntel.ToString () + "<size=24>/" + player.maxIntel.ToString () + "</size>";
			m_wantedLevel.text = player.currentWantedLevel.ToString () + "<size=24>/" + player.maxWantedLevel.ToString () + "</size>";
			break;
		case GameEvent.GameState_TurnNumberChanged:
			m_turn.text = GameManager.instance.game.turnNumber.ToString();
			break;
		case GameEvent.Organization_InfamyChanged:
			m_infamy.text = ((Organization)subject).currentInfamy.ToString () + "<size=24>/" + ((Organization)subject).maxInfamy.ToString() + "</size>";
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
