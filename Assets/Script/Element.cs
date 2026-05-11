using System;
using UnityEngine;

public class Element : MonoBehaviour
{
    public string ID;
    public GameObject SelectSprite;
    public bool IsSelect { get; private set; }

    private void OnMouseDown()
    {
        LevelManager.Instance.ElementDown(this);
    }

    private void OnMouseEnter()
    {
        LevelManager.Instance.ElementEnter(this);
    }

    private void OnMouseUp()
    {
        LevelManager.Instance.ElementUp(this);
    }

    public void SetIsSelect(Boolean isSelect)
    {
        IsSelect = isSelect;
        SelectSprite.SetActive(isSelect);
    }
}
