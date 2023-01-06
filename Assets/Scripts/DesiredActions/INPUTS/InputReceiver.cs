using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputReceiver : MonoBehaviour
{
    private static InputReceiver instance;
    public static InputReceiver Instance => instance;

    private Entity_Player playerRef;
    private PlayerAction action;
    public Vector2 moveDirection;
    [SerializeField] private Camera cam;

    public bool isADS = false;
    [SerializeField] float adsForceIn = 30;
    [SerializeField] float adsForceOut = 40;

    private Vector2 camInput;
    private float camRX, camRY;
    [SerializeField][Range(0.1f, 2.0f)] private float camspeed = 1f;

    //public bool isGrounded;
    public void CamRXYReset()
    {
        camRX = 0;
        camRY = 0;
        cam.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        playerRef = GetComponent<Entity_Player>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    public void OnTargetLook(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PlayerAction action = new PlayerAction(PlayerActionsType.LOOKATTARGET);
            playerRef.DesiredActions.AddAction(action);
        }
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        camInput = context.ReadValue<Vector2>();
    }

    void Look()
    {
        camRX += camInput.x * camspeed * 0.1f;
        camRY -= camInput.y * camspeed * 0.1f;

        camRY = Mathf.Clamp(camRY, -45f, 70f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PlayerAction action = new PlayerAction(PlayerActionsType.JUMP);
            playerRef.DesiredActions.AddAction(action);
        }
    }
        
    private void Update()
    {
        if (GameManager.Instance.IsPaused || Entity_Player.Instance.IsDead) { return; }

        Look();

        cam.transform.rotation = Quaternion.Euler(camRY, camRX, 0);
        playerRef.transform.rotation = Quaternion.Euler(0, camRX, 0);
    }

    public void OnGamePause(InputAction.CallbackContext context)
    {
        //if (context.performed && !manager.IsOver)
        //  panel.PauseMenu();
    }

    public void OnPickup(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Pikcup");
            PlayerAction action = new PlayerAction(PlayerActionsType.PICKUP);
            playerRef.DesiredActions.AddAction(action);
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerAction action = new PlayerAction(PlayerActionsType.THROW);
            playerRef.DesiredActions.AddAction(action);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            action = new PlayerAction(PlayerActionsType.WEAPONATTACK);
            playerRef.DesiredActions.AddAction(action);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerAction action = new PlayerAction(PlayerActionsType.CROUCH);
            playerRef.DesiredActions.AddAction(action);
        }
    }

    public void OnADS(InputAction.CallbackContext context)
    {
        if(!isADS && !this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CameraShake"))
        {
            isADS = true;
            this.gameObject.GetComponent<Camera>().fieldOfView = adsForceOut;
        }
        else if(isADS && !this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("CameraShakeADS"))
        {
            isADS = false;
            this.gameObject.GetComponent<Camera>().fieldOfView = adsForceIn;
        }
    }
}
