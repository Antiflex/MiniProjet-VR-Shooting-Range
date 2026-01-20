using UnityEngine;

public class TargetSmoother : MonoBehaviour
{
    public Transform rawTarget;
    public float positionLerp = 15f;
    public float rotationLerp = 15f;

    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            rawTarget.position,
            Time.deltaTime * positionLerp
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rawTarget.rotation,
            Time.deltaTime * rotationLerp
        );
    }
}
