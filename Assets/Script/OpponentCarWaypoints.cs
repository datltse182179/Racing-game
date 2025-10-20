using UnityEngine;

public class OpponentCarWaypoint : MonoBehaviour
{
    [Header("Opponent Car")]
    public OpponentCar opponentCar;
    public Waypoint currentWaypoint;

    private void wake()
    {
        opponentCar = GetComponent<OpponentCar>();
    }

    private void Start()
    {
        opponentCar.LocateDestination(currentWaypoint.GetPosition());
    }

    private void Update()
    {
        if (opponentCar.destinationReached)
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
            opponentCar.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}
