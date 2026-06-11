using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Attribute Attribute = Attribute.None;
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
        _Hp = MaxHp;
        UpdateHpText();
    }
    public void TakeDamage(int damage)
    {
        _Hp -= damage;
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
