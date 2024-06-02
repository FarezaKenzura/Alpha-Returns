using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaReturns.System.Unit
{
    public class Characters : MonoBehaviour
    {
        [Header("Base Stats")]
        public HealthStat healthPoint;
        public Stat attack;
        public Stat DefenseModifier;
        public ActionPointStat actionPoint;
        public Stat criticalChance;

        private BattleHUD battleHUD;

        private void Start()
        {
            battleHUD = FindObjectOfType<BattleHUD>();
        }

        public void UpdateHealthBar()
        {
            if (battleHUD != null)
                battleHUD.UpdateHUD();
        }

        public void UpdateActionPointBar()
        {
            if (battleHUD != null)
                battleHUD.UpdateHUD();
        }
    }
}
