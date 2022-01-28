using System;
using UnityEngine;

public class handlebutton : MonoBehaviour
{
	public colorController ColorController;
	public GameObject test;

	void Start()
	{
		ColorController = test.GetComponent<colorController>();
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("player"))
		{
			ColorController.switchColor();
		}
	}
}
