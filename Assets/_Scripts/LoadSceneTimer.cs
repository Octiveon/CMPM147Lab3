using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTimer : MonoBehaviour {

    public string scene = "MainMenu";
    public float timer = 5f;

    // Use this for initialization
    void Start () {
        StartCoroutine(LoadScene());
	}

   

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(scene);
    }
}
