using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputHorizontal;
    private Rigidbody2D rb;

    [SerializeField] private int velocidade = 5;
    [SerializeField] private Animator animacao;

    private bool isGrounded;
    private bool canDoubleJump;

    [SerializeField] private float jumpForce = 300f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Captura o input horizontal
        inputHorizontal = Input.GetAxis("Horizontal");

        // Flip visual do personagem
        spriteFlip(inputHorizontal);

        // Verifica se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Permite pulo duplo ao tocar o chão
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        // Lógica de pulo e pulo duplo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
                animacao.SetTrigger("espaço");
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
                animacao.SetTrigger("espaço");
                canDoubleJump = false;
            }
        }

        // Lógica das animações de movimento
        if (!isGrounded)
        {
            animacao.SetBool("parado", false); // Desativa idle no ar
            animacao.SetBool("andando", false); // Desativa andar no ar
        }
        else if (Mathf.Abs(inputHorizontal) > 0.01f)
        {
            animacao.SetBool("parado", false); // Desativa idle ao andar
            animacao.SetBool("andando", true); // Ativa andar
        }
        else
        {
            animacao.SetBool("andando", false); // Desativa andar
            animacao.SetBool("parado", true); // Ativa idle
        }
    }

    private void FixedUpdate()
    {
        // Aplica movimento horizontal
        rb.velocity = new Vector2(inputHorizontal * velocidade, rb.velocity.y);
    }

    private void spriteFlip(float horizontal)
    {
        if (horizontal < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (horizontal > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }
}
