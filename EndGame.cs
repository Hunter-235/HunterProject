using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "Player") {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }

}
