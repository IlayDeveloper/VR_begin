using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointModel : MonoBehaviour {
	public bool isUsualPoint = true;
	public bool isEndPoint = false;
	public bool isClosure = false;
	public bool isCenterPoint = false;
	public bool isTriple = false;
	public bool isTripleEdge = false;
	public bool isDouble = false;
	public bool isFourth = false;
	public bool isFifth = false;
	public bool isOpenSpace = false;
	public bool isEdge = false;

	public bool isAttention = false;
	public bool isCrossroadAttent = false;
	public PlayerController.Directions AttentDirect;

	public float up = 0;
	public float down = 0;
	public float right = 0;
	public float left = 0;
	
	public AsistModel.TurnerColors upCol ;
	public AsistModel.TurnerColors downCol;
	public AsistModel.TurnerColors leftCol;
	public AsistModel.TurnerColors rightCol;
	


	public GameObject nextPoint;
	public GameObject backPoint;
	public GameObject pathOne;
	public GameObject pathTwo;
	public GameObject pathThree;
	public GameObject pathFour;
	
	void Awake ()
	{
	
	}
	void Start () 
	{
		
	}
	
}
