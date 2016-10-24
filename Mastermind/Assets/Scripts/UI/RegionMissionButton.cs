using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class RegionMissionButton : MonoBehaviour {

	public Image m_buttonImage;
	public TextMeshProUGUI m_text;

	public void Initialize (MissionBase m)
	{
		if (m != null) {
			m_text.text = m.m_name.ToUpper ();
		} else {
			m_text.text = "NEW MISSION";
		}
	}
}
