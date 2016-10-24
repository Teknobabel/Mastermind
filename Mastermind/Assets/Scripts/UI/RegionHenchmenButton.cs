using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class RegionHenchmenButton : MonoBehaviour {

	public Image m_buttonImage;
	public TextMeshProUGUI m_text;

	public Region_ListViewItem m_parent;

	private int m_henchmenID = -1;

	public void Initialize (Henchmen h)
	{
		if (h != null) {
			m_henchmenID = h.id;
			m_text.text = h.henchmenName.ToUpper ();
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
