﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Henchmen : ScriptableObject {

	public enum state
	{
		None,
		Idle,
		OnMission,
	}

	private string m_name = "Null";

	private int m_rank = 1;

	private int 
	m_hireCost = 1,
	m_costPerTurn = 1,
	m_id = -1;

	private Sprite m_portrait;

	private Dictionary<TraitData.TraitClass, List<TraitData>> m_traitDict;

	private Region m_currentRegion = null;

//	private List<MissionBase> m_missions = new List<MissionBase>();

	private state m_state = state.None;

	public void Initialize (HenchmenData h)
	{
		m_id = GameManager.instance.newID;
		m_traitDict = new Dictionary<TraitData.TraitClass, List<TraitData>> ();
		m_state = state.Idle;

		name = h.m_name;
		m_name = h.m_name;
		m_rank = h.m_rank;
		m_hireCost = h.m_hireCost;
		m_costPerTurn = h.m_costPerTurn;
		m_portrait = h.m_portrait;

		foreach (TraitData t in h.m_startingTraits) {
			AddTrait (t);
		}
	}

	public void SetRegion (Region newRegion)
	{
		// remove from current region list

		if (m_currentRegion != null && m_currentRegion.currentHenchmen.Contains (this)) {
			m_currentRegion.RemoveHenchmen (this);
		}

		// add to new region list
		if (newRegion != null) {
			if (!newRegion.currentHenchmen.Contains (this)) {
				newRegion.currentHenchmen.Add (this);
			}
		}
			
		m_currentRegion = newRegion;


	}

	public void AddTrait (TraitData t)
	{
		if (m_traitDict.ContainsKey (t.m_class)) {

			List<TraitData> l = m_traitDict [t.m_class];
			if (!l.Contains (t)) {
				l.Add (t);
				m_traitDict [t.m_class] = l;
			}
		} else {
			List<TraitData> newList = new List<TraitData> ();
			newList.Add (t);
			m_traitDict.Add (t.m_class, newList);
		}
	}

	public List<TraitData> GetAllTraits ()
	{
		List<TraitData> t = new List<TraitData> ();

		if (m_traitDict.ContainsKey (TraitData.TraitClass.Skill)) {
			List<TraitData> skills = m_traitDict [TraitData.TraitClass.Skill];
			t.AddRange (skills);
		}
		if (m_traitDict.ContainsKey (TraitData.TraitClass.Resource)) {
			List<TraitData> resources = m_traitDict [TraitData.TraitClass.Resource];
			t.AddRange (resources);
		}
		if (m_traitDict.ContainsKey (TraitData.TraitClass.Gift)) {
			List<TraitData> gifts = m_traitDict [TraitData.TraitClass.Gift];
			t.AddRange (gifts);
		}
		if (m_traitDict.ContainsKey (TraitData.TraitClass.Flaw)) {
			List<TraitData> flaws = m_traitDict [TraitData.TraitClass.Flaw];
			t.AddRange (flaws);
		}

		return t;
	}

	public bool HasTrait (TraitData t)
	{
		if (m_traitDict.ContainsKey(t.m_class))
		{
			List<TraitData> l = m_traitDict [t.m_class];

			if (l.Contains (t)) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	public void ChangeState (state newState)
	{
		m_state = newState;
	}

//	public void AddMission(MissionBase m)
//	{
//		m_missions.Add (m);
//
//		if (m_state != state.OnMission) {
//			ChangeState (state.OnMission);
//		}
//	}
//
//	public MissionBase GetMission ()
//	{
//		if (m_missions.Count > 0) {
//			return m_missions [0];
//		} else {
//			return null;
//		}
//	}

	public int rank {get{return m_rank; }}
	public int hireCost {get{return m_hireCost; }}
	public int costPerTurn {get{return m_costPerTurn; }}
	public string henchmenName {get{return m_name; }}
	public Sprite portrait {get{return m_portrait;}}
	public int id {get{return m_id; }}
	public state currentState {get{return m_state;}}
	public Region currentRegion {get{return m_currentRegion; }}
}
