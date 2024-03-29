using System;
using Oxygen;

namespace Iron
{
    public interface IReadableFlashlight
    {
        event Action Enabled;
        event Action Disabled;
        
        public IReadableEnergy ReadableEnergy { get; }
        public bool IsFlashlightEnabled { get; }
    }
}