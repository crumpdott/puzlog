using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 1;
    public int Hp = 1;
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if(Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
