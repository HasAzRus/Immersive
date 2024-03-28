using UnityEngine;

namespace Oxygen
{
    public class Enemy : Character
    {
        [SerializeField] private float _closestDistance;
        [SerializeField] private LayerMask _obstacleLayerMask;
        
        private bool _isPlayerClosest;

        private Player _player;
        private Transform _playerTransform;

        private Transform _transform;

        protected virtual void OnPlayerClosest()
        {
            
        }

        protected virtual void OnPlayerFarAway()
        {
            
        }
        
        protected virtual void OnAlive()
        {
            var playerPosition = _playerTransform.position;
            var position = _transform.position;
            
            var distanceToPlayer = Vector3.Distance(position, playerPosition);

            var isDistanceClosest = distanceToPlayer < _closestDistance;
            
            var isObstacleNotExists = Physics.Linecast(position, playerPosition, _obstacleLayerMask);

            _isPlayerClosest = _player.IsDead &&
                               isDistanceClosest &&
                               isObstacleNotExists;

            if (_isPlayerClosest)
            {
                OnPlayerClosest();
            }
            else
            {
                OnPlayerFarAway();
            }
        }

        protected override void Start()
        {
            base.Start();

            _transform = transform;
        }

        protected override void Update()
        {
            base.Update();

            if (_player == null)
            {
                _player = FindFirstObjectByType<Player>();
                
                if (_player != null)
                {
                    _playerTransform = _player.transform;
                }
                
                return;
            }

            if (IsDead)
            {
                return;
            }
            
            OnAlive();
        }

        public Player GetPlayer()
        {
            return _player;
        }

        public Transform GetPlayerTransform()
        {
            return _playerTransform;
        }

        public Vector3 GetDirectionToPlayer()
        {
            return _playerTransform.position - _transform.position;
        }

        public Transform GetTransform()
        {
            return _transform;
        }

        public LayerMask GetObstacleLayerMask()
        {
            return _obstacleLayerMask;
        }
    }
}