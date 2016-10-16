using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class HenchmenForHire_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_henchmenName;
	public TextMeshProUGUI m_hireCost;
	public TextMeshProUGUI m_costPerTurn;
	public Image m_henchmenPortrait;

	public TraitButton[] m_traits;

	private int m_henchmenID = -1;
	private int m_cost = 99;

	public void Initialize (Henchmen h)
	{
		m_henchmenID = h.id;
		m_cost = h.hireCost;
		m_henchmenName.text = h.henchmenName.ToUpper();
		m_hireCost.text = h.hireCost.ToString();
		m_costPerTurn.text = h.costPerTurn.ToString() + " CP / TURN";
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

	public void HireButtonClicked ()
	{
		if (m_henchmenID > -1 && GameManager.instance.game.player.currentCommandPool >= m_cost ) {
			GameManager.instance.game.player.HireHenchmen (m_henchmenID);
		}
	}

	public void DismissButtonClicked ()
	{

	}
}
