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

	private ResearchObject m_researchObject;

	public RawImage m_buttonBG;
	public RawImage m_typeIndicator;
	public TextMeshProUGUI m_text;

	private ResearchState m_researchState = ResearchState.None;

	public void Initialize (ResearchObject r, TechTree.ResearchBranch rb)
	{
		m_researchObject = r;

		Organization p = GameManager.instance.game.player;

		m_text.text = r.m_name;

		switch (rb.m_branchType) {

		case TechTree.BranchType.Force:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_ForceIndicator);
			break;
		case TechTree.BranchType.Tech:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_TechIndicator);
			break;
		case TechTree.BranchType.Influence:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_InfluenceIndicator);
			break;
		case TechTree.BranchType.Lair:
			m_typeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_LairIndicator);
			break;
		}

		bool hasPrereqs = true;
		bool hasResearch = true;

		foreach (Asset a in r.m_prerequisiteResearch) {

			if (!p.currentResearch.Contains (a)) {

				hasPrereqs = false;
				break;
			}
		}

		foreach (Asset a in r.m_researchGained) {

			if (!p.currentResearch.Contains (a)) {

				hasResearch = false;
				break;
			}
		}


		// if player already has all research granted, set to owned state

		if (hasResearch) {

			ChangeState (ResearchState.Owned);

		} else if (hasPrereqs && !hasResearch) { // if player contains all prerequisites but not research granted, set to available

			ChangeState (ResearchState.Available);

		} else if (!hasPrereqs && !hasResearch) { // if player doesn't have all prerequisites, set to unavailable

			ChangeState (ResearchState.Unavailable);
		}




//		Debug.Log (m_researchState);




//		if (p.currentResearch.Contains (m_researchGranted)) {
//
//			// set to owned state
//
//			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Owned);
//			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Owned);
//
//			m_researchState = ResearchState.Owned;
//
//		} else if (!p.currentResearch.Contains (m_researchGranted) && (m_prerequisiteResearch == null || p.currentResearch.Contains (m_prerequisiteResearch))) {
//			
//			// set to available state
//
//			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Available);
//			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Available);
//
//			m_researchState = ResearchState.Available;
//
//		} else if (m_researchGranted != null && !p.currentResearch.Contains (m_prerequisiteResearch)) {
//
//			// set to unavailable state
//
//			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Unavailable);
//			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Unavailable);
//
//			m_researchState = ResearchState.Unavailable;
//		}
	}

	private void ChangeState (ResearchState newState)
	{
		m_researchState = newState;

		switch (newState) {

		case ResearchState.Unavailable:
			
			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Unavailable);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Unavailable);

			break;
		case ResearchState.Owned:
			
			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Owned);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Owned);

			break;
		case ResearchState.Available:
			
			m_buttonBG.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_BG_Available);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.LairMenu_Research_Text_Available);
			break;
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
	public ResearchObject researchObject {get{ return m_researchObject; }}
}
