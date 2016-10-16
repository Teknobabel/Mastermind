using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SectionHeader : MonoBehaviour {

	public TextMeshProUGUI m_sectionHeaderTitle;

	public void Initialize (string headerTitle)
	{
		m_sectionHeaderTitle.text = headerTitle;
	}
}
