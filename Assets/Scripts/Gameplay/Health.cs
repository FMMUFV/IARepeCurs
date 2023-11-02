using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    public bool PasarEstun;


    public int CharacterHealth
    {
        get { return m_Health; }
        private set { m_Health = value; }
    }
    public delegate void OnValueUpdate(int ammount);
    public event OnValueUpdate OnHealthUpdate;
    [HideInInspector]
    public int m_Health;

 

    private void Start()
    {
        PasarEstun = false;
        m_Health = MaxHealth;
    }

    public void Damage(int ammount)
    {
        PasarEstun = true;
        m_Health -= ammount;
        OnHealthUpdate?.Invoke(m_Health);
        if (m_Health <= 0)
        {
          
            if(this.gameObject.tag == "Player")
            {
                SceneManager.LoadScene(0);
            }
            {
               
                this.gameObject.SetActive(false);
            }
        }
    }
}
