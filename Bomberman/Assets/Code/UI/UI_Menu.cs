﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Menu : MonoBehaviour {
    public GameObject Player;
    public void Resume()
    {
        Player.GetComponent<Shooting_physics>().WaitTime = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    public void Quit()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Destroy(GameObject.Find("Stats"));
    }
    public void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
