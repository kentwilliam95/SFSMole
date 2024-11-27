using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Pool<T>
    {
        private static Pool<T> instance;
        public static Pool<T> Instance => instance;
        private T obj;
        private List<T> active;
        private Queue<T> notActive;
        private Func<T, T> OnCreate;
        private Action<T> OnGet;
        private Action<T> OnRecycle;
        private Action<T> OnDestroy;

        public int ActiveCount => active.Count;
        public int DisableCount => notActive.Count;

        public Pool(int capacity, Func<T, T> onCreate, Action<T> onRecycle, Action<T> onGet, Action<T> onDestroy, T obj)
        {
            instance = this;
            active = new List<T>(capacity);
            notActive = new Queue<T>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                var t = onCreate.Invoke(obj);
                notActive.Enqueue(t);
            }

            OnCreate = onCreate;
            OnRecycle = onRecycle;
            OnGet = onGet;
            OnDestroy = onDestroy;
            this.obj = obj;
        }

        public T Get()
        {
            if (notActive.Count <= 0)
            {
                var t = OnCreate.Invoke(obj);
                active.Add(t);
                OnGet.Invoke(t);
                return t;
            }
            else
            {
                var t = notActive.Dequeue();
                active.Add(t);
                OnGet.Invoke(t);
                return t;
            }
        }

        public void Recycle(T t)
        {
            if (active.Contains(t))
            {
                active.Remove(t);
                notActive.Enqueue(t);
                OnRecycle?.Invoke(t);
            }
            else
            {
                Debug.LogWarning($"[Pool]{t.ToString()} No Object in list active");
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < active.Count; i++)
            {
                OnDestroy.Invoke(active[i]);
            }

            var count = notActive.Count;
            for (int i = 0; i < count; i++)
            {
                var obj = notActive.Dequeue();
                OnDestroy.Invoke(obj);
            }

            active.Clear();
            notActive.Clear();
        }
    }
}
