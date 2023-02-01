using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using System.Timers;
using System.Collections;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace WandSpellss
{

    public class MyWeaponComponent : MonoBehaviour
    {

        float bTimerTime = 1500f;
        List<Material> myMaterials;
        Stack<GameObject> stack = new Stack<GameObject>();
        Item item;
        Item currentDistortion;
        Item current;
        Item previous;
        Item currentShooters;
        Item currentGoldenLight;
        Item currentAvadaLightning;
        Material evanescoDissolve;
        GameObject keyWordRecog;
        ItemData avadaLightning;
        public float spellSpeed;
        public bool magicEffect;
        private float explliarmusPower;
        int currIndex;
        Dictionary<string, ItemData> spells = new Dictionary<string, ItemData>();
        private KeyWordRecogWand speech;
        bool waitFlag;
        bool determiner;
        internal List<ItemData> spellsList = new List<ItemData>();
        List<string> spellsListWithText = new List<string>();
        int pressedIndex;
        AudioSource sourceCurrent;
        AudioSource touch;
        GameObject sound;
        int firstTouch;
        internal bool canSpeak;
        bool canFire;
        bool ignoreTouch;
        bool playSound;
        GameObject sourceObj;
        bool canRayCast;
        bool canAccio;
        bool canEngorgio;
        bool canEvanesco;
        float elapsedTime;
        float duration;
        Vector3 startPoint;
        Vector3 endPoint;
        GameObject parentLocal;
        RagdollHand oppositeHand;
        private Timer aTimer;
        ItemData distortionEffect;
        Creature currentHit;
        Vector3 engorgioMaxSize;
        Item dissovledItem;
        List<GameObject> evanescoStore = new List<GameObject>();
        float dissolveVal;
        ItemData textObject;
        private Timer bTimer;
        GameObject currentText;
        private bool objectIsHovering;

        float expelliarmusPower;

        public void Start()
        {
            canAccio = false;
            playSound = false;
            ignoreTouch = false;
            canFire = false;
            firstTouch = 0;
            touch = GetComponent<AudioSource>();
            pressedIndex = 0;
            waitFlag = false;
            avadaLightning = Catalog.GetData<ItemData>("AvadaKedavraLightning");
            distortionEffect = Catalog.GetData<ItemData>("Distortion");
            item = GetComponent<Item>();
            //Add all spell objects to a preloaded list on object creation
            spellsListWithText.Add("Stupefy");
            spellsListWithText.Add("Expelliarmus");
            spellsListWithText.Add("AvadaKedavra");
            spellsListWithText.Add("PetrifucTotallus");
            spellsListWithText.Add("Protego");
            spellsListWithText.Add("Lumos");
            spellsListWithText.Add("Accio");
            spellsListWithText.Add("Engorgio");
            spellsListWithText.Add("Evanesco");
            spellsListWithText.Add("Geminio");
            spellsListWithText.Add("SectumSempra");
            spellsListWithText.Add("Levicorpus");
            spellsListWithText.Add("WindgardiumLeviosa");
            //spellsListWithText.Add("Waddiwassi");
            //spellsListWithText.Add("Imperio");
            //spellsListWithText.Add("WingardiumLeviosa");
            spellsList.Add(Catalog.GetData<ItemData>("StupefyObject"));
            spellsList.Add(Catalog.GetData<ItemData>("ExpelliarmusObject"));
            spellsList.Add(Catalog.GetData<ItemData>("AvadaKedavraObject"));
            spellsList.Add(Catalog.GetData<ItemData>("PetrificusTotalusObject"));
            spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject"));
            spellsList.Add(Catalog.GetData<ItemData>("LumosObject"));
            spellsList.Add(Catalog.GetData<ItemData>("LumosObject"));
            spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject"));
            spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject"));
            spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject"));
            spellsList.Add(Catalog.GetData<ItemData>("SectumsempraObject"));
            spellsList.Add(Catalog.GetData<ItemData>("LevicorpusObject"));
            spellsList.Add(Catalog.GetData<ItemData>("LevicorpusObject"));
            //spellsList.Add(Catalog.GetData<ItemData>("SectumsempraObject"));
            //spellsList.Add(Catalog.GetData<ItemData>("SectumsempraObject"));

            //spellsList.Add(Catalog.GetData<ItemData>("SectumsempraObject"));

            textObject = Catalog.GetData<ItemData>("TextForSpells");
            
            //keyWordRecog = new GameObject();
            //keyWordRecog.transform.parent = null;
            //keyWordRecog.AddComponent<SpeechRecogWands>();
            engorgioMaxSize = new Vector3(2,2,2);
            var op = Addressables.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Evanesco.Mat");
            op.Completed += Op_Completed1; 
                

            dissolveVal = 0;
            currIndex = 0;
            item.OnHeldActionEvent += Item_OnHeldActionEvent;
            item.OnGrabEvent += Item_OnGrabEvent;
            //speech = new KeyWordRecogWand();
            
        }

        //load dissolve effect material
        private void Op_Completed1(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Material>obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                // Setting the material
                evanescoDissolve = obj.Result;
            }
        }

        /**
         *Does stuff when wand is grabbed by the player 
          **/
        private void Item_OnGrabEvent(Handle arg1, RagdollHand arg2)
        {
            
                //plays music the first time a player touches the wand
                if (firstTouch == 0 && magicEffect == true)
                {
                    canFire = false;
                    Vector3 transformLocal = new Vector3(Player.local.head.transform.position.x, Player.local.head.transform.position.y, Player.local.head.transform.position.z);
                    Quaternion tranformRotLocal = new Quaternion(Player.local.head.transform.rotation.x, Player.local.head.transform.rotation.y, Player.local.head.transform.rotation.z, Player.local.head.transform.rotation.w);
                    Catalog.GetData<ItemData>("GoldenLight").SpawnAsync(spawned =>
                    {

                        Debug.Log(spawned);
                        spawned.transform.position = new Vector3(Player.local.head.transform.position.x, Player.local.head.transform.position.y + 2.5f, Player.local.head.transform.position.z);
                        spawned.transform.rotation = Player.local.head.transform.rotation;
                        spawned.rb.useGravity = false;
                        spawned.rb.drag = 0.0f;

                        currentGoldenLight = spawned;

                    });

                    touch.Play();

                firstTouch++;
            }

            

        }
        
        public void Update()
        {
           
            if (currentText != null) {

                currentText.transform.position = item.flyDirRef.position;
                currentText.transform.rotation = Player.local.head.transform.rotation;


            }
            if (item.mainHandler != null)
            {
                oppositeHand = item.mainHandler.otherHand;
                endPoint = oppositeHand.transform.position;
            }
            if (currentDistortion != null)
            {
                currentDistortion.transform.position = item.flyDirRef.position;
                currentDistortion.transform.rotation = item.flyDirRef.rotation;
            }
               

            if (currentGoldenLight != null)
            {
                currentGoldenLight.transform.position = new Vector3(Player.local.head.transform.position.x, Player.local.head.transform.position.y + 2.5f, Player.local.head.transform.position.z);
                currentGoldenLight.transform.rotation = Player.local.head.transform.rotation;
            }


            if (currIndex == 4)
            {

                current.transform.position = item.flyDirRef.transform.position;
                current.transform.rotation = item.flyDirRef.transform.rotation;
            }
            else if (currIndex == 5)
            {
                current.transform.position = item.flyDirRef.position;
                current.transform.rotation = item.flyDirRef.transform.rotation;
            }

            //update object for movement if using accio spell
            if (canAccio == true)
            {

                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / duration;

                float distanceSqr = (endPoint - parentLocal.transform.position).sqrMagnitude;
                parentLocal.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.SmoothStep(0, 1, percentageComplete));

                if (distanceSqr <= 0.03f)
                {

                    canAccio = false;

                    elapsedTime = 0f;
                    if (oppositeHand == Player.local.handLeft.ragdollHand)
                    {
                        Player.currentCreature.GetHand(Side.Left).Grab(parentLocal.GetComponent<Item>().mainHandleRight);
                    }
                    else
                    {

                        Player.currentCreature.GetHand(Side.Right).Grab(parentLocal.GetComponent<Item>().mainHandleRight);

                    }

                }
            }

            if (objectIsHovering == true) {

                Debug.Log("Got here: past objectIsHoveringCheck");
                float targetPos = Vector3.Distance(item.gameObject.GetComponent<WingardiumLeviosaJoint>().hitObjectItem.transform.position, item.flyDirRef.transform.position);
                Vector3 newestPos = item.flyDirRef.transform.forward * targetPos;
                item.gameObject.GetComponent<WingardiumLeviosaJoint>().hitObjectItem.gameObject.GetComponent<ConfigurableJoint>().targetPosition = newestPos;

            }


            if (touch.isPlaying == false)
            {
                canFire = true;
                ignoreTouch = true;
                if (currentGoldenLight != null)
                {
                    currentGoldenLight.Despawn();
                }

            }

        }
         
        
        private void Item_OnHeldActionEvent(RagdollHand ragdollHand, Handle handle, Interactable.Action action)
        {


            
            if (action == Interactable.Action.UseStart) {
                parentLocal = null;
                if (item.gameObject.GetComponent<WingardiumLeviosa>() != null) {

                    item.gameObject.GetComponent<WingardiumLeviosa>().canLift = false;
                
                }
                if (currentText != null)
                {

                    Destroy(currentText);

                }
                if (currIndex == spellsList.Count - 1)
                    {
                        
                        
                        currIndex = 0;
                        pressedIndex = 0;
                        
                        textObject.SpawnAsync(text => {

                            text.gameObject.GetComponent<TextMesh>().text = spellsListWithText[currIndex];
                            currentText = text.gameObject;
                            SetTextTimer();

                        });
                    if (previous != null)
                        {
                            previous.Despawn();
                        }

                    }

                    else
                    {

                    

                        currIndex = currIndex + 1;
                        pressedIndex = 0;
                        textObject.SpawnAsync(text => {

                            text.gameObject.GetComponent<TextMesh>().text = spellsListWithText[currIndex];
                            currentText = text.gameObject;
                            bTimerTime = 1500f;
                            SetTextTimer();

                        });
                    if (previous != null)
                        {
                            previous.Despawn();
                        }
                    
                    }
                
            }
            
            
            if (action == Interactable.Action.AlternateUseStart && canFire == true)
            {
                //item.mainHandler.grabbedHandle.HapticRumble(item.mainHandler.playerHand);
                if (currIndex != 4 && currIndex != 5 && currIndex != 6 && currIndex != 7 && currIndex != 8 && currIndex != 9 && currIndex != 12 && currIndex != 13) {
                    SetTimer();
                    distortionEffect.SpawnAsync(distortion => {
                        currentDistortion = distortion;
                    });
                }
                if (currIndex != 6 && currIndex != 7 && currIndex != 8 && currIndex != 9 && currIndex != 12 && currIndex != 13)
                {
                    
                    spellsList[currIndex].SpawnAsync(projectile =>
                        {
                        // Configure projectile.
                        // Set the position and rotation of the projectile (the stupefy) the same as the flyDirRef position and rotation

                            projectile.transform.position = item.flyDirRef.position;
                            projectile.transform.rotation = item.flyDirRef.rotation;
                        // Same as usual
                            projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                            projectile.IgnoreObjectCollision(item);
                            projectile.Throw();


                            if (currIndex == 0)
                            {
                                projectile.gameObject.AddComponent<Stupefy>();

                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;

                                currentShooters = projectile;

                                //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                projectile.rb.AddForce(item.flyDirRef.transform.forward * spellSpeed, ForceMode.Impulse);
                                projectile.gameObject.AddComponent<SpellDespawn>();

                                foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                {

                                    switch (c.name)
                                    {

                                        case "StupefySound":
                                            sourceCurrent = c;
                                            break;

                                    }

                                }

                                switch(Player.currentCreature.data.gender){


                                    case CreatureData.Gender.Male:


                                        break;
                                
                                }

                                
                                sourceCurrent.Play();
                                playSound = true;
                            }

                            else if (currIndex == 1)
                            {
                                projectile.gameObject.AddComponent<Expelliarmus>();
                                projectile.gameObject.GetComponent<Expelliarmus>().power = expelliarmusPower;

                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;

                                currentShooters = projectile;

                                //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                projectile.rb.AddForce(item.flyDirRef.transform.forward * spellSpeed, ForceMode.Impulse);

                                projectile.gameObject.AddComponent<SpellDespawn>();
                                foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                {

                                    switch (c.name)
                                    {

                                        case "ExpelliarmusSound":
                                            sourceCurrent = c;
                                            break;

                                    }

                                }
                                sourceCurrent.Play();

                            }
                            else if (currIndex == 2)
                            {


                                projectile.gameObject.AddComponent<AvadaKedavra>();
                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;
                                currentShooters = projectile;
                                //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                projectile.rb.AddForce(item.flyDirRef.transform.forward * spellSpeed, ForceMode.Impulse);
                                projectile.gameObject.AddComponent<SpellDespawn>();
                                foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                {

                                    switch (c.name)
                                    {

                                        case "AvadaSound":
                                            sourceCurrent = c;
                                            break;

                                    }

                                }
                                sourceCurrent.Play();
                            }
                            else if (currIndex == 3)
                            {
                                projectile.gameObject.AddComponent<PetrificusTotalus>();

                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;
                                currentShooters = projectile;
                                //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                projectile.rb.AddForce(item.flyDirRef.transform.forward * spellSpeed, ForceMode.Impulse);
                                projectile.gameObject.AddComponent<SpellDespawn>();
                                foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                {

                                    switch (c.name)
                                    {

                                        case "PetrificusTotallusSound":
                                            sourceCurrent = c;
                                            break;

                                    }

                                }
                                sourceCurrent.Play();
                            }

                            else if (currIndex == 4)
                            {
                                pressedIndex = pressedIndex + 1;
                                projectile.gameObject.AddComponent<Protego>();

                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;
                                current = projectile;

                                if (pressedIndex == 2)
                                {
                                    previous.Despawn();
                                    previous = null;
                                    pressedIndex = 0;
                                    previous = current;
                                }
                                else
                                {
                                    if (previous != null)
                                    {
                                        previous.Despawn();
                                        previous = null;
                                    }
                                    previous = current;
                                }
                                sourceCurrent = projectile.gameObject.GetComponent<Protego>().source;
                                sourceCurrent.Play();

                            }

                            else if (currIndex == 5)
                            {
                                pressedIndex = pressedIndex + 1;
                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;

                                current = projectile;
                                if (pressedIndex == 2)
                                {

                                    previous.Despawn();
                                    previous = null;
                                    pressedIndex = 0;
                                    previous = current;
                                }
                                else
                                {
                                    if (previous != null)
                                    {
                                        previous.Despawn();
                                        previous = null;
                                    }
                                    previous = current;
                                }
                                sourceCurrent = projectile.gameObject.GetComponent<AudioSource>();
                                sourceCurrent.Play();

                            }
                            else if (currIndex == 10)
                            {

                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;
                                currentShooters = projectile;
                                //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                projectile.rb.AddForce(item.flyDirRef.transform.forward * 100f, ForceMode.Impulse);
                                projectile.gameObject.AddComponent<sempraDespawn>();


                            }

                            else if (currIndex == 11)
                            {
                                projectile.gameObject.AddComponent<Levicorpus>();
                                projectile.rb.useGravity = false;
                                projectile.rb.drag = 0.0f;
                                currentShooters = projectile;
                                //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                projectile.rb.AddForce(item.flyDirRef.transform.forward * spellSpeed, ForceMode.Impulse);
                                projectile.gameObject.AddComponent<SpellDespawn>();

                                foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                {

                                    switch (c.name)
                                    {

                                        case "LevicorpusSound":
                                            sourceCurrent = c;
                                            break;

                                    }

                                }
                                sourceCurrent.Play();
                            }

                        });
                        
                }

                if (currIndex == 6)
                {

                    item.gameObject.AddComponent<Accio>();
                    item.gameObject.GetComponent<Accio>().wand = this.item;
                    item.gameObject.GetComponent<Accio>().CastRay();
                    

                }


                if (currIndex == 7)
                {

                    item.gameObject.AddComponent<Engorgio>();
                    item.gameObject.GetComponent<Engorgio>().CastRay();
                    


                }
                if (currIndex == 8)
                {

                    item.gameObject.AddComponent<Evanesco>();
                    item.gameObject.GetComponent<Evanesco>().CastRay();
                    parentLocal = item.gameObject.GetComponent<Evanesco>().parentLocal;
                    item.gameObject.GetComponent<Evanesco>().evanescoDissolve = evanescoDissolve;
                   


                }


                if (currIndex == 9)
                {

                    item.gameObject.AddComponent<Geminio>();
                    item.gameObject.GetComponent<Geminio>().CastRay();

                }

                
                if (currIndex == 12)
                {

                    item.gameObject.AddComponent<WingardiumLeviosaJoint>();
                    item.gameObject.GetComponent<WingardiumLeviosaJoint>().wand = item;
                    objectIsHovering = item.gameObject.GetComponent<WingardiumLeviosaJoint>().objectIsHovering;
                    if (objectIsHovering == false)
                    {
                        item.gameObject.GetComponent<WingardiumLeviosaJoint>().CastRay();

                    }
                    
                    else {

                        item.gameObject.GetComponent<WingardiumLeviosaJoint>().hitObjectItem.gameObject.GetComponent<HingeJoint>().connectedBody = null;
                        item.gameObject.GetComponent<WingardiumLeviosaJoint>().objectIsHovering = false;
                    }
                    
                }
                
                
                /*
                if (currIndex == 11) {

                    item.gameObject.AddComponent<Waddiwassi>();
                    item.gameObject.GetComponent<Waddiwassi>().CastRay();
                
                }

                if (currIndex == 12) {

                    item.gameObject.AddComponent<Imperio>();
                    item.gameObject.GetComponent<Imperio>().CastRay();

                
                }

                if (currIndex == 13) {

                    item.gameObject.AddComponent<WingardiumLeviosa>();
                    item.gameObject.GetComponent<WingardiumLeviosa>().CastRay();
                
                }
                */



            }
        
            

        }

        private void SetTimer()
        {

            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(200);
            // Hook up the Elapsed event for the timer. 

            aTimer.Elapsed += OnTimedEvent;

            aTimer.AutoReset = false;
            aTimer.Enabled = true;

        }

        private void SetTextTimer()
        {

            // Create a timer with a two second interval.
            bTimer = new System.Timers.Timer(bTimerTime);
            // Hook up the Elapsed event for the timer. 

            bTimer.Elapsed += OnTextTimedEvent;

            bTimer.AutoReset = false;
            bTimer.Enabled = true;

        }

        private void OnTextTimedEvent(object sender, ElapsedEventArgs e)
        {
            Destroy(currentText);
            
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (currentDistortion != null) {
                currentDistortion.Despawn();
            }


        }



        public void Setup(float importSpeed, bool importMagicEffect, float importExpelliarmusPower)
        {
            spellSpeed = importSpeed;
            magicEffect = importMagicEffect;
            expelliarmusPower = importExpelliarmusPower;
        }


    }
}
