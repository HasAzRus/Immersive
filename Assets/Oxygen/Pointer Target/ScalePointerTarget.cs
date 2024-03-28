using UnityEngine;

namespace Oxygen
{
	public class ScalePointerTarget : BasePointerTarget
	{
		[SerializeField] private float _amount;

		[SerializeField] private Transform _meshTransform;

		private Vector3 _scale;
		private Vector3 _initialScale;

		protected override bool OnEnter(Player player)
		{
			_scale = _initialScale * _amount;

			return true;
		}

		protected override void OnExit(Player player)
		{
			base.OnExit(player);

			_scale = _initialScale;
		}

		protected override void Start()
		{
			base.Start();
			
			_initialScale = _meshTransform.localScale;
			_scale = _initialScale;
		}

		protected override void Update()
		{
			base.Update();

			_meshTransform.localScale = _scale;
		}
	}
}