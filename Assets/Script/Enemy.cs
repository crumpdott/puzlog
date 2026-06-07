using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 1;
    private int _Hp = 1;
    public TextMeshPro HpText;

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
            return;
        }
        UpdateHpText();
    }

    private void UpdateHpText()
    {
        HpText.text = _Hp + "/" + MaxHp;
    }
}
