using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security_System : MonoBehaviour {

    public GameObject towerCenter, doorsLab, doorForest;
    private bool doorsState, towers;
    private Animator animDoorsLab, animDoorForest;

    public Tower_System tower1, tower2, tower3;

    // Start is called before the first frame update
    void Start() {

        doorsState = false;
        towers = false;

        animDoorsLab = doorsLab.GetComponent<Animator>();
        animDoorForest = doorForest.GetComponent<Animator>();
    
    }

    // Update is called once per frame
    void Update() {

        Debug.Log(towers + " " + tower1.towerState + " " + tower2.towerState + " " + tower3.towerState + " " + doorsState);

        if (towers == false) {
            PanelInteraction();
        } else if (towers == true) {
            OpenDoors();
        }
    }

    private void PanelInteraction () {

        if (tower1.towerState == true) {
            if (tower1.towerState == true) {
                if (tower1.towerState == true) {
                    towers = true;
                }
            }
        }

    }

    private void OpenDoors () {

        animDoorsLab.SetBool("Open", true);
        doorsState = true;

    }  

}
