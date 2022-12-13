using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    private bool _paused = false;

    public float time;
    public int day;

    public GameObject pauseStatus;
    
    public void OnPause(InputValue val)
    {
        _paused = !_paused;
        
        Time.timeScale = _paused ? 0 : 1;
        pauseStatus.SetActive(_paused);
    }

    private void Start()
    {
        pauseStatus.SetActive(_paused);
    }
}
