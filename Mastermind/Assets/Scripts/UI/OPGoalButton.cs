using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class OPGoalButton : MonoBehaviour {

	public TextMeshProUGUI m_buttonText;
	public RawImage m_buttonImage;


	// Use this for initialization
	void Start () {
	
	}
	
	public void Initialize (OmegaPlan.Goal goal, int positionInList)
	{
		string text = "";

		switch (goal.m_state) {
		case OmegaPlan.Goal.State.Active:
			
			text = goal.m_goal.GetText ();
			m_buttonText.color = ColorManager.instance.GetColor (ColorManager.UIElement.OmegaPlanMenu_Goal_Text_Uncomplete);
			m_buttonImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.OmegaPlanMenu_Goal_BG_Uncomplete);

			break;
		case OmegaPlan.Goal.State.Completed:
			
			text = goal.m_goal.GetText ();
			text += "\nCOMPLETE";
			m_buttonText.color = ColorManager.instance.GetColor (ColorManager.UIElement.OmegaPlanMenu_Goal_Text_Complete);
			m_buttonImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.OmegaPlanMenu_Goal_BG_Complete);

			break;
		}


		TextCrawl tc = (TextCrawl) m_buttonText.transform.GetComponent<TextCrawl>();
		if (tc != null) {

			float delay = 0.15f * ((float)(positionInList));
			tc.Initialize(text.ToUpper(), delay);
		}

	}

	public void Deactivate ()
	{
		m_buttonImage.color = Color.black;
		m_buttonText.text = "??????";
	}
}
