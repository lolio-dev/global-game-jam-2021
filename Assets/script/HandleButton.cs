using System;
using UnityEngine;

public class HandleButton : MonoBehaviour
{
	public colorController ColorController;
	public GameObject test;
	public SpriteRenderer SpriteRenderer;

	public Sprite WhiteOff;
	public Sprite WhiteOn;


	void Start()
	{
		ColorController = test.GetComponent<colorController>();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("player"))
		{
			SpriteRenderer.sprite = WhiteOn;
			ColorController.TrySwitchColor();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		SpriteRenderer.sprite = WhiteOff;
	}
}
