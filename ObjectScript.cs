using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour {

    public string rightAction;
    public string rightInstrument;
    public bool isDone = false;
    public GameObject linked;
    public GameObject linked2;
    public int rightOrder;
    public string action;
    public Text currentTxt, nextTxt;

    public void Update () {

    }
        
    public void Action () {
        isDone = true;
        //Debug.Log ("actioned");
        switch (action) {
        case "lopata":
            NextNumber ();
            break;
        case "domkrat": 
            Domkrat ();
            break;
        case "koleso_otkr":
            KolesoOtkr ();
            break;
        case "koleso_ust":
            KolesoUst ();
            break;
        case "koleso_zakr":
            KolesoZakr ();
            break;
        case "domkrat_nazad":
            DomkratNazad ();
            break;
        case "nak":
            KolesoNak ();
            break;
        case "tf":
            TrueFalse ();
            break;
        case "tf2":
            TrueFalse ();
            NextNumber ();
            break;
        default:
            break;
        }
    }

    public void NextNumber () {
        currentTxt.color = Color.green;
        if (nextTxt != null)
            nextTxt.color = Color.yellow;
    }

    public void Domkrat () {
        linked.transform.Rotate (0, 0, 10);
        linked.transform.Translate (0, 0.2F, 0);
        linked2.SetActive (true);
        gameObject.SetActive (false);
    }

    public void DomkratNazad () {
        linked.transform.Rotate (0, 0, -10);
        linked.transform.Translate (0, -0.2F, 0);
        gameObject.SetActive (false);
        linked2.SetActive (true);
    }

    public void KolesoOtkr () {
        if (gameObject.name == "Колесо") {
            linked.SetActive (true);
            linked2.SetActive (true);
        }
        gameObject.SetActive (false);
    }

    public void KolesoUst () {
        linked.SetActive (true);
        gameObject.SetActive (false);
    }

    public void KolesoZakr () {

    }

    public void KolesoNak () {
        linked.SetActive (true);
        if (linked2 != null)
            linked2.SetActive (true);
        gameObject.SetActive (false);
    }

    public void TrueFalse () {
        linked.SetActive (true);
        linked2.SetActive (false);
        gameObject.SetActive (false);
    }

    public void KolesoPost () {
        linked.SetActive (true);
    }
} 
