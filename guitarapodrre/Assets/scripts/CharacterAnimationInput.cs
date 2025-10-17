using UnityEngine;
using System.Collections;

public class CharacterAnimationInput : MonoBehaviour
{
    [Header("Referências")]
    public Animator animator;

    [Header("Configurações")]
    public float transitionDelay = 0.15f; // tempo entre animações (em segundos)
    public string idleAnimation = "idle"; // nome da animação de idle

    private bool isPlaying = false;
    private string nextAnimation = null;

    void Update()
    {
        // Detecta inputs
        if (Input.GetKeyDown(KeyCode.D))
            TryPlay("hit light");
        if (Input.GetKeyDown(KeyCode.F))
            TryPlay("jump up");
        if (Input.GetKeyDown(KeyCode.J))
            TryPlay("crouch down");
        if (Input.GetKeyDown(KeyCode.K))
            TryPlay("knockback");
    }

    void TryPlay(string animName)
    {
        if (!isPlaying)
        {
            PlayAnimation(animName);
        }
        else
        {
            nextAnimation = animName;
            StopAllCoroutines(); // cancela delays antigos
            StartCoroutine(PlayNextAnimation());
        }
    }

    void PlayAnimation(string animName)
    {
        animator.Play(animName, 0, 0);
        isPlaying = true;

        // Quando a animação acabar, volta para o Idle
        StartCoroutine(ReturnToIdleAfter(transitionDelay));
    }

    IEnumerator PlayNextAnimation()
    {
        yield return new WaitForSeconds(transitionDelay);
        PlayAnimation(nextAnimation);
        nextAnimation = null;
    }

    IEnumerator ReturnToIdleAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play(idleAnimation, 0, 0);
        isPlaying = false;
    }
}
