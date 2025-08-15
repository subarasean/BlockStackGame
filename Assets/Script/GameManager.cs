using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isGameActive = false;

    public static event Action OnCubeSpawned;

    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner currentSpawner;

    public GameObject gameOverHUD;
    public GameObject startScreenHUD;

    public Camera mainCam;
    public float camMoveUp = 0.0001f;
    private Vector3 camPos;
    private float camMoveSpeed = 3f;

    public AudioSource sfxSound;
    public AudioClip stackSound;
    public AudioSource musicSound;
    public AudioClip backgroundMusic;

    private void Awake()
    {
        Time.timeScale = 0f;
        startScreenHUD.SetActive(true);
        gameOverHUD.SetActive(false);
        spawners = FindObjectsOfType<CubeSpawner>();

        camPos = mainCam.transform.position;

        if (musicSound != null && backgroundMusic != null)
        {
            musicSound.clip = backgroundMusic;
            musicSound.loop = true;
            musicSound.Play();
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !startScreenHUD.activeSelf)
        {
            if (MovingCube.CurrentCube != null)
            {
                MovingCube.CurrentCube.Stop();

                if (isGameActive)
                {   
                    GameOver();
                    isGameActive = false;
                }

            }

            
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            currentSpawner = spawners[spawnerIndex];

            currentSpawner.SpawnCube();
            OnCubeSpawned();

            if (sfxSound != null && stackSound != null)
            {
                sfxSound.PlayOneShot(stackSound);
            }

            camPos.y += camMoveUp;
        }
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos, Time.deltaTime * camMoveSpeed);
    }

    public void GameOver()
    {
        UnityEngine.Debug.Log("Gameover Called");
        gameOverHUD.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        startScreenHUD.SetActive(false);
        Time.timeScale = 1f;
        isGameActive = false;

        if (spawners != null)
        {
            spawners[0].SpawnCube();
        }
        else
        {
            UnityEngine.Debug.Log("CubeSpawner reference not assigned");
        }
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
