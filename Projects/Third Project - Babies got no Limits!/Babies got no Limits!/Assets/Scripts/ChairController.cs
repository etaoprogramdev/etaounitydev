using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChairController : MonoBehaviour
{
    public Animator animationController;
    public Animator babySprite;
    public GameObject babyObject;
    public Rigidbody2D babyObjectBody;
    public Slider chargeMeter;
    public GameObject chargeMeterGO;
    public ParticleSystem thrustEffect;
    private bool maxed = false;
    private float addMount = 0.01f;
    public CameraShake cameraController;
    public GameObject multiplierText;

    private void Start()
    {
        babyObjectBody.isKinematic = true;
        addMount = 0.03f;
        BabyController.multiplier = 1f;
        chargeMeterGO.SetActive(true);
        Input.multiTouchEnabled = false;
    }
    void Update()
    {
        //Sprite Declaration
        babySprite.SetInteger("SpriteIndex", GameManager._spriteIndex);

        //Animation controls

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            chargeMeter.value += addMount;

            if (chargeMeter.value >= chargeMeter.maxValue)
            {
                chargeMeter.value = chargeMeter.minValue;
            }

            if (touch.phase == TouchPhase.Began)
            {
                animationController.SetBool("isSpinning", true);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                addMount = 0f;
                animationController.SetBool("isSpinning", false);
                thrustEffect.Play();
                StartCoroutine("Timer");
                cameraController.shakeCamera = true;
                if (chargeMeter.value == Mathf.Clamp(chargeMeter.value, 0.8f, 0.9f))
                {
                    BabyController.multiplier = 1.5f;
                    multiplierText.SetActive(true);
                }
            }
        }
    }

    void Launch()
    {
        BabyController.Launch = true;
        babyObjectBody.isKinematic = false;
        chargeMeterGO.SetActive(false);
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        cameraController.shakeCamera = false;
    }
}
