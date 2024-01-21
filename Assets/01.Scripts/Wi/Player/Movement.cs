using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
	[Header("Flags")]
	[SerializeField] private bool anytimeMovable = false; // 제한 없이 언제나 움직일 수 있다
	public bool canHighShot = true; // 높은 샷을 쏠 수 있다
	[SerializeField] private bool returnToLastPos = false;

	[Header("Move Values")]
	[SerializeField] private float power = 5f; // 공이 발사되는 힘의 최대
	[SerializeField][Range(0, 1f)] private float yPower = 0.5f; // 공이 높이 발사될 때의 y의 힘
	[SerializeField] private float maxPowerDistance = 3f; // 드래그 했을 때의 최대 파워에 도달하는 거리
	[SerializeField] private float minimumYPos = -10f; // 플레이어의 최소 y 위치, 이 위치 이하로 낮아지면 시작지점으로 되돌아 간다
	[SerializeField] private float stoppingCheckTime = 1f; //멈춤을 체크하는 최소 거리
	private Vector3 originPos; // 시작지점
	private Vector3 lastPos; // 가장 최근 멈춘 지점
	public int MoveCount{ get; set; } // 움직일 수 있는 횟수
	public int CurrentMoveCount { get; private set; } // 움진인 횟수

	[Header("References")]
	[SerializeField] private TrailRenderer trail;
	[SerializeField] private Slider powerSlider;
	[SerializeField] private TextMeshProUGUI moveCountTxt;
	private Trajectory trajectory;
	private Rigidbody rigid;
	private Camera mainCam;

	[Header("Sound")]
	[SerializeField] private AudioClip impactClip;

	[Header("Values")]
	private bool isActive  = true;
	private bool isStopped = true;
	private bool isDrag = false;
	public bool isHighShot { get; set; } = false;
	private float enter;
	private float stopTime = 0;

	private Plane groundPlane; // 마우스 드래그 위치 확인을 위한 평면
	private RaycastHit hitInfo;

	[Header("Others")] 
	private PlayerCollision _playerCollision;

	private void Awake()
	{
		trajectory = GetComponent<Trajectory>();
		rigid = GetComponent<Rigidbody>();
		_playerCollision = GetComponent<PlayerCollision>();
		mainCam = Camera.main;
		originPos = transform.position;
		lastPos = originPos;

		groundPlane = new Plane();
		groundPlane.SetNormalAndPosition(Vector3.up, transform.position);
		powerSlider.gameObject.SetActive(false);
		trajectory.ShowTrajectory(false);
	}

	private void Start()
	{
		SetMoveCountText();
	}

	private void Update()
	{
		// 샷모드 변경이 가능하고 우클릭했을 시 샷모드 변경
		if (canHighShot && Input.GetMouseButtonDown(1))
		{
			isHighShot = !isHighShot;
			GameManager.Instance.UIController.SetShotMode(!isHighShot);
			//trajectory.ShowTrajectory(isHighShot);
		}

		if (!isActive) return;
		OnMove();
		OnStop();
	}

	/// <summary>
	/// 공이 움직일 때
	/// </summary>
	private void OnMove()
	{
		// 멈추었다면 반환
		if (isStopped) return;

		CheckYPos();

		// 경사면이면 빠르게 굴러가기 위해 Drag = 0, 평지면 빠르게 멈추기 위해 Drag = 15
		if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
		{
			rigid.angularDrag = (hitInfo.normal.y >= 1f) ? 15f : 0f;
		}

		// 속도가 0이면 멈춤 확인
		if (rigid.velocity.sqrMagnitude == 0)
		{
			stopTime += Time.deltaTime;
			if (stopTime >= stoppingCheckTime)
			{
				StopMovement();
				groundPlane.SetNormalAndPosition(Vector3.up, transform.position);
				stopTime = 0;
				isStopped = true;
				transform.rotation = Quaternion.Euler(Vector3.zero);
				SetMoveCountText();

				if (CheckMoveCount())
				{
					GameManager.Instance.LevelManager.ResetGame();
				}
			}
		}
	}

	/// <summary>
	/// 공이 멈추었을 때
	/// </summary>
	private void OnStop()
	{
		// 움직이는 중이면서 anytimeMovable이 false라면 반환
		if (!isStopped && !anytimeMovable) return;

		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			isDrag = true;
			lastPos = transform.position;
			//powerSlider.gameObject.SetActive(true);
			StopMovement();
			transform.rotation = Quaternion.Euler(Vector3.zero);
			trajectory.ShowTrajectory(true);
			SetMoveCountText();
		}

		if (isDrag)
		{
			if (Input.GetMouseButton(0))
			{
				Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
				groundPlane.Raycast(ray, out enter);
				Vector3 mouseWorldPos = ray.GetPoint(enter);
				mouseWorldPos.y = transform.position.y;
				Vector3 direction = transform.position - mouseWorldPos;

				//float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
				float distance = Vector3.Distance(transform.position, mouseWorldPos);
				distance = Mathf.Clamp(distance, 0, maxPowerDistance);
				//float percent = distance / maxPowerDistance;
				//powerSlider.transform.rotation = Quaternion.Euler(90, -angle, 0);
				//powerSlider.value = percent;

				direction.Normalize();
				direction.y = isHighShot ? yPower : 0;
				trajectory.DrawTrajectory(direction * (power * (distance / maxPowerDistance)), !isHighShot);
			}

			if (Input.GetMouseButtonUp(0))
			{
				CurrentMoveCount++;
				//powerSlider.gameObject.SetActive(false);

				Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
				groundPlane.Raycast(ray, out enter);
				Vector3 mouserWorldPos = ray.GetPoint(enter);
				mouserWorldPos.y = transform.position.y;
				float distance = Vector3.Distance(transform.position, mouserWorldPos);
				distance = Mathf.Clamp(distance, 0, maxPowerDistance);
				Vector3 direction = (transform.position - mouserWorldPos).normalized;
				ThrowBall(direction, power * (distance / maxPowerDistance));
				trajectory.ShowTrajectory(false);
			}
			
			if (isHighShot)
				_playerCollision.HighShotStart(true);
		}
	}

	private void SetMoveCountText()
	{
		moveCountTxt.gameObject.SetActive(MoveCount > 0);
		moveCountTxt.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
		moveCountTxt.text = $"{CurrentMoveCount} / {MoveCount}";
		moveCountTxt.enabled = MoveCount > 0 && !isDrag;
	}

	/// <summary>
	/// 행동 횟수 확인
	/// </summary>
	/// <returns>행동 횟수를 모두 사용하면 true, 아니라면 false</returns>
	private bool CheckMoveCount() => MoveCount != -1 && CurrentMoveCount == MoveCount;

	/// <summary>
	/// 공이 최소 높이 보다 낮아지는지 확인
	/// </summary>
	private void CheckYPos()
	{
		// 최소 높이 보다 낮을 경우
		if (transform.position.y < minimumYPos)
		{
			// returnToLastPos가 false 이거나 행동 횟수를 모두 소모했을 경우 재시작
			if (!returnToLastPos || CheckMoveCount())
			{
				GameManager.Instance.LevelManager.ResetGame();
				_playerCollision.HighShotStart(false);
			}
			else
			{
				StopMovement();
				transform.position = lastPos;
				trail.Clear();
			}
		}
	}

	/// <summary>
	/// 공을 움직이는 함수
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="power"></param>
	private void ThrowBall(Vector3 direction, float power)
	{
		SoundManager.Instance.PlaySound(impactClip);
		direction.y = isHighShot ? yPower : 0;
		rigid.velocity = direction * power;

		isStopped = false;
		isDrag = false;
	}

	/// <summary>
	/// 움직임을 정지하는 함수
	/// </summary>
	public void StopMovement()
	{
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
	}

	/// <summary>
	/// 초기화 함수
	/// </summary>
	public void ResetPlayer()
	{
		trajectory.ShowTrajectory(false);
		StopMovement();
		CurrentMoveCount = 0;
		isStopped = true;
		DisolveInit();
	}

	private void DisolveInit()
	{
		StartCoroutine(DisolveInitCo());
	}	

	private IEnumerator DisolveInitCo()
	{
		isActive = false;
		rigid.isKinematic = true;

		yield return _playerCollision.StartCoroutine(_playerCollision.PlayerDissolveCo(1f, 1f));

		trail.enabled = false;
		transform.position = originPos;
		trail.enabled = true;
		trail.Clear();
		groundPlane.SetNormalAndPosition(Vector3.up, transform.position);
		SetMoveCountText();

		yield return _playerCollision.StartCoroutine(_playerCollision.PlayerDissolveCo(-1f, 1f));

		rigid.isKinematic = false;
		isActive = true;
	}
}
