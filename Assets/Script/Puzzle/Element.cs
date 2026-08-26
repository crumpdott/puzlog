using System;
using UnityEngine;

public class Element : MonoBehaviour
{
    public string ID;
    public GameObject SelectSprite;
    public bool IsSelect { get; private set; }
    public ElementAttribute Attribute = ElementAttribute.None;
    public ElementEffect Effect = ElementEffect.None;
    public GameObject GoldSignifier;

    private void Awake()
    {
        UpdateEffectSignifier();
    }

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

    public void SetEffect(ElementEffect effect)
    {
        Effect = effect;
        UpdateEffectSignifier();
    }

    private void UpdateEffectSignifier()
    {
        if (GoldSignifier != null)
        {
            GoldSignifier.SetActive(Effect == ElementEffect.Gold);
        }
    }
}
