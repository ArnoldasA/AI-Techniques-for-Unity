using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//generic class that stores other objects
//Wherever this is called the class must use I comparable
public class ProrityQueue<T> where T: IComparable<T> 
{
    List<T> data;
    public int Count { get { return data.Count; } }

    public ProrityQueue()
    {
        this.data = new List<T>();
    }

    public void Enqueue(T item)
    {
        data.Add(item);

        int childIndex = data.Count - 1;

        while(childIndex >0)
        {
            int parentIndex = (childIndex - 1) / 2;

            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)// if the priority is greater equal then we can break, as the child is not smaller
                {
                break;
            }
            T tmp = data[childIndex]; // so what we do either is assign the child to become the parent then we rerun the while loop to see if our new place in the queque is the correct one
            data[childIndex] = data[parentIndex];
            data[parentIndex] = tmp;

            childIndex = parentIndex; // advance one more level up the binary tree as the child becomes the parent
        }
    }

    public T Dequeue()
    {
        int lastIndex = data.Count - 1;
        T frontItem = data[0];

        data[0] = data[lastIndex];

        data.RemoveAt(lastIndex);

        lastIndex--;

        int parentIndex=0;

        while(true)
        {
            int childIndex = parentIndex * 2 + 1;

            if(childIndex >lastIndex)
            {
                break;
            }
            int rightChild = childIndex + 1;
            if(rightChild <=lastIndex && data[rightChild].CompareTo(data[childIndex])<0)
            {
                childIndex = rightChild;
            }

            if(data[parentIndex].CompareTo(data[childIndex])<=0)
            {
                break;
            }
            T tmp = data[parentIndex];
            data[parentIndex] = data[childIndex];
            data[childIndex] = tmp;

            parentIndex = childIndex;
        }
        return frontItem;
    }

    public T Peek()
    {
        T fronItem = data[0];
        return fronItem;
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    public List<T> ToList()
    {
        return data;
    }
}
