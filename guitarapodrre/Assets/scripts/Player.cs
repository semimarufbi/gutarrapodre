using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputHorizontal;
    private Rigidbody2D rb;
    [SerializeField] private int velocidade = 5;
    [SerializeField] Animator animacao;
    public SpriteRenderer sprite;
    public AudioSource som;
    private bool isJumping = false;  // Controla o primeiro pulo
    private bool canDoubleJump = false;  // Permite o segundo pulo (double jump)
    private int numberOfJumps = 0;  // Conta o n�mero de pulos (1 para o primeiro pulo, 2 para o double jump)
    public AudioClip outroAudio;

    // For�a do pulo
    [SerializeField] private float jumpForce = 300f;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   

    private void Update()
    {
        // Captura o movimento horizontal do jogador
        inputHorizontal = Input.GetAxis("Horizontal");

        // Chama a fun��o para flipar o sprite (mudar dire��o)
        spriteFlip(inputHorizontal);

        // L�gica para pulo (aciona a anima��o de pulo)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            som.Play();

            if (!isJumping)  // Primeiro pulo
            {
                // Realiza o primeiro pulo
                rb.AddForce(Vector2.up * jumpForce);
                animacao.SetTrigger("espa�o"); // Anima��o de pulo
                isJumping = true; // O jogador est� no ar
                numberOfJumps = 1; // Um pulo foi realizado
            }
            else if (canDoubleJump)  // Segundo pulo (Double Jump)
            {
                // Realiza o double jump
                rb.velocity = new Vector2(rb.velocity.x, 0);  // Zera a velocidade vertical para pular corretamente
                rb.AddForce(Vector2.up * jumpForce);
                animacao.SetTrigger("espa�o"); // Anima��o de pulo
                numberOfJumps = 2;  // Double jump foi realizado
                canDoubleJump = false; // Desativa o double jump at� o jogador tocar o solo novamente
            }

        }

        // Se o jogador est� se movendo (andando) e n�o est� pulando, ativa a anima��o de andar
        if (inputHorizontal != 0 && !isJumping)
        {
            animacao.SetBool("andando", true);
        }
        else if (!isJumping)
        {
            animacao.SetBool("andando", false); // Se n�o estiver se movendo e n�o estiver pulando, desativa a anima��o de andar
        }

    }

    private void FixedUpdate()
    {
        // Movimenta o personagem com base no input horizontal
        rb.velocity = new Vector2(inputHorizontal * velocidade, rb.velocity.y);
    }

    // Fun��o para flipar o sprite dependendo do movimento horizontal
    public void spriteFlip(float horizontal)
    {
        if (horizontal < 0)
        {
            sprite.flipX = true; // Flip para a esquerda
        }
        else if (horizontal > 0)
        {
            sprite.flipX = false; // Flip para a direita
        }
    }
}
