using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Oxygen
{
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    public enum Direction2D
    {
        Left,
        Right
    }

    [Serializable]
    public class Range
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min => _min;
        public float Max => _max;
    }
    
    public class World
    {
        public static bool Trace(Vector3 origin, Vector3 direction, float distance, LayerMask layerMask,
            out RaycastHit hitInfo)
        {
            return Physics.Raycast(origin, direction, out hitInfo, distance, layerMask);
        }
        
        public static bool Trace(Ray ray, float distance, LayerMask layerMask, out RaycastHit hitInfo)
        {
            return Physics.Raycast(ray, out hitInfo, distance, layerMask);
        }

        public static void SetLayer(GameObject target, int layer, bool noChild)
        {
            target.layer = layer;

            if (noChild)
            {
                return;
            }

            var targetTransform = target.transform;

            for (var i = 0; i < targetTransform.childCount; i++)
            {
                var iChildGameObject = targetTransform.GetChild(i).gameObject;

                iChildGameObject.layer = layer;
            }
        }

        public static void Scale(Transform target, float amount, bool relative = true)
        {
            if (relative)
            {
                target.localScale *= amount;
            }
            else
            {
                target.localScale = Vector3.one * amount;
            }
        }

        public static int Choose(params int[] numbers)
        {
            return numbers[Random.Range(0, numbers.Length)];
        }
    }
}