using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject Ball;
    public int Speed;

    private Transform _cam;

    // Use this for initialization
    void Start()
    {
        _cam = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Shoot"))
        {
            Instantiate(Ball, _cam.position + _cam.forward * 2, _cam.rotation);
        }
    }

}