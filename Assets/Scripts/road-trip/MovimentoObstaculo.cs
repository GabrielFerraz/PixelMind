using UnityEngine;

public class MovimentoObstaculo : MonoBehaviour
{
    public float velocidade = 5f;

    void Update()
    {
        transform.Translate(Vector2.down * velocidade * Time.deltaTime);
    }
}