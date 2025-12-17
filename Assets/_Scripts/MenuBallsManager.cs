using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuBallsManager : MonoBehaviour
{
    [SerializeField] private GameObject menuBallPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private AnimationCurve ballCurve;
    [SerializeField] private float speed = 5;
    [SerializeField] private float YAmplitude = 5;
    private List<MenuBallData> menuBalls = new List<MenuBallData>();

    private class MenuBallData
    {
        public GameObject Ball;
        public  float Time; // how much time passed since spawn
        public Vector3 InitPos;
    }

    private void Awake()
    {
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        while (true)
        {
            SpawnBall();
            yield return new WaitForSeconds(Random.Range(0.7f, 2f));
        }
    }

    private void SpawnBall()
    {
        Vector3 pos = ballSpawnPoint.position + new Vector3(0,Random.Range(-4f,4f),0);
        GameObject instance = Instantiate(menuBallPrefab,pos , Quaternion.identity);
        menuBalls.Add(new MenuBallData()
        {
            Ball = instance,
            Time = 0,
            InitPos = pos 
        });
    }

    public void Update()
    {
        
        foreach (MenuBallData menuBall  in menuBalls)
        {
            menuBall.Time += Time.deltaTime;
            
            Vector3 pos = menuBall.Ball.transform.position;
            pos.x += speed * Time.deltaTime;
            pos.y  =  menuBall.InitPos.y +  ballCurve.Evaluate(menuBall.Time) * YAmplitude;
            menuBall.Ball.transform.position = pos;
        }
    }




}
