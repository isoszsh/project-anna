using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float jumpForce = 5f;
    private Vector2 movementData;
    private Rigidbody playerRigidBody;
    public Animator playerAnimator;
    private AudioSource audioSource;
    public bool sneaking;
    public bool isInteracted;
    public Rigidbody objectRigidbody;

    public Material faceMaterial;
    public Texture[] faceTextures;
    public AudioClip yawnSFX;

    [Serializable]
    public struct MaterialFootstepPair
    {
        public PhysicMaterial material;
        public AudioClip[] footstepSounds;
    }

    public MaterialFootstepPair[] materialFootstepPairs;
    public LayerMask raycastLayerMask;
    public GameObject groundCheck;
    public Transform interactableObject;


    private int _speedFloat = Animator.StringToHash("Speed");
    private int _sneakBool = Animator.StringToHash("Sneak");
    private int _moveObjectBool = Animator.StringToHash("MoveObject");
    private int _pullBool = Animator.StringToHash("Pull");

    private Dictionary<PhysicMaterial, AudioClip[]> materialFootstepDictionary;


    private bool isGrounded;
    private float timer;
    private bool isBlinking;

    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        InitializeFootstepDictionary();

        timer = 0f;
        isBlinking = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementData = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        Debug.Log("Jump");
        playerRigidBody.velocity = Vector3.zero;
        playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        playerAnimator.SetTrigger("Jump");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteraction();
        }

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            if (!isBlinking)
            {
                StartCoroutine(Blink());
            }
            timer = 0f;
        }
        
    }

    private System.Collections.IEnumerator Blink()
    {
        int dice = UnityEngine.Random.Range(0, 100);

        if(dice < 2 && playerAnimator.GetFloat(_speedFloat) < 0.1f)
        {
            isBlinking = true;
            audioSource.volume = .1f;
            audioSource.PlayOneShot(yawnSFX);
            yield return new WaitForSeconds(.5f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[2]);
            yield return new WaitForSeconds(.1f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[3]);
            yield return new WaitForSeconds(.2f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[4]);
            yield return new WaitForSeconds(.3f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[5]);
            yield return new WaitForSeconds(.5f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[6]);
            yield return new WaitForSeconds(.2f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[7]);
            yield return new WaitForSeconds(.1f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[8]);
            yield return new WaitForSeconds(1f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[0]);
            audioSource.volume = .3f;
            isBlinking = false;
        }
        else if (dice < 30)
        {
            yield return new WaitForSeconds(0.2f);
        }
        else if (dice > 30) 
        {
            isBlinking = true;
            if (faceMaterial != null && faceTextures[1] != null)
            {
                faceMaterial.SetTexture("_BaseMap", faceTextures[1]);
            }
            yield return new WaitForSeconds(0.2f);
            if (faceMaterial != null && faceTextures[0] != null)
            {
                faceMaterial.SetTexture("_BaseMap", faceTextures[0]);
            }
            isBlinking = false;
        }


        
    }

    void HandleInteraction()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f, raycastLayerMask) && hit.collider.CompareTag("MoveObject"))
        {
            if (!isInteracted)
            {
                InteractWithObject(hit);
            }
            else
            {
                ReleaseObject();
            }
        }
        else if (isInteracted)
        {
            ReleaseObject();
        }
    }

    private void InteractWithObject(RaycastHit hit)
    {
        isInteracted = true;
        playerAnimator.SetFloat(_speedFloat, 0f);
        interactableObject = hit.transform;
        objectRigidbody = interactableObject.GetComponent<Rigidbody>();
    }

    private void ReleaseObject()
    {
        isInteracted = false;
        playerAnimator.SetFloat(_speedFloat, 0f);
        if (interactableObject)
        {
            var audioSource = interactableObject.GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.Stop();
            }
        }
        interactableObject = null;
        objectRigidbody = null;
        LockConstraints("Open");
    }

    private void FixedUpdate()
    {

        MovePlayer();
        HandleSneak();
        CheckGroundedStatus();
    }

    void HandleSneak()
    {
        bool isMoving = playerRigidBody.velocity.sqrMagnitude > 0;
        bool isSneaking = Input.GetKey(KeyCode.LeftShift) && isMoving;

        if (sneaking != isSneaking)
        {
            sneaking = isSneaking;
            playerAnimator.SetBool(_sneakBool, sneaking);
            playerSpeed = sneaking ? 1.166f : 1.75f;
        }
    }

    private void InitializeFootstepDictionary()
    {
        materialFootstepDictionary = new Dictionary<PhysicMaterial, AudioClip[]>();
        foreach (var pair in materialFootstepPairs)
        {
            if (pair.material != null)
            {
                materialFootstepDictionary[pair.material] = pair.footstepSounds;
            }
        }
    }

    public void MovePlayer()
    {
        if (!isInteracted)
        {
            playerAnimator.SetBool(_moveObjectBool, false);
            Vector3 movement = new Vector3(movementData.x, 0f, movementData.y).normalized * playerSpeed;
            playerRigidBody.velocity = playerRigidBody.velocity = new Vector3(movement.x, playerRigidBody.velocity.y, movement.z);

            if (movement != Vector3.zero)
            {
                playerAnimator.SetFloat(_speedFloat, movement.magnitude);
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                Quaternion newRotation = Quaternion.Slerp(playerRigidBody.rotation, targetRotation, 15 * Time.fixedDeltaTime);
                playerRigidBody.MoveRotation(newRotation);
            }
            else
            {
                playerAnimator.SetFloat(_speedFloat, 0f);
            }
        }
        else
        {
            playerAnimator.SetBool(_moveObjectBool, true);

            Vector3 forward = interactableObject.TransformDirection(Vector3.forward);
            Vector3 toOther = Vector3.Normalize(transform.position - interactableObject.position);

            float dotProduct = Vector3.Dot(forward, toOther);
            Vector3 crossProduct = Vector3.Cross(forward, toOther);

            Vector3 movement = Vector3.zero;
            Vector2 relativeDirection = Vector2.zero;

            if (Mathf.Abs(dotProduct) < 0.5f)
            {
                if (crossProduct.y > 0)
                {
                    relativeDirection = new Vector2(1, 0);
                    movement = new Vector3(movementData.x, 0, 0).normalized * playerSpeed / 2;
                }
                else
                {
                    relativeDirection = new Vector2(-1, 0);
                    movement = new Vector3(movementData.x, 0, 0).normalized * playerSpeed / 2;
                }
            }
            else if (dotProduct < 0)
            {
                relativeDirection = new Vector2(0, -1);
                movement = new Vector3(0, 0, movementData.y).normalized * playerSpeed / 2;
            }
            else
            {
                relativeDirection = new Vector2(0, 1);
                movement = new Vector3(0, 0, movementData.y).normalized * playerSpeed / 2;
            }

            AudioSource objectAs = objectRigidbody.GetComponent<AudioSource>();
            if (movement != Vector3.zero)
            {
                objectRigidbody.isKinematic = false;
                objectRigidbody.velocity = movement;
                playerRigidBody.velocity = movement;

                if (!objectAs.isPlaying)
                {
                    objectAs.Play();
                }

                playerAnimator.SetFloat(_speedFloat, movement.magnitude);

                if (relativeDirection.x != 0)
                {
                    if (relativeDirection.x > 0)
                    {
                        playerAnimator.SetBool(_pullBool, movement.x > 0);
                        MoveObjectCalc(movement.x > 0 ? .8f : .98f);
                    }
                    else
                    {
                        playerAnimator.SetBool(_pullBool, movement.x <= 0);
                        MoveObjectCalc(movement.x <= 0 ? .8f : .98f);
                    }
                    LockConstraints("Z");
                }
                else
                {
                    if (relativeDirection.y > 0)
                    {
                        playerAnimator.SetBool(_pullBool, movement.z > 0);
                        MoveObjectCalc(movement.z > 0 ? .8f : .98f);
                    }
                    else
                    {
                        playerAnimator.SetBool(_pullBool, movement.z <= 0);
                        MoveObjectCalc(movement.z <= 0 ? .8f : .98f);
                    }
                    LockConstraints("X");
                }
            }
            else
            {
                MoveObjectCalc(.98f);

                LockConstraints("Open");
                if (objectAs.isPlaying)
                {
                    objectAs.Stop();
                }
                playerAnimator.SetFloat(_speedFloat, 0f);
                objectRigidbody.isKinematic = true;
            }
        }
    }

    void LockConstraints(string axis = "Open")
    {
        if (axis == "X")
        {
            playerRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
        else if (axis == "Z")
        {
            playerRigidBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            playerRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void MoveObjectCalc(float ReqDist, float moveSpeed = 10f, float rotationSpeed = 15f)
    {
        Vector3 directionToTarget = objectRigidbody.position - playerRigidBody.position;
        directionToTarget.y = 0;

        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > ReqDist)
        {
            Vector3 moveToTarget = directionToTarget.normalized * (distanceToTarget - ReqDist);
            playerRigidBody.position = Vector3.Lerp(playerRigidBody.position, playerRigidBody.position + moveToTarget, Time.deltaTime * moveSpeed);
        }
        else if (distanceToTarget < ReqDist)
        {
            Vector3 moveAwayFromTarget = directionToTarget.normalized * (ReqDist - distanceToTarget);
            playerRigidBody.position = Vector3.Lerp(playerRigidBody.position, playerRigidBody.position - moveAwayFromTarget, Time.deltaTime * moveSpeed);
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        playerRigidBody.MoveRotation(Quaternion.Slerp(playerRigidBody.rotation, targetRotation, Time.deltaTime * rotationSpeed));
    }

    public void HandleFootsteps()
    {

        if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, raycastLayerMask))
        {
            if (hit.collider.sharedMaterial != null && materialFootstepDictionary.TryGetValue(hit.collider.sharedMaterial, out AudioClip[] footstepSounds))
            {
                if (footstepSounds.Length > 0)
                {
                    audioSource.PlayOneShot(footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)]);
                }
            }
        }
    }

    private void CheckGroundedStatus()
    {
        isGrounded = Physics.Raycast(groundCheck.transform.position, Vector3.down, 0.1f, raycastLayerMask);
        playerAnimator.SetBool("IsGrounded", isGrounded);
    }
}
