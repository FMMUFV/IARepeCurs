using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    public bool Player;
    public int CharacterHealth
    {
        get { return m_Health; }
        private set { m_Health = value; }
    }
    public delegate void OnValueUpdate(int ammount);
    public event OnValueUpdate OnHealthUpdate;

    private int m_Health;

    private void Start()
    {
        m_Health = MaxHealth;
    }

    public void Damage(int ammount)
    {
        m_Health -= ammount;
        OnHealthUpdate?.Invoke(m_Health);
        if (m_Health <= 0)
        {
          
            if(Player == true)
            {
                SceneManager.LoadScene(0);
            }
            {
                Debug.Log("Enemigo");
            }
        }
    }
}
