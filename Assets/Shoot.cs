using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    public GameObject Ball;

    void Update() {
        if (Input.GetButtonDown("Shoot")) {
            Instantiate(Ball, transform.position + transform.forward * 2, transform.rotation);
        }
    }
}