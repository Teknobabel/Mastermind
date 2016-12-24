using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TurnResults_ListViewItem : MonoBehaviour {

	public TextMeshProUGUI m_description;

	public RawImage m_icon;

	public void Initialize (TurnResultsEntry t, int positionInList)
	{
//		m_description.text = t.m_resultsText;

		// start text crawl for trait name
		TextCrawl tc = (TextCrawl) m_description.transform.GetComponent<TextCrawl>();
		if (tc != null) {

			float delay = 0.2f * ((float)(positionInList));
			tc.Initialize(t.m_resultsText.ToUpper(), delay);
		}

		switch (t.m_iconType) {

		case TurnResultsEntry.IconType.None:
		case TurnResultsEntry.IconType.Mission:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [0].texture;
			break;
		case TurnResultsEntry.IconType.OmegaPlan:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [2].texture;
			break;
		case TurnResultsEntry.IconType.Travel:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [1].texture;
			break;
		case TurnResultsEntry.IconType.Henchmen:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [3].texture;
			break;
		case TurnResultsEntry.IconType.Agent:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [4].texture;
			break;
		case TurnResultsEntry.IconType.World:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [6].texture;
			break;
		case TurnResultsEntry.IconType.WantedLevel:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [5].texture;
			break;
		case TurnResultsEntry.IconType.Organization:
			m_icon.texture = ActivityMenu.instance.m_turnResultsIcons [7].texture;
			break;
		}
	}
}
