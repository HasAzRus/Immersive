using UnityEngine;

namespace Oxygen
{
    public class LocalizationHandler : Behaviour
    {
        [SerializeField] private LocalizationText[] _localizationTexts;

        private void OnLocalizationRegionChanged(Language language)
        {
            foreach (var localizationText in _localizationTexts)
            {
                localizationText.UpdateTextByLanguage(language);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
			
            Localization.LanguageChanged += OnLocalizationRegionChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
			
            Localization.LanguageChanged -= OnLocalizationRegionChanged;
        }

        protected override void Start()
        {
            base.Start();

            foreach (var localizationText in _localizationTexts)
            {
                localizationText.Initialize();
                localizationText.UpdateText();
            }
        }
    }
}