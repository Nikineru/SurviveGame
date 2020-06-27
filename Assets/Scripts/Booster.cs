using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float Health;
    public float Food;
    public float Thirsty;
    public float Stamina;
    public float SleepEnergy;
    private void Start()
    {
        List<MemberInfo> options = typeof(PlayerControll).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(n => Attribute.IsDefined(n, typeof(BoostName))).ToList();

        foreach (var item in options)
        {
            Debug.Log(item);
        }
    }
}
