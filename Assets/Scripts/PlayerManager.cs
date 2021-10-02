using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D body;

    [Header("MOVEMENT")]
    public float[] movementAngles;
    private int index = 0;
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
            index++;
            if (index > movementAngles.Length)
                index = 0;
            angle = movementAngles[index];
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            index--;
            if (index < 0)
                index = movementAngles.Length - 1;
            angle = movementAngles[index];
        }
        Vector2 newDirection =new Vector2( Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
        direction = Vector2.Lerp(direction, newDirection, Time.deltaTime);
        body.velocity = direction * movementSpeed;
        float angleRotation = Mathf.Atan2(body.velocity.y, body.velocity.x) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.Euler(new Vector3(0, 0, angleRotation));
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
    }
}
