using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetFinalScore : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text finalScore;
    void Start()
    {
        finalScore = GameObject.FindGameObjectWithTag("finalScore").GetComponent<TMP_Text>();
        finalScore.text = "With a Score of: " + GameManager.Instance.score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
