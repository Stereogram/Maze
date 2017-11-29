using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int Speed;
    public int AliveTime;

    public AudioClip Hit;
    private AudioSource _audioSource;
    private Score _score;

    void Start ()
	{
        _score = GameObject.Find("Canvas").GetComponent<Score>();
        _audioSource = GameObject.Find("FirstPersonCharacter").GetComponent<AudioSource>();
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed, ForceMode.VelocityChange);
	}

	void Update ()
	{
        Destroy(gameObject, AliveTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _audioSource.PlayOneShot(Hit);
        if (collision.gameObject.CompareTag("Unitychan"))
        {
            _score.SetText();
            Destroy(gameObject);
        }
    }

}