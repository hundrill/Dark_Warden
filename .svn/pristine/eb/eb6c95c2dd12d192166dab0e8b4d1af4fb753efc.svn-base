using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        // 마우스 클릭 처리
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            CheckInput(Input.mousePosition);
        }

        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended) // 터치가 끝났을 때
            {
                CheckInput(touch.position);
            }
        }
    }

    // 입력 좌표로 오브젝트 확인 및 씬 로드
    void CheckInput(Vector3 inputPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {            
            //if (hit.collider.gameObject.CompareTag("LoadSceneObject"))
            {
                LoadScene();
            }
        }
    }

    void LoadScene()
    {
        Debug.Log("Loading scene: " + "Game");
        SceneManager.LoadScene("Game");
    }
}
