using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
	public class NPCConnectedPatrol : MonoBehaviour {

		[SerializeField]
		bool _patrolWaiting;

		[SerializeField]
		float _totalWaitTime = 3f;

		[SerializeField]
		float _switchProbability = 0.2f;

		NavMeshAgent _navMeshAgent;
		ConnectedWayPoint _currentWayPoint;
		ConnectedWayPoint _previousWayPoint;

		bool _travelling;
		bool _waiting;
		float _waitTimer;
		int _waypointsVisited;

		// Use this for initialization
		public void Start () {
			_navMeshAgent = this.GetComponent<NavMeshAgent> ();

			if (_navMeshAgent == null) {
				Debug.Log ("THe nav mesh agent component is not attached to " + gameObject.name);
			}
			else
			{
				if (_currentWayPoint == null)
				{
					GameObject[] allWayPoints = GameObject.FindGameObjectsWithTag ("WayPoint");

					if (allWayPoints.Length > 0) {
						while (_currentWayPoint == null) {
							int random = UnityEngine.Random.Range (0, allWayPoints.Length);
							ConnectedWayPoint startingWayPoint = allWayPoints [random].GetComponent<ConnectedWayPoint> ();

							if (startingWayPoint != null) {
								_currentWayPoint = startingWayPoint;
							}
						}
					}
					else
					{
						Debug.LogError ("Failed to find any way points for use in the scene.");
					}
				}

				SetDestination ();
			}
		}

		public void FixedUpdate()
		{
			if (Input.GetKey ("escape"))
				Application.Quit ();
		}
		public void Update()
		{
			if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
			{
				_travelling = false;
				_waypointsVisited++;

				if (_patrolWaiting) {
					_waiting = true;
					_waitTimer = 0f;
				} 
				else 
				{
					SetDestination ();
				}
			}

			if (_waiting)
			{
				_waitTimer += Time.deltaTime;
				if (_waitTimer >= _totalWaitTime)
				{
					_waiting = false;

					SetDestination ();
				}
			}
		}

	public void SetDestination()
	{
		if(_waypointsVisited > 0)
		{
			ConnectedWayPoint nextWayPoint = _currentWayPoint.NextWayPoint(_previousWayPoint);
			_previousWayPoint = _currentWayPoint;
			_currentWayPoint = nextWayPoint;
		}

		Vector3 targetVector = _currentWayPoint.transform.position;
		_navMeshAgent.SetDestination(targetVector);
		_travelling = true;
	}
}
}