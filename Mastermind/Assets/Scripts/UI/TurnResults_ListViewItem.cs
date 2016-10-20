using UnityEngine;
using System.Collections;
using TMPro;

public class TurnResults_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_description;

	public void Initialize (TurnResultsEntry t)
	{
		m_description.text = t.m_resultsText;
	}
}
