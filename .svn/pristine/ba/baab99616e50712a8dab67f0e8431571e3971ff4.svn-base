using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SceneChange());        
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(2);

		SceneManager.LoadScene("Game");
	}
}
