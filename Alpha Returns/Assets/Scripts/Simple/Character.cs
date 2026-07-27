using UnityEngine;

public class Character : MonoBehaviour
{
    public string Name;
    public int Hp;
    public int Attack;

    private void Serang(Character target)
    {
        target.Hp -= this.Attack;
        Debug.Log($"{Name} menyerang {target.Name} sebesar {Attack} damage!");

        if (target.Hp <= 0)
        {
            Debug.Log($"{target.Name} telah Kalah!");
        }
    }
}
