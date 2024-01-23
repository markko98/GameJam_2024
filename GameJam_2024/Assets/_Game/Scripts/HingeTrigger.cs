
using UnityEngine;

public class HingeTrigger : MonoBehaviour
{
    Rigidbody rb;
    Vector3 velocity;
    bool isTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = new Vector3(0, 100, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }


    void FixedUpdate()
    {
        if (isTriggered)
        {
            Quaternion deltaRotation = Quaternion.Euler(velocity * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
            if (transform.rotation.y < -150)
            {
                isTriggered = false;
            }
        }
    }
}
