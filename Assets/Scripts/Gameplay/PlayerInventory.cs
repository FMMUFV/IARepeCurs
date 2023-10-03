using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int CharacterGold
    {
        get { return m_GoldAmmount; }
        private set { m_GoldAmmount = value; }
    }
    public delegate void OnValueUpdate(int ammount);
    public event OnValueUpdate OnGoldUpdate;

    private int m_GoldAmmount = 0;

    public void Gold(int amount)
    {
        if (amount > 0)
        {
            m_GoldAmmount += amount;
            OnGoldUpdate?.Invoke(m_GoldAmmount);
        }
    }
}
