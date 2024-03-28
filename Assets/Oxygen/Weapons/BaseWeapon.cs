using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oxygen
{
	public abstract class BaseWeapon : Behaviour
	{
		public event Action<float> CooldownValueChanged;

		public event Action Cooldowning;
		public event Action Cooldowned;

		[Header("Base Weapon")]

		[SerializeField] private string _name;
		[SerializeField] private bool _isActive;

		[SerializeField] private float _damage;
		[SerializeField] private float _maxCooldownTime;

		[SerializeField] private float _fireSpeed;

		private Transform _ownerTransform;

		private bool _isCooldown;
		private float _cooldownTime;

		private bool _isSelected;

		private GameObject _owner;
		
		public void Construct(GameObject owner)
		{
			OnConstruct(owner);
			
			_owner = owner;
		}

		protected abstract bool OnFire(int mode);

		protected virtual void OnConstruct(GameObject owner)
		{
			
		}
		
		protected virtual void OnStopFire(int mode) 
		{
			
		}

		protected virtual void OnSelect()
		{

		}

		protected virtual void OnDeselect()
		{

		}

		protected virtual void OnClear()
		{

		}

		protected override void Start()
		{
			base.Start();

			_ownerTransform = _owner.transform;
		}

		protected override void Update()
		{
			base.Update();

			if (_isCooldown)
			{
				if (_cooldownTime < _maxCooldownTime)
				{
					_cooldownTime += Time.deltaTime * _fireSpeed;
				}
				else
				{
					_cooldownTime = 0f;
					_isCooldown = false;

					Cooldowned?.Invoke();
				}

				CooldownValueChanged?.Invoke(_cooldownTime / _maxCooldownTime);
			}
		}

		protected void StartCooldown()
		{
			_isCooldown = true;

			Cooldowning?.Invoke();
		}

		protected bool CheckCooldown()
		{
			return _isCooldown;
		}

		public bool Fire(int mode)
		{
			return OnFire(mode);
		}

		public void StopFire(int mode)
		{
			OnStopFire(mode);
		}

		public void Select()
		{
			_isSelected = true;

			OnSelect();
		}

		public void Deselect()
		{
			_isSelected = false;

			OnDeselect();
		}

		public void Clear()
		{
			OnClear();
		}

		public bool CheckActive()
		{
			return _isActive;
		}
		
		public void SetActive(bool value)
		{
			_isActive = value;
		}

		public void SetFireSpeed(float value)
		{
			_fireSpeed = value;
		}

		public bool CheckSelected()
		{
			return _isSelected;
		}

		public float GetDamage()
		{
			return _damage;
		}

		public string GetName()
		{
			return _name;
		}

		public GameObject GetOwner()
		{
			return _owner;
		}
		
		public Transform GetOwnerTransform()
		{
			return _ownerTransform;
		}
	}
}