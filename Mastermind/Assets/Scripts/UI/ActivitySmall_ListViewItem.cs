using UnityEngine;
using System.Collections;
using TMPro;

public class ActivitySmall_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_text;

	public void Initialize (TurnResultsEntry t)
	{
		string s = t.m_resultsText + " (TURN " + t.m_turnNumber.ToString () + ")";
		m_text.text = s;
	}
}
