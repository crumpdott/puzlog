using System;
using UnityEngine;

public class Element : MonoBehaviour
{
    public string ID;
    public GameObject SelectSprite;
    public bool IsSelect { get; private set; }

    private void OnMouseDown()
    {
        PuzzleManager.Instance.ElementDown(this);
    }

    private void OnMouseEnter()
    {
        PuzzleManager.Instance.ElementEnter(this);
    }

    private void OnMouseUp()
    {
        PuzzleManager.Instance.ElementUp();
    }

    public void SetIsSelect(Boolean isSelect)
    {
        IsSelect = isSelect;
        SelectSprite.SetActive(isSelect);
    }
}
