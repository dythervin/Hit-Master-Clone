using System.Collections.Generic;

public class MyHashSet<T> : HashSet<T>
{
    private List<T> list;
    public MyHashSet()
    {
        list = new List<T>();
    }
    public T this[int index]
    {
        get { return list[index]; }
    }

    public new bool Add(T item)
    {
        if (base.Add(item))
        {
            list.Add(item);
            return true;
        }
        return false;
    }

    public void AddMany(params T[] values)
    {
        foreach (T item in values)
        {
            Add(item);
        }
    }
    public new bool Remove(T item)
    {
        if (base.Remove(item))
        {
            list.Remove(item);
            return true;
        }
        return false;
    }

    public new void Clear()
    {
        base.Clear();
        list.Clear();
    }

    public new void UnionWith(IEnumerable<T> other)
    {
        base.UnionWith(other);
        list = new List<T>(this);
    }
    public new void ExceptWith(IEnumerable<T> other)
    {
        base.ExceptWith(other);
        list = new List<T>(this);
    }
    public new void IntersectWith(IEnumerable<T> other)
    {
        base.IntersectWith(other);

        list = new List<T>(this);
    }
    public new void SymmetricExceptWith(IEnumerable<T> other)
    {
        base.SymmetricExceptWith(other);
        list = new List<T>(this);
    }

    ~MyHashSet()
    {
        list.Clear();
        list = null;
    }

}

