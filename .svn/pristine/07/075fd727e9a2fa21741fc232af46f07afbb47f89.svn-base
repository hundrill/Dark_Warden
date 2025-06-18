using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraLogic : MonoBehaviour
{
	public CinemachineVirtualCamera virtualCamera;
	private CinemachineBasicMultiChannelPerlin perlinNoise;
	private float shakeDuration = 0f; // 흔들림 지속 시간

	public float transitionDuration = 1f;          // FOV 변경에 걸리는 시간

	private Coroutine fovCoroutine;

	public static CameraLogic instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		if (perlinNoise == null)
			perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

		var composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
		if (composer == null)
		{
			Debug.LogError("Cinemachine Composer가 Virtual Camera에 설정되지 않았습니다.");
			return;
		}

		// ScreenX 값 설정
		//composer.m_ScreenX = 0.35f;
		composer.m_ScreenX = 0.3f;
		virtualCamera.m_Lens.FieldOfView = 50;
	}

	private void Start()
	{
		Character.instance.OnSpawnArrive += OnSpawnArrive;
	}

	public void OnSpawnArrive(Vector3 pos, MONSTER _type)
	{
		float fov;
		switch (_type)
		{
			case MONSTER.RAPTOR:
				fov = 60;
				break;

			default:
				fov = 45;
				break;
		}

		ChangeFOV(fov);
	}

	void Update()
	{
		// 흔들림 지속 시간 감소
		if (shakeDuration > 0)
		{
			shakeDuration -= Time.deltaTime;

			// 흔들림이 끝나면 Amplitude를 0으로 설정
			if (shakeDuration <= 0)
			{
				perlinNoise.m_AmplitudeGain = 0f;
			}
		}
	}

	// 흔들림 효과 시작
	public void Shake(float intensity, float duration)
	{
		perlinNoise.m_AmplitudeGain = intensity; // 흔들림 강도
		shakeDuration = duration;                // 흔들림 시간
	}

	public void ChangeFOV(float targetFOV)
	{
		if (virtualCamera == null)
		{
			Debug.LogError("Cinemachine Virtual Camera가 할당되지 않았습니다.");
			return;
		}

		// 진행 중인 코루틴 중지
		if (fovCoroutine != null)
		{
			StopCoroutine(fovCoroutine);
		}

		// 새로운 코루틴 실행
		fovCoroutine = StartCoroutine(SmoothChangeFOV(targetFOV));
	}

	private IEnumerator SmoothChangeFOV(float targetFOV)
	{
		float startFOV = virtualCamera.m_Lens.FieldOfView; // 현재 FOV
		float elapsedTime = 0f;

		// Y값 유지용
		Transform camTransform = virtualCamera.transform;
		Vector3 initialPosition = camTransform.position;

		while (elapsedTime < transitionDuration)
		{
			elapsedTime += Time.deltaTime;
			float t = Mathf.Clamp01(elapsedTime / transitionDuration);

			// FOV 선형 보간
			virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

			// 카메라 Y 위치 유지
			camTransform.position = new Vector3(camTransform.position.x, initialPosition.y, camTransform.position.z);

			yield return null;
		}

		// FOV 최종 값 설정
		virtualCamera.m_Lens.FieldOfView = targetFOV;

		// Y 위치 유지
		camTransform.position = new Vector3(camTransform.position.x, initialPosition.y, camTransform.position.z);
	}

}