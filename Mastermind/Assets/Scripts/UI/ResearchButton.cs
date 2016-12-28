using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ResearchButton : MonoBehaviour {

	public enum ResearchState
	{
		None,
		Owned,
		Available,
		Unavailable,
	}

	public Asset 
	m_researchGranted,
	m_prerequisiteResearch;

	public RawImage m_buttonBG;
	public RawImage m_typeIndicator;
	public TextMeshProUGUI m_text;

	private ResearchState m_researchState = ResearchState.None;

	public void Initialize ()
	{
		Organization p = GameManager.instance.game.player;

		switch (m_researchGranted.m_assetType) {

		case Asset.AssetType.Research_Force:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_ForceIndicator);
			break;
		case Asset.AssetType.Research_Tech:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_TechIndicator);
			break;
		case Asset.AssetType.Research_Influence:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_InfluenceIndicator);
			break;
		case Asset.AssetType.Research_Lair:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_LairIndicator);
			break;
		}

		if (p.currentResearch.Contains (m_researchGranted)) {

			// set to owned state

			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Owned);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Owned);

			m_researchState = ResearchState.Owned;

		} else if (!p.currentResearch.Contains (m_researchGranted) && (m_prerequisiteResearch == null || p.currentResearch.Contains (m_prerequisiteResearch))) {
			
			// set to available state

			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Available);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Available);

			m_researchState = ResearchState.Available;

		} else if (m_researchGranted != null && !p.currentResearch.Contains (m_prerequisiteResearch)) {

			// set to unavailable state

			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Unavailable);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Unavailable);

			m_researchState = ResearchState.Unavailable;
		}
	}

	public void ResearchButtonClicked ()
	{
		MissionBase currentMission = GameManager.instance.game.player.GetMission (GameManager.instance.game.player.homeRegion);

		if (currentMission == null) {
			LairMenu.instance.SelectMissionForResearchButton (this);
		}
	}

	public ResearchState researchState {get{ return m_researchState; }}
}
