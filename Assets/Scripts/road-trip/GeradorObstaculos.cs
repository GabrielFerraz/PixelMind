using UnityEngine;
using System.Collections;

public class GeradorObstaculos : MonoBehaviour
{
    public GameObject[] obstaculosPrefabs;
    public Transform[] pontosGeracao;
    public float intervaloMin = 1f;
    public float intervaloMax = 2f;
    public GameObject alertaPrefab;

    private bool podeGerar = true;
    private ControladorVelocidade controladorVelocidade;

    void Start()
    {
        controladorVelocidade = FindObjectOfType<ControladorVelocidade>();
        StartCoroutine(GerarObstaculos());
    }

    private IEnumerator GerarObstaculos()
    {
        while (true)
        {
            if (controladorVelocidade != null && !controladorVelocidade.isPaused && podeGerar)
            {
                int indice = Random.Range(0, obstaculosPrefabs.Length);
                int ponto = Random.Range(0, pontosGeracao.Length);

                if (obstaculosPrefabs[indice].CompareTag("Especial") && Random.value > 0.2f)
                {
                    yield return new WaitForSeconds(intervaloMax);
                    continue;
                }

                GameObject obstaculo = Instantiate(obstaculosPrefabs[indice], pontosGeracao[ponto].position, Quaternion.identity);

                if (obstaculo.CompareTag("Especial"))
                {
                    ExibirAlerta(pontosGeracao[ponto]);
                }

                var movimento = obstaculo.GetComponent<MovimentoObstaculo>();
                if (movimento != null)
                {
                    movimento.velocidade = controladorVelocidade.velocidade;
                }
            }

            float intervalo = CalcularIntervalo();
            yield return new WaitForSeconds(intervalo);
        }
    }

    private float CalcularIntervalo()
    {
        if (controladorVelocidade == null) return intervaloMax;

        float velocidadeAtual = controladorVelocidade.velocidade;
        float velocidadeMaxima = controladorVelocidade.velocidadeMaxima;

        // Reduz o intervalo com base na velocidade atual
        return Mathf.Lerp(intervaloMax, intervaloMin, velocidadeAtual / velocidadeMaxima);
    }

    private void ExibirAlerta(Transform pontoGeracao)
    {
        GameObject alerta = Instantiate(alertaPrefab, transform);

        RectTransform alertaRect = alerta.GetComponent<RectTransform>();
        if (alertaRect != null)
        {
            alertaRect.anchoredPosition = Camera.main.WorldToScreenPoint(pontoGeracao.position);
        }

        alerta.SetActive(true);
        StartCoroutine(DesativarAlerta(alerta, 1.5f));
    }

    private IEnumerator DesativarAlerta(GameObject alerta, float tempo)
    {
        yield return new WaitForSeconds(tempo);
        alerta.SetActive(false);
        Destroy(alerta);
    }

    public void PausarGeracaoObstaculos(float tempo)
    {
        StartCoroutine(PausarGeracao(tempo));
    }

    private IEnumerator PausarGeracao(float tempo)
    {
        podeGerar = false;
        yield return new WaitForSeconds(tempo);
        podeGerar = true;
    }
}
