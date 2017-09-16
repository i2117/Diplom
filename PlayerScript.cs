using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerScript : MonoBehaviour {

    public int rightAnswers = 0;
    public int wrongAnswers = 0;
    public float rayDistance = 10.0F;
    public int totalRightAnswers = 20;
    public GameObject rayed;
    public GameObject current;
    public GameObject previous;
    public Button[] Actions;
    public GameObject panel;
    public Text pickHint;
    public Text errorMessage;
    public Text rightText, wrongText;
    public Material highlightedMat;
    public GameObject neVibran;
    public GameObject fullCanvas;
    public GameObject endCanvas;
    public GameObject crosshair;
    //Current instrument
    private InstrumentScript instrumentScript;
    private ObjectScript objectScript;
    private FirstPersonController FPSscript;
    private BackScript backScript;
    private RaycastHit hit;
    private MeshRenderer rendRayed;
    public Image instrPicture;
    public Text instrText;
    public Text errorCount;


    void Start () {
        FPSscript = GetComponentInParent<FirstPersonController> ();
        backScript = panel.GetComponent<BackScript> ();
        instrumentScript = current.GetComponent<InstrumentScript> ();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        CommonRay ();
        if (Input.GetButtonDown ("Fire2")) {
            SwapTo (neVibran);
        }

        if (Input.GetButtonDown ("Cancel"))
            Application.Quit ();
    }

    public void TheEnd () {
        panel.SetActive (true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        FPSscript.enabled = false;

        errorCount.text += wrongAnswers.ToString ();
        fullCanvas.SetActive (false);
        endCanvas.SetActive (true);
    }

    public void SwapTo (GameObject newInstrument) {
        errorMessage.text = null;
        previous = current;
        //Take the instrument
        current = newInstrument;
        instrumentScript = current.GetComponent<InstrumentScript> ();
        //Set the picture and text
        instrPicture.sprite = instrumentScript.pic;
        instrText.text = instrumentScript.instrName;
        newInstrument.SetActive (false);
        //Drop current instrument
        previous.SetActive (true);
        backScript.ActionsList (instrumentScript.Actions);
        //Debug.Log (instrumentScript.ID);
    }

    public void ClearError () {
        errorMessage.text = null;
    }

    void CommonRay ()
    {
        Vector3 fwd = transform.TransformDirection (Vector3.forward);        
        if (Physics.Raycast (transform.position, fwd, out hit, rayDistance)) {

            rayed = hit.transform.gameObject;
            pickHint.text = rayed.name;
            //rendRayed = rayed.GetComponent<MeshRenderer> ();
            //rendRayed.material.color = Color.red;        
            //Instrument or object?
            if (rayed.tag == "instr") {
                InstrumentRay ();
            } else {
                ObjectRay ();
            }

        } else {
            pickHint.text = null;
            //rendRayed.material.color = Color.white ;

        }
    }

    void InstrumentRay () {
        if (Input.GetButtonDown ("Fire1"))
            SwapTo (rayed); 
    }

    public void ErrorMessage (string message) {
        CancelInvoke ();
        float time = 3;
        errorMessage.color = Color.red;
        errorMessage.text = message;
        Invoke ("ClearError", time);
    }

    public void CorrectMessage () {
        CancelInvoke ();
        float time = 3;
        errorMessage.color = Color.green;
        errorMessage.text = "Правильно!";
        Invoke ("ClearError", time);
    }

    public int RightAnswers
    {
        get { return rightAnswers; }
        set {
            rightAnswers = value;
            rightText.text = rightAnswers.ToString ();
            if (rightAnswers == totalRightAnswers)
                TheEnd ();
        }
    }

    public int WrongAnswers
    {
        get { return wrongAnswers; }
        set {
            wrongAnswers = value;
            wrongText.text = wrongAnswers.ToString ();
        }
    }
        
    public void ForItem () {
        errorMessage.text = null;
        previous = current;
        //Take the instrument
        current = neVibran;
        instrumentScript = current.GetComponent<InstrumentScript> ();
        //Set the picture and text
        instrPicture.sprite = instrumentScript.pic;
        instrText.text = instrumentScript.instrName;
        objectScript.linked.SetActive (true);
        objectScript.gameObject.SetActive (false);
        backScript.ActionsList (instrumentScript.Actions);
        backScript.StartMenu (instrumentScript.Actions, objectScript.rightAction);
    }
        
    void ObjectRay () {

        if (Input.GetButtonDown ("Fire1") && !backScript.inMenu && current) {

            if (current != neVibran) {

                //Open menu    
                objectScript = rayed.GetComponent<ObjectScript> ();
                panel.SetActive (true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                FPSscript.enabled = false;
                backScript.inMenu = true;
                crosshair.SetActive (false);
                backScript.StartMenu (instrumentScript.Actions, objectScript.rightAction);

            } else {
                ErrorMessage ("Сначала нужно выбрать инструмент");
            }

        } 
            
        if ((backScript.isBacking) && (backScript.inMenu)) {
            //Checking for answer
            if (backScript.isAnswering) {
                backScript.isAnswering = !backScript.isAnswering;
                //Проверяем правильность, выводим ошибки
                if (!objectScript.isDone) {
                    if (objectScript.rightInstrument == instrumentScript.instrName) {
                        if (backScript.answer) {
                            if (rightAnswers == objectScript.rightOrder) {
                        
                                CorrectMessage ();
                                RightAnswers++;
                                objectScript.Action ();

                                if (instrumentScript.isItem) {
                                    ForItem ();
                                    CorrectMessage ();
                                }
                                
                            } else {
                                WrongAnswers++;
                                ErrorMessage ("Неправильный порядок действий");
                            }
                        } else {
                            WrongAnswers++;
                            ErrorMessage ("Неправильное действие");
                        }
                            
                    } else {
                        WrongAnswers++;
                        ErrorMessage ("Нужен другой инструмент");
                    }
                } else {
                    WrongAnswers++;
                    ErrorMessage ("Это действие уже выполнено");
                }
            }
                
            //Closing menu
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            FPSscript.enabled = true;
            backScript.inMenu = false;
            backScript.isBacking = false;
            panel.SetActive (false);
            crosshair.SetActive (true);

        }            
    }
        
    void ObjectRay2 () {

        if (Input.GetButtonDown ("Fire1")) {

            objectScript = rayed.GetComponent<ObjectScript> ();

            if (objectScript.rightInstrument == instrumentScript.instrName) {
                rightAnswers++;
                //Открыть замок
                if (objectScript.isLocked == true) {
                    objectScript.Opening ();
                    wrong.text = null;
                } else {
                    wrong.text = "Замок уже открыт";
                }

                //rayed.SetActive (false);
                //Открыть крышку
                if (lockScript.isLocked)
                    lockScript.Check ();
            } else {
                wrongAnswers++;
                Debug.Log ("wrong instrument");
                wrong.text = "Нужен другой инструмент";
            }
        }
    }
        
    void EnterMenu2 () {
        
        for (int i = 0; i < instrumentScript.Actions.Length; i++) {
                
            Actions [i].gameObject.SetActive (true);
            Text txt = Actions [i].GetComponentInChildren<Text> ();
            txt.text = instrumentScript.Actions [i];
    
            objectScript = rayed.GetComponent<ObjectScript> ();
            backScript = Actions [i].gameObject.GetComponent<BackScript> ();
            backScript.isRight = (objectScript.rightAction == txt.text);
            Debug.Log (backScript.isRight);

        } 

        for (int i = instrumentScript.Actions.Length; i < Actions.Length; i++) {
                Actions [i].gameObject.SetActive (false);
        }
    }

} 
