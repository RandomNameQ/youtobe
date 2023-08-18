using System.Reflection;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Door : MonoBehaviour, IIntractable
{

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private TypeDoor _typeDoor;
    public enum TypeDoor
    {
        Normal,
        Some
    }

    [SerializeField]
    private bool _canBeOpen = true;

    [SerializeField]
    private bool _itOpenToZRotation;
    [SerializeField]
    private bool _itClosed = true;

    private bool _ghostBlockDoor;

    private float _timer;

    private GameObject _player;
    private int _ghostAggression;

    public void Interact(bool isPlayer, int ghostAggression)
    {
        _ghostAggression = ghostAggression;
        if (isPlayer)
        {
            if (_ghostBlockDoor) return;
            if (!_canBeOpen) return;
            FindAnyObjectByType<InteractionKeeper>().GetComponent<IKeepeing>().SaveInteract(this.gameObject, !_itClosed);
        }

        OpenDoor();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerMovement>().gameObject;

        float zRotation = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);



        if (zRotation == 90f)
        {
            _itOpenToZRotation = false;
        }
        else
        {
            _itOpenToZRotation = true;
        }



    }

    private void Initiation()
    {
        // TODO сделать видос по получению всех способов получени яротации
        /*  if (transform.eulerAngles.z == 0) _itOpenToYRotation = true;

         if (transform.eulerAngles.z == 90f) _itOpenToYRotation = false;


         Debug.Log(transform.localEulerAngles.z);

         float zRotation = transform.localEulerAngles.z;

         if (Mathf.Approximately(zRotation, 0f))
         {
             _itOpenToYRotation = true;
         }

         if (Mathf.Approximately(zRotation, 90f))
         {
             _itOpenToYRotation = false;
         }

         Debug.Log(zRotation);

         Quaternion localRotation = transform.localRotation;
         zRotation = Quaternion.Euler(localRotation.eulerAngles).z;
         Debug.Log("Z Rotation using Quaternion.Euler: " + zRotation);

         Vector3 localEulerAngles = transform.localEulerAngles;
         zRotation = Quaternion.Euler(localEulerAngles).z;
         Debug.Log("Z Rotation using Quaternion.Euler with localEulerAngles: " + zRotation); */



        /* Quaternion targetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
         zRotation = Quaternion.Angle(Quaternion.identity, targetRotation);
        Debug.Log("Z Rotation using Quaternion.Angle with Quaternion.LookRotation: " + zRotation);

         zRotation = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
        Debug.Log("Z Rotation using Mathf.Atan2 with transform.forward: " + zRotation); */


    }

    private void Update()
    {

        if (_ghostBlockDoor)
        {
            _timer += Time.deltaTime;
            if (_timer > 2f)
            {
                _timer = 0;
                _ghostBlockDoor = false;
            }
        }

    }

    private void OpenDoor()
    {

        if (_itClosed)
        {

            StartCoroutine(Open(CheckWhereIsPlayer(), 140f));
            _itClosed = false;
        }
        else
        {
            // close door
            if (_itOpenToZRotation)
            {
                StartCoroutine(Open(0f, 140f));

            }
            else
            {
                StartCoroutine(Open(90f, 140f));
            }
            _itClosed = true;

        }


    }

    private float CheckWhereIsPlayer()
    {

        float normalAngle = 50f;

        Vector3 playerPosition = _player.transform.position;
        Vector3 doorPosition = transform.position;

        float relativeX = playerPosition.x - doorPosition.x;
        float relativeZ = playerPosition.z - doorPosition.z;
        if (_itOpenToZRotation)
        {

            if (relativeZ > 0)
            {
                normalAngle = -50f;
            }

        }
        if (!_itOpenToZRotation)
        {

            if (relativeX < 0)
            {
                normalAngle = 130;
            }
        }
        /* Debug.Log("Character position relative to the door:");
        Debug.Log("Relative X: " + relativeX);
        Debug.Log("Relative Z: " + relativeZ); */
        return normalAngle;
    }

    private IEnumerator Open(float targetRotationY, float rotationSpeed)
    {

        _canBeOpen = false;
        PlaySound();
        Quaternion targetRotation = Quaternion.identity;
        if (_itOpenToZRotation)
        {
            targetRotation = Quaternion.Euler(-90f, targetRotationY, 0f);
        }
        else
        {
            targetRotation = Quaternion.Euler(-90f, 0f, targetRotationY);
        }


        while (transform.localRotation != targetRotation)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        _canBeOpen = true;
        _ghostAggression=0;
    }

    private void BlockDoor()
    {
        if (!_itClosed)
        {
            OpenDoor();
        }
        _ghostBlockDoor = true;

    }

    private void PlaySound()
    {
        _audioSource.Play();
    }



}
