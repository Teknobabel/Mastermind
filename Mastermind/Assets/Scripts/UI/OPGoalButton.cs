using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class OPGoalButton : MonoBehaviour {

	public TextMeshProUGUI m_buttonText;
	public Image m_buttonImage;


	// Use this for initialization
	void Start () {
	
	}
	
	public void Initialize (OmegaPlan.Goal goal)
	{
		switch (goal.m_state) {
		case OmegaPlan.Goal.State.Active:
			m_buttonText.text = goal.m_goal.GetText ();
			m_buttonImage.fillCenter = false;
			m_buttonImage.color = Color.black;
			break;
		case OmegaPlan.Goal.State.Completed:
			m_buttonImage.fillCenter = true;
			m_buttonImage.color = Color.green;
			m_buttonText.text = "COMPLETE";
			break;
		}

	}

	public void Deactivate ()
	{
		m_buttonImage.color = Color.black;
		m_buttonText.text = "??????";
	}
}
