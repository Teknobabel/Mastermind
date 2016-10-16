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
		m_costPerTurn;

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
			Organization o2 = (Organization)subject;
			m_currentCommandPoints.text = o2.currentCommandPool.ToString ();
			m_commandPoolHeader.text = "COMMAND POINTS: " + o2.commandPool.ToString ();
			m_costPerTurn.text = "-" + o2.costPerTurn.ToString () + " / TURN";
			break;
		}
	}

	public void ExecuteTurnButtonClicked ()
	{
		GameManager.instance.PushMenuState(State.AlertView_ExecuteTurn);
		GameManager.instance.ChangeGameState (GameManager.instance.missionPhase);

	}
}
