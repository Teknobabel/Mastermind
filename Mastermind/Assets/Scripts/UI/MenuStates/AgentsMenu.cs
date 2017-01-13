using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentsMenu : MenuState {

	public GameObject m_agentsMenu;
	public GameObject m_scrollViewContent;

	public GameObject m_sectionHeader;
	public GameObject m_agentListViewItem;

	private List<GameObject> m_listViewItems = new List<GameObject> ();

	public override void OnActivate(MenuTab tabInfo)
	{
		m_tabInfo = tabInfo;
		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Selected);
		}

		m_agentsMenu.SetActive (true);

		UpdateAgentList ();
	}

	public override void OnDeactivate()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		if (m_tabInfo != null) {
			m_tabInfo.m_tabButton.ChangeState (TabButton.State.Unselected);
			m_tabInfo = null;
		}

		m_agentsMenu.SetActive (false);
	}

	public override void OnHold()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}
	}

	public override void OnReturn()
	{
		base.OnReturn ();
	}

	public override void OnUpdate()
	{
	}

	private void UpdateAgentList ()
	{
		while (m_listViewItems.Count > 0) {
			GameObject g = m_listViewItems [0];
			m_listViewItems.RemoveAt (0);
			Destroy (g);
		}

		AgentOrganization agentOrg = GameManager.instance.game.agentOrganization;

		GameObject h = (GameObject)(Instantiate (m_sectionHeader, m_scrollViewContent.transform));
		h.transform.localScale = Vector3.one;
		h.GetComponent<SectionHeader> ().Initialize ("AGENTS");
		m_listViewItems.Add (h);

		if (agentOrg.currentAgents.Count > 0) {
			
			for (int i = 0; i < agentOrg.currentAgents.Count; i++) {

				if (agentOrg.currentAgents [i].m_hasBeenRevealed)
				{
					
					AgentWrapper thisAgent = agentOrg.currentAgents [i];
					GameObject g = (GameObject)(Instantiate (m_agentListViewItem, m_scrollViewContent.transform));
					g.transform.localScale = Vector3.one;
					m_listViewItems.Add (g);
					g.GetComponent<Henchmen_ListViewItem> ().InitializeAgent (thisAgent);
				}
			}
		}
	}
}
