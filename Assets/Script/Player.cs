using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] GameObject scythePrefab;
    [SerializeField] float scytheTime = 2;
    float currentScytheTimer;
    Rigidbody2D rb;

    private void Start()
    {
            rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        currentScytheTimer -= Time.deltaTime;
        if (currentScytheTimer <= 0)
        {
            //Spawn le Scythe
            Instantiate(scythePrefab, transform.position, Quaternion.identity);
            currentScytheTimer += scytheTime;
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(x, y) * MoveSpeed;
    }
}
