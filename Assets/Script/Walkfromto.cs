using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Walkfromto : MonoBehaviour
{
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public Transform endPoint;
    [SerializeField] public Transform leaving;
    [SerializeField] float walk_duration;
    [SerializeField] float turn_speed;

    [SerializeField] StyleOver sO;
    private bool mooving = false;
    private bool turning = false;
    private Vector3 trajet;
    private Quaternion turjet;
    private float lastTime;
    private float firstTime;
    private bool isArrived = false;

    public void CommencerLeDeplacement()
    {
        sO.enabled = false;
        mooving = true;
        this.gameObject.transform.position = spawnPoint.position;
        this.gameObject.transform.rotation = spawnPoint.rotation;
        trajet = endPoint.position - spawnPoint.position;
        lastTime = Time.time;
        firstTime = lastTime;
    }

    public void Rentrer()
    {
        sO.noModification();
        turning = true;
        gameObject.GetComponent<Animator>().SetBool("IsIdle", false);
        trajet = spawnPoint.position - endPoint.position;
        lastTime = Time.time;
        firstTime = lastTime;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mooving)
        {
            float TimeNow = Time.time;
            float deltaTime = TimeNow - lastTime;
            lastTime = TimeNow;
            float percentageOfTrajet = deltaTime / walk_duration;
            gameObject.transform.position += trajet * percentageOfTrajet;
            //Fin du déplacement
            if(TimeNow-firstTime > walk_duration)
            {
                mooving=false;
                gameObject.GetComponent<Animator>().SetBool("IsIdle", true);
                turning = true;
                if(isArrived)
                {
                    Debug.Log("Dino dead");
                    transform.parent.GetComponent<SpawnDinoEmployee>().MyDinoEmployeeEnded();
                }
            }
        }
        if (turning)
        {
            float TimeNow = Time.time;
            float deltaTime = TimeNow - lastTime;
            lastTime = TimeNow;
            turjet = transform.rotation;
            if (isArrived)
            {
                transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, leaving.rotation, turn_speed * deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, endPoint.rotation, turn_speed * deltaTime);
            }
            //détection de la fin de "se tourner"
            if ((transform.rotation == endPoint.rotation && isArrived == false) || (isArrived == true && transform.rotation == leaving.rotation))
            {
                turning = false;
                if(!isArrived)
                {
                    sO.enabled = true;
                }
                if (isArrived == true)
                {
                    mooving = true;
                }
                isArrived = true;
            }
        }
    }
}
