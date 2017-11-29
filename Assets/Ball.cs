using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int Speed;
    public int AliveTime;

    public AudioClip Hit;
    public Score score;
    private AudioSource _audioSource;

    void Start ()
	{
        _audioSource = GetComponent<AudioSource>();
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed, ForceMode.VelocityChange);
	}

	void Update ()
	{
        Destroy(gameObject, AliveTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Unitychan"))
        {

        }
        else
        {
            _audioSource.PlayOneShot(Hit);
        }
    }

}