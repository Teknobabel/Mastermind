using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Henchmen_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_henchmenName;
	public TextMeshProUGUI m_currentMission;
	public TextMeshProUGUI m_currentLocation;
	public TextMeshProUGUI m_buttonText;
	public TextMeshProUGUI m_hireCost;
	public TextMeshProUGUI m_costPerTurn;
	public RawImage m_henchmenPortrait;

	public Button 
		m_callButton,
		m_fireButton,
		m_dismissButton,
		m_hireButton;

	public Transform m_traitPanel;
	public GameObject m_traitButton;

	private List<TraitButton> m_traitButtons = new List<TraitButton>();
	private TraitButton m_statusTrait;
	private int m_henchmenID = -1;
	private int m_cost = 99;

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

		for (int i = 0; i < 9; i++) {

			GameObject thisT = (GameObject)(Instantiate (m_traitButton, m_traitPanel));
			thisT.transform.localScale = Vector3.one;
			TraitButton tb = (TraitButton)thisT.GetComponent<TraitButton> ();

			if (i < 8) {

				m_traitButtons.Add (tb);

				if (i < traits.Count) {
					TraitData td = traits [i];
					tb.Initialize (td, true, i);
				} else {
					tb.Deactivate ();
				}

			} else {

				m_statusTrait = tb;
				m_statusTrait.Initialize (h.statusTrait, true);
			}
		}

		m_statusTrait.Initialize (h.statusTrait, true);
	}

	public void CallButtonClicked ()
	{
		if (GameManager.instance.currentMenuState == MenuState.State.SelectHenchmenMenu && m_henchmenID != -1) {
			SelectHenchmenMenu.instance.SelectHenchmen (m_henchmenID);
		
		} else if (GameManager.instance.currentMenuState == MenuState.State.HenchmenDetailMenu) {

			GameManager.instance.game.player.FireHenchmen (m_henchmenID);
			GameManager.instance.PopMenuState ();
			
		} else {
			HenchmenDetailMenu.instance.henchmenID = m_henchmenID;
			GameManager.instance.PushMenuState (MenuState.State.HenchmenDetailMenu);
		}
	}

	public void FireButtonClicked ()
	{
		if (m_henchmenID > -1) {
			GameManager.instance.game.player.FireHenchmen (m_henchmenID);
		}
	}

	public void HireButtonClicked ()
	{
		
		if (m_henchmenID > -1 && GameManager.instance.game.player.currentCommandPool >= m_cost ) {
			GameManager.instance.game.player.HireHenchmen (m_henchmenID);
		}
	}

	public void DismissButtonClicked ()
	{
		if (m_henchmenID > -1) {
			GameManager.instance.game.player.DismissHenchmen (m_henchmenID);
		}
	}

	public void InitializeHenchmen (Henchmen h)
	{
		DisplayMenu(h);

		m_costPerTurn.gameObject.SetActive (false);
		m_hireCost.gameObject.SetActive (false);
		m_hireButton.gameObject.SetActive (false);
		m_dismissButton.gameObject.SetActive (false);

		string mission = "MISSION:\n";
		if (h.currentState == Henchmen.state.OnMission) {
			MissionBase m = GameManager.instance.game.player.GetMission (h).m_mission;
			mission += m.m_name.ToUpper ();
		} else {
			mission += "NONE";
		}
		m_currentMission.text = mission;

		string location = "REGION:\n";
		location += h.currentRegion.regionName.ToUpper ();
		m_currentLocation.text = location;
	}

	public void InitializeHenchmenForHire (Henchmen h)
	{
		DisplayMenu(h);

		m_cost = h.hireCost;

		m_currentMission.gameObject.SetActive (false);
		m_currentLocation.gameObject.SetActive (false);
		m_fireButton.gameObject.SetActive (false);
		m_callButton.gameObject.SetActive (false);

		m_hireCost.text = h.hireCost.ToString() + "<size=36>CP</size>";
		m_costPerTurn.text = h.costPerTurn.ToString() + " CP / TURN";
	}

	public void InitializeAgent (AgentWrapper a)
	{
		DisplayMenu(a.m_agent);

		m_currentMission.gameObject.SetActive (false);
		m_fireButton.gameObject.SetActive (false);
		m_dismissButton.gameObject.SetActive (false);
		m_callButton.gameObject.SetActive (false);
		m_hireButton.gameObject.SetActive (false);

		string location = "REGION:\n";
		if (a.m_vizState == AgentWrapper.VisibilityState.Hidden) {
			location += "UNKNOWN";
		} else {
			location += a.m_agent.currentRegion.regionName.ToUpper ();
		}

		m_currentLocation.text = location;
	}
}
