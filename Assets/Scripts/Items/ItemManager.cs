using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Transform rightHandSlot;
    [SerializeField] private float pickupRange = 2f;

    private FlashlightItem equippedFlashlight;
    private List<FlashlightItem> nearbyItems = new List<FlashlightItem>();

    private void Update()
    {
        // Check for pickups
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equippedFlashlight != null)
            {
                DropItem();
            }
            else if (nearbyItems.Count > 0)
            {
                PickupItem(nearbyItems[0]);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        FlashlightItem flashlight = collision.GetComponent<FlashlightItem>();
        if (flashlight != null && !flashlight.IsEquipped())
        {
            nearbyItems.Add(flashlight);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        FlashlightItem flashlight = collision.GetComponent<FlashlightItem>();
        if (flashlight != null)
        {
            nearbyItems.Remove(flashlight);
        }
    }

    public void PickupItem(FlashlightItem flashlight)
    {
        if (equippedFlashlight != null)
        {
            DropItem();
        }

        equippedFlashlight = flashlight;
        equippedFlashlight.Equip(rightHandSlot);
        nearbyItems.Remove(flashlight);
    }

    public void DropItem()
    {
        if (equippedFlashlight != null)
        {
            Vector3 dropPos = transform.position + transform.forward * 1f;
            equippedFlashlight.Unequip(dropPos);
            equippedFlashlight = null;
        }
    }

    public FlashlightItem GetEquippedFlashlight() => equippedFlashlight;
    public List<FlashlightItem> GetNearbyItems() => nearbyItems;
}
