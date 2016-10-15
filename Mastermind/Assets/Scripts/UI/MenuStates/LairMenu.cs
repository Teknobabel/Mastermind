using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LairMenu : MenuState {

	public GameObject m_lairMenu;

	public Text m_lairText;

	public override void OnActivate(MenuTab tabInfo)
	{
		Debug.Log ("Starting Lair Menu");
//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
//		t.m_tabButton.ChangeState (TabButton.State.Selected);
		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_lairMenu.gameObject.SetActive (true);

		UpdateText ();
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

		m_lairMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	private void UpdateText ()
	{
		Organization player = GameManager.instance.game.player;

		string s = "ORG NAME: " + player.orgName;
		s += "\n INFAMY: " + player.currentInfamy.ToString();
		s += "\n WANTED LEVEL: " + player.currentWantedLevel.ToString();
		s += "\n INTEL: " + player.currentIntel.ToString () + " / " + player.maxIntel.ToString ();
		s += "\n UPGRADES:";
		s += "\n ASSETS:";

		foreach (Asset a in player.currentAssets) {
			s += "\n  - " + a.m_name.ToUpper();
		}

		m_lairText.text = s;
	}
}
