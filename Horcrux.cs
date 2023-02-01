using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
    class Horcrux : MonoBehaviour
    {

        Item item;
        float percentageOfSoul;
        string id;
        string localizationId;
        string prefabAddress;


        public void Awake() {

            item = GetComponent<Item>();
            percentageOfSoul = Player.local.creature.gameObject.GetComponent<Soul>().DivideSoul();

        }

        public void Start() {

            JSONParser parser = new JSONParser();
            parser.item = item;
            parser.GenerateJson();

        }
    }
}
