using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SectionHeader : MonoBehaviour {

	public Text m_sectionHeaderTitle;

	public void Initialize (string headerTitle)
	{
		m_sectionHeaderTitle.text = headerTitle;
	}
}
