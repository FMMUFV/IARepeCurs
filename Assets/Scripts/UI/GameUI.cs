using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private VisualElement m_RootElement;
    private VisualElement m_Pointer;
    private Label m_HealthLabel;
    private Label m_GoldLabel;

    public void Start()
    {
        UnityEngine.Cursor.visible = false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController;
            Health playerHealth;
            PlayerInventory playerInventory;
            if (player.TryGetComponent<PlayerController>(out playerController))
            {
                playerController.OnAimUpdate += UpdatePointer;
            }
            if (player.TryGetComponent<Health>(out playerHealth))
            {
                playerHealth.OnHealthUpdate += UpdateHealth;
            }
            if (player.TryGetComponent<PlayerInventory>(out playerInventory))
            {
                playerInventory.OnGoldUpdate += UpdateGold;
            }
        }

        UIDocument doc = GetComponent<UIDocument>();
        m_RootElement = doc.rootVisualElement;
        m_Pointer = m_RootElement.Q<VisualElement>("Pointer");
        m_HealthLabel = m_RootElement.Q<Label>("HealthLabel");
        m_GoldLabel = m_RootElement.Q<Label>("GoldLabel");
    }

    private void OnDestroy()
    {
        UnityEngine.Cursor.visible = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController;
            Health playerHealth;
            PlayerInventory playerInventory;
            if (player.TryGetComponent<PlayerController>(out playerController))
            {
                playerController.OnAimUpdate -= UpdatePointer;
            }
            if (player.TryGetComponent<Health>(out playerHealth))
            {
                playerHealth.OnHealthUpdate -= UpdateHealth;
            }
            if (player.TryGetComponent<PlayerInventory>(out playerInventory))
            {
                playerInventory.OnGoldUpdate -= UpdateGold;
            }
        }
    }

    private void UpdatePointer(Vector2 position)
    {
        Vector2 mousePositionCorrected = new Vector2(position.x, Screen.height - position.y);
        mousePositionCorrected = RuntimePanelUtils.ScreenToPanel(m_RootElement.panel, mousePositionCorrected);
        m_Pointer.style.left = mousePositionCorrected.x;
        m_Pointer.style.top = mousePositionCorrected.y;
    }

    private void UpdateHealth(int ammount)
    {
        m_HealthLabel.text = ammount.ToString();
    }

    private void UpdateGold(int ammount)
    {
        m_GoldLabel.text = ammount.ToString();
    }
}
