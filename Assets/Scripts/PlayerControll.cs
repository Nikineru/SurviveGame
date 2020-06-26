using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor.UIElements;

public class PlayerControll : MonoBehaviour
{
    public float Health;

    private Rigidbody PlayerRigedBody;

    public float MoveSpeed;
    public float Sensetive;
    public float JumpForse;
    public float stamina;
    private float SitSpeed;
    private float RunSpeed;
    private float SimpleSpeed;
    private float MouseX;
    private float MouseY;
    private List<float> Characteristicks;
    private void Start()
    {
        PlayerRigedBody = GetComponent<Rigidbody>();
        SimpleSpeed = MoveSpeed;
        SitSpeed = SimpleSpeed * 0.5f;
        RunSpeed = SimpleSpeed * 1.5f;

        Characteristicks = new List<float>() { Health, stamina };
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
            GetComponent<CapsuleCollider>().height -= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            MoveSpeed = SimpleSpeed;
            GetComponent<CapsuleCollider>().height += 2;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (stamina > 0) 
            {
                MoveSpeed = RunSpeed;
                var corutin = SmoothDropping(1, 20,5); 
                StartCoroutine(corutin);    
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MoveSpeed = SimpleSpeed;
        }
    }
    public IEnumerator SmoothDropping(float SpeedOfDropping,params int[] ValueOfDropping)
    {
        while (true) 
        {
            yield return new WaitForSeconds(SpeedOfDropping);
            var Zeroes = Characteristicks.Where(i => i == 0).ToList();
            if (Zeroes.Count >= Characteristicks.Count)
                break;

            for (int i = 0; i < Characteristicks.Count; i++)
            {
                if (Characteristicks[i] <= 0)
                    continue;
                try
                {
                    Characteristicks[i] -= ValueOfDropping[i];
                    Debug.Log(Characteristicks[i]);
                }
                catch
                {
                    Debug.Log("NullReferense");
                    i = 0;
                }
            }
            Health = Characteristicks[0];
            stamina = Characteristicks[1];
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
