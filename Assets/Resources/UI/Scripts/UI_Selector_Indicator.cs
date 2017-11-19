using UnityEngine;

public class UI_Selector_Indicator : MonoBehaviour {

    private SpriteRenderer sprite;

    public float rotation_Speed;

    public SpriteRotator rotator;

    private void Start()
    {
        rotator.enabled = true;
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update ()
    {
        Rotate();
    }

    public void Activate()
    {
        sprite.enabled = true;
    }

    public void Deactivate()
    {
        sprite.enabled = false;
    }

    private void Rotate()
    {
        if (sprite.enabled)
        {
            float speed = rotation_Speed * Time.deltaTime * 100;

            sprite.transform.Rotate(0, 0, speed);
        }
    }
}
