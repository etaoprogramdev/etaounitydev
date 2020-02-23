using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public PlayerController playerController;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    public Animator characterAnimator;
    #endregion

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            characterAnimator.SetTrigger("isTakeoff");
        }

    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        //Move Player Accordingly
        playerController.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        characterAnimator.SetFloat("isWalking", Mathf.Abs(horizontalMove));
        jump = false;
    }


}
