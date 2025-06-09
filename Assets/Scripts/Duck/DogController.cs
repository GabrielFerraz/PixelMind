using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public SpriteRenderer signRenderer;
    public Transform topPosition;
    public Transform bottomPosition;
    public float moveSpeed = 5f;

    [Header("Sprites das Placas")]
    public Sprite redSignSprite;
    public Sprite blueSignSprite;
    public Sprite neutralSignSprite;

    private Dictionary<string, Sprite> colorSpriteMap;

    void Awake()
    {
        colorSpriteMap = new Dictionary<string, Sprite>
        {
            { "Red", redSignSprite },
            { "Blue", blueSignSprite }
        };
    }

    public IEnumerator SetInitialSign()
    {
        signRenderer.sprite = neutralSignSprite;
        transform.position = topPosition.position;
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator MoveDown(string colorName)
    {
        if (colorSpriteMap.TryGetValue(colorName, out Sprite targetSprite))
        {
            signRenderer.sprite = targetSprite;
        }
        else
        {
            Debug.LogWarning($"Sprite da placa para '{colorName}' não encontrado.");
        }

        while (Vector3.Distance(transform.position, bottomPosition.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bottomPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator MoveUp()
    {
        while (Vector3.Distance(transform.position, topPosition.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, topPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
