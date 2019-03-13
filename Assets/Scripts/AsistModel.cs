using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsistModel : MonoBehaviour {

	public const float USUALSPEED = 95;
	public const float FASTSPEED = 120;
	public const float SLOWSPEED = 60;

	public const float MAXDISTANCE = 90;
	public const float MINDISTANCE = 1;
	public enum DriveMode
	{
		simple,
		catchingUp,
		interact,
		crossRoad
	}

	public SoundController SoundController;
	public PlayerController PlayerController;

	private Animator AsistAnimator;
	public Animator PointerAnimator;
	public Renderer UpTurner;
	public Renderer DownTurner;
	public Renderer LeftTurner;
	public Renderer RightTurner;
	private Vector3 offsetFromPlayer;
	private Vector3 offsetY;
	public float speed = USUALSPEED;

	public bool playerWantInteract = false;

	public enum TurnerColors
	{
		Red,
		Blue,
		Green,
		Yellow
	}
	public Color blueTurner;
	public Color redTurner;
	public Color greenTurner;
	public Color yellowTurner;

	public DriveMode mode = DriveMode.simple;

	public Vector3 aimPoint;

	void Awake ()
	{
		this.offsetY = new Vector3 (0, 7, 0);
		this.offsetFromPlayer = transform.InverseTransformVector(this.PlayerController.transform.position) -  transform.InverseTransformVector(transform.position) + this.offsetY;
	}
	void Start () 
	{
		this.AsistAnimator = GetComponentInChildren<Animator>();
	}
	
	void FixedUpdate () 
	{
		float step = this.speed * Time.deltaTime;
		
		if (mode == DriveMode.simple || mode == DriveMode.crossRoad)
		{
			this.transform.position = Vector3.MoveTowards(transform.position, this.aimPoint, step);
			Quaternion rotate = Quaternion.LookRotation(this.aimPoint - transform.position);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, step);
		}
	
		
		this.CheckDistance();

		if (mode == DriveMode.catchingUp)
		{
			Quaternion rotate = Quaternion.LookRotation(this.aimPoint - transform.position);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, step);

			Vector3 defaultLocalPosition = transform.InverseTransformVector(this.PlayerController.transform.position) - this.offsetFromPlayer;
			if ( (transform.TransformVector(defaultLocalPosition) - transform.position).magnitude > 3 )
			{
				this.transform.position = Vector3.MoveTowards(transform.position, transform.TransformVector(defaultLocalPosition), step);
			}
			else
			{
				this.speed = USUALSPEED;
				this.mode = DriveMode.simple; 
			}
		}

		if (mode == DriveMode.interact)
		{
			Quaternion rotate = Quaternion.LookRotation(-PlayerController.transform.position + transform.position);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, step);
		}
	}

	private IEnumerator GoToPlayer ()
	{
		yield return new WaitForSeconds(12);
		StopCoroutine("Decelaration");
		this.speed = FASTSPEED;
		this.mode = DriveMode.catchingUp;
		this.AsistAnimator.SetBool("CrossRoad", false);
	}

	private IEnumerator Decelaration ()
	{
		while (this.speed > 80)
		{
			this.speed -= 0.09f;
			yield return null;
		}
	}

	private void ToDefaultPosition (float step)
	{
		Vector3 defaultLocalPosition = transform.InverseTransformVector(this.PlayerController.transform.position) - this.offsetFromPlayer;
		this.transform.position = Vector3.MoveTowards(transform.position, transform.TransformVector(defaultLocalPosition), step);
	}

	private void SetTurnersColor (PointModel PM)
	{
		this.UpTurner.material.color = this.ChooseColor(PM.upCol);
		this.DownTurner.material.color = this.ChooseColor(PM.downCol);
		this.LeftTurner.material.color = this.ChooseColor(PM.leftCol);
		this.RightTurner.material.color = this.ChooseColor(PM.rightCol);
	}

	private Color ChooseColor (AsistModel.TurnerColors color)
	{
		switch (color)
		{
			case TurnerColors.Blue:
				return blueTurner;
			case TurnerColors.Red:
				return redTurner;
			case TurnerColors.Green:
				return greenTurner;
			case TurnerColors.Yellow:
				return yellowTurner;
			default:
				return new Color();
		}
	}

	private bool IsRelevantAttention (PointModel PM)
	{
		if (PM.AttentDirect == PlayerController.myDirect)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void CheckIsAttention (PointModel PM)
	{
		if (PM.isAttention && this.IsRelevantAttention(PM))
		{
			if (PM.isCrossroadAttent)
			{
				this.mode = DriveMode.crossRoad;
				this.AsistAnimator.SetBool("CrossRoad", true);
				this.AsistAnimator.SetFloat("TurnUp", PM.up);
				this.AsistAnimator.SetFloat("TurnDown", PM.down);
				this.AsistAnimator.SetFloat("TurnRight", PM.right);
				this.AsistAnimator.SetFloat("TurnLeft", PM.left);
				this.SetTurnersColor(PM);

				StartCoroutine("Decelaration");
				StartCoroutine("GoToPlayer");
			}
			else
			{
				//Анимация предупреждения
				//this.AsistAnimator.SetBool("OutAll", true);
			}
		}
	}

	public void ChangeDirection ()
	{
		Vector3 aimPoint = transform.InverseTransformVector(PlayerController.currentPoint.transform.position) - this.offsetFromPlayer;
		aimPoint = transform.TransformVector(aimPoint);
		this.aimPoint = aimPoint;
	}

	private void CheckDistance ()
	{
		if (this.mode == DriveMode.simple)
		{
			float distance = (transform.position - PlayerController.transform.position).magnitude;
			if ( distance > MAXDISTANCE || distance < MINDISTANCE )
			{
				this.ToDefaultPosition(this.speed * 1.2f);
			}
		}
	}

	public void PlayerPointEnter ()
	{
		this.playerWantInteract = true;
		this.PointerAnimator.SetBool("OnEnter", false);
		if (this.mode == DriveMode.simple)
		{
			this.PointerAnimator.SetBool("OnEnter", true);
			StartCoroutine("WaitForInteract");
		}
	}

	public void PlayerPointerExit ()
	{
		this.playerWantInteract = false;
	}

	public void PressExitButt ()
	{
		this.playerWantInteract = false;
		if ( this.mode == DriveMode.interact )
		{
			this.PointerAnimator.SetBool("OnEnter", true);
			StartCoroutine("WaitForInteractOut");
		}
	}

	public void OutExitButt ()
	{
		this.playerWantInteract = true;
		this.PointerAnimator.SetBool("OnEnter", false);
	}
	
	IEnumerator WaitForInteract ()
	{
		yield return new WaitForSeconds(2.5f);
		if (this.playerWantInteract && this.mode == DriveMode.simple)
		{
			this.mode = DriveMode.interact;
			StartCoroutine("GoToPlayerInteract");
			this.AsistAnimator.SetBool("Interact", true);
			this.PointerAnimator.SetBool("OnEnter", false);

			this.speed = SLOWSPEED;
			PlayerController.playerSpeed = 0;
			PlayerController.trainSpeed = 0;
		}
	}

	IEnumerator WaitForInteractOut ()
	{
		yield return new WaitForSeconds(2.5f);
		if (! this.playerWantInteract)
		{
			this.mode = DriveMode.simple;
			this.speed = USUALSPEED;
			PlayerController.playerSpeed = PlayerController.PLAYERSPEED;
			PlayerController.trainSpeed = PlayerController.TRAINSPEED;

			this.SoundController.canInteract = false;
			this.AsistAnimator.SetBool("Interact", false);
			this.PointerAnimator.SetBool("OnEnter", false);
			this.ToDefaultPosition(this.speed);
		}
	}

	IEnumerator GoToPlayerInteract ()
	{
		float maxCount = 20;
		float counter = 0;
		while (mode == DriveMode.interact && counter < maxCount)
		{
			Vector3 interactPosition = this.PlayerController.cam.transform.TransformVector( this.PlayerController.cam.transform.InverseTransformDirection(this.PlayerController.cam.transform.position) + new Vector3(0, 0, 8) );
			transform.position = Vector3.MoveTowards(transform.position, interactPosition, this.speed/10);
			counter++;
			yield return null;
		}
		this.SoundController.canInteract = true;
	}
}
