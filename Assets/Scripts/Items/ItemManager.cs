using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Transform rightHandSlot;
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private SanitySystem sanitySystem;

    private FlashlightItem equippedFlashlight;
    private AlmondWaterItem equippedAlmondWater;
    private List<FlashlightItem> nearbyFlashlights = new List<FlashlightItem>();
    private List<AlmondWaterItem> nearbyAlmondWaters = new List<AlmondWaterItem>();

    private void Start()
    {
        if (sanitySystem == null)
            sanitySystem = GetComponent<SanitySystem>();
    }

    private void Update()
    {
        // Pickup/Drop with E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equippedFlashlight != null)
            {
                DropFlashlight();
            }
            else if (nearbyFlashlights.Count > 0)
            {
                PickupFlashlight(nearbyFlashlights[0]);
            }
        }

        // Consume Almond Water with F
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (equippedAlmondWater != null)
            {
                ConsumeAlmondWater();
            }
            else if (nearbyAlmondWaters.Count > 0)
            {
                PickupAlmondWater(nearbyAlmondWaters[0]);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        FlashlightItem flashlight = collision.GetComponent<FlashlightItem>();
        if (flashlight != null && !flashlight.IsEquipped())
        {
            nearbyFlashlights.Add(flashlight);
        }

        AlmondWaterItem almondWater = collision.GetComponent<AlmondWaterItem>();
        if (almondWater != null)
        {
            nearbyAlmondWaters.Add(almondWater);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        FlashlightItem flashlight = collision.GetComponent<FlashlightItem>();
        if (flashlight != null)
        {
            nearbyFlashlights.Remove(flashlight);
        }

        AlmondWaterItem almondWater = collision.GetComponent<AlmondWaterItem>();
        if (almondWater != null)
        {
            nearbyAlmondWaters.Remove(almondWater);
        }
    }

    public void PickupFlashlight(FlashlightItem flashlight)
    {
        if (equippedFlashlight != null)
        {
            DropFlashlight();
        }

        equippedFlashlight = flashlight;
        equippedFlashlight.Equip(rightHandSlot);
        nearbyFlashlights.Remove(flashlight);
    }

    public void DropFlashlight()
    {
        if (equippedFlashlight != null)
        {
            Vector3 dropPos = transform.position + transform.forward * 1f;
            equippedFlashlight.Unequip(dropPos);
            equippedFlashlight = null;
        }
    }

    public void PickupAlmondWater(AlmondWaterItem almondWater)
    {
        equippedAlmondWater = almondWater;
        almondWater.transform.SetParent(rightHandSlot);
        almondWater.transform.localPosition = new Vector3(0, -0.15f, 0.1f);
        nearbyAlmondWaters.Remove(almondWater);
    }

    public void ConsumeAlmondWater()
    {
        if (equippedAlmondWater != null)
        {
            equippedAlmondWater.Consume(sanitySystem);
            equippedAlmondWater = null;
        }
    }

    public FlashlightItem GetEquippedFlashlight() => equippedFlashlight;
    public AlmondWaterItem GetEquippedAlmondWater() => equippedAlmondWater;
}
