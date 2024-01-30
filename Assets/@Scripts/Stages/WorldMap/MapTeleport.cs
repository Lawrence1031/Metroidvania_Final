using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapTeleport : MonoBehaviour
{
    // 순간이동 위치 - 마을, 포탈1(시작부분), 포탈2(보스방 앞), 포탈3(미니보스방)
    // 포탈4(TD부분), 포탈5(Stage 2 입구), 포탈6(Stage 3 입구)

    [SerializeField] 
    private GameObject _checkCanvas;
    private int _selectedButtonIndex;

    public Button[] portalButtons;
    public Transform[] portalLocations;
    public TextMeshProUGUI portalText;

    private void Awake()
    {
        _checkCanvas.SetActive(false);

        for (int i = 0; i < portalButtons.Length; i++)
        {
            int index = i;
            portalButtons[i].onClick.AddListener(() => ClickTeleport(index));
        }
    }

    public void ClickTeleport(int index)
    {
        _selectedButtonIndex = index;
        PortalText(index);
        _checkCanvas.SetActive(true);

        if (index != 0)
        {
            MapManager.Instance.moveMapCamera(portalLocations[_selectedButtonIndex].position);
        }
    }

    public void ClickYes()
    {
        _checkCanvas.SetActive(false);
        MapManager.Instance.CloseLargeMap();
        Teleport(_selectedButtonIndex);
    }

    public void ClickNo()
    {
        _checkCanvas.SetActive(false);
    }

    private void Teleport(int index)
    {
        GameManager.Instance.player.transform.position = portalLocations[index].position;
    }

    private void PortalText(int index)
    {
        if (index == 0)
        {
            portalText.text = "마을(으/로)\r\n" + "이동하시겠습니까?";
        }
        else
        {
            portalText.text = "포탈" + index + "(으/로)\r\n" + "이동하시겠습니까?";
        }
    }
}
