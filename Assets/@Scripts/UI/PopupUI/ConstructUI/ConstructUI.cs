using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ConstructUI : MonoBehaviour
{
    #region properties
    public ConstructInformPanel InformPanel { get; private set; }
    public ConstructList BuildingList { get; private set; }
    #endregion

    #region variables
    private bool _isInitialized = false;
    private GameObject _exitButton;
    #endregion

    private void Awake() 
    {
        // ExitButton
        _exitButton = transform.Find("ExitButton").gameObject;
        Button button = _exitButton.GetComponent<Button>();
        button.onClick.AddListener( () => { gameObject.SetActive(false);});

        InformPanel = GetComponentInChildren<ConstructInformPanel>();
        BuildingList = GetComponentInChildren<ConstructList>();

        InformPanel.Initialize(this);
        BuildingList.Initialize(this);

        _isInitialized = true;
    }

    private void OnEnable()
    {
        if(!_isInitialized) return;

        _exitButton.SetActive(false);
        transform.position = new Vector3(960, 2160, 0);
        transform.DOMoveY(540, 1).SetEase(Ease.OutBack);
        TimerManager.Instance.StartTimer(1f, () => _exitButton.SetActive(true));   
    }
}
