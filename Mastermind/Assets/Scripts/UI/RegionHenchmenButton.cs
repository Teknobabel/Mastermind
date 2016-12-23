using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RegionHenchmenButton : MonoBehaviour {

	public RawImage m_buttonImage;
	public TextMeshProUGUI m_text;

	public Region_ListViewItem m_parent;

	private int m_henchmenID = -1;
	private Region.HenchmenSlot m_henchmenSlot;

	public void Initialize (Region.HenchmenSlot hSlot)
	{
		m_henchmenSlot = hSlot;

		if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Player) {

			m_henchmenID = hSlot.m_henchmen.id;
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.RegionListView_HenchmenSlot_Text_OccupiedHenchmen);
			m_text.text = hSlot.m_henchmen.henchmenName.ToUpper ();
			m_buttonImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.RegionListView_HenchmenSlot_BG_OccupiedHenchmen);
			m_buttonImage.texture = hSlot.m_henchmen.portraitShort.texture;

		} else if (hSlot.m_state == Region.HenchmenSlot.State.Occupied_Agent && (hSlot.m_agent.m_vizState == AgentWrapper.VisibilityState.Visible || 
			hSlot.m_agent.m_vizState == AgentWrapper.VisibilityState.Tracked )) {

			m_henchmenID = hSlot.m_agent.m_agent.id;
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.RegionListView_HenchmenSlot_Text_OccupiedAgent);
			m_text.text = hSlot.m_agent.m_agent.henchmenName.ToUpper ();
			m_buttonImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.RegionListView_HenchmenSlot_BG_OccupiedAgent);
			m_buttonImage.texture = hSlot.m_agent.m_agent.portraitShort.texture;

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
			} else {

				m_text.text = "EMPTY";
			}

			m_buttonImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.RegionListView_HenchmenSlot_BG_Empty);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.RegionListView_HenchmenSlot_Text_Empty);
		}
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
