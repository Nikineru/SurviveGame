using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourseGenerator : MonoBehaviour
{
    public struct GeneratePlace
    {
        public Vector3 StartPlace;
        public float Width, Lenght;
        public List<GameObject> Resourses;
        public int ResourseCount;

        public GeneratePlace(Vector3 startPlace, float width, float lenght, List<GameObject> resourses,int count)
        {
            StartPlace = startPlace;
            Width = StartPlace.x + width;
            Lenght = StartPlace.z + lenght;
            Resourses = resourses;
            ResourseCount = count;
        }
    }
    public List<GameObject> waterResourse = new List<GameObject>();
    public static List<GameObject> WaterResourse = new List<GameObject>();
    public List<GameObject> food = new List<GameObject>();
    public static List<GameObject> Food = new List<GameObject>();

        public  List<GeneratePlace> ResoursePlaces = new List<GeneratePlace>()
        {
        new GeneratePlace(new Vector3(0,1.5f,0),10,10,Food,10),
        new GeneratePlace(new Vector3(10,1.5f,10),10,10,WaterResourse,10)
        };

    public void Generate() 
    {
        foreach (var item in ResoursePlaces)
        {
            for (int i = 0; i < item.ResourseCount; i++)
            {
                Instantiate(item.Resourses[Random.Range(0, item.Resourses.Count)], new Vector3(Random.Range(item.StartPlace.x, item.Width), item.StartPlace.y, Random.Range(item.StartPlace.z, item.Lenght)), Quaternion.identity);
            }
        }
    }
    void Start()
    {
        foreach (var item in waterResourse)
        {
            WaterResourse.Add(item);
        }
        foreach (var item in food)
        {
            Food.Add(item);
        }
        Generate();
    }
}
