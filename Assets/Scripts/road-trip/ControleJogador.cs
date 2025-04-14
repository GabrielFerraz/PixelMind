using UnityEngine;

public class ControleJogador : MonoBehaviour
{
    public Transform[] faixas;
    private int faixaAtual = 1;

    private Vector2 inicioToque;
    private bool deslizando = false;

    private float destinoX;
    public float velocidadeDeslizamento = 10f;

    void Start()
    {
        destinoX = faixas[faixaAtual].position.x;
        var pos = transform.position;
        transform.position = new Vector3(destinoX, pos.y, pos.z);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            inicioToque = Input.mousePosition;
            deslizando = true;
        }
        else if (Input.GetMouseButtonUp(0) && deslizando)
        {
            float deslize = Input.mousePosition.x - inicioToque.x;

            if (deslize > 50 && faixaAtual < 2) faixaAtual++;
            else if (deslize < -50 && faixaAtual > 0) faixaAtual--;

            destinoX = faixas[faixaAtual].position.x;
            deslizando = false;
        }
#else
        if (Input.touchCount > 0)
        {
            var toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Began)
            {
                inicioToque = toque.position;
                deslizando = true;
            }
            else if (toque.phase == TouchPhase.Ended && deslizando)
            {
                float deslize = toque.position.x - inicioToque.x;

                if (deslize > 50 && faixaAtual < 2) faixaAtual++;
                else if (deslize < -50 && faixaAtual > 0) faixaAtual--;

                destinoX = faixas[faixaAtual].position.x;
                deslizando = false;
            }
        }
#endif

        Vector3 posAtual = transform.position;
        transform.position = Vector3.MoveTowards(posAtual, new Vector3(destinoX, posAtual.y, posAtual.z), velocidadeDeslizamento * Time.deltaTime);
    }
}

