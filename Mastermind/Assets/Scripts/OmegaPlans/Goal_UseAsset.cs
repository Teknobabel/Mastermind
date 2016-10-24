﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Goal_UseAsset : OPGoalBase, IObserver {

	public Asset m_asset;

	public override OPGoalBase GetObject ()
	{
		Goal_UseAsset g = Goal_UseAsset.CreateInstance<Goal_UseAsset> ();
		g.m_asset = m_asset;
		return g;
	}

	public override void Initialize ()
	{

		// add observers as needed to detect goal completion

	}

	public override string GetText ()
	{
		string s = "USE:\n";
		Debug.Log (m_asset);
		if (m_asset != null) {
			s += m_asset.m_name.ToUpper ();
		}

		return s;
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		//		switch (thisGameEvent) {
		//		case GameEvent.Organization_HenchmenDismissed:
		//		case GameEvent.Organization_HenchmenHired:
		//
		//			break;
		//		}
	}
}
