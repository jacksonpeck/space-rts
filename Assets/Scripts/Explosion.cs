using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float AlphaDissipation;
    public float ExpansionRate;
    public Vector2 Velocity;

    private SpriteRenderer _render;

    private void Awake()
    {
        _render = GetComponent<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        float alpha = _render.color.a - Time.deltaTime * AlphaDissipation;
        if (alpha < 0.0f)
        {
            Destroy(this.gameObject);
            return;
        }

        Color color = _render.color;
        _render.color = new Color(color.r, color.g, color.b, alpha);

        float size = (transform.localScale.x + Time.deltaTime * ExpansionRate);
        this.transform.localScale = size * Vector3.one;

        this.transform.position += Time.deltaTime * (Vector3)Velocity;
    }
}
