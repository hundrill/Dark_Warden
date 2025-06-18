using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneAddManager : MonoBehaviour
{
	string sceneToLoad = "Lobby";

	public Scene mapScene;

	public static SceneAddManager instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		//Vector3 offset_card = new Vector3(0, 5, 0);

		SceneManager.LoadSceneAsync("t1classic", LoadSceneMode.Additive).completed += (operation) =>
		{
			mapScene = SceneManager.GetSceneByName("t1classic");

			if (mapScene.IsValid())
			{


				// 씬의 루트 오브젝트 이동
				GameObject[] rootObjects = mapScene.GetRootGameObjects();
				foreach (GameObject obj in rootObjects)
				{
					//obj.transform.position += offset_card;

					if (obj.name.Contains("Main Camera"))
					{
						Camera mainCamera = obj.GetComponent<Camera>();
						GameObject cameraCardObj = GameObject.Find("Camera_Card");
						if (cameraCardObj != null)
						{
							Camera cameraCard = cameraCardObj.GetComponent<Camera>();

							if (cameraCard != null)
							{
								// ?? Projection 값 복사
								cameraCard.orthographic = mainCamera.orthographic;
								cameraCard.orthographicSize = mainCamera.orthographicSize;
								cameraCard.nearClipPlane = mainCamera.nearClipPlane;
								cameraCard.farClipPlane = mainCamera.farClipPlane;

								// ?? 위치 복사
								cameraCardObj.transform.position = obj.transform.position;
								cameraCardObj.transform.rotation = obj.transform.rotation;

								Debug.Log("카메라 설정 복사 완료!");
							}
							else
							{
								Debug.LogError("Camera_Card에 Camera 컴포넌트가 없습니다!");
							}
						}

						obj.SetActive(false);
					}

					/*if (obj.name.Contains("Overlay"))
					{
						Camera.main.GetComponent<UniversalAdditionalCameraData>().cameraStack.Add(obj.GetComponent<Camera>());
					}*/

					/*if (obj.name.Contains("Light"))
					{
						if (UI_Main.instance) UI_Main.instance.list_Profile_On.Add(obj);
					}*/

				}
			}
		};

		SceneManager.LoadSceneAsync("Map_1", LoadSceneMode.Additive).completed += (operation) =>
		{
			mapScene = SceneManager.GetSceneByName("Map_1");

			if (mapScene.IsValid())
			{
				// 씬의 루트 오브젝트 이동
				GameObject[] rootObjects = mapScene.GetRootGameObjects();
				foreach (GameObject obj in rootObjects)
				{
					if (obj.name.Contains("Camera"))
					{
						obj.SetActive(false);
					}

					if (obj.name.Contains("Light"))
					{
						if (UI_Main.instance) UI_Main.instance.list_Profile_On.Add(obj);
					}
				}
			}
		};


		Vector3 offset = new Vector3(1000, 100, 100);
		SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += (operation) =>
		{
			Scene loadedScene = SceneManager.GetSceneByName(sceneToLoad);

			if (loadedScene.IsValid())
			{
				// 씬의 루트 오브젝트 이동
				GameObject[] rootObjects = loadedScene.GetRootGameObjects();
				foreach (GameObject obj in rootObjects)
				{
					obj.transform.position += offset;

					if (obj.name.Contains("Camera"))
					{
						if (UI_Main.instance) UI_Main.instance.list_Profile_Off.Add(obj);
					}

					if (obj.name.Contains("Light"))
					{
						if (UI_Main.instance) UI_Main.instance.list_Profile_Off.Add(obj);
					}

					if (obj.name.Contains("Canvas"))
					{
						obj.SetActive(false);
					}

					if (obj.name.Contains("Audio Source"))
					{
						obj.SetActive(false);
					}

					if (obj.name.Contains("LobbyManager"))
					{
						obj.SetActive(false);
					}
				}

				Debug.Log($"Moved all objects in scene '{sceneToLoad}' by offset {offset}");


				if (UI_Main.instance)
					UI_Main.instance.TurnLobbyObj(false);
			}
		};
	}

}

