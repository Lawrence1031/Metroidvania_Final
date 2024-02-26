using UnityEngine;
using UnityEngine.UI;

public class ConstructButton : MonoBehaviour
{
    #region variables
    private int _currentID;
    private Button _button;
    private GameObject _YNPanel;
    private ConstructUI _constructUI;
    #endregion

    public void Initialize(ConstructUI constructUI)
    {
        _button = GetComponent<Button>();
        _YNPanel = Resources.Load<GameObject>("UI/ConstructYNPanel");

        _constructUI = constructUI;
        _button.onClick.AddListener(OnPopupYN);
    }

    public void Disable()
    {
        _button.enabled = false;
    }

    public void Enable(int ID)
    {
        _currentID = ID;
        _button.enabled = true;
    }

    private void OnPopupYN()
    {
        ConstructYNPanel YNPanel = Instantiate(_YNPanel, UIManager.Instance.TempUI).GetComponent<ConstructYNPanel>();
        YNPanel.Initialize(_currentID);
        YNPanel.InitAction(_constructUI);
    }
}