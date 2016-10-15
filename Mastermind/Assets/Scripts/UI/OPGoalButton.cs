using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OPGoalButton : MonoBehaviour {

	public Text m_buttonText;
	public Image m_buttonImage;

	// Use this for initialization
	void Start () {
	
	}
	
	public void Initialize (OPGoal goal)
	{
		m_buttonText.text = goal.m_goalName;
	}
}
