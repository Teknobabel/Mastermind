using UnityEngine;
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
	}

	public State m_state = State.None;

	public abstract void OnActivate(); // player enters menu
	public abstract void OnHold(); // another state is pushed on top, but player can return
	public abstract void OnReturn();
	public abstract void OnDeactivate(); // player leaves menu for previous state
	public abstract void OnUpdate();

	public State state {get{return m_state;}}
}
