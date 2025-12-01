 using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public float attackRange = 10f;
    private bool playerInRange = false;
    public Transform player;
    public float walkSpees = 1.5f;
    public Transform detectPoint;
    public float distance;
    public LayerMask detectLayer;
    private bool facingLeft = true;
    public float chaseSpeed = 2.5f;
    public float retiriveDistance = 2.5f;

    public Transform attackPoint;
    public float attackRadius = 2f;
    public LayerMask attackLayer;
    public int maxHealth = 3;
    
    void Start()
    {

    }

    void Update()
    {
        if (maxHealth <= 0)
        {
            die();
        }

        // Vérifie si le joueur est dans la portée
        if (player == null)
        {animator.SetBool("PlayerDead", true);
            return;
        }
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange)
        {
            // Flip vers le joueur
            if (player.position.x > transform.position.x && facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }

            // Déplacement vers le joueur
            if (Vector2.Distance(transform.position, player.position) > retiriveDistance)
            {animator.SetBool("Attack",false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack",true);
            }
        }
        else
        {
            // Patrouille gauche/droite
            float dir = facingLeft ? -1f : 1f;
            transform.Translate(Vector2.right * dir * walkSpees * Time.deltaTime);

            // Détection du bord (vers le bas)
            RaycastHit2D groundHit = Physics2D.Raycast(detectPoint.position, Vector2.down, distance, detectLayer);
            if (!groundHit)
            {
                Flip();
                return; // évite de faire aussi la détection de mur dans le même frame
            }

            // Détection du mur (devant)
            Vector2 forwardDir = facingLeft ? Vector2.left : Vector2.right;
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, forwardDir, 1.5f, detectLayer);

            if (wallHit)
            {
                Flip();
            }
        }
    }

    public void Attack()
        {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.GetComponent<PlayerMouvment>() != null)
            {
                collInfo.GetComponent<PlayerMouvment>().PlayerTackeDamage(1);
            }
        }

    }


    public void EnemyTackeDamage(int damage)
    {
        if (maxHealth<=0)
        { return; }
        {
            
        }
        maxHealth -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null)
        {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);
        if(attackPoint != null)
        {
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
        }
    }
    void die()
    {
        Debug.Log(this.gameObject.gameObject + "died");
        Destroy(this.gameObject);
    }
    private void Flip()
    {
        facingLeft = !facingLeft;

        if (facingLeft)
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        else
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
    }
}
