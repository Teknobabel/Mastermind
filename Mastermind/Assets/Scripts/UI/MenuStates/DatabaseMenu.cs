using UnityEngine;
using System.Collections;

public class DatabaseMenu : MenuState {
	public static DatabaseMenu instance;

	public GameObject m_databaseMenuParent;
	public GameObject m_scrollViewContent;

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
		m_databaseMenuParent.gameObject.SetActive (true);

		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}
	}
	public override void OnHold()
	{
		m_databaseMenuParent.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		m_databaseMenuParent.gameObject.SetActive (true);
	}

	public override void OnDeactivate()
	{
		m_databaseMenuParent.gameObject.SetActive (false);

		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}
	}
	public override void OnUpdate()
	{

	}


}
