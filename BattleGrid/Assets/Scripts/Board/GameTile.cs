using UnityEngine;
using System.Collections;

public class GameTile : MonoBehaviour
{
	SpriteRenderer _tileRenderer;

	public void SetColor(Color color)
	{
		StartCoroutine(CrossFadeColor(color, 2f));
	}

	private IEnumerator CrossFadeColor(Color targetColor, float duration)
	{
		Color startingColor = _tileRenderer.color;
		float elapsedTime = 0f;

		while(elapsedTime < duration)
		{
			var step = elapsedTime / duration;
			_tileRenderer.color = Color.Lerp(startingColor, targetColor, step);
			elapsedTime += Time.fixedDeltaTime;
			yield return null;
		}

		yield return null;
		_tileRenderer.color = targetColor;
		yield break;
	}

	void Awake()
	{
		_tileRenderer = gameObject.GetComponent<SpriteRenderer>();
		var newLayer = LayerMask.NameToLayer("Tile");
		gameObject.layer = newLayer;
	}
}
