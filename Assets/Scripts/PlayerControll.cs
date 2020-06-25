using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerControll : MonoBehaviour
{
    private Rigidbody PlayerRigedBody;
    public float MoveSpeed;
    public float Sensetive;
    public float JumpForse;
    public float stamina;
    public float SitSpeed;
    public float RunSpeed;
    private float SimpleSpeed;
    private float MouseX;
    private float MouseY;
    private void Start()
    {
        PlayerRigedBody = GetComponent<Rigidbody>();
        SimpleSpeed = MoveSpeed;
    }
    public void PlayerMove() 
    {
        if (Input.GetKey(KeyCode.W)) 
        {
            PlayerRigedBody.MovePosition(PlayerRigedBody.position + transform.forward * MoveSpeed*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            PlayerRigedBody.MovePosition(PlayerRigedBody.position - transform.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            PlayerRigedBody.MovePosition(PlayerRigedBody.position + transform.right * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerRigedBody.MovePosition(PlayerRigedBody.position - transform.right * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position,Vector3.down, out hit))
            {
                if (hit.distance < GetComponent<CapsuleCollider>().height/2)
                {
                    PlayerRigedBody.AddForce(Vector3.up * JumpForse);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MoveSpeed = SitSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            MoveSpeed = SimpleSpeed;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MoveSpeed = RunSpeed;
            var corutin = StaminaDropping(1,1);
            StartCoroutine(corutin);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MoveSpeed = SimpleSpeed;
        }
    }
    private IEnumerator StaminaDropping(float SpeedOfDropping,float value) 
    {
        while (stamina >= 0) 
        {
            yield return new WaitForSeconds(SpeedOfDropping);
            stamina -= value;
        }
    }
    private void RotateCamera() 
    {
        MouseX += Input.GetAxis("Mouse X")*Sensetive;
        MouseY -= Input.GetAxis("Mouse Y")*Sensetive;
        MouseY = Mathf.Clamp(MouseY, -45, 45);
        gameObject.transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0);
    }
    private void Update()
    {
        PlayerMove();
        RotateCamera();
    }
}
