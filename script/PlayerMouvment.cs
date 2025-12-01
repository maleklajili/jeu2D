
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMouvment : MonoBehaviour
{
    public GameObject victoryUI;
    public GameObject gameOverUI;
    public int currentCoin = 0;

    public Text currentCoinText;
    public Text maxHealthText;
    public int maxHealth = 10;
    public Rigidbody2D rb;                 // Référence au Rigidbody2D du joueur
    public float jumpHeight = 10f;         // Force du saut
    private float movement;                // Valeur du déplacement horizontal
    public float speed = 5f;               // Vitesse du joueur
    private bool isGround;                 // Vérifie si le joueur touche le sol
    public Animator animator;              // Référence à l'Animator pour animations

    private bool facingRight = true;       // Orientation du joueur (droite/gauche)
    public Transform attackPoint;
    public float attackRadius = 1.5f;
    public LayerMask targetLayer;
    private bool isWon = false;
   
    // Start est appelé au lancement du jeu
    void Start()
    {
        isGround = true;                   // Le joueur commence au sol
    }

    // Update est appelé une fois par frame
    void Update()
    {  if (isWon)
        {
            animator.SetFloat("walk", 0);
            return;
        }
        if (maxHealth <= 0)
        {
            die();
        }
        currentCoinText.text = currentCoin.ToString();
        maxHealthText.text = maxHealth.ToString();
        movement = Input.GetAxis("Horizontal"); // Lire les touches gauche/droite (-1 à 1)

        // Gérer l'orientation du joueur vers la gauche
        if (movement < 0f && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        // Gérer l'orientation vers la droite
        else if (movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // Saut : espace + joueur au sol
        if (Input.GetKey(KeyCode.Space) && isGround == true)
        {
            Jump();                        // Lance le saut
            isGround = false;              // Le joueur quitte le sol
            animator.SetBool("Jump", true); // Animation de saut
        }

        // Animation de marche si mouvement
        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("walk", 1f); // Marche
        }
        else if (movement < .1f)
        {
            animator.SetFloat("walk", 0f); // Idle
        }

        // Gestion des attaques quand clic souris
        if (Input.GetMouseButton(0))
        {
            int randomIndex = Random.Range(0, 3); // Attaque aléatoire (0, 1 ou 2)

            if (randomIndex == 0)
            {
                animator.SetTrigger("Attack1");   // Animation 1
            }
            else if (randomIndex == 1)
            {
                animator.SetTrigger("Attack2");   // Animation 2
            }
            else
            {
                animator.SetTrigger("Attack3");   // Animation 3
            }
        }
    }

    // FixedUpdate pour les mouvements physiques
    private void FixedUpdate()
    {
        // Déplacement horizontal fluide en physique
        transform.position += new Vector3(movement, 0f, 0f) * speed * Time.fixedDeltaTime;
    }

    // Méthode qui applique le saut
    private void Jump()
    {
        Vector2 velocity = rb.velocity;   // On récupère la vitesse actuelle
        velocity.y = jumpHeight;          // On modifie l’axe Y pour faire un saut
        rb.velocity = velocity;           // On applique la nouvelle vitesse
    }


    public void PlayerAttack()
    {
        Collider2D hitInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, targetLayer);
        if (hitInfo)
        {
            if (hitInfo.GetComponent<Enemy>() != null) {
                hitInfo.GetComponent<Enemy>().EnemyTackeDamage(1);
            }

        }
    }

    // Détection de collision avec le sol
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Vérifie si on touche le sol
        {
            isGround = true;                           // Le joueur est au sol
            animator.SetBool("Jump", false);           // Fin animation saut
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collect");
            Destroy(other.gameObject, 1f);
        }
        if (other.gameObject.tag == "Key") {
           victoryUI.SetActive(true);
            isWon = true;  
            Destroy(other.gameObject);
        }
    }
   
    public void PlayerTackeDamage(int damage)
    {
        if (maxHealth <= 0)
        { return; }
        {

        }
        maxHealth -= damage;
    }
    private void OnDrawGizmosSelected()
    {

        if (attackPoint == null)
        {
            return;
           
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    void die()
    {
        Debug.Log(this.transform.name + "died");
        gameOverUI.SetActive(true);
        Destroy(this.gameObject);
    }
}