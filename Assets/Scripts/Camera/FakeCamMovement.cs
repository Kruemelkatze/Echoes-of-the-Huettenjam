using System;
using UnityEngine;

public class FakeCamMovement : MonoBehaviour
{
    [SerializeField] private float speed = 45;


    private void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        // rotate camera based on horizontal and vertical around own position
        transform.RotateAround(transform.position, Vector3.up, horizontal * speed * Time.deltaTime);
        transform.RotateAround(transform.position, transform.right, -vertical * speed * Time.deltaTime);
    }
}