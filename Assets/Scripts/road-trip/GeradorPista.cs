using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeradorPista : MonoBehaviour
{
    public GameObject[] tiles;
    public float velocidade = 5f;
    public float pontoGeracaoY = 10f;
    public float pontoDestruir = -10f;
    public Transform pontoInicial1;
    public Transform pontoInicial2;
    public float intervaloGeracao = 0.5f;

    private Queue<GameObject> pistaAtiva1 = new Queue<GameObject>();
    private Queue<GameObject> pistaAtiva2 = new Queue<GameObject>();
    private float alturaTile;
    private bool gerandoPista = true;

    private ControladorVelocidade controladorVelocidade;

    void Start()
    {
        controladorVelocidade = FindObjectOfType<ControladorVelocidade>();
        alturaTile = tiles[0].GetComponent<SpriteRenderer>().bounds.size.y;
        StartCoroutine(GerarPistaContinuamente());
    }

    void Update()
    {
        if (controladorVelocidade == null || !controladorVelocidade.jogoAtivo) return;

        foreach (var tile in pistaAtiva1)
        {
            tile.transform.Translate(Vector2.down * velocidade * Time.deltaTime);
        }

        foreach (var tile in pistaAtiva2)
        {
            tile.transform.Translate(Vector2.down * velocidade * Time.deltaTime);
        }

        if (pistaAtiva1.Count > 0 && pistaAtiva1.Peek().transform.position.y <= pontoDestruir)
        {
            GameObject tile = pistaAtiva1.Dequeue();
            Destroy(tile);
        }

        if (pistaAtiva2.Count > 0 && pistaAtiva2.Peek().transform.position.y <= pontoDestruir)
        {
            GameObject tile = pistaAtiva2.Dequeue();
            Destroy(tile);
        }
    }

    private IEnumerator GerarPistaContinuamente()
    {
        while (gerandoPista)
        {
            if (controladorVelocidade != null && controladorVelocidade.jogoAtivo)
            {
                CriarTile(pontoInicial1.position + new Vector3(0, pontoGeracaoY, 0), pistaAtiva1);
                CriarTile(pontoInicial2.position + new Vector3(0, pontoGeracaoY, 0), pistaAtiva2);
            }

            yield return new WaitForSeconds(intervaloGeracao);
        }
    }

    private void CriarTile(Vector3 posicao, Queue<GameObject> fila)
    {
        GameObject novoTile = Instantiate(tiles[Random.Range(0, tiles.Length)], posicao, Quaternion.identity);
        novoTile.transform.parent = transform;
        fila.Enqueue(novoTile);
    }
}
