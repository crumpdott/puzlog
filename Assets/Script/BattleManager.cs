using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set;}

    public Enemy[] Enemies;
    public int DamagePerElement = 1;

    private void Awake()
    {
        Instance = this;
    }

    public void AttackEnemy(int elementCount)
    {
        int damage = elementCount * DamagePerElement;
        Enemy target = GetTargetEnemy();
        if(target != null){
            target.TakeDamage(damage);
            return;
        }
    }

    private Enemy GetTargetEnemy()
    {
        foreach (var enemy in Enemies)
        {
            if(enemy != null)
            {
                return enemy;
            }
        }
        return null;
    }
}
