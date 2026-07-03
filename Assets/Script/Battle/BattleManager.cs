using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int TmpMoney = 0;
    public static BattleManager Instance { get; private set;}
    public Enemy[] Enemies;
    public int DamagePerElement = 1;
    public GameObject DeadDialog;
    public float WeakRate;
    public float ResistRate;

    private void Start()
    {
        Instance = this;
    }

    public void AttackEnemy(List<Element> elements)
    {
        float damage = elements.Count * DamagePerElement;
        Enemy target = GetTargetEnemy();
        if(ElementAttributeCompatibility.IsWeak(elements[0].Attribute, target.Attribute))
        {
            damage *= WeakRate;
        } else if (ElementAttributeCompatibility.IsResist(elements[0].Attribute, target.Attribute))
        {
            damage *= ResistRate;
        }

        int roundedDamage = (int)MathF.Round(damage,0,MidpointRounding.AwayFromZero);
        target.TakeDamage(roundedDamage);
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
