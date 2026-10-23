using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public int TmpMoney = 0;
    public static BattleManager Instance { get; private set;}
    public Enemy[] Enemies;
    public GameObject DeadDialog;
    public float WeakRate;
    public float ResistRate;

    private void Start()
    {
        Instance = this;
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] == null) continue;
            Enemies[i].Attribute = (ElementAttribute)(1 + (PlayerData.Round - 1) % 3);
            Enemies[i].MaxHp = PlayerData.Round * 10;
            Enemies[i].Money = 10;
            Enemies[i].RefreshForRound();
        }
    }

    public void AttackEnemy(List<Element> elements)
    {
        Enemy target = GetTargetEnemy();
        if (target == null || elements.Count == 0) return;
        ElementAttribute attribute = elements[0].Attribute;
        float damage = elements.Count + 2f * (1f + PlayerData.AttributeAttackPower[(int)attribute]) + PlayerData.CurrentBaseAttackPower;
        float affinity = 1f;
        if(ElementAttributeCompatibility.IsWeak(elements[0].Attribute, target.Attribute))
        {
            affinity = WeakRate;
        } else if (ElementAttributeCompatibility.IsResist(elements[0].Attribute, target.Attribute))
        {
            affinity = ResistRate;
        }
        bool critical = elements.Exists(element => (element.Effect & ElementEffect.Critical) != 0);
        damage = Mathf.Max(0f, damage * affinity * (critical ? PlayerData.CriticalMultiplier : 1f));
        int roundedDamage = (int)MathF.Round(damage, 0, MidpointRounding.AwayFromZero);
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
