﻿using UnityEngine;
using System.Collections;

public class Swarmer : MonoBehaviour {
	public static readonly int KeyPositionsAroundTarget = 6;

    private HexGridManager _gridContainer = null;
	private NavMeshAgent _navAgent = null;
	private SwarmTarget _target = null;
    private bool needsDestination = true;
	
    private HexGridManager.Occupant _occupant = null;
    private HexGridManager.Reservation _reservation = null;
	
    HexGridManager.IntVector2 currentGrid = new HexGridManager.IntVector2();
    HexGridManager.IntVector2 tempGrid = new HexGridManager.IntVector2();
	
	// Use this for initialization
	void Start () {
		GameObject gridContainer = GameObject.FindGameObjectWithTag("GridContainer");
        _gridContainer = gridContainer.GetComponent< HexGridManager >();

		_navAgent = GetComponent< NavMeshAgent >();

		GameObject swarmGameObject = GameObject.FindGameObjectWithTag ("SwarmTarget");
		_target = swarmGameObject.GetComponent< SwarmTarget > ();

		_occupant = _gridContainer.CreateOccupant(gameObject, 1);
		
		_gridContainer.PositionToGrid(transform.position, currentGrid);
	}
	
	void OnDestroy() 
	{
		_gridContainer.ReturnOccupant(ref _occupant);
        _gridContainer.ReturnReservation(ref _reservation);
	}
	
	void FindSwarmDestination()
	{
        _gridContainer.GetClosestVacantNeighbor(_target.gameObject, tempGrid, gameObject);
        Vector3 destination = Vector3.zero;
        _gridContainer.GridToPosition(tempGrid, ref destination);

        _navAgent.destination = destination;
        _reservation = _gridContainer.CreateReservation(destination);

        needsDestination = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if ( needsDestination )
		{
			FindSwarmDestination();
		} 		
	}

    public void OnGridChanged()
    {
        if( _gridContainer.IsOccupied( tempGrid, _reservation ) )
        {
            _gridContainer.ReturnReservation(ref _reservation);
            needsDestination = true;
        }
    }
}
