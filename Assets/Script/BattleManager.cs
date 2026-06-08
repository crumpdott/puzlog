using NUnit.Framework;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int TmpMoney = 0;
    public static BattleManager Instance { get; private set;}
    public Enemy[] Enemies;
    public int DamagePerElement = 1;
    public GameObject DeadDialog;

    private void Start()
    {
        Instance = this;
    }

    public void AttackEnemy(int elementCount)
    {
        int damage = elementCount * DamagePerElement;
        Enemy target = GetTargetEnemy();
        target.TakeDamage(damage);
        Enemy nextTarget = GetTargetEnemy();
        if(nextTarget == null)
        {
            EndTurn();
        }
        return;
    }

    private Enemy GetTargetEnemy()
    {
        foreach (var enemy in Enemies)
        {
            if(enemy != null && !enemy.isDead)
            {
                return enemy;
            }
        }
        return null;
    }

    public void PlayerDead()
    {
        DeadDialog.SetActive(true);
    }

    private void EndTurn()
    {
        PuzzleManager.Instance.StopPuzzle();
        ResultManager.Instance.StartResult();
    }
}
