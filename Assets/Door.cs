using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    void OnTriggerEnter(Collider other) {
        SceneManager.LoadScene("Pong");
    }
    void OnCollisionEnter(Collision collision) {
        SceneManager.LoadScene("Pong");
    }
}
