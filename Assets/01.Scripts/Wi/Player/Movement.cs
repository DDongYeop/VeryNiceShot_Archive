using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
	[Header("Flags")]
	[SerializeField] private bool anytimeMovable = false; // ���� ���� ������ ������ �� �ִ�
	public bool canHighShot = true; // ���� ���� �� �� �ִ�
	[SerializeField] private bool returnToLastPos = false;

	[Header("Move Values")]
	[SerializeField] private float power = 5f; // ���� �߻�Ǵ� ���� �ִ�
	[SerializeField][Range(0, 1f)] private float yPower = 0.5f; // ���� ���� �߻�� ���� y�� ��
	[SerializeField] private float maxPowerDistance = 3f; // �巡�� ���� ���� �ִ� �Ŀ��� �����ϴ� �Ÿ�
	[SerializeField] private float minimumYPos = -10f; // �÷��̾��� �ּ� y ��ġ, �� ��ġ ���Ϸ� �������� ������������ �ǵ��� ����
	[SerializeField] private float stoppingCheckTime = 1f; //������ üũ�ϴ� �ּ� �Ÿ�
	private Vector3 originPos; // ��������
	private Vector3 lastPos; // ���� �ֱ� ���� ����
	public int MoveCount{ get; set; } // ������ �� �ִ� Ƚ��
	public int CurrentMoveCount { get; private set; } // ������ Ƚ��

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

	private Plane groundPlane; // ���콺 �巡�� ��ġ Ȯ���� ���� ���
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
		// ����� ������ �����ϰ� ��Ŭ������ �� ����� ����
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
	/// ���� ������ ��
	/// </summary>
	private void OnMove()
	{
		// ���߾��ٸ� ��ȯ
		if (isStopped) return;

		CheckYPos();

		// �����̸� ������ �������� ���� Drag = 0, ������ ������ ���߱� ���� Drag = 15
		if (Physics.Raycast(transform.position, Vector3.down, out hitInfo))
		{
			rigid.angularDrag = (hitInfo.normal.y >= 1f) ? 15f : 0f;
		}

		// �ӵ��� 0�̸� ���� Ȯ��
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
	/// ���� ���߾��� ��
	/// </summary>
	private void OnStop()
	{
		// �����̴� ���̸鼭 anytimeMovable�� false��� ��ȯ
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
	/// �ൿ Ƚ�� Ȯ��
	/// </summary>
	/// <returns>�ൿ Ƚ���� ��� ����ϸ� true, �ƴ϶�� false</returns>
	private bool CheckMoveCount() => MoveCount != -1 && CurrentMoveCount == MoveCount;

	/// <summary>
	/// ���� �ּ� ���� ���� ���������� Ȯ��
	/// </summary>
	private void CheckYPos()
	{
		// �ּ� ���� ���� ���� ���
		if (transform.position.y < minimumYPos)
		{
			// returnToLastPos�� false �̰ų� �ൿ Ƚ���� ��� �Ҹ����� ��� �����
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
	/// ���� �����̴� �Լ�
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
	/// �������� �����ϴ� �Լ�
	/// </summary>
	public void StopMovement()
	{
		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;
	}

	/// <summary>
	/// �ʱ�ȭ �Լ�
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
