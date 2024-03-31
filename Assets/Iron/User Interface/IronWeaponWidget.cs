using Oxygen;
using UnityEngine;
using UnityEngine.UI;

namespace Iron
{
    public class IronWeaponWidget : PageWidget
    {
        [SerializeField] private Image _crosshairImage;

        [SerializeField] private float _crosshairAlpha;
        [SerializeField] [Range(0.01f, 0.99f)] private float _crosshairScaleAmount;
        
        [SerializeField] private Text _ammoText;
        [SerializeField] private GameObject _ammoGameObject;

        private RectTransform _crosshairRectTransform;

        private Coroutine _crosshairImageCrossFadingRoutine;
        private Coroutine _crosshairScalingRoutine;

        private Vector2 _crosshairInitialSize;
        private float _crosshairScale;
        
        private BaseFirearmWeapon _currentFirearmWeapon;

        private IronPlayer _ironPlayer;
        
        public void Construct(IronPlayer ironPlayer)
        {
            ironPlayer.WeaponManager.WeaponChanged += OnWeaponChanged;

            _ironPlayer = ironPlayer;
        }

        protected override void Start()
        {
            base.Start();
            
            _crosshairRectTransform = _crosshairImage.GetComponent<RectTransform>();

            _crosshairInitialSize = _crosshairRectTransform.sizeDelta;
            _crosshairScale = 1f;
            
            SetWeaponCrosshairCrossFade(true);
        }

        private void SetAmmoText(int ammo, int max)
        {
            _ammoText.text = $"{ammo}/{max}";
        }

        private void SetWeaponCrosshairCrossFade(bool fade)
        {
            if (_crosshairImageCrossFadingRoutine != null)
            {
                Coroutines.Stop(_crosshairImageCrossFadingRoutine);
            }

            _crosshairImageCrossFadingRoutine =
                Coroutines.Run(ProcedureAnimation.CrossFadeImage(fade ? 0f : _crosshairAlpha, 1f, 
                    _crosshairImage));
        }

        private void SetWeaponCrosshairScaling(bool scale)
        {
            if (_crosshairScalingRoutine != null)
            {
                Coroutines.Stop(_crosshairScalingRoutine);
            }

            _crosshairScalingRoutine = Coroutines.Run(ProcedureAnimation.PlayRoutine(1f, deltaTime =>
            {
                _crosshairScale = Mathf.Lerp(_crosshairScale, scale ? _crosshairScaleAmount : 1f,
                    deltaTime);
                _crosshairRectTransform.sizeDelta = _crosshairInitialSize * _crosshairScale;
            }, null));
        }

        private void UpdateFirearmWeaponDraw(BaseFirearmWeapon weapon)
        {
            if (weapon == null)
            {
                _ammoText.text = string.Empty;
                _ammoGameObject.Deactivate();
                
                SetWeaponCrosshairCrossFade(true);

                return;
            }

            _ammoGameObject.Activate();
            
            SetWeaponCrosshairCrossFade(false);
            SetAmmoText(weapon.Ammo, weapon.MaxAmmo);
        }
        
        private void OnWeaponChanged(BaseWeapon weapon)
        {
            if (_currentFirearmWeapon != null)
            {
                _currentFirearmWeapon.AmmoChanged -= OnWeaponAmmoChanged;
                _currentFirearmWeapon.Aiming -= OnWeaponAiming;
                _currentFirearmWeapon.AimStopped -= OnWeaponAimStopped;

                UpdateFirearmWeaponDraw(null);
            }

            if (weapon is not BaseFirearmWeapon firearmWeapon)
            {
                return;
            }

            firearmWeapon.AmmoChanged += OnWeaponAmmoChanged;
            
            firearmWeapon.Aiming += OnWeaponAiming;
            firearmWeapon.AimStopped += OnWeaponAimStopped;
            
            UpdateFirearmWeaponDraw(firearmWeapon);
            
            _currentFirearmWeapon = firearmWeapon;
        }

        private void OnWeaponAimStopped()
        {
            SetWeaponCrosshairScaling(false);
        }

        private void OnWeaponAiming()
        {
            SetWeaponCrosshairScaling(true);
        }

        private void OnWeaponAmmoChanged(int ammo)
        {
            SetAmmoText(ammo, _currentFirearmWeapon.MaxAmmo);
        }

        public void Clear()
        {
            _ironPlayer.WeaponManager.WeaponChanged -= OnWeaponChanged;
        }
    }
}