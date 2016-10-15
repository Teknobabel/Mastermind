using UnityEngine;
using System.Collections;

public class MenuTab {

	public string m_name = "Null";
	public MenuState.State m_menuState = MenuState.State.None;
	public TabButton m_tabButton = null;

	private int m_tabID = -1;
	private int m_objectID = -1;

	public void Initialize ()
	{
		m_tabID = GameManager.instance.newID;
	}

	public int id {get{return m_tabID; }}
	public int objectID {get{return m_objectID; }set{m_objectID = value; }}
}
