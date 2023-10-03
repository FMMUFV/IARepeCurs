using System.Collections;
using UnityEngine;

public class Chest : Interactable
{
    public float LidSpeed = 10000f;
    public float ChestDistance = 4f;

    private bool m_ClosedLid = true;

    public override void Interact(GameObject sender)
    {
        if (m_ClosedLid && sender.CompareTag("Player") && (Vector3.Distance(sender.transform.position, transform.position) < ChestDistance))
        {
            m_ClosedLid = false;
            StartCoroutine(OpenLid());
        }
    }

    private IEnumerator OpenLid()
    {
        float currentAngle = transform.localEulerAngles.x;
        float targetAngle = 270f;
        float difference = Mathf.Abs(Mathf.DeltaAngle(targetAngle, currentAngle));
        float step = (LidSpeed / 90f) * Time.fixedDeltaTime;
        currentAngle = 0;
        while (difference - currentAngle > 0)
        {
            currentAngle += step;
            transform.Rotate(-step, 0f, 0f, Space.Self);
            yield return new WaitForFixedUpdate();
        }

        transform.localEulerAngles = new Vector3(targetAngle, 0f, 0f);
    }
}
