using UnityEngine;
using TMPro;
using System.Collections;

public class ControladorVelocidade : MonoBehaviour
{
    public float velocidade = 0;
    public float velocidadeMaxima = 130;
    public float aceleracao = 0.5f;

    public bool jogoAtivo = true;
    public bool isPaused = false;

    public Renderer jogadorRenderer;
    public Color corInicialJogador;
    public float tempoParaReiniciar = 1f;

    public int obstaculosDesviados = 0;
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoVelocidade;

    void Start()
    {
        if (jogadorRenderer != null)
        {
            corInicialJogador = jogadorRenderer.material.color;
        }
    }

    void Update()
    {
        if (jogoAtivo && velocidade < velocidadeMaxima)
        {
            velocidade += Mathf.Log(1 + aceleracao * Time.deltaTime) * 20;
        }

        AtualizarTextoVelocidade();
        AtualizarTextoPontuacao();
    }

    public void ReiniciarAposColisao()
    {
        if (!isPaused)
        {
            StartCoroutine(SequenciaColisao());
        }
    }

    private IEnumerator SequenciaColisao()
    {
        isPaused = true;
        jogoAtivo = false;

        while (velocidade > 0)
        {
            velocidade -= 10;
            if (velocidade < 0) velocidade = 0;
            AtualizarTextoVelocidade();
            yield return new WaitForSeconds(0.05f);
        }

        if (jogadorRenderer != null)
        {
            jogadorRenderer.material.color = Color.red;
        }

        yield return new WaitForSeconds(tempoParaReiniciar);

        if (jogadorRenderer != null)
        {
            jogadorRenderer.material.color = corInicialJogador;
        }

        isPaused = false;
        jogoAtivo = true;
    }

    public void IncrementarObstaculosDesviados()
    {
        obstaculosDesviados++;
        AtualizarTextoPontuacao();
    }

    private void AtualizarTextoVelocidade()
    {
        if (textoVelocidade != null)
        {
            textoVelocidade.text = $"Velocidade: {Mathf.RoundToInt(velocidade)} km/h";
        }
    }

    private void AtualizarTextoPontuacao()
    {
        if (textoPontuacao != null)
        {
            int pontuacao = obstaculosDesviados * 10;
            textoPontuacao.text = $"Pontuação: {pontuacao}";
        }
    }
}
