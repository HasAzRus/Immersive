using UnityEngine;

namespace Oxygen
{
    public class Message : Behaviour
    {
        public void Print(string text)
        {
            Debug.Log(text);
        }
    }
}