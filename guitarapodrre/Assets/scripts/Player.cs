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
        inputHorizontal = Input.GetAxis("Horizontal");
        spriteFlip(inputHorizontal);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
                animacao.SetTrigger("espa�o");
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
                animacao.SetTrigger("espa�o");
                canDoubleJump = false;
            }
        }

        // L�gica de anima��o com 'parado' e sem conflito
        if (!isGrounded)
        {
            animacao.SetBool("parado", false); // No ar, desativa "parado"
        }
        else if (Mathf.Abs(inputHorizontal) > 0.01f)
        {
            animacao.SetBool("parado", false); // Est� andando
            animacao.SetBool("andando", true);//ativa a movimenta��o de andar
        }
        else
        {
            animacao.SetBool("andando", false);   
            animacao.SetBool("parado", true); // Est� no ch�o e parado
        }
    }

    private void FixedUpdate()
    {
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
