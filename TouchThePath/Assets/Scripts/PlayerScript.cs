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
    public float handPrintDistance = 1.0f;
    int handPrintLayerMask;


    [Space]
    public Transform spritePivot;
    public SpriteRenderer eyeBlinkDown;
    public SpriteRenderer eyeBlinkSideL;
    public SpriteRenderer eyeBlinkSideR;
    public float eyeBlinkInterval = 3f;
    public float eyeBlinkCloseTime = 0.2f;

    bool controlable = true;
    Vector2 controlDirection;
	MoveDirection faceDirection = MoveDirection.Down;
	MoveDirection moveDirection = MoveDirection.Down;
    bool isHandOut = false;
	bool isPlayingAnimationHandOut = false;

	public enum MoveDirection
    {
        None = 0,
        Down,
        Up,
        Left,
        Right,
    }

    static string GetWalkAnimationName(MoveDirection dir)
    {
		if (dir == MoveDirection.Up)
			return "Player_Walk_Up";
		else if (dir == MoveDirection.Left)
			return "Player_Walk_Left";
        else if (dir == MoveDirection.Right)
			return "Player_Walk_Right";
		else
			return "Player_Walk_Down";
	}

    static MoveDirection GetVectorDirecition(Vector2 v)
    {
        if (Mathf.Abs(v.x) < Mathf.Abs(v.y) - 0.1f)
			return v.y > 0 ? MoveDirection.Up : MoveDirection.Down;
        else
            return v.x > 0 ? MoveDirection.Right : MoveDirection.Left;
	}
	static Vector2 GetDirecitionVector(MoveDirection dir)
	{
        if (dir == MoveDirection.Down)
            return Vector2.down;
        else if (dir == MoveDirection.Up)
            return Vector2.up;
        else if (dir == MoveDirection.Left)
            return Vector2.left;
        else if (dir == MoveDirection.Right)
            return Vector2.right;
        else
            return Vector2.zero;
	}

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        handPrintLayerMask = LayerMask.GetMask("Wall");

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Chasm")
        {
            DropDown();
        }
    }


    //Death when touch wall at night
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var objTag = collision.gameObject.tag;

        if (GameController.Instance && GameController.Instance.IsAtNight)
        {
			if (objTag == "Door")
            {
                GameController.Instance.OnPlayerWin();
            }
            else if (objTag == "Wall")
            {
                GetPunch();
			}
            else if (objTag == "Chasm")
            {
                DropDown();
			}
        }
        else
        {
			if (objTag == "Treasure")
			{
                DoGetTreasure();
				GameController.Instance.OnTurnNight();
			}
		}
    }

	private void FixedUpdate()
	{
        if (!controlable)
		{
			controlDirection = Vector2.zero;
		}
        else
		{
			//float inputX = Input.GetAxis("Horizontal");
			//float inputY = Input.GetAxis("Vertical");
			float inputX = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
			float inputY = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
			inputX = Mathf.Sign(inputX) * (Mathf.Abs(inputX) > 0.9f ? 1.0f : 0f);
			inputY = Mathf.Sign(inputY) * (Mathf.Abs(inputY) > 0.9f ? 1.0f : 0f);
			controlDirection = new Vector2(inputX, inputY);
			if (inputX != 0 || inputY != 0)
			{
				controlDirection.Normalize();
			}
		}

		//Behavior: move
        rigidbody2.velocity = controlDirection * SpeedMultifier;
	}

	void Update()
    {
		//Behavior: handPrint
		//长按时持续伸手
		if (Input.GetKey(KeyCode.Space) && GameController.Instance.CanAddHandPrint())
		{
			if (!isHandOut && HandPrintLoadtime <= 0f)
			{
				//raycast
				//Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				Vector2 dir = GetDirecitionVector(faceDirection);
				RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, handPrintDistance, handPrintLayerMask);
				//float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				if (hit.collider != null)
				{
					//Initiate a handprint on wall
					HandPrintLoadtime = HandPrintInterval;
					GameController.Instance.AddHandPrint(hit.point, hit.normal, hit.transform);
				}
			}
			StartPutHand();
			return;
		}
		else
		{
			if (isHandOut && !isPlayingAnimationHandOut)
			{
				EndPutHand();
			}
		}

		//timer for handprint
		if (HandPrintLoadtime > 0f)
		{
			HandPrintLoadtime -= Time.deltaTime;
		}

		//不能控制, 不播放行走动画
		if (controlable)
        {
			//根据方向控制动画
			if (controlDirection.x != 0 || controlDirection.y != 0)
			{
				//var direction = controlDirection.normalized;
				var direction = controlDirection;
				MoveDirection dir = GetVectorDirecition(direction);
				if (dir != moveDirection)
				{
					//eyeBlinkSideL.gameObject.SetActive(dir == MoveDirection.Left);
					//eyeBlinkSideR.gameObject.SetActive(dir == MoveDirection.Right);
					//eyeBlinkDown.gameObject.SetActive(dir == MoveDirection.Down);

					string aniName = GetWalkAnimationName(dir);
					//spritePivot.transform.eulerAngles = new Vector3(0, needTurn ? 180f : 0, 0);

					animator.Play(aniName);
					animator.speed = 1.0f;
					faceDirection = dir;
					moveDirection = dir;
				}
			}
			else
			{
				if (moveDirection != MoveDirection.None)
				{
					string aniName = GetWalkAnimationName(faceDirection);
					animator.Play(aniName, 0, 0f);
					animator.speed = 0.0f;
					moveDirection = MoveDirection.None;
				}
			}

			//眨眼
			int eyeBlinkTimeIndex = (int)(Time.time / eyeBlinkInterval);
			float eyeBlickTime = Time.time - (eyeBlinkTimeIndex * eyeBlinkInterval);
			bool eyeClose = eyeBlickTime < eyeBlinkCloseTime;
			if (eyeBlinkDown.enabled != eyeClose)
				eyeBlinkDown.enabled = eyeClose;
			if (eyeBlinkSideL.enabled != eyeClose)
				eyeBlinkSideL.enabled = eyeClose;
			if (eyeBlinkSideR.enabled != eyeClose)
				eyeBlinkSideR.enabled = eyeClose;
		}
    }

    void DoGetTreasure()
    {
		StartCoroutine(IE_GetTreasure());
	}

	IEnumerator IE_GetTreasure()
	{
		isPlayingAnimationHandOut = true;
		StartPutHand();
		yield return new WaitForSeconds(1f);
		EndPutHand();
		isPlayingAnimationHandOut = false;
	}


    void StartPutHand()
    {
		string aniName;
		if (faceDirection == MoveDirection.Up)
			aniName = "Player_Touch_Up";		
		else if (faceDirection == MoveDirection.Left)
			aniName = "Player_Touch_Left";
		else if (faceDirection == MoveDirection.Right)
			aniName = "Player_Touch_Right";
        else
			aniName = "Player_Touch_Down";

		animator.Play(aniName);
        animator.speed = 0f;
		controlable = false;
		controlDirection = Vector2.zero;

		eyeBlinkSideL.gameObject.SetActive(false);
		eyeBlinkSideR.gameObject.SetActive(false);
		eyeBlinkDown.gameObject.SetActive(false);

		isHandOut = true;
	}


    void EndPutHand()
    {
        controlable = true;
        isHandOut = false;
        moveDirection = MoveDirection.Down;  //临时
	}



    public void GetPunch()
    {
        controlable = false;
		animator.Play("Player_Died");
		animator.speed = 1.0f;

		GameController.Instance.ShowMonsterEyes(transform.position);
		GameController.Instance.OnPlayerDied(2f);
		Destroy(GetComponent<PlayerScript>());
	}

    public void DropDown()
    {
		controlable = false;
        //人物螺旋着掉入深渊√
        animator.Play("Player_Drop");
        animator.speed = 1.0f;

        Destroy(GetComponent<PlayerScript>());
        GameController.Instance.OnPlayerDied(1f);
		AudioManager.Instance.PlaySfx(Sound.PlayerDrop);
	}


    /*
     * AudioManager会循环播放音效，动画event会鬼畜 && 播放音效需要另挂脚本（因PlayerScript死亡时会被摧毁）
    public void PlayInhaleAudio()
    {
        AudioManager.Instance.PlayBgm(Sound.PlayerInhale);
    }

    public void PlayPunchAudio()
    {
        AudioManager.Instance.PlayBgm(Sound.PlayerBeingHit);
    }
    */
}
