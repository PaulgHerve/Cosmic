using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    SpriteRenderer spriteRend;
    Quaternion cameraRotation;
    Quaternion spriteRotation;
    Transform tForm;

    private void Awake()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        SetRotationToCamera();
    }

    private void SetRotationToCamera()
    {
        cameraRotation = Camera.main.transform.rotation;

        spriteRotation = new Quaternion(-cameraRotation.x, -cameraRotation.y, -cameraRotation.z, -cameraRotation.w);

        transform.rotation = spriteRotation;
    }
}
