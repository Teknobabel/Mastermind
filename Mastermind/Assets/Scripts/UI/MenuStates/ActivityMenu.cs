using UnityEngine;
using System.Collections;

public class ActivityMenu : MenuState {

	public override void OnActivate(MenuTab tabInfo)
	{
		Debug.Log ("Starting Activity Menu");
		//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		//		t.m_tabButton.ChangeState (TabButton.State.Selected);
		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
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
		//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
		//		t.m_tabButton.ChangeState (TabButton.State.Unselected);
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

	}

	public override void OnUpdate()
	{

	}
}
