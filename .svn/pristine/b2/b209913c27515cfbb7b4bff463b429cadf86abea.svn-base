using UnityEngine;
using System.Collections;

public class SpecManager : MonoBehaviour
{
	float fpsThreshold = 40f;        // FPS 기준
	float checkCooldown = 2f;        // FPS 체크 간격
	float smoothing = 0.1f;          // FPS 계산 부드럽게
	float check_Finish_time = 500;
	private float deltaTime = 0f;
	private float cooldownTimer = 0f;
	private bool check_Finish = false;

	void Start()
	{
		int startlevel = QualitySettings.names.Length - 1;
		// 가장 좋은 품질(가장 첫 번째 품질)로 시작
		QualitySettings.SetQualityLevel(startlevel);
		check_Finish = false;

		Debug.Log("kkk시작 품질: " + QualitySettings.names[1]);
		check_Finish_time = 100;
	}

	void Update()
	{
		// FPS 계산
		deltaTime += (Time.deltaTime - deltaTime) * smoothing;
		float fps = 1.0f / deltaTime;

		cooldownTimer += Time.unscaledDeltaTime;
		check_Finish_time -= Time.unscaledDeltaTime;

		if (check_Finish_time <= 0)
		{
			check_Finish = true;
		}

		if (!check_Finish)
		{
			if (cooldownTimer >= checkCooldown)
			{
				check_Finish_time -= checkCooldown;

				if (fps < fpsThreshold)
				{
					int currentLevel = QualitySettings.GetQualityLevel();
					int maxIndex = QualitySettings.names.Length - 1;

					if (currentLevel > 0)
					{
						int newLevel = currentLevel - 1;
						QualitySettings.SetQualityLevel(newLevel);
						Debug.Log("kkkFPS 낮음 → 품질 낮춤: " + QualitySettings.names[newLevel]);
						cooldownTimer = 0f;
					}
					else
					{
						Debug.Log("kkk더 이상 낮출 품질 없음!");
						check_Finish = true;
					}
				}
				else if (fps >= fpsThreshold)
				{
					//check_Finish = true;
					Debug.Log("kkkFPS 충분 → 품질 유지: " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
				}
			}
		}
	}


	/*void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 100, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color(255.0f, 255.0f, 255.0f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);

		rect = new Rect(0, 200, w, h * 2 / 100);
		text = string.Format("num : {0} )", QualitySettings.names[0]);
		GUI.Label(rect, text, style);

		rect = new Rect(0, 300, w, h * 2 / 100);
		text = string.Format("num : {0} )", QualitySettings.names[1]);
		GUI.Label(rect, text, style);

		rect = new Rect(0, 400, w, h * 2 / 100);
		text = string.Format("num : {0} )", QualitySettings.names[2]);
		GUI.Label(rect, text, style);

		int currentLevel = QualitySettings.GetQualityLevel();
		rect = new Rect(0, 500, w, h * 2 / 100);
		text = string.Format("현재 품질 : {0} )", QualitySettings.names[currentLevel]);
		GUI.Label(rect, text, style);



	}*/
}
