using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor.UIElements;

public class PlayerControll : MonoBehaviour
{

    public float Sensetive;

    [Header("Скорости персонажа")]
    public float MoveSpeed;//Обычная скорость движения
    public float JumpForse;//Сила прыжка
    private float SitSpeed;//Скорость в присиде
    private float RunSpeed;//Скорость при беге
    private float SimpleSpeed;//Хранит начальную скорость

    [Header("Характеристики")]
    public float Stamina;//Энергия для бега
    public float Health;//Здоровье
   [BoostName("Еда")] public float Food;//Еда
    public float SleepEnergy = 100;//Енергия до сна
    public float Thirst;//Жажда

    private float MouseX;//Положение мыши по Иксу
    private float MouseY;//Положение мыщи по Игрику
    private Rigidbody PlayerRigedBody;//Физика персонажа
    private Coroutine StaminaDroppingCorutin;//Ссылка на корутину усталости
    private List<float> Characteristick;//Лист всех характеристик
    private List<GameObject> CharacteristicksIcons = new List<GameObject>();//Иконки всех характеристик
    private void Start()
    {
        PlayerRigedBody = GetComponent<Rigidbody>();
        SimpleSpeed = MoveSpeed;
        SitSpeed = SimpleSpeed * 0.5f;
        RunSpeed = SimpleSpeed * 1.5f;
        Characteristick = new List<float>() {Stamina,Health,Food,SleepEnergy,Thirst};//Заполняем лист всеми характеристиками
        CharacteristicksIcons = GameObject.Find("PlayerUI/Icons").GetComponentsInChildren<Transform>().Select(i=>i.gameObject).ToList();//Выбираем все GameObject из обьекта с иконкми
        CharacteristicksIcons = CharacteristicksIcons.Where(i => i.GetComponent<ProgressBarScript>() != null).ToList();//Выбираем те на которых весит скрипт ProGressBarScript

        StartCoroutine(CharacteristicDropping(3, 0.005f,EndValue: 0,Speed: 0.01f));//Снижение Голода
        StartCoroutine(CharacteristicDropping(2, 0.05f, EndValue: 0, Speed: 0.1f));//Снижение Сна
    }
    public IEnumerator PlayerMove() 
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
        if (Input.GetKeyDown(KeyCode.LeftShift)&&Stamina>0)
        {
            MoveSpeed = RunSpeed;
            if(StaminaDroppingCorutin!=null)
            StopCoroutine(StaminaDroppingCorutin);
            StaminaDroppingCorutin = StartCoroutine(CharacteristicDropping(0,0.5f,Speed:0.1f));
            Stamina = Characteristick[0];
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopCoroutine(StaminaDroppingCorutin);
            MoveSpeed = SimpleSpeed;
            yield return new WaitForSeconds(5);
            StaminaDroppingCorutin = StartCoroutine(CharacteristicDropping(0,0.5f,100,0.1f,false));
            Stamina = Characteristick[0];
        }
    }
    private void RotateCamera() 
    {
        MouseX += Input.GetAxis("Mouse X")*Sensetive;
        MouseY -= Input.GetAxis("Mouse Y")*Sensetive;
        MouseY = Mathf.Clamp(MouseY, -45, 45);
        gameObject.transform.localRotation = Quaternion.Euler(gameObject.transform.localRotation.eulerAngles.x, MouseX, 0);
        GetComponentsInChildren<Transform>().FirstOrDefault(i=>i.GetComponent<Camera>()!=null).localRotation = Quaternion.Euler(MouseY, GetComponentsInChildren<Transform>().FirstOrDefault(i => i.GetComponent<Camera>() != null).localRotation.eulerAngles.y, 0);
    }

    public IEnumerator CharacteristicDropping(int index,float Value = 5, float EndValue = 0, float Speed = 1,bool Mode = true) 
    {
        if (Mode)
        {
            while (Characteristick[index] > EndValue)
            {
                yield return new WaitForSeconds(Speed);
                Characteristick[index] -= Value;
                SetHaracteristiclk(Characteristick[index], index);
                CharacteristicksIcons[index].GetComponent<ProgressBarScript>().SetProgressBar(Characteristick[index] / 100);
            }
        }
        else
        {
            while (Characteristick[index] < EndValue)
            {
                yield return new WaitForSeconds(Speed);
                Characteristick[index] += Value;
                SetHaracteristiclk(Characteristick[index], index);
                CharacteristicksIcons[index].GetComponent<ProgressBarScript>().SetProgressBar(Characteristick[index] / 100);
            }
        }
    }
    private void SetHaracteristiclk(float value, int index) 
    {
        switch (index)
        {
            case 0:
                Stamina = value;
                break;
            case 1:
                Health = value;
                break;
            case 2:
                Food = value;
                break;
            case 3:
                SleepEnergy = value;
                break;
            case 4:
                Thirst = value;
                break;
        }
    }

    private void Update()
    {
        StartCoroutine(PlayerMove());
        RotateCamera();
    }
}
