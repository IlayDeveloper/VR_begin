using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRouter : MonoBehaviour {

	void Start () {
		
	}
	
	public static void FindNextPoint (PointModel PM, GameObject previousPoint, GameObject currentPoint, IDriver Driver)
	{
		if (PM.isUsualPoint)
		{
			if (previousPoint == PM.backPoint || previousPoint.GetComponent<PointModel>().isCenterPoint )
			{
				currentPoint = PM.nextPoint;
				Driver.UpdateCurrentPoint(PM.nextPoint);
				Driver.UpdateDirection(PlayerController.Directions.Forward);
			}
			else
			{
				Driver.UpdateCurrentPoint(PM.backPoint);
				Driver.UpdateDirection(PlayerController.Directions.Back);
			}
		}
		else if (PM.isTriple)
		{
			if (previousPoint == PM.pathOne || previousPoint == PM.pathTwo)
			{
				if (previousPoint.GetComponent<PointModel>().backPoint == PM.gameObject)
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}		
			}
			else
			{
				if ( previousPoint == PM.backPoint)
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);		
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}		
			}
		}
		else if (PM.isTripleEdge)
		{
			if ( previousPoint == PM.pathOne || previousPoint == PM.pathTwo )
			{
				if (PM.backPoint.GetComponent<PointModel>().isCenterPoint)
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}		
			}
			else
			{
				if ( previousPoint == PM.backPoint )
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);	
				}
				else
				{		
					Driver.UpdateCurrentPoint(PM.backPoint);
				}		
			}
		}
		else if (PM.isCenterPoint)
		{
			Driver.UpdateCurrentPoint(PM.nextPoint);
		}
		else if (PM.isDouble)
		{
			if ( previousPoint == PM.pathOne )
			{
				if ( previousPoint.GetComponent<PointModel>().backPoint == PM.gameObject )
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}		
			}
			else
			{
				if ( previousPoint == PM.backPoint)
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}		
			}
		}
		else if (PM.isFourth)
		{
			if ( previousPoint == PM.pathOne || previousPoint == PM.pathTwo || previousPoint == PM.pathThree )
			{
				if ( previousPoint.GetComponent<PointModel>().backPoint == PM.gameObject )
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}		
			}
			else
			{
				if ( previousPoint == PM.backPoint)
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}		
			}
		}
		else if (PM.isFifth)
		{
			if ( previousPoint == PM.pathOne || previousPoint == PM.pathTwo || previousPoint == PM.pathThree || previousPoint == PM.pathFour )
			{
				if ( previousPoint.GetComponent<PointModel>().backPoint == PM.gameObject )
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}		
			}
			else
			{
				if ( previousPoint == PM.backPoint )
				{
					Driver.UpdateCurrentPoint(PM.nextPoint);
				}
				else
				{
					Driver.UpdateCurrentPoint(PM.backPoint);
				}		
			}
		}
		else if ( PM.isClosure )
		{
			if ( previousPoint == PM.backPoint )
			{
				Driver.UpdateCurrentPoint(PM.nextPoint);
			}
			else
			{
				Driver.UpdateCurrentPoint(PM.backPoint);
			}
		}
		else
		{
			Driver.TrackEnded();
		}
	}
}
