using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDriver  
{
	void UpdateCurrentPoint(GameObject newCurPoint);
	void UpdatePreviousPoint(GameObject newPreviousPoint);
	void UpdateDirection(PlayerController.Directions direct);
	void TrackEnded();

}
