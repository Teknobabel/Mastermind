using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Henchmen_ListViewItem : MonoBehaviour {

	public Text m_henchmenName;
	public Text m_currentMission;
	public Text m_currentLocation;
	public Text m_turnCost;
	public Image m_henchmenPortrait;

	public TraitButton[] m_traits;

	private int m_henchmenID = -1;

	public void Initialize (Henchmen h)
	{
		m_henchmenID = h.id;
		m_henchmenName.text = h.henchmenName.ToUpper();
//		m_currentMission.text = 
//		m_currentLocation.text = 
		m_turnCost.text = h.costPerTurn.ToString() + "CP / TURN";
		m_henchmenPortrait.sprite = h.portrait;

		List<TraitData> traits = h.GetAllTraits ();

		for (int i = 0; i < m_traits.Length; i++) {

			TraitButton tb = m_traits [i];

			if (i < traits.Count) {
				TraitData td = traits [i];
				tb.Initialize (td);
			} else {
				tb.Deactivate ();
			}
		}
	}

	public void CallButtonClicked ()
	{

	}
}
