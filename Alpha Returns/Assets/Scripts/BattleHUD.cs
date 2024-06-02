using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace AlphaReturns.System.Unit
{
    public class BattleHUD : MonoBehaviour 
    {
        [SerializeField] private Slider healthPointSlider;
        [SerializeField] private Slider actionPointSlider;

        private Characters currentCharacter;

        public void SetHUD(Characters unit)
        {
            currentCharacter = unit;
            UpdateHUD();
        }

        public void UpdateHUD()
        {
            if (currentCharacter != null)
            {
                healthPointSlider.maxValue = currentCharacter.healthPoint.MaxHealth;
                healthPointSlider.value = currentCharacter.healthPoint.CurrentHealth;

                actionPointSlider.maxValue = currentCharacter.actionPoint.MaxActionPoints;
                actionPointSlider.value = currentCharacter.actionPoint.CurrentActionPoints;
            }
        }

        public void SetHP(int hp)
        {
            if (currentCharacter != null)
            {
                currentCharacter.healthPoint.CurrentHealth = hp;
                healthPointSlider.value = hp;
            }
        }
    }
}