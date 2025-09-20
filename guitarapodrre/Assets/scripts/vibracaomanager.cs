using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class vibracaomanager : MonoBehaviour
{
    public static vibracaomanager instance;
    private Gamepad gamepad;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        gamepad = Gamepad.current;
    }

    // Vibração em gamepad
    public void Vibrar(float intensidade, float duracao)
    {
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(intensidade, intensidade);
            Invoke(nameof(PararVibracao), duracao);
        }

        // Vibração em celular
#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }

    private void PararVibracao()
    {
        if (gamepad != null)
            gamepad.SetMotorSpeeds(0, 0);
    }
}
