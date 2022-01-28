using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class colorController : MonoBehaviour
{
	public Color color = Color.white;

	public List<GameObject> platforms = new List<GameObject>();

	public void switchColor()
	{
		if (color == Color.white)
		{
			color = Color.black;
		}
		else if (color == Color.black)
		{
			color = Color.white;
		}

		foreach (var spriteR in platforms.Select(platform => gameObject.GetComponent<SpriteRenderer>()))
		{
			spriteR.color = color;
		}
	}
}