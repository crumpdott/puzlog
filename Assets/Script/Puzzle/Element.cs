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
    public GameObject InstantKillSignifier;
    public bool IsExplosion;

    private void Awake()
    {
        UpdateEffectSignifier();
    }

    private void OnMouseDown()
    {
        if (IsExplosion) PuzzleManager.Instance.Explode(this);
        else PuzzleManager.Instance.ElementDown(this);
    }

    private void OnMouseEnter()
    {
        if (!IsExplosion) PuzzleManager.Instance.ElementEnter(this);
    }

    private void OnMouseUp()
    {
        if (!IsExplosion) PuzzleManager.Instance.ElementUp();
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
        if (InstantKillSignifier != null)
        {
            InstantKillSignifier.SetActive((Effect & ElementEffect.InstantKill) != 0);
        }
    }
}
