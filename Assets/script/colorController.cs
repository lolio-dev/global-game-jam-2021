using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class colorController : MonoBehaviour
{
	public Color color = Color.white;
	public bool isBlack;
	
	public GameObject[] whitePlatforms;
	public GameObject[] blackPlatforms;
	public GameObject[] togglePlatforms;

	public GameObject[] players;

	public Sprite blackSprite;
	public Sprite whiteSprite;

	public Camera cam;

	private void Start()
	{
		cam.backgroundColor = Color.white;
		
		foreach (var spriteR in togglePlatforms.Select(platform => platform.GetComponent<SpriteRenderer>()))
		{
			spriteR.color = Color.black;
		}
		
		foreach (var spriteR in whitePlatforms.Select(platform => platform.GetComponent<SpriteRenderer>()))
		{
			spriteR.color = Color.white;
		}
		
		foreach (var spriteR in blackPlatforms.Select(platform => platform.GetComponent<SpriteRenderer>()))
		{
			spriteR.color = Color.black;
		}
		
		foreach (var boxCollider2D in whitePlatforms.Select(platform => platform.GetComponent<BoxCollider2D>()))
		{
			boxCollider2D.isTrigger = !isBlack;
		}
		
		foreach (var boxCollider2D in blackPlatforms.Select(platform => platform.GetComponent<BoxCollider2D>()))
		{
			boxCollider2D.isTrigger = isBlack;
		}
	}

	public void switchColor()
	{
		isBlack = !isBlack;
		
		cam.backgroundColor = isBlack switch
		{
			false => Color.white,
			true => Color.black,
		};
		
		foreach (var player in players)
		{
			var spriteR = player.GetComponent<SpriteRenderer>();

			spriteR.sprite = isBlack switch
			{
				true => whiteSprite,
				false => blackSprite,
			};
		}
		
		foreach (var spriteR in togglePlatforms.Select(platform => platform.GetComponent<SpriteRenderer>()))
		{
			spriteR.color = isBlack ? Color.white : Color.black;
		}
		
		foreach (var boxCollider2D in whitePlatforms.Select(platform => platform.GetComponent<BoxCollider2D>()))
		{
			boxCollider2D.isTrigger = !isBlack;
		}
		
		foreach (var boxCollider2D in blackPlatforms.Select(platform => platform.GetComponent<BoxCollider2D>()))
		{
			boxCollider2D.isTrigger = isBlack;
		}
	}
}
