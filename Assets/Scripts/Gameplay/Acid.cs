using UnityEngine;

public class Acid : MonoBehaviour
{
    public int AcidDamage = 1;

    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("Damage", AcidDamage, SendMessageOptions.DontRequireReceiver);
    }
}
