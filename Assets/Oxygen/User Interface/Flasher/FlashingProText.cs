using TMPro;
using UnityEngine;

namespace Oxygen
{
    public class FlashingProText : Behaviour
    {
        [SerializeField] private float _speed;

        [SerializeField] private bool _beginToFlashOnStart;

        [SerializeField] private TextMeshPro _text;

        private bool _isFlashing;

        protected override void Start()
        {
            base.Start();

            _isFlashing = _beginToFlashOnStart;
        }

        protected override void Update()
        {
            base.Update();

            if(_isFlashing)
            {
                var color = _text.color;
                color.a = Mathf.Abs(Mathf.Sin(Time.time * _speed));

                _text.color = color;
            }
        }
    }
}