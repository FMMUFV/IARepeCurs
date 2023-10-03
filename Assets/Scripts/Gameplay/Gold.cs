using UnityEngine;

public class Gold : Interactable
{
    public enum InteractionDestroy { DoNothing, DestroyObject, DestroyInteraction };
    public float GoldDistance = 4f;
    public int Ammount = 1;
    public InteractionDestroy AutoDestroy = InteractionDestroy.DoNothing;

    public override void Interact(GameObject sender)
    {
        if (sender.CompareTag("Player") && (Vector3.Distance(sender.transform.position, transform.position) < GoldDistance))
        {
            sender.SendMessage("Gold", Ammount, SendMessageOptions.DontRequireReceiver);
            switch (AutoDestroy)
            {
                case InteractionDestroy.DestroyObject:
                    Destroy(gameObject); break;
                case InteractionDestroy.DestroyInteraction:
                    Destroy(this); break;
            }
        }
    }
}
