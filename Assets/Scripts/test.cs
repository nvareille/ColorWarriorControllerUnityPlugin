using System;
using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        ColorWarriorInput.Init();

	    int count = 0;

        //while (count < 256)
        ColorWarriorInput.SetButtonColor(0, Color.red);
        ColorWarriorInput.SetButtonColor(1, Color.green);
        ColorWarriorInput.SetButtonColor(2, Color.blue);
        ColorWarriorInput.SetButtonColor(3, Color.black);
        
	}
	
	// Update is called once per frame
	void Update ()
	{
        Debug.Log("0: " + ColorWarriorInput.GetInput(0));
        Debug.Log("1: " + ColorWarriorInput.GetInput(1));
        Debug.Log("2: " + ColorWarriorInput.GetInput(2));
        Debug.Log("3: " + ColorWarriorInput.GetInput(3));
        //Debug.Log("up");

	    if (Input.GetButton("Jump"))
	    {
            ColorWarriorInput.Serial.Close();
            Debug.Log("KLOSE");
	    }
	}
}
