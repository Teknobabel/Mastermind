using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class OmegaPlanMenu : MenuState {
	public static OmegaPlanMenu instance;

	public GameObject m_opPanelParent;

	public TextMeshProUGUI 
		m_opName,
		m_lockedOPHelpText;

	public Transform m_omegaPlanGoalPanel;

	public GameObject m_omegaPlanGoal;

	private List<GameObject> m_listViewItems = new List<GameObject> ();

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

		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_opPanelParent.gameObject.SetActive (true);

		if (m_tabInfo != null && m_tabInfo.objectID > -1) {
			
			OmegaPlan op = GameManager.instance.game.player.omegaPlansByID [m_tabInfo.objectID];

			m_opName.text = op.opName.ToUpper ();
//				m_lockedOPHelpText.gameObject.SetActive (false);



			for (int i = 0; i < op.goals.Count; i++) {

				OmegaPlan.Goal g = op.goals [i];

				GameObject thisG = (GameObject)(Instantiate (m_omegaPlanGoal, m_omegaPlanGoalPanel));
				thisG.transform.localScale = Vector3.one;
				m_listViewItems.Add (thisG);
				OPGoalButton opg = (OPGoalButton)thisG.GetComponent<OPGoalButton> ();
				opg.Initialize (g, i);
			}
		}
	}

	public override void OnHold()
	{
		while (m_listViewItems.Count > 0) {

			GameObject go = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (go);
		}
	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {

			GameObject go = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (go);
		}

		m_opPanelParent.gameObject.SetActive (false);

		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}
	}

	public override void OnUpdate()
	{

	}
}
