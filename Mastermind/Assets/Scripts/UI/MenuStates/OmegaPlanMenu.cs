using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class OmegaPlanMenu : MenuState {
	public static OmegaPlanMenu instance;

	public GameObject m_opPanelParent;
//	public GameObject m_sortPanelParent;

	public TextMeshProUGUI 
		m_opName,
		m_lockedOPHelpText;

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
		Debug.Log ("Starting Omega Plan Menu");
//		MenuTab t = GameManager.instance.game.player.menuTabs [m_state];
//		t.m_tabButton.ChangeState (TabButton.State.Selected);
		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_opPanelParent.gameObject.SetActive (true);
//		m_sortPanelParent.gameObject.SetActive (false);

		if (m_tabInfo != null && m_tabInfo.objectID > -1) {
			
			OmegaPlan op = GameManager.instance.game.player.omegaPlansByID [m_tabInfo.objectID];

			if (op.state == OmegaPlan.State.Revealed) {
				
				m_opName.text = op.opName.ToUpper ();
				m_lockedOPHelpText.gameObject.SetActive (false);

				for (int i = 0; i < m_goalButtons.Length; i++) {
				
					OPGoalButton b = m_goalButtons [i];

					if (i < op.goals.Count) {
						OmegaPlan.Goal g = op.goals [i];
						b.Initialize (g);
					}
				}

			} else if (op.state == OmegaPlan.State.Hidden) {

				m_opName.text = "UNKNOWN OMEGA PLAN";
				m_lockedOPHelpText.gameObject.SetActive (true);

				for (int i = 0; i < m_goalButtons.Length; i++) {

					OPGoalButton b = m_goalButtons [i];

					b.Deactivate ();
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
