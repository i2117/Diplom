using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackScript : MonoBehaviour {
    public bool inMenu = false;
    public bool isBacking = false;
    public bool isRight = false;
    public bool isAnswering = false;
    public bool answer;
    public Button[] Buttons;
    public Button backButton;
    public Text[] Texts;
    public int rightN = -1;

    public void Back () {
        isAnswering = false;
        isBacking = true;
        rightN = -1;
    }

    public void BackWithAnswer (int n) {
        isAnswering = true;
        answer = (n == rightN);
        isBacking = true;
        rightN = -1;

    }
        
    public void StartMenu (string[] Actions, string rightAnswer) {

        for (int i = 0; i < Actions.Length; i++) {
            Buttons [i].gameObject.SetActive (true);
            Text txt = Buttons [i].GetComponentInChildren<Text> ();
            txt.text = Actions [i];

            if (Actions [i] == rightAnswer)
                rightN = i;
        } 

        for (int i = Actions.Length; i < Buttons.Length; i++) {
            Buttons [i].gameObject.SetActive (false);
        }
    
    }

    public void ActionsList (string[] Actions) {

        for (int i = 0; i < Actions.Length; i++)
            Texts[i].text = Actions [i]; 

        for (int i = Actions.Length; i < Texts.Length; i++)
            Texts[i].text = null;

    }
        
} 
