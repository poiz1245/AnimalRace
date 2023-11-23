using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    float movex;
    public float speed =5f;
    float movez;

    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movex = Input.GetAxis("Horizontal");
        movez = Input.GetAxis("Vertical");

        rigid.velocity = new Vector3(movex * speed, rigid.velocity.y, movez * speed);
    }
}
