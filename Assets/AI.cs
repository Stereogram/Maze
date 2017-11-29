using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {
    public Transform Target;
    public float Speed;
    public float TurnSpeed;

    private Animator _animator;
    private bool _isColliding = false;

    void Start() {
        _animator = GetComponent<Animator>();
        _animator.speed = 1.5f;
    }

    void Update() {
        var lookPos = Target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurnSpeed);

        if (_isColliding) {
            if (lookPos.x < 0) {
                transform.Rotate(Vector3.up, Mathf.LerpAngle(transform.rotation.eulerAngles.y, 30, Time.deltaTime));
            } else {
                transform.Rotate(Vector3.up, Mathf.LerpAngle(transform.rotation.eulerAngles.y, -30, Time.deltaTime));
            }
        } else {
            _animator.SetFloat("Direction", 0);
        }

        if (Vector3.Distance(transform.position, Target.position) > 5) {
            _animator.SetFloat("Speed", 1);
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        } else {
            _animator.SetFloat("Speed", 0);
        }

        if (Vector3.Distance(transform.position, Target.position) > 30) {
            transform.position = Target.position - (Target.forward * 5);
        }
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.transform.tag != "Player" && collision.transform.tag != "Finish") {
            _isColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.transform.tag != "Player" && collision.transform.tag != "Finish") {
            _isColliding = false;
        }
    }

}
