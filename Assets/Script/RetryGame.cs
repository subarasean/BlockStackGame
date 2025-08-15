using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour
{
    public GameObject retryGame;
    private void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
}
