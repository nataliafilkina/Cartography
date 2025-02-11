using Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UISaveButton : MonoBehaviour
{
    private PinsEditor _pinsEditor;
    private Button _button;

    [Inject]
    public void Construct(PinsEditor pinsEditor)
    {
        _pinsEditor = pinsEditor;
        _pinsEditor.OnFirstChanged += HasChanged;
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if(_button != null ) 
            _button.onClick.AddListener(OnClick);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _pinsEditor.Save();
        gameObject.SetActive(false);
    }

    private void HasChanged()
    {
        gameObject.SetActive(true);
    }
}
