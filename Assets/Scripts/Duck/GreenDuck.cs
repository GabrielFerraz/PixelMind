using UnityEngine;

public class GreenDuck : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public float changeDirectionTime = 2f;
    public Sprite deadSprite;

    private Vector2 direction;
    private float speed;
    private float timer;
    private bool isDead = false;

    // private Animator animator; // Não está usando animação
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // animator = GetComponent<Animator>(); // Comentado
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        SetRandomDirection();
    }

    void Update()
    {
        if (isDead) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetRandomDirection();
        }

        rb.linearVelocity = direction * speed;

        CheckBounds();

        // Para PC e Editor
        if (Input.GetMouseButtonDown(0))
        {
            HandleClickOrTouch(Input.mousePosition);
        }

        // Para Celular
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleClickOrTouch(Input.GetTouch(0).position);
        }
    }

    void HandleClickOrTouch(Vector2 screenPosition)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject && !isDead)
        {
            Debug.Log("🟢 Pato clicado!");
            Die();
        }
        else
        {
            Debug.Log("❌ Erro: Tocou fora do pato!");
            FindObjectOfType<GameManager>().MissedShot();
        }
    }

    void SetRandomDirection()
    {
        // Movimento vertical (de baixo pra cima) com leve desvio lateral
        float x = Random.Range(-0.3f, 0.3f); // Pequena variação horizontal
        float y = Random.Range(0.8f, 1f);    // Sempre subindo
        direction = new Vector2(x, y).normalized;

        // Multiplicador de dificuldade vindo do GameManager
        float difficulty = 1f;
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null) difficulty = gm.difficultyMultiplier;

        speed = Random.Range(minSpeed, maxSpeed) * difficulty;
        timer = changeDirectionTime;
    }

    void CheckBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);

        // Se o pato sair da tela por cima e não foi abatido, conta como erro
        if (screenPosition.y > 1 && !isDead)
        {
            FindObjectOfType<GameManager>().MissedShot();
            Destroy(gameObject);
        }

        // Destruir se sair lateralmente (opcional)
        if (screenPosition.x < 0 || screenPosition.x > 1)
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        FindObjectOfType<GameManager>().DuckHit();
        isDead = true;

        // animator.enabled = false; // Comentado
        spriteRenderer.sprite = deadSprite;
        rb.linearVelocity = Vector2.down * 2f; // Cai pra baixo

        Destroy(gameObject, 1.5f);
    }
}
