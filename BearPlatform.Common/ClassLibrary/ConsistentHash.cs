using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BearPlatform.Common.ClassLibrary;

public class ConsistentHash<T>
{
    SortedDictionary<int, T> Circle { get; set; } = new SortedDictionary<int, T>();
    int _replicate = 100; //default _replicate count
    int[] _ayKeys; //cache the ordered keys for better performance

    //it's better you override the GetHashCode() of T.
    //we will use GetHashCode() to identify different node.
    public void Init(IEnumerable<T> nodes)
    {
        Init(nodes, _replicate);
    }

    public void Init(IEnumerable<T> nodes, int replicate)
    {
        _replicate = replicate;

        foreach (T node in nodes)
        {
            Add(node, false);
        }

        _ayKeys = Circle.Keys.ToArray();
    }

    public void Add(T node)
    {
        Add(node, true);
    }

    private void Add(T node, bool updateKeyArray)
    {
        for (int i = 0; i < _replicate; i++)
        {
            int hash = BetterHash(node.GetHashCode().ToString() + i);
            Circle[hash] = node;
        }

        if (updateKeyArray)
        {
            _ayKeys = Circle.Keys.ToArray();
        }
    }

    public void Remove(T node)
    {
        for (int i = 0; i < _replicate; i++)
        {
            int hash = BetterHash(node.GetHashCode().ToString() + i);
            if (!Circle.Remove(hash))
            {
                throw new System.Exception("can not remove a node that not added");
            }
        }

        _ayKeys = Circle.Keys.ToArray();
    }

    //we keep this function just for performance compare
    private T GetNode_slow(String key)
    {
        int hash = BetterHash(key);
        if (Circle.ContainsKey(hash))
        {
            return Circle[hash];
        }

        int first = Circle.Keys.FirstOrDefault(h => h >= hash);
        if (first == new int())
        {
            first = _ayKeys[0];
        }

        T node = Circle[first];
        return node;
    }

    //return the index of first item that >= val.
    //if not exist, return 0;
    //ay should be ordered array.
    int First_ge(int[] ay, int val)
    {
        int begin = 0;
        int end = ay.Length - 1;

        if (ay[end] < val || ay[0] > val)
        {
            return 0;
        }

        int mid = begin;
        while (end - begin > 1)
        {
            mid = (end + begin) / 2;
            if (ay[mid] >= val)
            {
                end = mid;
            }
            else
            {
                begin = mid;
            }
        }

        if (ay[begin] > val || ay[end] < val)
        {
            throw new System.Exception("should not happen");
        }

        return end;
    }

    public T GetNode(String key)
    {
        //return GetNode_slow(key);

        int hash = BetterHash(key);

        int first = First_ge(_ayKeys, hash);

        //int diff = circle.Keys[first] - hash;

        return Circle[_ayKeys[first]];
    }

    //default String.GetHashCode() can't well spread strings like "1", "2", "3"
    public static int BetterHash(String key)
    {
        uint hash = MurmurHash2.Hash(Encoding.ASCII.GetBytes(key));
        return (int)hash;
    }
}
