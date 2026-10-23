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
    public GameObject CriticalSignifier;

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
        bool critical = (Effect & ElementEffect.Critical) != 0;
        if (GoldSignifier != null)
        {
            GoldSignifier.SetActive((Effect & ElementEffect.Gold) != 0);
        }
        if (CriticalSignifier != null)
        {
            CriticalSignifier.SetActive(critical);
        }
        else
        {
            var sprite = GetComponent<SpriteRenderer>();
            if (sprite != null) sprite.color = critical ? new Color(0.65f, 1f, 1f) : Color.white;
        }
    }
}
