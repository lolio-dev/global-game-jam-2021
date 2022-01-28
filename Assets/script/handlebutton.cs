using System;
using UnityEngine;

public class handlebutton : MonoBehaviour
{
	public colorController ColorController;

	void Start()
	{
	}

	void Update()
	{
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("player"))
		{
			colorController.switchColor();
		}
	}
}