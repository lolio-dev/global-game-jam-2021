using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class colorController : MonoBehaviour
{
    public Color color = Color.white;

    public GameObject[] platforms;
    public GameObject[] players;	
    

    public Sprite blackSprite;
    public Sprite whiteSprite;

    public Camera cam;

    private void Start()
    {
	    cam.backgroundColor = Color.black;
    }

    public void switchColor()
    {
	    foreach (var player in players)
	    {
		    var spriteR = player.GetComponent<SpriteRenderer>();

		    if (color == Color.white) spriteR.sprite = blackSprite;
		    if (color == Color.black) spriteR.sprite = whiteSprite;
	    }
	    
	    if (color == Color.white)
	    {
		    cam.backgroundColor = Color.white;
		    color = Color.black;
	    } else if (color == Color.black)
	    {
		    cam.backgroundColor = Color.black;
		    color = Color.white;
	    }

	    foreach (var spriteR in platforms.Select(platform => platform.GetComponent<SpriteRenderer>()))
	    {
		    spriteR.color = color;
	    }
    }
}
