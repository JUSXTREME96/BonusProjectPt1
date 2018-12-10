using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
	public class ConnectedWayPoint : WayPoint {

	[SerializeField]
	protected float _connectivityRadius = 50f;

	List<ConnectedWayPoint> _connections;

	// Use this for initialization
	public void Start () {
			GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag ("WayPoint");

			_connections = new List<ConnectedWayPoint> ();

			for(int i = 0; i < allWaypoints.Length; i++)
			{
				ConnectedWayPoint nextWayPoint = allWaypoints [i].GetComponent<ConnectedWayPoint> ();

				if(nextWayPoint != null)
				{
					if (Vector3.Distance (this.transform.position, nextWayPoint.transform.position) <= _connectivityRadius && nextWayPoint != this) 
					{
						_connections.Add (nextWayPoint);
					}
				}
			}
		}

		public override void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (transform.position, debugDrawRadius);
		
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere (transform.position, _connectivityRadius);
		}

		public ConnectedWayPoint NextWayPoint(ConnectedWayPoint previousWayPoint)
		{
			if(_connections.Count == 0)
			{
				Debug.LogError ("Insufficient waypoint count.");
				return null;
			}
			else if(_connections.Count == 1 && _connections.Contains(previousWayPoint))
			{
				return previousWayPoint;
			}
			else
			{
				ConnectedWayPoint nextWayPoint;
				int nextIndex = 0;

				do {
					nextIndex = UnityEngine.Random.Range (0, _connections.Count);
					nextWayPoint = _connections [nextIndex];

				} while (nextWayPoint == previousWayPoint);

				return nextWayPoint;
			}
		}
	}
}
