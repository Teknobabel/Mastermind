using UnityEngine;
using System.Collections;

public class LairMenu : MenuState {

	public override void OnActivate()
	{
		Debug.Log ("Starting Lair Menu");
		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		t.m_tabButton.ChangeState (TabButton.State.Selected);
	}

	public override void OnHold()
	{
	}

	public override void OnReturn()
	{
	}

	public override void OnDeactivate()
	{
		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		t.m_tabButton.ChangeState (TabButton.State.Unselected);
	}

	public override void OnUpdate()
	{

	}
}
