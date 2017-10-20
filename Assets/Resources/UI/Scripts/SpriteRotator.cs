using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    Quaternion cameraRotation;
    Quaternion spriteRotation;
    Transform tForm;

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
