using System;
using UnityEngine;

namespace AlphaReturns.System.Unit 
{
    [Serializable]
    public class ActionPointStat : Stat {
        // Properti untuk Action Point maksimum
        public float MaxActionPoints {
            get {
                return Value; // Nilai maksimum Action Points sama dengan nilai akhir dari Stat
            }
        }

        // Properti untuk Action Point saat ini
        private float _currentActionPoints;
        public float CurrentActionPoints {
            get {
                return _currentActionPoints;
            }
            set {
                // Pastikan Action Point saat ini tidak melebihi Action Point maksimum
                _currentActionPoints = Mathf.Clamp(value, 0, MaxActionPoints);
            }
        }

        // Konstruktor default
        public ActionPointStat() : base() {
            CurrentActionPoints = MaxActionPoints;
        }

        // Konstruktor dengan nilai dasar
        public ActionPointStat(float baseValue) : base(baseValue) {
            CurrentActionPoints = MaxActionPoints;
        }

        // Metode untuk menggunakan Action Points
        public void UseActionPoints(float points) {
            CurrentActionPoints -= points;
            if (CurrentActionPoints < 0) {
                CurrentActionPoints = 0;
            }
        }

        // Metode untuk memulihkan Action Points
        public void RecoverActionPoints(float points) {
            CurrentActionPoints += points;
            if (CurrentActionPoints > MaxActionPoints) {
                CurrentActionPoints = MaxActionPoints;
            }
        }

        // Metode untuk mengatur ulang Action Points (misalnya setelah ronde baru)
        public void ResetActionPoints() {
            CurrentActionPoints = MaxActionPoints;
        }
    }
}