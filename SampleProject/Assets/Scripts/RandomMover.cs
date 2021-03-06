﻿using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour {

    public bool roam = false;

    private HexGridManager _gridManager = null;
    private NavMeshAgent _navAgent = null;
    private HexGridManager.Occupant _occupant = null;
    private Vector3 tempGrid = Vector3.zero;

	// Use this for initialization
	void Start () {
        GameObject gridContainer = GameObject.FindGameObjectWithTag("GridContainer");
        _gridManager = gridContainer.GetComponent< HexGridManager >();
        _navAgent = GetComponent< NavMeshAgent >();
        _occupant = _gridManager.CreateOccupant(gameObject, 1);
	}

    void OnDestroy() 
    {
        _gridManager.ReturnOccupant(ref _occupant);
    }

	// Update is called once per frame
	void Update () {

        float mag = _navAgent.velocity.sqrMagnitude;
        if ( mag < 0.001f && roam )
        {
            GetNewDestination();
        } 
	}

    void OnGridChanged()
    {
        if( _gridManager.IsOccupied( tempGrid ) )
        {
            GetNewDestination();
        }
    }

    void GetNewDestination()
    {
        tempGrid.Set(Random.Range(-70, 70), 0, Random.Range(-70, 70));
        
        if (_gridManager.IsValid(tempGrid) && !_gridManager.IsOccupied(tempGrid))
        {
            Vector3 destination = Vector3.zero;
            _gridManager.GridToPosition( tempGrid, ref destination );
            _navAgent.destination = destination;
        }
    }
}
