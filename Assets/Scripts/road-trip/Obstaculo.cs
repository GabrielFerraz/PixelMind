using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    public float velocidade = 2f;
    private float velocidadeAlvo;
    private ControladorVelocidade controladorVelocidade;

    void Start()
    {
        controladorVelocidade = FindObjectOfType<ControladorVelocidade>();
        if (controladorVelocidade != null)
        {
            velocidadeAlvo = controladorVelocidade.velocidade;
        }
    }

    void Update()
    {
        if (controladorVelocidade == null || controladorVelocidade.isPaused) return;

        velocidadeAlvo = controladorVelocidade.velocidade;
        velocidade = Mathf.MoveTowards(velocidade, velocidadeAlvo, Time.deltaTime * 0.5f);
        transform.Translate(Vector2.down * velocidade * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            if (controladorVelocidade != null)
            {
                controladorVelocidade.IncrementarObstaculosDesviados();
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Jogador"))
        {
            if (controladorVelocidade != null)
            {
                controladorVelocidade.ReiniciarAposColisao();
            }

            Destroy(gameObject);
        }
    }
}
