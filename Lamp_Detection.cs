using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp_Detection : MonoBehaviour {

    private Animator anim;

    // Start is called before the first frame update
    void Start() {

        anim = GetComponent<Animator>();

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            anim.SetBool("Red", false);
            anim.SetBool("On", true);
        } else if (other.gameObject.tag == "Enemy") {
            anim.SetBool("Red", true);
            anim.SetBool("On", true);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            anim.SetBool("Red", false);
            anim.SetBool("On", true);
        } else if (other.gameObject.tag == "Enemy") {
            anim.SetBool("Red", true);
            anim.SetBool("On", true);
        }
    }

    private void OnTriggerExit(Collider other) {

        anim.SetBool("On", false);

    }
}
