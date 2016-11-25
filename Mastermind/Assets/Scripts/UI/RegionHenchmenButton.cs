using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RegionHenchmenButton : MonoBehaviour {

	public Image m_buttonImage;
	public TextMeshProUGUI m_text;

	public Region_ListViewItem m_parent;

	private int m_henchmenID = -1;
	private Region.HenchmenSlot m_henchmenSlot;

	public void Initialize (Region.HenchmenSlot hSlot)
	{
		m_henchmenSlot = hSlot;

		if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Player) {

			m_henchmenID = hSlot.m_henchmen.id;
			m_text.color = Color.white;
			m_text.text = hSlot.m_henchmen.henchmenName.ToUpper ();
			m_buttonImage.color = Color.white;
			m_buttonImage.sprite = hSlot.m_henchmen.portraitShort;

		} else if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Agent && (hSlot.m_agent.m_vizState == AgentWrapper.VisibilityState.Visible || 
			hSlot.m_agent.m_vizState == AgentWrapper.VisibilityState.Tracked )) {

			m_henchmenID = hSlot.m_agent.m_agent.id;
			m_text.color = Color.red;
			m_text.text = hSlot.m_agent.m_agent.henchmenName.ToUpper ();
			m_buttonImage.color = Color.white;
			m_buttonImage.sprite = hSlot.m_agent.m_agent.portraitShort;

		} else if (hSlot.m_state == Region.HenchmenSlot.State.Empty || (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Agent && hSlot.m_agent.m_vizState == AgentWrapper.VisibilityState.Hidden))
		{
			List<Henchmen> validHenchmen = new List<Henchmen> ();

			foreach (Henchmen h in hSlot.m_enRoute) {

				if (h.owner == Region.Owner.Player) {
					validHenchmen.Add (h);
				}
			}

			if (validHenchmen.Count > 0) {

				m_text.color = Color.grey;

				m_text.text = "( ";

				foreach (Henchmen h in validHenchmen) {

					m_text.text += h.henchmenName.ToUpper ();
				}

				m_text.text += " )";
			}

		}

//		if (hSlot.m_state == Region.HenchmenSlot.State.Reserved && hSlot.m_henchmen != null) {
//			m_henchmenID = hSlot.m_henchmen.id;
//			m_text.color = Color.grey;
//			m_text.text = "( " + hSlot.m_henchmen.henchmenName.ToUpper () + " )";
//		} else if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Player && hSlot.m_henchmen != null) {
//			
//			m_henchmenID = hSlot.m_henchmen.id;
//
//			m_text.color = Color.white;
//			m_text.text = hSlot.m_henchmen.henchmenName.ToUpper ();
//			m_buttonImage.color = Color.white;
//			m_buttonImage.sprite = hSlot.m_henchmen.portraitShort;
//
//
//		} else if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Agent && hSlot.m_agent != null) {
//
//			m_henchmenID = hSlot.m_agent.m_agent.id;
//
//			m_text.color = Color.red;
//			m_text.text = hSlot.m_agent.m_agent.henchmenName.ToUpper ();
//
//			if (hSlot.m_agent.m_vizState == AgentWrapper.VisibilityState.Hidden) {
//				m_buttonImage.color = Color.red;
//			} else {
//				m_buttonImage.color = Color.white;
//			}
//			m_buttonImage.sprite = hSlot.m_agent.m_agent.portraitShort;
//		}
	}

	public void Deactivate ()
	{
		gameObject.SetActive (false);
	}

	public void HenchmenButtonClicked ()
	{
		if (m_henchmenSlot.m_state != Region.HenchmenSlot.State.Occupied_Player) {
			m_parent.EmptyHenchmenButtonClicked (m_henchmenSlot);
		}
	}
}
