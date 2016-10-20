using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Henchmen_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_henchmenName;
	public TextMeshProUGUI m_currentMission;
	public TextMeshProUGUI m_currentLocation;
	public TextMeshProUGUI m_turnCost;
	public Image m_henchmenPortrait;

	public TraitButton[] m_traits;

	private int m_henchmenID = -1;

	public void Initialize (Henchmen h)
	{
		m_henchmenID = h.id;
		m_henchmenName.text = h.henchmenName.ToUpper();

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

		m_turnCost.text = h.costPerTurn.ToString() + "CP / TURN";
		m_henchmenPortrait.sprite = h.portrait;

		List<TraitData> traits = h.GetAllTraits ();

		for (int i = 0; i < m_traits.Length; i++) {

			TraitButton tb = m_traits [i];

			if (i < traits.Count) {
				TraitData td = traits [i];
				tb.Initialize (td, true);
			} else {
				tb.Deactivate ();
			}
		}
	}

	public void CallButtonClicked ()
	{
		CallHenchmenMenu.instance.henchmenID = m_henchmenID;
		GameManager.instance.PushMenuState (MenuState.State.CallHenchmenMenu);
	}
}
