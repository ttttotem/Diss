using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelComplete : MonoBehaviour
{
    public Text text;

    Scene scene;

    public void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    public void NextLevel()
    {
        int next = scene.buildIndex + 1;
        if (next >= SceneManager.sceneCountInBuildSettings)
        {
            text.text = "No more levels";
        } else
        {
            SceneManager.LoadScene(next);
        }
    }
}
