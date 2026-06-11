using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckModal : MonoBehaviour
{
    public TextMeshProUGUI TitleUI;
    public TextMeshProUGUI DescribeUI;
    public TextMeshProUGUI ConfirmUI;
    public TextMeshProUGUI CancelUI;
    public Button ConfirmButton;
    public Button CancelButton;
    public GameObject CancelButtonRoot;
    private Action _OnConfirm;
    private Action _OnCancel;

    public void Open(string title, string[] describes, string confirm="確定", string cancel="キャンセル", Action confirmAction=null, Action cancelAction=null)
    {
        TitleUI.text = title;
        DescribeUI.text = string.Join("\n",describes);
        ConfirmUI.text = confirm;
        bool showCancelButton = cancel != null;
        CancelButtonRoot.gameObject.SetActive(showCancelButton);
        if(showCancelButton){
            CancelUI.text = cancel;
        }

        _OnConfirm = confirmAction;
        _OnCancel = cancelAction;

        gameObject.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ConfirmButton.onClick.AddListener(() =>
        {
            Action onConfirm = _OnConfirm;
            Close();
            onConfirm?.Invoke();
        });

        CancelButton.onClick.AddListener(() =>
        {
            Action onCancel = _OnCancel;
            Close();
            onCancel?.Invoke();
        });
    }

    public void Close()
    {
        _OnConfirm = null;
        _OnCancel = null;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
