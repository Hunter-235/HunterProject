using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ElevatorLab : MonoBehaviour {

    // Inputs
    private Keyboard keyboard;      // Keyboard
    private Gamepad pad;            // XBOX Controller
    private float key_e, b_button;  // Keys

    public GameObject textElevator;

    // Start is called before the first frame update
    void Start() {

        textElevator.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update() {

        Input();

    }

    // TRIGGER ENTER
    private void OnTriggerStay(Collider other) {

        Debug.Log(other.gameObject.name);

        if (other.gameObject.tag == "Player") {
            textElevator.gameObject.SetActive(true);
            if (key_e >= 1.0f) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            textElevator.gameObject.SetActive(false);
        }
    }

    private void Input() {

        // Gamepad pad = InputSystem.GetDevice<Gamepad>();
        Keyboard keyboard = InputSystem.GetDevice<Keyboard>();

        key_e = keyboard.eKey.ReadValue();              // Tecla E
        // b_button = pad.buttonEast.ReadValue();          // Button B

    }
}
