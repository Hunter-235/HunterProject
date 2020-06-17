using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class Controller_System : MonoBehaviour {

    public InputMaster controls;
    public GameObject my_camera, player;

    // KeyBoard
    private Keyboard keyboard;
    private float key_w, key_a, key_s, key_d, key_e, key_f, key_shift, key_space, key_enter, key_esc;

    // Mouse
    private Mouse mouse;
    private float right_click, left_click;
    private Vector2 scroll_m, mousedelta;

    // Gamepad
    private Gamepad pad;
    private Vector2 l_stick, r_stick, pad_trigger;
    private float l_button, r_button, y_button, b_button, a_button, x_button, start_button;
    private float lb_button, rb_button, lt_shoulder, rt_shoulder;

    private float held, down, up, last;

    /** Parametros Camara **/
    public float distance,
                    h_position,
                    v_position,
                    mouse_speed,
                    gamepad_speed,
                    min_height,
                    max_height,
                    zoom_speed,
                    min_distance,
                    max_distance,
                    zoom_smooth,
                    camera_smooth;
    private float _distance,
                    _v_position,
                    _h_position;

    // Player
    private float movementSpeed;
    public float movementRun;
    private float currentSpeed = 0.0f;
    private float rotationSpeed = 1.0f;
    bool movementState;
    private float Horizontal, Vertical;
    public GameObject handLamp;         // Hand Light
    private bool handLampState;         // Hand Light State

    // Player Components
    private Transform mainCameraTransform;
    private CharacterController controller;
    private Animator anim, animHandLamp;

    // SFX
    public AudioClip attack, walk, run;
    // public AudioSource mySpeakerSFX;

    private void Start() {

        InicializeValues();
        controller = player.GetComponent<CharacterController>();    // Componente Character Controller
        anim = player.GetComponent<Animator>();                     // Componente Animator del Player
        animHandLamp = handLamp.GetComponent<Animator>();           // Componente Animator de la Lampara de Mano
        mainCameraTransform = my_camera.transform;                  // Componente Transform de la Camara
        // mySpeakerSFX = player.GetComponent<AudioSource>();              // Componente Sonido

    }

    // Update is called once per frame
    void Update() {

        ReadKeyboard();
        ReadMouse();
        // ReadGamepad();

        Move();
        CameraControl();    // Control de la camara
        Actions();

    }

    // Late Update is called before the void Update
    private void LateUpdate() {

        Vector3 position = Vector3.zero;

        position.y = (Mathf.Sin(_v_position * Mathf.Deg2Rad) * _distance) + player.transform.position.y;
        float radius = (Mathf.Cos(_v_position * Mathf.Deg2Rad) * _distance);
        position.x = (Mathf.Cos(_h_position * Mathf.Deg2Rad) * radius) + player.transform.position.x;
        position.z = (Mathf.Sin(_h_position * Mathf.Deg2Rad) * radius) + player.transform.position.z;

        my_camera.transform.position = position;
        my_camera.transform.LookAt(player.transform.position);

    }

    // Inicializar variables
    private void InicializeValues() {

        Horizontal = 0.0f;
        Vertical = 0.0f;
        movementState = false;
        handLampState = true;

        _distance = distance;
        _v_position = v_position;
        _h_position = h_position;
    }

    /** VOID CAMERA **/
    private void CameraControl() {

        h_position -= (mousedelta.x * mouse_speed * Time.deltaTime);
        v_position += (mousedelta.y * mouse_speed * Time.deltaTime);
        h_position -= (-r_stick.x * gamepad_speed * Time.deltaTime);
        v_position += (-r_stick.y * gamepad_speed * Time.deltaTime);

        if (v_position < min_height) v_position = min_height;
        if (v_position > max_height) v_position = max_height;

        distance += -r_stick.y * zoom_speed * Time.deltaTime;
        distance += -scroll_m.y * zoom_speed * Time.deltaTime;
        if (distance < min_distance) distance = min_distance;
        if (distance > max_distance) distance = max_distance;

        _distance = Mathf.Lerp(_distance, distance, Time.smoothDeltaTime * zoom_smooth);
        _v_position = Mathf.Lerp(_v_position, v_position, Time.smoothDeltaTime * camera_smooth);
        _h_position = Mathf.Lerp(_h_position, h_position, Time.smoothDeltaTime * camera_smooth);

    }
    /** VOID CAMERA **/

    /** VOID PLAYER **/
    // Acciones del Jugador
    private void Actions() {

        if (left_click >= 1.0f) {
            anim.SetBool("Attack1", true);
        } else {
            anim.SetBool("Attack1", false);
        }

        if (right_click >= 1.0f) {
            anim.SetBool("Attack2", true);
        } else {
            anim.SetBool("Attack2", false);
        }

        if ((key_f >= 1.0f) && (handLampState == true)) {
            handLampState = false;
            animHandLamp.SetBool("On", false);
        } else if ((key_f >= 1.0f) && (handLampState == false)) {
            handLampState = true;
            animHandLamp.SetBool("On", true);
        }

        /**
        if (key_space >= 1.0f) {
            anim.SetBool("Jump", true);
        } else {
            anim.SetBool("Jump", false);
        }
        **/

    }

    // Movimiento del Player
    private void Move() {

        if (key_a >= 1.0f) Horizontal = -key_a;
        else Horizontal = key_d;
        if (key_w >= 1.0f) Vertical = key_w;
        else Vertical = -key_s;
        Vector2 movementInput = new Vector2(Horizontal, Vertical);
        if (key_shift >= 1.0f) movementState = true;
        else movementState = false;
        if (movementInput == Vector2.zero) {
            currentSpeed = 0.0f;
        } else if (movementState == true) {
            movementSpeed = movementRun;
            currentSpeed = 1.0f;
        } else if (movementState == false) {
            movementSpeed = 2f;
            currentSpeed = 0.3f;
        }

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * movementInput.y + right * movementInput.x).normalized;
        if (moveDirection != Vector3.zero) player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotationSpeed);

        controller.Move(moveDirection * movementSpeed * Time.deltaTime);
        anim.SetFloat("MoveVelocity", currentSpeed);

        // Debug.Log(forward + " " + right + " " + moveDirection);

    }
    /** VOID PLAYER **/

    /** VOID INPUTS **/
    // Keyboard
    public void ReadKeyboard() {

        Keyboard keyboard = InputSystem.GetDevice<Keyboard>();

        key_w = keyboard.wKey.ReadValue();              // Keys W, A, S, D, E, F
        key_a = keyboard.aKey.ReadValue();
        key_s = keyboard.sKey.ReadValue();
        key_d = keyboard.dKey.ReadValue();
        key_e = keyboard.eKey.ReadValue();
        key_f = keyboard.fKey.ReadValue();
        key_shift = keyboard.shiftKey.ReadValue();      // Key SHIFT
        key_enter = keyboard.enterKey.ReadValue();      // Key ENTER
        key_space = keyboard.spaceKey.ReadValue();      // Key SPACE
        key_esc = keyboard.escapeKey.ReadValue();       // Key ESC

    }

    // Mouse
    public void ReadMouse() {

        Mouse mouse = InputSystem.GetDevice<Mouse>();

        right_click = mouse.rightButton.ReadValue();    // RIGHT Click
        left_click = mouse.leftButton.ReadValue();      // LEFT Click
        scroll_m = mouse.scroll.ReadValue();            // Mouse WHEEL
        mousedelta = mouse.delta.ReadValue();           // Mouse Movement

    }

    // X Controller
    public void ReadGamepad() {

        Gamepad pad = InputSystem.GetDevice<Gamepad>();

        l_stick = pad.leftStick.ReadValue();            // Left Stick & Button
        l_button = pad.leftStickButton.ReadValue();
        r_stick = pad.rightStick.ReadValue();           // Right Stick & Button
        r_button = pad.rightStickButton.ReadValue();

        y_button = pad.buttonNorth.ReadValue();         // Button Y
        b_button = pad.buttonEast.ReadValue();          // Button B
        a_button = pad.buttonSouth.ReadValue();         // Button A
        x_button = pad.buttonWest.ReadValue();          // Button X

        start_button = pad.startButton.ReadValue();     // Button START

        lb_button = pad.leftTrigger.ReadValue();        // LB Button & LT Shoulder       
        lt_shoulder = pad.leftShoulder.ReadValue();
        rb_button = pad.rightTrigger.ReadValue();       // RB Button & RT Shoulder
        rt_shoulder = pad.rightShoulder.ReadValue();

    }
    /** VOID INPUTS **/
}
