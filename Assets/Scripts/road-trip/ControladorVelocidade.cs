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
    public TextMeshProUGUI textoVidas;
    public TextMeshProUGUI textoRecorde;

    private int vidas = 5;
    private int pontuacaoAtual = 0;
    private int recordePessoal = 0;

    void Start()
    {
        if (jogadorRenderer != null)
        {
            corInicialJogador = jogadorRenderer.material.color;
        }
        AtualizarTextoVidas();
        AtualizarTextoRecorde();
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
            vidas--;
            AtualizarTextoVidas();

            if (vidas <= 0)
            {
                AtualizarRecorde();
                ReiniciarJogo();
            }
            else
            {
                StartCoroutine(SequenciaColisao());
            }
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

    private void ReiniciarJogo()
    {
        vidas = 5;
        pontuacaoAtual = 0;
        AtualizarTextoVidas();
        AtualizarTextoPontuacao();
        StartCoroutine(SequenciaColisao());
    }

    private void AtualizarRecorde()
    {
        if (pontuacaoAtual > recordePessoal)
        {
            recordePessoal = pontuacaoAtual;
        }
        AtualizarTextoRecorde();
    }

    public void IncrementarObstaculosDesviados()
    {
        obstaculosDesviados++;
        pontuacaoAtual = obstaculosDesviados * 10;
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
            textoPontuacao.text = $"Pontuação: {pontuacaoAtual}";
        }
    }

    private void AtualizarTextoVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = $"Vidas: {vidas}";
        }
    }

    private void AtualizarTextoRecorde()
    {
        if (textoRecorde != null)
        {
            textoRecorde.text = $"Recorde: {recordePessoal}";
        }
    }
}
