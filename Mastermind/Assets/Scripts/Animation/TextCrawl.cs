using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TextCrawl : MonoBehaviour {

	public TextMeshProUGUI m_m_textLabel;

	private string m_textString;

	private float
	m_timer = 0.0f,
	m_textCrawlSpeed = 0.03f;

	private bool m_textCrawlComplete = true;

	private List<string> m_remainingCharacters;

	public void Initialize (string textString)
	{
		m_remainingCharacters = new List<string> ();
		for (int i = 0; i < textString.Length; i++)
		{
			m_remainingCharacters.Add(textString[i].ToString());
		}

		m_m_textLabel.text = "";

		m_textCrawlComplete = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!m_textCrawlComplete) {
			
			if (m_timer >= m_textCrawlSpeed) {

				m_timer = 0;

				string s = m_m_textLabel.text;
				s += m_remainingCharacters[0];
				m_m_textLabel.text = s;
				m_remainingCharacters.RemoveAt(0);

				if (m_remainingCharacters.Count == 0) {

					m_textCrawlComplete = true;
				}
			}

			m_timer += Time.deltaTime;
		}
	}
}
