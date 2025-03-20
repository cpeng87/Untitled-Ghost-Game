using UnityEngine;

public class PotCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BobaCollision>())
        {
            collision.transform.parent.SetParent(transform, true);
            if (collision.transform.GetComponent<Rigidbody>().linearVelocity.magnitude > 0f)
                collision.transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log(collision.transform.GetComponent<Rigidbody>().linearVelocity.magnitude);
        if (collision.gameObject.GetComponent<BobaCollision>())
        {
            if (collision.transform.GetComponent<Rigidbody>().linearVelocity.magnitude > 6f)
            {
                collision.gameObject.transform.localPosition = Vector3.zero;
            }
            collision.transform.parent.SetParent(null, true);
        }
    }
}