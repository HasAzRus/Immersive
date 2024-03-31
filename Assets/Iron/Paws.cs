using System;
using UnityEngine;

namespace Iron
{
	[Serializable]
	public class Limb
	{
		[SerializeField] private float _distance;
		[SerializeField] private float _height;

		public float GetDistance() 
		{
			return _distance;
		}

		public float GetHeight()
		{
			return _height;
		}
	}

	[Serializable]
	public class Paw
	{
		[SerializeField] private LineRenderer _lineRenderer;

		private Limb[] _limbs;

		private int _count;

		private Vector3 _position;

		public void Initialize(Limb[] limbs)
		{
			_count = limbs.Length + 2;
			_lineRenderer.positionCount = _count;
		
			_limbs = limbs;
		}

		public void Update(Vector3 position)
		{
			_lineRenderer.SetPosition(0, position);
		}

		public void Place(Vector3 startPosition, Vector3 direction, Vector3 endPosition, Vector3 normal)
		{
			for(var i = 0; i < _limbs.Length; i++)
			{
				var limb = _limbs[i];

				_lineRenderer.SetPosition(i + 1, startPosition + direction * limb.GetDistance() + normal * limb.GetHeight());
			}

			_position = endPosition;

			_lineRenderer.SetPosition(_count - 1, endPosition);

			_lineRenderer.enabled = true;
		}

		public void Remove()
		{
			_lineRenderer.enabled = false;
		}

		public Vector3 GetPosition()
		{
			return _position;
		}

		public void SetAlpha(float value)
		{
			var startColor = _lineRenderer.startColor;
			var endColor = _lineRenderer.endColor;

			startColor.a = value;
			endColor.a = value;

			_lineRenderer.startColor = startColor;
			_lineRenderer.endColor = endColor;
		}
	}

	public class Paws : Oxygen.Behaviour
	{
		[SerializeField] private float _maxLineDistance;

		[SerializeField] private LayerMask _lineLayerMask;

		[SerializeField] private Limb[] _limbs;

		[SerializeField] private Paw[] _paws;

		private bool _isFading;
		private float _fadeTime;

		private int _count;

		private bool _allowDestroy;

		private Transform _transform;

		protected override void Start()
		{
			base.Start();

			_transform = transform;

			_count = _paws.Length;

			_isFading = false;

			foreach(var paw in _paws) 
			{
				paw.Initialize(_limbs);
			}
		}

		protected override void Update()
		{
			base.Update();

			for (var i = 0; i < _count; i++)
			{
				var paw = _paws[i];

				paw.Update(_transform.position);

				if (Vector3.Distance(_transform.position, paw.GetPosition()) > _maxLineDistance)
				{
					var direction = UnityEngine.Random.onUnitSphere;

					if (Physics.Raycast(_transform.position, direction, out var hit, _maxLineDistance,
						_lineLayerMask))
					{
						paw.Place(_transform.position, direction, hit.point, hit.normal);
					}
					else
					{
						paw.Remove();
					}
				}
			}

			if (_isFading)
			{
				if (_fadeTime < 1)
				{
					_fadeTime += Time.deltaTime / 4;
				}
				else
				{
					_fadeTime = 0;
					_isFading = false;

					if (_allowDestroy)
					{
						Destroy(gameObject);
					}
				}

				foreach (var paw in _paws)
				{
					paw.SetAlpha(1 - _fadeTime);
				}
			}
		}

		public void Fade(bool allowDestroy)
		{
			_isFading = true;

			_allowDestroy = allowDestroy;
		}
	}
}