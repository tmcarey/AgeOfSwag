using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour
{
    private bool _paused = false;

    public float time;
    public int day;

    private int _currentMultiplierIdx = 0;
    
    public List<float> multipliers = new List<float>();
    
    [Header("UI")]
    public GameObject pauseStatus;

    public TextMeshProUGUI speedText;
    
    public void OnPause(InputValue val)
    {
        _paused = !_paused;
        
        Time.timeScale = _paused ? 0 : 1;
        pauseStatus.SetActive(_paused);
    }

    public void OnChangeSpeed(InputValue val)
    {
        _currentMultiplierIdx = Math.Clamp(_currentMultiplierIdx + (int)val.Get<float>(), 0, multipliers.Count - 1);
        Time.timeScale = multipliers[_currentMultiplierIdx];
        speedText.text = multipliers[_currentMultiplierIdx] + "x";
    }

    private void Start()
    {
        pauseStatus.SetActive(_paused);
        Time.timeScale = multipliers[_currentMultiplierIdx];
        speedText.text = multipliers[_currentMultiplierIdx] + "x";
    }
}
