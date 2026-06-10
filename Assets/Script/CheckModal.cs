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
    private Action _OnConfirm;
    private Action _OnCancel;

    public void Open(string title, string[] describes, string confirm="確定", string cancel="キャンセル", Action confirmAction=null, Action cancelAction=null)
    {
        TitleUI.text = title;
        DescribeUI.text = string.Join("\n",describes);
        ConfirmUI.text = confirm;
        CancelUI.text = cancel;

        _OnConfirm = confirmAction;
        _OnCancel = cancelAction;

        gameObject.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ConfirmButton.onClick.AddListener(() =>
        {
            _OnConfirm?.Invoke();
            Close();
        });

        CancelButton.onClick.AddListener(() =>
        {
            _OnCancel?.Invoke();
            Close();
        });
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
