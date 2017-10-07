using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class scr_Menu : MonoBehaviour {

    public GameObject rulesPanel, quitJob; 

    public void QuitJob() {
        //quitJob.SetActive(true);
        StartCoroutine( this.GetComponent<scr_GamePlay>().iQuitJob());
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }

    public void ShowRules(bool _bool) {
        rulesPanel.SetActive(_bool);
    }
}

