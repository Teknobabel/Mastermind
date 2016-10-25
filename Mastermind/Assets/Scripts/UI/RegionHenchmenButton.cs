using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class RegionHenchmenButton : MonoBehaviour {

	public Image m_buttonImage;
	public TextMeshProUGUI m_text;

	public Region_ListViewItem m_parent;

	private int m_henchmenID = -1;
	private int m_henchmenSlotID = -1;

	public void Initialize (Region.HenchmenSlot hSlot)
	{
		m_henchmenSlotID = hSlot.m_id;

		if (hSlot.m_state == Region.HenchmenSlot.State.Reserved && hSlot.m_henchmen != null) {
			m_henchmenID = hSlot.m_henchmen.id;
			m_text.color = Color.grey;
			m_text.text = "( " + hSlot.m_henchmen.henchmenName.ToUpper () + " )";
		}
		else if (hSlot.m_state == Region.HenchmenSlot.State.Occupied && hSlot.m_henchmen != null) {
			m_henchmenID = hSlot.m_henchmen.id;
			m_text.text = hSlot.m_henchmen.henchmenName.ToUpper ();
		}
	}

	public void Deactivate ()
	{
		gameObject.SetActive (false);
	}

	public void HenchmenButtonClicked ()
	{
		if (m_henchmenID == -1) {
			m_parent.EmptyHenchmenButtonClicked ();
		}
	}
}
