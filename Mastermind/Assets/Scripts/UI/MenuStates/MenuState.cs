﻿using UnityEngine;
using System.Collections;

public abstract class MenuState : MonoBehaviour {
	
	public enum State
	{
		None,
		MainMenu,
		HenchmenMenu,
		LairMenu,
		WorldMenu,
		OmegaPlanMenu,
		TabMenu,
		ActivityMenu,
		AlertView_ExecuteTurn,
		PauseMenu,
		HenchmenDetailMenu, 
		SelectHenchmenMenu,
		SelectMissionMenu,
		AgentsMenu,
		DatabaseMenu,
		SelectResearchMenu,
		MissionMenu,
		SelectRegionMenu,
	}

	public State m_state = State.None;
	protected MenuTab m_tabInfo;

	public abstract void OnActivate(MenuTab tabInfo); // player enters menu
	public abstract void OnHold(); // another state is pushed on top, but player can return
	public abstract void OnDeactivate(); // player leaves menu for previous state
	public abstract void OnUpdate();

	public virtual void DisplayActionPane (){
	}

	public virtual void HideActionPane (){
	}

	public virtual void BackButtonPressed (){
	}

//	public virtual void SelectMission (MissionWrapper mw) {
//	}

	public virtual void SelectMission (MissionBase m) {
	}

	public virtual void OnReturn()
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
	}

	public State state {get{return m_state;}}
	public MenuTab tabInfo {get{return m_tabInfo; } set{m_tabInfo = value; }}
}
