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
		m_buttonText.text = goal.m_goal.GetText ();
	}
}
