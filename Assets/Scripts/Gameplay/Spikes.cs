using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int SpikesDamage = 1;
    public Transform SpikesObject;
    public float UpSpeed = 1f;
    public float DownSpeed = 1f;
    public float TimeUp = 1f;

    private GameObject m_InsideObject;
    private bool m_Ready = true;

    private void OnTriggerEnter(Collider other)
    {
        if (m_Ready)
        {
            m_Ready = false;
            m_InsideObject = other.gameObject;
            StartCoroutine(MoveSpikes());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_InsideObject = null;
    }

    private IEnumerator MoveSpikes()
    {
        Vector3 downPosition = SpikesObject.position;
        Vector3 upPosition = SpikesObject.position;
        upPosition.y = downPosition.y + 2;
        m_InsideObject.SendMessage("Damage", SpikesDamage, SendMessageOptions.DontRequireReceiver);
        float step = (UpSpeed / (downPosition - upPosition).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step;
            SpikesObject.position = Vector3.Lerp(downPosition, upPosition, t);
            yield return new WaitForFixedUpdate();
        }
        SpikesObject.position = upPosition;
        yield return new WaitForSeconds(TimeUp);
        t = 1;
        while (t >= 0f)
        {
            t -= step;
            SpikesObject.position = Vector3.Lerp(downPosition, upPosition, t);
            yield return new WaitForFixedUpdate();
        }
        SpikesObject.position = downPosition;
        m_Ready = true;
    }
}
