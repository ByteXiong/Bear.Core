using System;
using System.Collections;
using System.Collections.Generic;

namespace BearPlatform.Common.ClassLibrary;

public class SynchronizedCollection<T> : IEnumerable<T>, IDisposable
{
    #region 外部接口

    public T this[int index]
    {
        get
        {
            using (Lock.Read())
            {
                return _list[index];
            }
        }
        set
        {
            using (Lock.Write())
            {
                _list[index] = value;
            }
        }
    }

    public int Count
    {
        get
        {
            using (Lock.Read())
            {
                return _list.Count;
            }
        }
    }

    public void Add(T item)
    {
        using (Lock.Write())
        {
            _list.Add(item);
        }
    }

    public void Clear()
    {
        using (Lock.Write())
        {
            _list.Clear();
        }
    }

    public bool Contains(T item)
    {
        using (Lock.Read())
        {
            return _list.Contains(item);
        }
    }

    public void CopyTo(T[] array, int index)
    {
        using (Lock.Read())
        {
            _list.CopyTo(array, index);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        using (Lock.Read())
        {
            return _list.GetEnumerator();
        }
    }

    public int IndexOf(T item)
    {
        using (Lock.Read())
        {
            return _list.IndexOf(item);
        }
    }

    public void Insert(int index, T item)
    {
        using (Lock.Write())
        {
            _list.Insert(index, item);
        }
    }

    public bool Remove(T item)
    {
        using (Lock.Write())
        {
            return _list.Remove(item);
        }
    }

    public void RemoveAt(int index)
    {
        using (Lock.Write())
        {
            _list.RemoveAt(index);
        }
    }

    #endregion

    #region 私有成员

    private UsingLock<object> Lock { get; } = new UsingLock<object>();
    private List<T> _list = new List<T>();

    IEnumerator IEnumerable.GetEnumerator()
    {
        using (Lock.Read())
        {
            return _list.GetEnumerator();
        }
    }

    public void Dispose()
    {
        _list.Clear();
        _list = null;

        Lock.Dispose();
    }

    #endregion
}
