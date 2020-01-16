using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 150f;
    [SerializeField] float mainThrust = 90f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    Rigidbody rigidBody;
    AudioSource audioData;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Rotation();
            Thrust();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collisions when dead

        switch (collision.gameObject.tag)
        {
            case "Friendly": // do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioData.Stop();
        audioData.PlayOneShot(successSound);
        Invoke("LoadNextLevel", 1f); // delay finish by one sec
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioData.Stop();
        audioData.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", 1f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // todo allow for more than one level
    }

    private void Rotation()
    {
        rigidBody.freezeRotation = true; // take manual control of the rotation
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) // only able to either rotate left or right
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) // able to thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audioData.isPlaying)
            {
                audioData.PlayOneShot(mainEngine);
            }
        }
        else
        {
            audioData.Stop();
        }
    }
}
