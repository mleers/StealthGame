using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f; //.15s turn speed

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_Animator = GetComponent<Animator>();  //creates reference to Animator
        m_Rigidbody = GetComponent<Rigidbody>();

        m_AudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();  //keep direction, but change magnitude to 1

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);  //detects horizontal input, true when horizontal is non-zero
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);  //detects vertical  input
        bool isWalking = hasHorizontalInput || hasVerticalInput;  //lateral movement in either of the above detected as input
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();  //play audio if it isnt playing already
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);  //calc forward vector by multiplying change per second by time the frame takes, not change per frame
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);  //movement
        m_Rigidbody.MoveRotation(m_Rotation);  //rotation


    }

}
