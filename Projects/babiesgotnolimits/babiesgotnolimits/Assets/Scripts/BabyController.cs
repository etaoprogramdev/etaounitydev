using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class BabyController : MonoBehaviour
{
    #region Variables
    static public bool Launch;
    public AudioSource bodyHit;
    static public float multiplier = 0;
    public GameObject babyInfo;
    public ParticleSystem hitEffect;
    public ParticleSystem dashEffect;
    public Rigidbody2D rb;
    public PointEffector2D power;
    public bool hasBoosted;
    public int toughness;
    public Animator babySpringAnimation;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI Toughness;
    public CinemachineVirtualCamera Camera;
    static public bool gameEnd;
    public Animator babySprite;
    public GameObject GUI;
    static public float scoreValue;
    public PhysicsMaterial2D bounceMaterial;
    public CameraShake cameraController;
    public SpriteRenderer babyBody;
    public SpriteRenderer babyBum;
    private bool Grounded = false;
    #endregion
    private void Start()
    {
        gameEnd = false;
        Launch = false;
        GameManager.isPaused = false;
        babyBody.enabled = false;
        babyBum.enabled = false;
        hasBoosted = false;
        Time.timeScale = 1f;
        Score.text = 0.ToString();
        //Static variable information pass
        toughness = GameManager._toughness;
    }
    void Update()
    {
        //Check if one ground for too long
        if (rb.velocity.y <= 0)
        {
            StartCoroutine("GroundedDetection");
        }
        //Bounciness Updater
        bounceMaterial.bounciness = GameManager._bounce;
        if (Launch)
        {
            //Activate Enable Visibility
            babyBody.enabled = true;
            babyBum.enabled = true;
            //Activate GUI
            GUI.SetActive(true);
            //Reset Camera
            Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.5f;
            Camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.5f;
            //Thrust Mechanics
            power.forceMagnitude = GameManager._thrust * multiplier;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began && hasBoosted == false)
                {
                    rb.AddForce(transform.up * -GameManager._boost, ForceMode2D.Impulse);
                    CreateStream();
                    hasBoosted = true;
                    StartCoroutine("Timer");
                    cameraController.shakeCamera = true;
                }
            }
        }
        //Check for death
        if (toughness <= 0)
        {
            rb.mass = 100;
            gameEnd = true;
            Time.timeScale = 0f;
        }
        else if (toughness > 0)
        {
            rb.mass = 1;
            gameEnd = false;
            Toughness.text = toughness.ToString();
        }
        //Reset Spring
        babySpringAnimation.SetBool("isPressed", false);
        //GUI system
        scoreValue = Mathf.Round(rb.position.x);
        Score.text = scoreValue.ToString();
    }
    private void OnCollisionEnter2D(Collision2D hitInfo)
    {
        if (hitInfo.gameObject.tag == ("Ground"))
        {
            toughness -= 1;
            Debug.Log(toughness);
            hasBoosted = false;
            babySpringAnimation.SetBool("isPressed", true);
            StartCoroutine("Timer");
            cameraController.shakeCamera = true;
            CreateExplosion();
            bodyHit.Play();
        }

    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(0.1f);
        cameraController.shakeCamera = false;
    }
    IEnumerator GroundedDetection()
    {
        yield return new WaitForSeconds(3);
        if (rb.transform.position.y <= -3.5)
        {
            CreateExplosion();
            StartCoroutine("Timer");
            cameraController.shakeCamera = true;
            gameEnd = true;
        }
    }
    public void CreateExplosion()
    {
        hitEffect.Play();
    }
    public void CreateStream()
    {
        dashEffect.Play();
    }
}
