﻿using UnityEngine;
using System.Collections;

public class UI_Selector_Indicator : MonoBehaviour {

    private SpriteRenderer sprite;

    public float rotation_Speed;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Update ()
    {
        Rotate();
    }

    private void Rotate()
    {
        float speed = rotation_Speed * Time.deltaTime * 100;

        transform.Rotate(0, 0, speed);
    }

    public void Activate()
    {
        sprite.enabled = true;
    }

    public void Deactivate()
    {
        sprite.enabled = false;
    }
}