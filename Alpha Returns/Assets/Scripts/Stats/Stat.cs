using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlphaReturns.System.Unit {
    [Serializable]
    public class Stat {
        // Nilai dasar statistik
        public float BaseValue;

        // Menghitung nilai akhir statistik
        public virtual float Value {
            get {
                if (isDirty || lastBaseValue != BaseValue) {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true; // Menandakan perlu atau tidak perlu perhitungan ulang nilai akhir
        protected float _value; // Nilai akhir statistik
        protected float lastBaseValue = float.MinValue; // Nilai dasar statistik terakhir

        protected readonly List<StatModifier> statModifiers; // Daftar modifikasi statistik
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // Daftar modifikasi statistik yang hanya bisa dibaca

        // Konstruktor untuk statistik
        public Stat() {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        // Konstruktor overload untuk statistik dengan nilai dasar tertentu
        public Stat(float baseValue) : this() {
            BaseValue = baseValue;
        }

        // Metode untuk menambahkan modifikasi statistik
        public virtual void AddModifier(StatModifier mod) {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        // Metode untuk menghapus modifikasi statistik
        public virtual bool RemoveModifier(StatModifier mod) {
            if (statModifiers.Remove(mod)) {
                isDirty = true;
                return true;
            }
            return false;
        }

        // Metode untuk menghapus semua modifikasi dari suatu sumber
        public virtual bool RemoveAllModifiersFromSource(object source) {
            bool didRemove = false;
        
            for (int i = statModifiers.Count - 1; i >= 0; i--) {
                if (statModifiers[i].Source == source) {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }
        
        // Metode untuk membandingkan urutan modifikasi
        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b) {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }

        // Metode untuk menghitung nilai akhir statistik
        protected virtual float CalculateFinalValue() {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++) {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                    finalValue += mod.Value;
                else if (mod.Type == StatModType.PercentAdd) {
                    sumPercentAdd += mod.Value;

                    // Jika tidak ada modifikasi persentase tambahan lagi, hitung hasilnya
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd) {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                    finalValue *= 1 + mod.Value;
            }

            // Mengembalikan nilai akhir dengan pembulatan
            return (float)Math.Round(finalValue, 4);
        }
    }
}