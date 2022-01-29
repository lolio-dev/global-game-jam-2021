using System;
using System.Linq;
using UnityEngine;

public class colorController : MonoBehaviour
{
	[Header("Data")]

	public GameplayParameters gameplayParameters;


	public bool isBlack;

	public GameObject[] whitePlatforms;
	public GameObject[] blackPlatforms;
	public GameObject[] togglePlatforms;
	public GameObject[] players;

	public Sprite blackSprite;
	public Sprite whiteSprite;

	public Camera cam;

	public GameObject player1Tag;
	public GameObject player2Tag;

	public Sprite oneWhiteTag;
	public Sprite oneBlackTag;
	public Sprite twoWhiteTag;
	public Sprite twoBlackTag;


	/* State */

	/// Time until next switch is allowed
	private float switchCooldownLeft = 0;


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

		foreach (var platform in blackPlatforms)
		{
			platform.gameObject.SetActive(!isBlack);
		}

		foreach (var platform in whitePlatforms)
		{
			platform.gameObject.SetActive(isBlack);
		}
	}

	public void TrySwitchColor()
	{
		if (switchCooldownLeft > 0f)
		{
			return;
		}

		isBlack = !isBlack;

		cam.backgroundColor = isBlack ? Color.black : Color.white;

		foreach (var player in players)
		{
			var spriteR = player.GetComponent<SpriteRenderer>();

			spriteR.sprite = isBlack ? whiteSprite : blackSprite;

			if (player.name == "player1")
			{
				player1Tag.GetComponent<SpriteRenderer>().sprite = isBlack ? oneWhiteTag : oneBlackTag;
			}
			else if (player.name == "player2")
			{
				player2Tag.GetComponent<SpriteRenderer>().sprite = isBlack ? oneWhiteTag : oneBlackTag;
			}
		}

		foreach (var spriteR in togglePlatforms.Select(platform => platform.GetComponent<SpriteRenderer>()))
		{
			spriteR.color = isBlack ? Color.white : Color.black;
		}

		foreach (var platform in blackPlatforms)
		{
			platform.gameObject.SetActive(!isBlack);
		}

		foreach (var platform in whitePlatforms)
		{
			platform.gameObject.SetActive(isBlack);
		}

		// Start cooldown timer
		switchCooldownLeft = gameplayParameters.switchCooldownDuration;
	}

	private void FixedUpdate()
	{
		if (switchCooldownLeft > 0f)
		{
			// Count down cooldown time left
			switchCooldownLeft = Mathf.Max(0f, switchCooldownLeft - Time.deltaTime);
		}
	}
}
