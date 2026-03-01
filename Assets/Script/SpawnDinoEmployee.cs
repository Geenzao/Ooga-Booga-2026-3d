using UnityEngine;

public class SpawnDinoEmployee : MonoBehaviour
{
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public Transform endPoint;
    [SerializeField] public Transform leaving;
    [SerializeField] GameObject prefabDinoEmployee;
    [SerializeField] float timeminBetweenTwoEmployees;
    [SerializeField] float timemaxBetweenTwoEmployees;
    private float timeAtWitchIKilledLastEmployee;
    private float tirageuntilnextEmploye;
    private GameObject dinoEmployee;

    public void Start()
    {
        GameStateManager.Instance.OnGameReset += resetGame;
        tirageuntilnextEmploye = UnityEngine.Random.Range(timeminBetweenTwoEmployees, timemaxBetweenTwoEmployees);
        timeAtWitchIKilledLastEmployee = Time.time;
    }

    public void MyDinoEmployeeEnded()
    {
        Destroy(dinoEmployee);
        timeAtWitchIKilledLastEmployee = Time.time;
    }

    public void resetGame()
    {
        if (dinoEmployee != null)
        {
            Destroy(dinoEmployee);
        }
        tirageuntilnextEmploye = UnityEngine.Random.Range(timeminBetweenTwoEmployees, timemaxBetweenTwoEmployees);
        timeAtWitchIKilledLastEmployee = Time.time;
    }

    public void SpawnEmployee()
    {
        tirageuntilnextEmploye = UnityEngine.Random.Range(timeminBetweenTwoEmployees, timemaxBetweenTwoEmployees);
        dinoEmployee = Instantiate(prefabDinoEmployee,gameObject.transform);
        dinoEmployee.GetComponent<Walkfromto>().spawnPoint = spawnPoint;
        dinoEmployee.GetComponent<Walkfromto>().endPoint = endPoint;
        dinoEmployee.GetComponent<Walkfromto>().leaving = leaving;
        dinoEmployee.GetComponent<Walkfromto>().CommencerLeDeplacement();
    }

    public void Update()
    {
        if(StatsManager.Instance.XPLvl == 2)
        {
            if (dinoEmployee == null)
            {
                if (Time.time - timeAtWitchIKilledLastEmployee > tirageuntilnextEmploye)
                {
                    SpawnEmployee();
                }
            }
        }
    }
}
