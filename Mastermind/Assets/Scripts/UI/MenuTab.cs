using UnityEngine;
using System.Collections;

public class MenuTab: IObserver {

	public string m_name = "Null";
	public MenuState.State m_menuState = MenuState.State.None;
	public TabButton m_tabButton = null;

	private int m_tabID = -1;
	private int m_objectID = -1;
	private bool m_drawTab = true;

	public void Initialize ()
	{
		m_tabID = GameManager.instance.newID;
	}

	public string GetTabName ()
	{
		string s = m_name.ToUpper ();

		if (m_menuState == MenuState.State.OmegaPlanMenu && m_objectID != -1) {
			OmegaPlan op = GameManager.instance.game.player.omegaPlansByID [objectID];
			s = "<size=18>" + m_name.ToUpper () + ":</size>";

			if (op.state == OmegaPlan.State.Hidden) {
				s += "\nUNKNOWN";
//				op.AddObserver (this);
			} else {
				s += "\n" + op.opNameShort.ToUpper ();
			}
		}

		return s;
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.Organization_OmegaPlanRevealed:
			
			Debug.Log ("OMEGA PLAN REVEALED");

			if (!m_drawTab) {

				OmegaPlan op = GameManager.instance.game.player.omegaPlansByID [objectID];
				op.RemoveObserver (this);

				m_drawTab = true;
				TabMenu.instance.UpdateTabs ();
			}

//			OmegaPlan op = (OmegaPlan)subject;
//			m_tabButton.Initialize(this);
			break;
		}
	}

	public int id {get{return m_tabID; }}
	public int objectID {get{return m_objectID; }set{m_objectID = value; }}
	public bool drawTab {get{ return m_drawTab; } set{ m_drawTab = value; }}
}
