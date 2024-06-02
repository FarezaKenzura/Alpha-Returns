using System;
using UnityEngine;

namespace AlphaReturns.System.Unit 
{
    [Serializable]
    public class HealthStat : Stat {
        // Properti untuk kesehatan maksimum
        public float MaxHealth {
            get {
                return Value; // Nilai maksimum kesehatan sama dengan nilai akhir dari Stat
            }
        }

        // Properti untuk kesehatan saat ini
        private float _currentHealth;
        public float CurrentHealth {
            get {
                return _currentHealth;
            }
            set {
                // Pastikan kesehatan saat ini tidak melebihi kesehatan maksimum
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            }
        }

        // Konstruktor default
        public HealthStat() : base() {
            CurrentHealth = MaxHealth;
        }

        // Konstruktor dengan nilai dasar
        public HealthStat(float baseValue) : base(baseValue) {
            CurrentHealth = MaxHealth;
        }

        // Metode untuk menerima kerusakan
        public void TakeDamage(float damage) {
            CurrentHealth -= damage;
            if (CurrentHealth < 0) {
                CurrentHealth = 0;
            }
        }

        // Metode untuk penyembuhan
        public void Heal(float healAmount) {
            CurrentHealth += healAmount;
            if (CurrentHealth > MaxHealth) {
                CurrentHealth = MaxHealth;
            }
        }

        // Metode untuk mengatur ulang kesehatan (misalnya setelah level up)
        public void ResetHealth() {
            CurrentHealth = MaxHealth;
        }
    }
}