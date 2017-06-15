using System;
using UnityEngine;

namespace BlindWizard.MonoBehaviours
{
	public class WizWave : MonoBehaviour
	{
		private float _time;
		private Vector3 _startPosition;

		private void Start ()
		{
			_time = 0;
			_startPosition = transform.localPosition;
		}

		[SerializeField]
		private Light _spotlight;
		[SerializeField]
		private float _angle;
		[SerializeField]
		private float _distance;
		[SerializeField]
		private float _castRate;
		[SerializeField]
		private float _chaseRate;

		private void Update ()
		{
			if (_spotlight == null) return;
			if (_time < _distance)
			{
				_spotlight.spotAngle = _angle;
				_spotlight.range = _time;
				transform.localPosition = _startPosition;
				_time += Time.deltaTime * _castRate;
			}
			else if (_time < _distance * 2)
			{
				var opposite = Math.Tan(Math.PI * (_angle / 2d) / 180d) * _distance;
				var inverseDistance = _distance * 2d - _time;
				var rad = Math.Atan(opposite / inverseDistance);
				_spotlight.spotAngle = (float) (rad * (180d / Math.PI) * 2d);
				_spotlight.range = (float) inverseDistance;
				var offset = _startPosition;
				offset.z += (float) (_distance - inverseDistance);
				transform.localPosition = offset;
				_time += Time.deltaTime * _chaseRate;
			}
			else _time = 0;
		}
	}
}
