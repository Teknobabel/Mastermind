using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class OmegaPlanMenu : MenuState {
	public static OmegaPlanMenu instance;

	public GameObject m_opPanelParent;
	public GameObject m_sortPanelParent;

	public TextMeshProUGUI m_opName;

	public OPGoalButton[] m_goalButtons;

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
		Debug.Log ("Starting Lair Menu");
//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
//		t.m_tabButton.ChangeState (TabButton.State.Selected);
		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_opPanelParent.gameObject.SetActive (true);
		m_sortPanelParent.gameObject.SetActive (false);

		if (m_tabInfo != null && m_tabInfo.objectID > -1) {
			OmegaPlan op = GameManager.instance.game.player.omegaPlansByID [m_tabInfo.objectID];
			m_opName.text = op.opName.ToUpper();

			for (int i=0; i < m_goalButtons.Length; i++){
				
				OPGoalButton b = m_goalButtons [i];

				if (i < op.goals.Length) {
					OPGoal g = op.goals [i];
					b.Initialize (g);
				}

			}
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
		m_opPanelParent.gameObject.SetActive (false);


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
