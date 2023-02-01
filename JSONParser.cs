using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;

namespace WandSpellss
{
    class JSONParser : MonoBehaviour
    {
        internal Item item;
        ItemData itemData;
        string stringJson;


        internal void GenerateJson() {

            HorcruxData horcruxData = new HorcruxData();
            itemData = item.data.DeepCopyByExpressionTree();

            itemData.id = "Horcrux:" + item.name;
            itemData.localizationId = "Horcrux:" + item.name;
            itemData.author = "braxton3300(Apoz123)";
            //itemData.modules[0].
            //itemData.modules.Add(horcruxData);

            //File.Create("C:/Program Files (x86)/Steam/steamapps/common/Blade & Sorcery/BladeAndSorcery_Data/StreamingAssets/Mods/HarryPotterSpells/Horcruxes/Item_Horcrux_" + item.name + ".json");
            //stringJson = JsonConvert.SerializeObject(itemData, Formatting.Indented);
            //Debug.Log(stringJson);
            //File.WriteAllText("C:/Program Files (x86)/Steam/steamapps/common/Blade & Sorcery/BladeAndSorcery_Data/StreamingAssets/Mods/HarryPotterSpells/Horcruxes/Item_Horcrux_" + item.name+".json", stringJson);

        
        }

    }
}
