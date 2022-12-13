using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HUDController : Singleton<HUDController>
{
    private Constructor _constructor;
    private bool _selectBoxOpen;
    private Vector2 _selectStartPosition;
    private Vector2 _mousePosition;

    public RectTransform selectBoxImage;
    public GameObject constructionPanel;
    public GameObject primaryPanel;

    [Header("Timing")] public RectTransform clock;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI clockText;

    [Header("InfoBox")] public InfoBox citizenInfoBox;

    private void Awake()
    {
        _constructor = GetComponent<Constructor>();
    }

    public void OnEndConstruction(InputValue val)
    {
        if(constructionPanel.activeSelf)
        {
            CloseConstruction();
        }
    }

    public void OpenConstruction()
    {
        constructionPanel.SetActive(true);
        primaryPanel.SetActive(false);
    }

    public Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(_mousePosition);
    }
    
    public void StartConstruction(StructureScriptableObject structure)
    {
        constructionPanel.SetActive(false);
        _constructor.StartConstruction(structure);
    }
    
    public void CloseConstruction()
    {
        constructionPanel.SetActive(false);
        primaryPanel.SetActive(true);
    }

    public void OnClick(InputValue val)
    {
        _selectBoxOpen = val.Get<float>() > 0;
        selectBoxImage.GetComponent<Image>().enabled = _selectBoxOpen;
        if (_selectBoxOpen)
        {
            _selectStartPosition = _mousePosition;
        }

        if(Physics.Raycast(Camera.main.ScreenPointToRay(_mousePosition), out var hit))
        {
            if(hit.collider.gameObject.TryGetComponent(out Citizen citizen))
            {
                citizenInfoBox.SelectCitizen(citizen);
            }
        }
    }

    public void OnMousePosition(InputValue val)
    {
        _mousePosition = val.Get<Vector2>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _selectBoxOpen = false;
        selectBoxImage.GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_selectBoxOpen)
        {
            selectBoxImage.position = _selectStartPosition;
            Vector2 mouseDelta = _mousePosition - _selectStartPosition;
            selectBoxImage.localScale = new Vector3(mouseDelta.x < 0 ? -1.0f : 1.0f, mouseDelta.y > 0 ? -1.0f : 1.0f, 1);
            selectBoxImage.sizeDelta = new Vector2(Mathf.Abs(mouseDelta.x), Mathf.Abs(mouseDelta.y));
        }

        {
            clock.rotation = Quaternion.Euler(0, 0, -90.0f + 360.0f * GameManager.Instance.currentTime.time);
            dayText.text = "Day " + GameManager.Instance.currentTime.day.ToString();
            clockText.text = GameManager.Instance.currentTime.time.ToString("0.00");
        }
    }
}
