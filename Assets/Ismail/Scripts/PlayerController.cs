using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using System.Globalization;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float cursorSpeed;
    public float jumpForce = 5f;
    private Vector2 movementData;
    private Rigidbody playerRigidBody;
    private Rigidbody cursorRigidBody;
    public Animator playerAnimator;
    private AudioSource audioSource;
    public bool sneaking;
    public bool isInteracted;
    public Rigidbody objectRigidbody;

    public Material faceMaterial;
    public Texture[] faceTextures;
    public Texture angerTexture;
    public AudioClip yawnSFX;

    private CapsuleCollider playerCollider;

    //Aim Variables
    public CinemachineVirtualCamera virtualCamera;
    public GameObject cursor3D;

    public Transform lastCheckPoint;

    [Serializable]
    public struct MaterialFootstepPair
    {
        public PhysicMaterial material;
        public AudioClip[] footstepSounds;
    }

    public AudioClip diggingSfx;

    public MaterialFootstepPair[] materialFootstepPairs;
    public LayerMask raycastLayerMask;
    public GameObject groundCheck;
    public Transform interactableObject;
    public GameObject stonePrefab;
    public Transform handTransform;
    public GameObject ripplePrefab;


    private int _speedFloat = Animator.StringToHash("Speed");
    private int _sneakBool = Animator.StringToHash("Sneak");
    private int _moveObjectBool = Animator.StringToHash("MoveObject");
    private int _pullBool = Animator.StringToHash("Pull");

    private Dictionary<PhysicMaterial, AudioClip[]> materialFootstepDictionary;


    private bool isGrounded;
    private float timer;
    private bool isBlinking;
    private bool isAiming;
    private bool isSneaking;
    private bool isThrowing = false;
    private bool isAngry = false;

    public bool lockControls;

    public Transform pickPoint;
    public GameObject pickedItem;
    public GameObject willPick;

    public Event currentEvent;
    public SoundReflector currentReflector;

    public DialogueData dialogueData;

    public GameObject scannerPrefab;
    public AudioClip scannerClip;

    public float lastHintTime = 0f;


    private void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        InitializeFootstepDictionary();

        playerCollider = GetComponent<CapsuleCollider>();

        timer = 0f;
        isBlinking = false;
    }


    public void WakeUp(bool checkpoint = false)
    {
        StartCoroutine(WakeNow(checkpoint));
    }


    IEnumerator WakeNow(bool checkpoint)
    {
        lockControls = true;
        if(checkpoint)
        {
            transform.position = lastCheckPoint.transform.position;
        }
        playerAnimator.SetTrigger("WakeUp");
        yield return new WaitForSeconds(4f);
        lockControls = false;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementData = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !lockControls)
        {
            Jump();
        }
    }

    public void Anger()
    {
        StartCoroutine(AngerCoroutine());
    }


    IEnumerator AngerCoroutine()
    {
        isAngry = true;
        faceMaterial.SetTexture("_BaseMap", angerTexture);
        faceMaterial.SetTexture("_EmissionMap", angerTexture);
        yield return new WaitForSeconds(3f);
        faceMaterial.SetTexture("_BaseMap", faceTextures[0]);
        faceMaterial.SetTexture("_EmissionMap", faceTextures[0]);
        isAngry = false;
    }
    private void Jump()
    {
        playerRigidBody.velocity = Vector3.zero;
        playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if(isGrounded)
        {
            playerAnimator.SetBool("isJumping", true);
        }
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && !lockControls && pickedItem != null)
        {
            StartCoroutine(Drop());
        }


        if (Input.GetKeyDown(KeyCode.F) && !lockControls)
        {
            if(currentEvent)
            {
                ResetVelocity();
                currentEvent.TriggerEvent();
            }
            else if(pickedItem != null)
            {
                ResetVelocity();
                string ItemType = pickedItem.GetComponent<PickUpItem>().type;
                if (ItemType == "Stone" && !isThrowing)
                {
                    if (!isAiming)
                    {
                        lockControls = true;
                        StartAim();
                        isAiming = true;
                    }
                    else
                    {
                        StartCoroutine(StartThrow());
                    }
                }
                else if (ItemType == "Flute")
                {
                    StartCoroutine(PlayFlute());
                }
                else if (ItemType == "Plant")
                {
                    StartCoroutine(Plant());
                }
                else if (ItemType == "Shovel")
                {
                    StartCoroutine(Dig());
                }
                else if(ItemType == "Brush")
                {
                    StartCoroutine(Paint());
                }
                else if(ItemType == "Mantar")
                {
                    StartCoroutine(Mantar());
                }
                else if(ItemType == "Key"){
                    StartCoroutine(OpenDoor());
                }
            }
            else
            {
                if(willPick !=  null)
                {
                    StartCoroutine(PickUp());
                }
                else
                {
                    Debug.Log("Handle");
                    HandleInteraction();
                }
            }

            
        }



        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSneaking = !isSneaking;
            playerAnimator.SetBool("Sneak", isSneaking);
            if(isSneaking)
            {
                playerCollider.height = .6f;
                playerCollider.center = new Vector3(0,0.3f,0);
            }
            else
            {
                playerCollider.height = 1.2f;
                playerCollider.center = new Vector3(0, 0.6f, 0);
            }
        }

     
     

       

        timer += Time.deltaTime;

        if(!isAngry)
        {
            if (timer >= 1f)
            {
                if (!isBlinking)
                {
                    StartCoroutine(Blink());
                }
                timer = 0f;
            }
        }
        else
        {
            StopCoroutine(Blink());
        }
       
        
    }



    public void ResetVelocity()
    {
        playerRigidBody.velocity = Vector3.zero;
        playerAnimator.SetFloat("Speed", 0);
    }

    IEnumerator Paint()
    {
        playerAnimator.SetTrigger("Paint");
        yield return null;
    }



    public void RemovePickupItem()
    {
        pickedItem.transform.parent = null;
        pickedItem.transform.rotation = Quaternion.Euler(0, 0, 0);
        Destroy(pickedItem.gameObject);
        pickedItem = null;
    }
    IEnumerator Drop()
    {
        lockControls = true;
        ResetVelocity();
        pickedItem.transform.parent = null;
        pickedItem.transform.rotation = Quaternion.Euler(0, 0, 0);

        string itemType = pickedItem.GetComponent<PickUpItem>().type;
        float itemHeight = 0;
        if (itemType == "Shovel")
        {
            itemHeight = 0.278f;
        }
        else if (itemType == "Plant")
        {
            itemHeight = 0;
        }

        pickedItem.transform.position = new Vector3(pickedItem.transform.position.x, transform.position.y +  itemHeight, pickedItem.transform.position.z);
        pickedItem.GetComponent<BoxCollider>().enabled = true;
        pickedItem = null;
        lockControls = false;
        willPick = null;
        yield return null;
    }

   
    IEnumerator PickUp()
    {
        StartCoroutine(ShowPUHint());
        playerAnimator.SetTrigger("Pick");
        lockControls = true;
        ResetVelocity();
        yield return new WaitForSeconds(.7f);
        willPick.transform.position = pickPoint.position;
        willPick.transform.rotation = pickPoint.rotation;
        willPick.transform.parent = pickPoint.parent;
        willPick.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        
        pickedItem = willPick;

        willPick = null;
        lockControls = false;
    }

    IEnumerator ShowPUHint()
    {

        if(Time.time - lastHintTime > 20 || lastHintTime == 0f)
        {

            lastHintTime = Time.time;
            GameManager.Instance.hintPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            GameManager.Instance.hintPanel.gameObject.SetActive(false);

        }
        
    }

    IEnumerator Dig()
    {
        lockControls = true;
        ResetVelocity();
        playerAnimator.SetTrigger("Dig");
        audioSource.PlayOneShot(diggingSfx);
        Vector3 storedPos = pickedItem.transform.localPosition;
        Quaternion storedRotation = pickedItem.transform.localRotation;
        Vector3 shovelPos = new Vector3(0.00282f, 0.00255f, 0.00112f);
        Quaternion ShovelRotation = Quaternion.Euler(200.269f, -3.300995f, 51.412f);
        pickedItem.transform.localPosition = shovelPos;
        pickedItem.transform.localRotation = ShovelRotation;
        if (currentReflector != null)
        {
            StartCoroutine(currentReflector.DigReflector());
            yield return new WaitForSeconds(5f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            Instantiate(GameManager.Instance.holePrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(4f);
        }
        
        
        
        pickedItem.transform.localPosition = storedPos;
        pickedItem.transform.localRotation = storedRotation;
        lockControls = false;
    }
    IEnumerator PlayFlute()
    {
        
        playerAnimator.SetTrigger("Paint");
        lockControls = true;
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(scannerClip);
        Vector3 instantiatePos = new Vector3(transform.position.x,0,transform.position.z);
        Instantiate(scannerPrefab, instantiatePos, Quaternion.identity);
        yield return new WaitForSeconds(.3f);
        Instantiate(scannerPrefab, instantiatePos, Quaternion.identity);
        yield return new WaitForSeconds(.3f);
        Instantiate(scannerPrefab, instantiatePos, Quaternion.identity);
        yield return new WaitForSeconds(.3f);
        yield return new WaitForSeconds(1f);
        lockControls = false;

    }


    void StartAim()
    {
       GameObject cursor =  Instantiate(cursor3D,transform.position, Quaternion.identity);
       virtualCamera.Follow = cursor.transform;
       virtualCamera.LookAt = cursor.transform;
       cursorRigidBody = cursor.transform.GetComponent<Rigidbody>();

    }

    IEnumerator StartThrow()
    {
        isAiming = false;
        isThrowing = true;
        Vector3 cursorPosition = cursorRigidBody.transform.position;
        Vector3 directionToCursor = (cursorPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToCursor);
        playerRigidBody.rotation = lookRotation;
        

       

        GameObject stone = Instantiate(stonePrefab, handTransform.position, Quaternion.identity);
        Rigidbody stoneRigidbody = stone.GetComponent<Rigidbody>();
        StoneController sc  = stoneRigidbody.GetComponent<StoneController>();

        sc.ripple = ripplePrefab;

        stone.transform.position = new Vector3(stone.transform.position.x, -1, stone.transform.position.x);

        Vector3 throwVelocity = CalculateThrowVelocity(handTransform.position, cursorPosition, stoneRigidbody);
        virtualCamera.Follow = transform;
        virtualCamera.LookAt = transform;
        playerAnimator.SetTrigger("Throw");
        Destroy(cursorRigidBody.gameObject);

        if (isSneaking)
        {
            yield return new WaitForSeconds(0.66f);
            stone.transform.position = handTransform.position;
            stone.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
            stoneRigidbody.velocity = throwVelocity * 0.65f;
            virtualCamera.Follow = stone.transform;
            virtualCamera.LookAt = stone.transform;
            yield return new WaitForSeconds(3f);
            
        }
        else
        {
            yield return new WaitForSeconds(0.35f);
            stone.transform.position = handTransform.position;
            stone.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f),0);
            stoneRigidbody.velocity = throwVelocity * 1.05f;
            virtualCamera.Follow = stone.transform;
            virtualCamera.LookAt = stone.transform;
            yield return new WaitForSeconds(3f);
        }

        virtualCamera.Follow = transform;
        virtualCamera.LookAt = transform;
        isThrowing = false;
        lockControls = false;
    }


    Vector3 CalculateThrowVelocity(Vector3 startPosition, Vector3 targetPosition, Rigidbody stoneRigidbody)
    {
        // Hedefe olan yatay mesafeyi hesapla
        Vector3 horizontalTarget = new Vector3(targetPosition.x, startPosition.y, targetPosition.z);
        float horizontalDistance = Vector3.Distance(startPosition, horizontalTarget);

        // Dikey y�kseklik fark�n� hesapla
        float heightDifference = targetPosition.y - startPosition.y;

        // �deal h�z i�in s�reyi hesapla
        float gravity = Physics.gravity.magnitude;
        float timeToReach = Mathf.Sqrt(2 * Mathf.Abs(heightDifference) / gravity);

        // Ta��n yatay ve dikey h�z bile�enlerini hesapla
        float horizontalSpeed = horizontalDistance / timeToReach;
        float verticalSpeed = gravity * timeToReach / 2;

        // Hedef yukar�da m� yoksa a�a��da m�?
        verticalSpeed = heightDifference > 0 ? verticalSpeed : -verticalSpeed;

        Vector3 direction = (horizontalTarget - startPosition).normalized;
        Vector3 throwVelocity = direction * horizontalSpeed;
        throwVelocity.y = verticalSpeed;

        return throwVelocity;
    }

    void HandleAim()
    {
        if(Vector3.Distance(transform.position, cursorRigidBody.transform.position) < 3f)
        {
            Vector3 movement = new Vector3(movementData.x, 0f, movementData.y).normalized * cursorSpeed;
            cursorRigidBody.velocity = cursorRigidBody.velocity = new Vector3(movement.x, cursorRigidBody.velocity.y, movement.z);
        }
        else
        {
            cursorRigidBody.velocity = new Vector3(0f, 0f, 0f);
        }


    }
    IEnumerator Plant()
    {
        lockControls = true;
        playerAnimator.SetTrigger("Plant");
 
        yield return new WaitForSeconds(2);
        pickedItem.transform.parent = null;
        pickedItem.transform.rotation = Quaternion.Euler(0, 0, 0);
        pickedItem.transform.position = new Vector3(pickedItem.transform.position.x,0,pickedItem.transform.position.z);
        pickedItem.GetComponent<BoxCollider>().enabled = true;
        pickedItem = null;
        yield return new WaitForSeconds(5.5f);
        lockControls = false;
    }

    IEnumerator Mantar(){
        lockControls = true;
        playerAnimator.SetTrigger("Plant");
        yield return new WaitForSeconds(2);
        pickedItem.transform.parent = null;
        pickedItem.transform.rotation = Quaternion.Euler(0, 0, 0);
        pickedItem.transform.position = new Vector3(pickedItem.transform.position.x,0,pickedItem.transform.position.z);
        pickedItem.GetComponent<BoxCollider>().enabled = true;
        pickedItem = null;
        yield return new WaitForSeconds(5.5f);
        lockControls = false;
    }

    IEnumerator OpenDoor(){
        lockControls = true;
        playerAnimator.SetTrigger("Plant");
        yield return new WaitForSeconds(2);
        pickedItem.transform.parent = null;
        pickedItem.transform.rotation = Quaternion.Euler(0, 0, 0);
        pickedItem.transform.position = new Vector3(pickedItem.transform.position.x,0,pickedItem.transform.position.z);
        pickedItem.GetComponent<BoxCollider>().enabled = true;
        pickedItem = null;
        yield return new WaitForSeconds(5.5f);
        lockControls = false;
    }

    private System.Collections.IEnumerator Blink()
    {
        int dice = UnityEngine.Random.Range(0, 100);

        if(dice < 2 && playerAnimator.GetFloat(_speedFloat) < 0.1f && !lockControls)
        {
            isBlinking = true;
            audioSource.volume = .1f;
            audioSource.PlayOneShot(yawnSFX);
            yield return new WaitForSeconds(.5f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[2]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[2]);
            yield return new WaitForSeconds(.1f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[3]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[3]);
            yield return new WaitForSeconds(.2f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[4]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[4]);
            yield return new WaitForSeconds(.3f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[5]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[5]);
            yield return new WaitForSeconds(.5f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[6]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[6]);
            yield return new WaitForSeconds(.2f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[7]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[7]);
            yield return new WaitForSeconds(.1f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[8]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[8]);
            yield return new WaitForSeconds(1f);
            faceMaterial.SetTexture("_BaseMap", faceTextures[0]);
            faceMaterial.SetTexture("_EmissionMap", faceTextures[0]);
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
                faceMaterial.SetTexture("_EmissionMap", faceTextures[1]);
            }
            yield return new WaitForSeconds(0.2f);
            if (faceMaterial != null && faceTextures[0] != null)
            {
                faceMaterial.SetTexture("_BaseMap", faceTextures[0]);
                faceMaterial.SetTexture("_EmissionMap", faceTextures[0]);
            }
            isBlinking = false;
        }


        
    }

    void HandleInteraction()
    {
        Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f, raycastLayerMask) && hit.collider.CompareTag("MoveObject"))
        {
            Debug.Log(hit.collider.tag);
            if (!isInteracted)
            {
                Debug.Log("interacting");
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

        if(!lockControls)
        {
            MovePlayer();
            HandleSneak();
        }

        if(isAiming)
        {
            HandleAim();
        }
        
        CheckGroundedStatus();
        CheckFallingStatus();
    }

    void HandleSneak()
    {
        bool isMoving = playerRigidBody.velocity.sqrMagnitude > 0;
        

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
        if(isGrounded && playerAnimator.GetBool("isFalling"))
        {
            playerAnimator.SetBool("isJumping", false);
        }
    }

    private void CheckFallingStatus()
    {
 
        
        if(playerRigidBody.velocity.y < -0.1f && !isGrounded)
        {
            playerAnimator.SetBool("isFalling", true);
        }
        else
        {
            playerAnimator.SetBool("isFalling", false);
        }

    }
}
