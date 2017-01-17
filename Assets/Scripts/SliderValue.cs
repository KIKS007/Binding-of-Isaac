using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour 
{
	public Text text;
	private Slider slider;

	// Use this for initialization
	void Start () 
	{
		slider = GetComponent<Slider> ();
	}

	void Update ()
	{
		text.text = slider.value.ToString ();
	}
}
