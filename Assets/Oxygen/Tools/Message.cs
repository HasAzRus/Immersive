using UnityEngine;

namespace Oxygen
{
    public sealed class Message : Behaviour
    {
        public void Print(string text)
        {
            Debug.Log(text);
        }
    }
}