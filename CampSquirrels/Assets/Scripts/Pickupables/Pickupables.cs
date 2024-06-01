using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Pickupables : MonoBehaviour
{
    [SerializeField]
    private ItemType itemType;
    public enum ItemType { wood, weapon, powerup, lantern };
    public ItemType GetItemType() {
        return itemType;
    }
}
