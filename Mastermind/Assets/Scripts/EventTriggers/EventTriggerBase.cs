using UnityEngine;
using System.Collections;

public class EventTriggerBase : ScriptableObject {

	public virtual bool EvaluateTrigger (){return false;}

	public virtual void ExecuteEvent (){}
}
