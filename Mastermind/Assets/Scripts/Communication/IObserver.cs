using UnityEngine;
using System.Collections;

public interface IObserver {

	void OnNotify (ISubject subject, GameEvent thisGameEvent);
}
