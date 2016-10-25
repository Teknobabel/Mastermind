using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Goal_ThreatenTheWorld : OPGoalBase, IObserver {

	public override OPGoalBase GetObject ()
	{
		Goal_ThreatenTheWorld g = Goal_ThreatenTheWorld.CreateInstance<Goal_ThreatenTheWorld> ();
		return g;
	}

	public override void Initialize (OmegaPlan op, Organization o)
	{
		base.Initialize (op, o);
		// add observers as needed to detect goal completion

	}

	public override string GetText ()
	{
		string s = "THREATEN THE WORLD";

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
