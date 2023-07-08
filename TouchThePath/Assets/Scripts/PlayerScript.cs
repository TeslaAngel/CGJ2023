using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    //[HideInInspector]
    Rigidbody2D rigidbody2;
    Animator animator;
    //public GameObject Prefab_HandPrint;
    public float SpeedMultifier;

    [Space]
    internal float HandPrintLoadtime = 0f;
    public float HandPrintLimit;
    public float HandPrintInterval;

    [Space]
    public bool ChaseByCamera;
    public float CameraDrag;

    [Space]
    public Transform spritePivot;
    public SpriteRenderer eyeBlinkDown;
    public SpriteRenderer eyeBlinkSide;
    public float eyeBlinkInterval = 3f;
    public float eyeBlinkCloseTime = 0.2f;

    public enum MoveDirection
    {
        None = 0,
        Down,
        Up,
        Left,
        Right,
    }

    static string GetWalkAnimationName(MoveDirection dir, out bool needTurn)
    {
        needTurn = false;
		if (dir == MoveDirection.Up)
		{
			return "Player_Walk_Up";
		}
		else if (dir == MoveDirection.Down)
		{
			return "Player_Walk_Down";
		}
		else if (dir == MoveDirection.Right)
		{
			return "Player_Walk_Right";
		}
		else
		{
            needTurn = true;
			return "Player_Walk_Right";
		}
	}

    static MoveDirection GetVectorDirecition(Vector2 v)
    {
        if (Mathf.Abs(v.x) < Mathf.Abs(v.y) - 0.1f)
			return v.y > 0 ? MoveDirection.Up : MoveDirection.Down;
        else
            return v.x > 0 ? MoveDirection.Right : MoveDirection.Left;
	}

    MoveDirection faceDirection = MoveDirection.Down;

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    //Death when touch wall at night
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var objTag = collision.gameObject.tag;

        if (GameController.Instance.IsAtNight)
        {
			if (objTag == "Door")
            {
                GameController.Instance.OnPlayerWin();
            }
            else if (objTag == "Wall")
            {
				//Bad ending
				Destroy(GetComponent<PlayerScript>());
				GameController.Instance.OnPlayerDied();
			}
        }
        else
        {
			if (objTag == "Treasure")
			{
				GameController.Instance.OnTurnNight();
			}
		}
    }

	private void FixedUpdate()
	{
		//Behavior: move
		rigidbody2.velocity = new Vector2(Input.GetAxis("Horizontal") * SpeedMultifier, Input.GetAxis("Vertical") * SpeedMultifier);
	}

	void Update()
    {
        //根据方向控制动画
        var velocity = rigidbody2.velocity;
        if (velocity.sqrMagnitude > 0.25f)
        {
            var direction = velocity.normalized;
            MoveDirection facingDir = GetVectorDirecition(direction);
            if (facingDir != faceDirection)
            {
                var isUpDown = facingDir == MoveDirection.Up || facingDir == MoveDirection.Down;
                eyeBlinkSide.gameObject.SetActive(!isUpDown);
                eyeBlinkDown.gameObject.SetActive(isUpDown);

                string aniName = GetWalkAnimationName(facingDir, out var needTurn);
				spritePivot.transform.eulerAngles = new Vector3(0, needTurn ? 180f : 0, 0);

				animator.Play(aniName);
                animator.speed = 1.0f;
                faceDirection = facingDir;
            }
        }
        else
        {
            if (faceDirection != MoveDirection.None)
            {
				string aniName = GetWalkAnimationName(faceDirection, out var needTurn);
                animator.Play(aniName, 0, 0f);
				animator.speed = 0.0f;
				faceDirection = MoveDirection.None;
			}
        }

        //眨眼
        int eyeBlinkTimeIndex = (int)(Time.time / eyeBlinkInterval);
        float eyeBlickTime = Time.time - (eyeBlinkTimeIndex * eyeBlinkInterval);
        bool eyeClose = eyeBlickTime < eyeBlinkCloseTime;
        if (eyeBlinkDown.enabled != eyeClose)
            eyeBlinkDown.enabled = eyeClose;
		if (eyeBlinkSide.enabled != eyeClose)
			eyeBlinkSide.enabled = eyeClose;

        //Behavior: handPrint
        if(Input.GetAxis("Fire1") > 0 && HandPrintLoadtime <= 0f && GameController.Instance.CanAddHandPrint())
        {
            //raycast
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (hit.collider != null)
            {
                //Initiate a handprint on wall
                GameController.Instance.AddHandPrint(hit.point, angle);
            }
            else
            {
                return;
            }

            HandPrintLoadtime = HandPrintInterval;
        }

        //timer for handprint
        if(HandPrintLoadtime > 0f)
        {
            HandPrintLoadtime -= Time.deltaTime;
        }

        //Camera Chase
        if (ChaseByCamera)
        {
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, -10f);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, Time.deltaTime * CameraDrag);
        }
        
    }
}
