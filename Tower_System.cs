using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tower_System : MonoBehaviour {

    // Inputs
    private Keyboard keyboard;      // Keyboard
    private Gamepad pad;            // XBOX Controller
    private float key_e, b_button;  // Keys

    public GameObject activeTowerText, disableTowerText, tower;
    public bool towerState;
    private Animator anim;

    // Start is called before the first frame update
    void Start() {

        anim = tower.GetComponent<Animator>();
        towerState = false;
        activeTowerText.gameObject.SetActive(false);
        disableTowerText.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update() {

        Input();

    }

    /** ON TRIGGER **/
    // Trigger Stay
    public void OnTriggerStay(Collider other) {

        if ((other.gameObject.tag == "Player") && (towerState == false)) {
            disableTowerText.gameObject.SetActive(true);
            if (key_e >= 1.0f) {
                towerState = true;
                anim.SetBool("Active", true);
            }
        } else if ((other.gameObject.tag == "Player") && (towerState == true)) {
            disableTowerText.gameObject.SetActive(false);
            activeTowerText.gameObject.SetActive(true);
        }

    }

    // Trigger Exit
    public void OnTriggerExit(Collider other) {

        if (other.gameObject.tag == "Player") {
            disableTowerText.gameObject.SetActive(false);
            activeTowerText.gameObject.SetActive(false);
        }

    }
    /** ON TRIGGER **/


    // Input
    private void Input() {

        // Gamepad pad = InputSystem.GetDevice<Gamepad>();
        Keyboard keyboard = InputSystem.GetDevice<Keyboard>();

        key_e = keyboard.eKey.ReadValue();              // Tecla E
        // b_button = pad.buttonEast.ReadValue();       // Button B

    }

}
