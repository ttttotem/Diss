using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSystem : MonoBehaviour
{
    public void Switch()
    {
        GameManager.GM.SwitchSystem();
        SceneManager.LoadScene(0);
    }
}
