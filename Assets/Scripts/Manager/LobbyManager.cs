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
        // ���콺 Ŭ�� ó��
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            CheckInput(Input.mousePosition);
        }

        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended) // ��ġ�� ������ ��
            {
                CheckInput(touch.position);
            }
        }
    }

    // �Է� ��ǥ�� ������Ʈ Ȯ�� �� �� �ε�
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
