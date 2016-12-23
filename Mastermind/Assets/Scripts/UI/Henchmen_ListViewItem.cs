using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Henchmen_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_henchmenName;
	public TextMeshProUGUI m_currentMission;
	public TextMeshProUGUI m_currentLocation;
//	public TextMeshProUGUI m_turnCost;
	public RawImage m_henchmenPortrait;

	public TraitButton[] m_traits;
	public TraitButton m_statusTrait;

	private int m_henchmenID = -1;

	public void Initialize (AgentWrapper a)
	{
		DisplayMenu(a.m_agent);

		m_currentMission.gameObject.SetActive (false);

		string location = "REGION:\n";
		if (a.m_vizState == AgentWrapper.VisibilityState.Hidden) {
			location += "UNKNOWN";
		} else {
			location += a.m_agent.currentRegion.regionName.ToUpper ();
		}
		m_currentLocation.text = location;
	}

	public void Initialize (Henchmen h)
	{
		DisplayMenu (h);

		string mission = "MISSION:\n";
		if (h.currentState == Henchmen.state.OnMission) {
			MissionBase m = GameManager.instance.game.player.GetMission (h);
			mission += m.m_name.ToUpper ();
		} else {
			mission += "NONE";
		}
		m_currentMission.text = mission;

		string location = "REGION:\n";
		location += h.currentRegion.regionName.ToUpper ();
		m_currentLocation.text = location;
	}

	private void DisplayMenu ( Henchmen h)
	{
		m_henchmenID = h.id;

		// start text crawl for henchmen name
		TextCrawl tc = (TextCrawl) m_henchmenName.transform.GetComponent<TextCrawl>();
		if (tc != null) {

			tc.Initialize(h.henchmenName.ToUpper());
		}

		//		m_turnCost.text = h.costPerTurn.ToString() + "CP / TURN";
		m_henchmenPortrait.texture = h.portrait.texture;

		List<TraitData> traits = h.GetAllTraits ();

		for (int i = 0; i < m_traits.Length; i++) {

			TraitButton tb = m_traits [i];

			if (i < traits.Count) {
				TraitData td = traits [i];
				tb.Initialize (td, true, i);
			} else {
				tb.Deactivate ();
			}
		}

		m_statusTrait.Initialize (h.statusTrait, true);
	}

	public void CallButtonClicked ()
	{
		if (GameManager.instance.currentMenuState == MenuState.State.SelectHenchmenMenu && m_henchmenID != -1) {
			SelectHenchmenMenu.instance.SelectHenchmen (m_henchmenID);
		} else {
			CallHenchmenMenu.instance.henchmenID = m_henchmenID;
			GameManager.instance.PushMenuState (MenuState.State.CallHenchmenMenu);
		}
	}

	public void FireButtonClicked ()
	{
		if (m_henchmenID > -1) {
			GameManager.instance.game.player.FireHenchmen (m_henchmenID);
		}
	}
}
