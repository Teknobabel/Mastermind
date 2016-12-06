using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SectionHeader : MonoBehaviour, ISubject {

	public enum MinimizeState
	{
		Normal,
		Minimized,
	}

	public TextMeshProUGUI m_sectionHeaderTitle;

	private MinimizeState m_minimizeState = MinimizeState.Normal;

	private List<IObserver>
	m_observers = new List<IObserver> ();

	public List<GameObject> m_children = new List<GameObject>();

	public void Initialize (string headerTitle)
	{
		m_sectionHeaderTitle.text = headerTitle;
	}

	public void MinimizeButtonClicked ()
	{
		switch (m_minimizeState) {

		case MinimizeState.Normal:
			m_minimizeState = MinimizeState.Minimized;
			break;
		case MinimizeState.Minimized:
			m_minimizeState = MinimizeState.Normal;
			break;
		}

		Notify (this, GameEvent.UI_SectionHeader_MinimizeButtonClicked);

//		if (m_minimizeState == MinimizeState.Minimized) {
//
//
//		}
	}

	public void AddObserver (IObserver observer)
	{
		if (!m_observers.Contains(observer))
		{
			m_observers.Add (observer);
		}
	}

	public void RemoveObserver (IObserver observer)
	{
		if (m_observers.Contains(observer))
		{
			m_observers.Remove(observer);
		}
	}

	public void Notify (ISubject subject, GameEvent thisGameEvent)
	{
		List<IObserver> observers = new List<IObserver> (m_observers);

		for (int i=0; i < observers.Count; i++)
		{
			observers[i].OnNotify(subject, thisGameEvent);
		}
	}

	public MinimizeState minimizeState {get{ return m_minimizeState; }}
}
