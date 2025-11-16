using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public class PlayerMouvment : MonoBehaviour
{
    public Rigidbody2D rb;                 // Référence au Rigidbody2D du joueur
    public float jumpHeight = 10f;         // Force du saut
    private float movement;                // Valeur du déplacement horizontal
    public float speed = 5f;               // Vitesse du joueur
    private bool isGround;                 // Vérifie si le joueur touche le sol
    public Animator animator;              // Référence à l'Animator pour animations

    private bool facingRight = true;       // Orientation du joueur (droite/gauche)

    // Start est appelé au lancement du jeu
    void Start()
    {
        isGround = true;                   // Le joueur commence au sol
    }

    // Update est appelé une fois par frame
    void Update()
    {
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

    // Détection de collision avec le sol
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Vérifie si on touche le sol
        {
            isGround = true;                           // Le joueur est au sol
            animator.SetBool("Jump", false);           // Fin animation saut
        }
    }
}
