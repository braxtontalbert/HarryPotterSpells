using System;
using System.Security.Cryptography;
using ThunderRoad;
using UnityEngine;
namespace WandSpellss
{
    public class AccioNimbus : MonoBehaviour
    {
        private Item item;
        private RagdollHand otherHand;
        private Rigidbody rigid;

        public void Setup(RagdollHand otherHand)
        {
            this.otherHand = otherHand;
        }
        private void Start()
        {
            item = GetComponent<Item>();
            rigid = item.GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (item && otherHand)
            {
                float distance = (item.transform.position - otherHand.transform.position).magnitude;
                item.physicBody.rigidBody.velocity = (otherHand.transform.position - item.transform.position).normalized * 15f;

                if (distance < 0.1f) {

                    if (!otherHand.isGrabbed)
                    {
                        otherHand.Grab(item.mainHandleRight);
                        Destroy(this);
                    }
                }
            }
        }
    }
}