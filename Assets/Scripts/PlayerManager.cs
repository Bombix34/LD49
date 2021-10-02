using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D body;

    [Header("MOVEMENT")]
    public float movementSpeed = 1f;
    private Vector2 direction = Vector2.up;
    private float angle = 0;

    [Header("ROTATION")]
    public float rotationSpeed = 1f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            angle += 0.5f;
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            angle -= 0.5f;
        }
        Vector2 newDirection =new Vector2( Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
        direction = Vector2.Lerp(direction, newDirection, Time.deltaTime);
        body.velocity = direction * movementSpeed;
        float angleRotation = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.Euler(new Vector3(0, 0, angleRotation));
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }
}
