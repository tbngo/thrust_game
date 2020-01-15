using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        Thrust();
    }

    private void Rotation()
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation

        if (Input.GetKey(KeyCode.A)) // only able to either rotate left or right
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // able to thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioData.isPlaying)
            {
                audioData.Stop();
            }
        }
        else
        {
            audioData.Pause();
        }
    }
}
