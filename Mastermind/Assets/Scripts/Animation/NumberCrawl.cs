using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class NumberCrawl : MonoBehaviour {

	public TextMeshProUGUI m_m_textLabel;

	private bool m_crawlComplete = true;

	private float
	m_timer = 0.0f,
	m_textCrawlSpeed = 0.08f;

	private int
		m_currentNumber = 0,
		m_targetNumber = 0;

	public void Initialize (int targetNumber)
	{
		m_targetNumber = targetNumber;

		m_crawlComplete = false;

//		m_remainingCharacters = new List<string> ();
//		for (int i = 0; i < textString.Length; i++)
//		{
//			m_remainingCharacters.Add(textString[i].ToString());
//		}
//
//		m_m_textLabel.text = "";
//
//		m_textCrawlComplete = false;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!m_crawlComplete) {

			if (m_timer >= m_textCrawlSpeed) {

				m_timer = 0;

				if (m_targetNumber == m_currentNumber) {

					m_crawlComplete = true;
				}
				else if (m_targetNumber > m_currentNumber) {

					m_currentNumber++;

				} else if (m_targetNumber < m_currentNumber) {

					m_currentNumber--;
				}

				m_m_textLabel.text = m_currentNumber.ToString ();
			}
		}

		m_timer += Time.deltaTime;
	}
}
