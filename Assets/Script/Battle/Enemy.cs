using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public ElementAttribute Attribute = ElementAttribute.None;
    public int MaxHp = 1;
    private int _Hp = 1;
    public TextMeshPro HpText;
    public int Money = 0;
    public bool isDead
    {
        get {return _Hp <= 0; }
    }

    public void Start()
    {
        RefreshForRound();
    }
    public void RefreshForRound()
    {
        _Hp = MaxHp;
        UpdateHpText();
        var renderer = GetComponentInChildren<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Attribute == ElementAttribute.Fire ? new Color(1f, 0.56f, 0.48f)
                : Attribute == ElementAttribute.Water ? new Color(0.48f, 0.74f, 1f)
                : new Color(0.54f, 0.9f, 0.55f);
        }
    }
    public void TakeDamage(int damage)
    {
        _Hp = Mathf.Max(0, _Hp - Mathf.Max(0, damage));
        if(_Hp <= 0)
        {
            Destroy(gameObject);
            BattleManager.Instance.TmpMoney += Money;
            return;
        }
        UpdateHpText();
    }

    private void UpdateHpText()
    {
        HpText.text = _Hp + "/" + MaxHp;
    }
}
