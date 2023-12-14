using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class NullAwareList<T>
{
    [SerializeField] private List<T> list;

    public NullAwareList()
    {
        this.list = new List<T>();
    }

    public NullAwareList(int capacity)
    {
        this.list = new List<T>(capacity);
    }

    public void GenerateList(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            this.list.Add(default(T));
        }
    }

    public bool IsContainsNull()
    {
        for (int i = 0; i < this.list.Count; i++)
        {
            if (this.list[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsAllNull()
    {
        for (int i = 0; i < this.list.Count; i++)
        {
            if (this.list[i] != null)
            {
                return false;
            }
        }
        return true;
    }

    public void Add(T child)
    {
        int nullIndex = this.list.FindIndex(item => item == null);
        if (nullIndex != -1)
        {
            this.list[nullIndex] = child;
        }
        else
        {
            this.list.Add(child);
        }
    }

    public void Add(T child, int index)
    {
        this.list[index] = child;
    }

    public void Remove(T child)
    {
        int childIndex = this.list.IndexOf(child);
        this.list.RemoveAt(childIndex);
        this.list.Insert(childIndex, default(T));
    }

    public void RemoveAt(int index)
    {
        this.list.RemoveAt(index);
        this.list.Insert(index, default(T));
    }
    public List<T> GetList()
    {
        return this.list;
    }
}
