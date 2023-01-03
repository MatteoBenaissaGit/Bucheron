using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterInventory : MonoBehaviour
    {
        [SerializeField] private Slot _woodSlot;
        [SerializeField] private Slot _carrotSlot;

        private void Update()
        {
            PickupItem();
        }

        private void PickupItem()
        {
            RaycastHit2D[] raycastHits = 
                Physics2D.BoxCastAll(transform.position, Vector2.one * 0.5f, 0, transform.forward, Single.PositiveInfinity);
            foreach (RaycastHit2D hit in raycastHits)
            {
                Collectible collectible = hit.collider.gameObject.GetComponent<Collectible>();
                if (collectible != null && collectible.CanBePickedUp && collectible.HasBeenPickedUp == false)
                {
                    GetItem(collectible);
                    collectible.PickedUp();
                }
            }
        }

        private void GetItem(Collectible collectible)
        {
            if (collectible.GetComponent<Wood>())
            {
                _woodSlot.SetStack(1);
            }
            if (collectible.GetComponent<Carrot>())
            {
                _carrotSlot.SetStack(1);
            }
        }
    }
}