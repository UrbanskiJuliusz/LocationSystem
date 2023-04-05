using UnityEngine;

namespace Assets.Scripts.DesignPatterns
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T instance;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(this);
            }
        }

        public void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}