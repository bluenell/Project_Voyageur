using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doormat : MonoBehaviour
{
    [SerializeField]
    int sortingOrder;

    public int GetSortingOrder()
    {
        return sortingOrder;
    }
}
