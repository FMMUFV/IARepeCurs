using UnityEngine;

public class CharacterEventReceiver : MonoBehaviour
{
    public void AttackEffect()
    {
        transform.parent.SendMessage("AttackEffect", SendMessageOptions.DontRequireReceiver);
    }

    public void EndAttack()
    {
        transform.parent.SendMessage("EndAttack", SendMessageOptions.DontRequireReceiver);
    }
}
