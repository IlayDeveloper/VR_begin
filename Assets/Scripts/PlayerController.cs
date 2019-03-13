using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDriver {

	public const float TRAINSPEED = 65;
	public const float PLAYERSPEED = 30;
	public Rigidbody rb;
	public Camera cam;
	public Directions myDirect = Directions.Forward;
	public GameObject startPoint;
	public GameObject currentPoint;
	public GameObject previousPoint;
	public float trainSpeed;
	public float playerSpeed;
	public AsistModel AsistModel;
	private Vector3 trainVelocity;

	public TrackRouter TrackRouter;
	public bool trackIsOver = false;
	public enum Directions
	{
		Forward,
		Back
	}

	void Awake ()
	{
		this.rb = gameObject.GetComponent<Rigidbody>();
		this.cam = gameObject.GetComponentInChildren<Camera>();
		this.trainSpeed = TRAINSPEED;
		this.playerSpeed = PLAYERSPEED;
	}

	void Start () 
	{
		this.currentPoint = this.startPoint;
		this.previousPoint = this.startPoint;
		AsistModel.ChangeDirection();
	}

	void FixedUpdate ()
	{
		if (! this.trackIsOver)
		{
			Vector3 vel = Vector3.Normalize(this.cam.transform.forward) *  this.playerSpeed;
			this.rb.velocity = vel;
		}

		float step = this.trainSpeed * Time.deltaTime;
		Vector3 newVector =  Vector3.MoveTowards(transform.position, this.currentPoint.transform.position, step);
		this.transform.position = newVector;
	}

	void OnTriggerEnter (Collider col)
	{
		PointModel PM = col.gameObject.GetComponent<PointModel>();
		TrackRouter.FindNextPoint(PM, this.previousPoint, this.currentPoint, this);
		this.UpdatePreviousPoint(PM.gameObject);

		this.AsistModel.ChangeDirection();
		this.AsistModel.CheckIsAttention(PM);
	}

	public void UpdateCurrentPoint (GameObject newCurPoint)
	{
		this.currentPoint = newCurPoint;
	}

	public void UpdatePreviousPoint (GameObject newPreviousPoint)
	{
		this.previousPoint = newPreviousPoint;
	}

	public void UpdateDirection (Directions dir)
	{
		this.myDirect = dir;
	}

	public void TrackEnded ()
	{
		this.trackIsOver = true;
		this.rb.velocity = Vector3.zero;
	}

}
