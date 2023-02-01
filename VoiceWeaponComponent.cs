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
    class VoiceWeaponComponent : MonoBehaviour
    {

        Stack<GameObject> stack = new Stack<GameObject>();
        Item item;
        Item current;
        Material evanescoDissolve;

        Item currentShooters;

        //start up values
        public float spellSpeed;
        public bool magicEffect;
        private float expelliarmusPower;

        Dictionary<string, ItemData> spells = new Dictionary<string, ItemData>();
        internal List<ItemData> spellsList = new List<ItemData>();
        List<string> spellsListWithText = new List<string>();
        AudioSource sourceCurrent;
        private Timer aTimer;
        List<GameObject> evanescoStore = new List<GameObject>();
        AnimationClip animation1;
        List<AnimationClip> animationClips = new List<AnimationClip>();
        ItemData DarkMark;
        internal List<Creature> hitByLevicorpus = new List<Creature>();

        AnimationData animData;

        //Spell Component References
        Evanesco evanescoComp;
        Ascendio ascendioComp;
        Engorgio engorgioComp;
        Accio accioComp;
        WingardiumLeviosa wingardiumComp;

        ArrestoMomentum arrestoComp;

        GameObject recognizer;
        string recognizedWord;
        KeyWordRecogWand recogWand;
        DictationWand dictWand;
        private bool usedAscendio;

        CreatureData patronus;

        List<Horcrux> horcruxes = new List<Horcrux>();

        Soul playerSoul;

        bool canMakeHorcrux;
        private bool usedArrestoMomento;
        internal Item currentText;

        Dictionary<string, Spell> spellDict = new Dictionary<string, Spell>();

        void Start() {

            spellDict.Add("Stupefy", new Stupefy());
            //Horcrux info setup
            playerSoul = Player.local.creature.gameObject.GetComponent<Soul>();
            canMakeHorcrux = false;

            //item initialization
            item = GetComponent<Item>();

            //voice recognizer setup
            item.gameObject.AddComponent<KeyWordRecogWand>();
            //item.gameObject.AddComponent<DictationWand>();
            recognizer = item.gameObject;
            recogWand = recognizer.GetComponent<KeyWordRecogWand>();
            //dictWand = recognizer.GetComponent<DictationWand>();
            

            //Spell data setup
            spellsList.Add(Catalog.GetData<ItemData>("StupefyObject"));
            spellsList.Add(Catalog.GetData<ItemData>("ExpelliarmusObject"));
            spellsList.Add(Catalog.GetData<ItemData>("AvadaKedavraObject"));
            spellsList.Add(Catalog.GetData<ItemData>("PetrificusTotalusObject"));
            spellsList.Add(Catalog.GetData<ItemData>("LevicorpusObject"));
            spellsList.Add(Catalog.GetData<ItemData>("LumosObject"));
            spellsList.Add(Catalog.GetData<ItemData>("ProtegoObject"));
            spellsList.Add(Catalog.GetData<ItemData>("SectumsempraObject"));
            spellsList.Add(Catalog.GetData<ItemData>("MorsmordreObject"));
            spellsList.Add(Catalog.GetData<ItemData>("TarantallegraObject"));
            spellsList.Add(Catalog.GetData<ItemData>("MagicText"));
            DarkMark = Catalog.GetData<ItemData>("TheDarkMark");

            patronus = Catalog.GetData<CreatureData>("stagpatronus");


            //All non projectile spell components
            evanescoComp = item.gameObject.AddComponent<Evanesco>(); //evanesco
            ascendioComp = item.gameObject.AddComponent<Ascendio>(); //ascendio
            engorgioComp = item.gameObject.AddComponent<Engorgio>(); //engorgio
                                                                     
            accioComp = item.gameObject.AddComponent<Accio>();//accio
            wingardiumComp = item.gameObject.AddComponent<WingardiumLeviosa>(); // wingardium leviosa
            arrestoComp = item.gameObject.AddComponent<ArrestoMomentum>();


            accioComp.wand = item;



            //animation data
            animData = Catalog.GetData<AnimationData>("HPSDances");



            //Evanesco Material Setup
            var op = Addressables.LoadAssetAsync<Material>("apoz123Wand.SpellEffect.Evanesco.Mat");
            //var op2 = Addressables.LoadAssetAsync<AnimationClip>("apoz123Wand.Dances.Animation1");
            //op2.Completed += Op2_Completed;
            op.Completed += Op_Completed1;

            //On land event for ascendio 
            Player.local.locomotion.OnGroundEvent += Locomotion_OnGroundEvent;


        }

        private void Op2_Completed(AsyncOperationHandle<AnimationClip> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded) {


                animation1 = obj.Result;
                animationClips.Add(animation1);
            }
        }

        private void Op_Completed1(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Material> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                // Setting the material
                evanescoDissolve = obj.Result;
            }
        }
        

        void Update() {

            if (recogWand != null)
            {
                if (item.mainHandler != null)
                {
                    recogWand.isEnabled = true;
                    if (recogWand.hasRecognizedWord == true)
                    {

                        recogWand.hasRecognizedWord = false;

                        if (recogWand.knownCurrent != null)
                        {
                            //Debug.Log(recogWand.knownCurrent);

                            switch (recogWand.knownCurrent) {


                                case string stupefy when recogWand.knownCurrent.Contains("Stewpify"):
                                    spellsList[0].SpawnAsync(projectile =>
                                    {
                                        // Configure projectile.
                                        // Set the position and rotation of the projectile (the stupefy) the same as the flyDirRef position and rotation

                                        projectile.transform.position = item.flyDirRef.position;
                                        projectile.transform.rotation = item.flyDirRef.rotation;
                                        // Same as usual
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();


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

                                        sourceCurrent.Play();

                                    });
                                    break;

                                case string expelliarmus when recogWand.knownCurrent.Contains("Expelliarmus"):
                                    spellsList[1].SpawnAsync(projectile =>
                                    {
                                        projectile.transform.position = item.flyDirRef.position;
                                        projectile.transform.rotation = item.flyDirRef.rotation;
                                        // Same as usual
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();


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


                                    });
                                    break;

                                case string avadakedavra when recogWand.knownCurrent.Contains("Ahvahduhkuhdahvra"):

                                    spellsList[2].SpawnAsync(projectile =>
                                    {

                                        projectile.transform.position = item.flyDirRef.position;
                                        projectile.transform.rotation = item.flyDirRef.rotation;
                                        // Same as usual
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();

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


                                    });

                                    canMakeHorcrux = true;

                                    SetTimer();
                                    break;

                                case string petrificustotalus when recogWand.knownCurrent.Contains("PetrificusTotalus"):

                                    spellsList[3].SpawnAsync(projectile =>
                                    {
                                        projectile.transform.position = item.flyDirRef.position;
                                        projectile.transform.rotation = item.flyDirRef.rotation;
                                        // Same as usual
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();

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

                                    });
                                    break;

                                case string levicorpus when recogWand.knownCurrent.Contains("Levicorpus"):

                                    spellsList[4].SpawnAsync(projectile =>
                                    {

                                        projectile.transform.position = item.flyDirRef.position;
                                        projectile.transform.rotation = item.flyDirRef.rotation;
                                        // Same as usual
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();

                                        projectile.gameObject.AddComponent<Levicorpus>();
                                        projectile.gameObject.GetComponent<Levicorpus>().spawnerWeapon = item;
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


                                    });

                                    break;

                                case string tarantallegra when recogWand.knownCurrent.Contains("Tarantallegra"):
                                    spellsList[9].SpawnAsync(projectile =>
                                    {

                                        projectile.transform.position = item.flyDirRef.position;
                                        projectile.transform.rotation = item.flyDirRef.rotation;
                                        // Same as usual
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();

                                        projectile.gameObject.AddComponent<Tarantallegra>();
                                        projectile.gameObject.GetComponent<Tarantallegra>().animationData = animData;
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

                                    });
                                   
                                    break;

                                case string liberacorpus when recogWand.knownCurrent.Contains("Liberacorpus"):
                                    if (hitByLevicorpus.Count > 0)
                                    {
                                        foreach (Creature creature in hitByLevicorpus)
                                        {

                                            Debug.Log(creature);
                                            if (creature.footLeft.gameObject.GetComponent<SpringJoint>() != null && creature.footRight.gameObject.GetComponent<SpringJoint>() != null)
                                            {
                                                Debug.Log("Got past double foot check.");
                                                Destroy(creature.footLeft.gameObject.GetComponent<SpringJoint>());

                                                Destroy(creature.footRight.gameObject.GetComponent<SpringJoint>());


                                            }


                                        }

                                        hitByLevicorpus.Clear();
                                        hitByLevicorpus.TrimExcess();

                                    }

                                    break;

                                case string lumos when recogWand.knownCurrent.Contains("Lumos"):
                                    if (current != null)
                                    {

                                        if (current.name.Contains("Lumos") == false)
                                        {
                                            spellsList[5].SpawnAsync(projectile =>
                                            {

                                                current = projectile;
                                                sourceCurrent = projectile.gameObject.GetComponent<AudioSource>();
                                                sourceCurrent.Play();
                                            });
                                        }

                                    }

                                    else
                                    {

                                        spellsList[5].SpawnAsync(projectile =>
                                        {

                                            current = projectile;
                                            sourceCurrent = projectile.gameObject.GetComponent<AudioSource>();
                                            sourceCurrent.Play();
                                        });

                                    }

                                    break;

                                case string nox when recogWand.knownCurrent.Contains("Nox"):
                                    if (current != null)
                                    {

                                        if (current.name.Contains("Lumos"))
                                        {
                                            foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                            {

                                                switch (c.name)
                                                {

                                                    case "NoxSound":
                                                        sourceCurrent = c;
                                                        break;

                                                }

                                            }
                                            sourceCurrent.Play();

                                            current.Despawn();
                                            current = null;

                                        }

                                    }


                                    break;

                                case string protego when recogWand.knownCurrent.Contains("Protego"):
                                    spellsList[6].SpawnAsync(projectile =>
                                    {


                                        projectile.rb.useGravity = false;
                                        projectile.rb.drag = 0.0f;
                                        current = projectile;


                                        foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                        {

                                            switch (c.name)
                                            {

                                                case "ProtegoSound":
                                                    sourceCurrent = c;
                                                    break;

                                            }

                                        }

                                        sourceCurrent.Play();
                                    });

                                    break;

                                case string evanesco when recogWand.knownCurrent.Contains("Evanesco"):
                                    evanescoComp.evanescoDissolve = evanescoDissolve;
                                    evanescoComp.CastRay();

                                    break;

                                case string ascendio when recogWand.knownCurrent.Contains("Ascendio"):

                                    ascendioComp.Ascend();

                                    usedAscendio = true;

                                    break;

                                case string arrestomomentum when recogWand.knownCurrent.Contains("Arresto Momentum"):
                                    arrestoComp.StartArrestoMomentum();
                                    usedArrestoMomento = true;

                                    break;

                                case string vinceremortem when recogWand.knownCurrent.Contains("Vincere mortem"):
                                    if (item.mainHandler.otherHand.grabbedHandle.item.gameObject.GetComponent<Horcrux>() == null && canMakeHorcrux == true)
                                    {
                                        if (item.mainHandler.otherHand != null)
                                        {

                                            item.mainHandler.otherHand.grabbedHandle.item.gameObject.AddComponent<Horcrux>();
                                            horcruxes.Add(item.mainHandler.otherHand.grabbedHandle.item.gameObject.GetComponent<Horcrux>());
                                            canMakeHorcrux = false;


                                        }
                                    }

                                    break;

                                case string engorgio when recogWand.knownCurrent.Contains("Engorgio"):
                                    engorgioComp.command = "Engorgio";
                                    engorgioComp.CastRay();

                                    break;

                                case string reducio when recogWand.knownCurrent.Contains("Reducio"):
                                    engorgioComp.command = "Reducio";
                                    engorgioComp.CastRay();

                                    break;

                                case string accio when recogWand.knownCurrent.Contains("Ackio"):
                                   
                                    accioComp.CastRay();

                                    break;

                                case string wingardiumleviosa when recogWand.knownCurrent.Contains("Wingardium leviosa"):


                                    wingardiumComp.CastRay();

                                    break;

                                case string morsmordre when recogWand.knownCurrent.Contains("Morsmordre"):
                                    spellsList[8].SpawnAsync(projectile =>
                                    {

                                        projectile.transform.position = item.flyDirRef.position;
                                        projectile.transform.rotation = item.flyDirRef.rotation;
                                        // Same as usual
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();

                                        projectile.gameObject.AddComponent<Morsmordre>();
                                        projectile.gameObject.GetComponent<Morsmordre>().darkMark = DarkMark;
                                        projectile.rb.useGravity = false;
                                        projectile.rb.drag = 0.0f;

                                        currentShooters = projectile;

                                        //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                        projectile.rb.AddForce(item.flyDirRef.transform.forward * spellSpeed, ForceMode.Impulse);

                                        projectile.gameObject.AddComponent<SpellDespawn>();

                                    });


                                    break;

                                case string sectumsmepra when recogWand.knownCurrent.Contains("Sectumsempra"):
                                    spellsList[7].SpawnAsync(projectile =>
                                    {

                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);
                                        projectile.Throw();

                                        projectile.rb.useGravity = false;

                                        Debug.Log(projectile);
                                        projectile.rb.drag = 0.0f;
                                        currentShooters = projectile;
                                        //Add the force in the direction of the flyDirRef (the blue axis in unity)
                                        projectile.rb.AddForce(item.flyDirRef.transform.forward * 100f, ForceMode.Impulse);
                                        projectile.gameObject.AddComponent<SpellDespawn>();



                                        foreach (AudioSource c in item.GetComponentsInChildren<AudioSource>())
                                        {

                                            switch (c.name)
                                            {

                                                case "SectumsempraSound":
                                                    sourceCurrent = c;
                                                    break;

                                            }

                                        }

                                        sourceCurrent.Play();
                                    });

                                    break;

                                case string pronunciation when recogWand.knownCurrent.Contains("Pronunciation"):

                                    spellsList[10].SpawnAsync(projectile =>
                                    {

                                        
                                        projectile.IgnoreRagdollCollision(Player.local.creature.ragdoll);
                                        projectile.IgnoreObjectCollision(item);

                                        projectile.rb.useGravity = false;
                                        projectile.rb.drag = 0.0f;
                                        currentShooters = projectile;

                                        projectile.transform.position = item.flyDirRef.position + item.flyDirRef.forward * 4f;
                                        projectile.transform.rotation = item.flyDirRef.rotation;

                                        projectile.gameObject.AddComponent<MagicText>();

                                        currentText = projectile;

                                    });

                                    break;

                                case string pronounced when recogWand.knownCurrent.Contains("Deletrius"):

                                    currentText.Despawn();

                                    break;

                                case string ExpectoPatronum when recogWand.knownCurrent.Contains("Expecto Patronum"):

                                    patronus.SpawnAsync(item.flyDirRef.position, item.flyDirRef.rotation.y);


                                    break;


                                default:
                                    break;
                            
                            }
                            

                        }
                    }
                    

                    if (current != null)
                    {

                        if (current.name.Contains("Lumos"))
                        {

                            current.transform.position = item.flyDirRef.transform.position;
                            current.transform.rotation = item.flyDirRef.transform.rotation;

                        }

                        else if (current.name.Contains("Quad"))
                        {

                            current.transform.position = item.flyDirRef.transform.position;
                            current.transform.rotation = item.flyDirRef.transform.rotation;


                            if (sourceCurrent != null)
                            {
                                if (!sourceCurrent.isPlaying)
                                {
                                    current.Despawn();
                                    current = null;
                                }
                            }


                        }
                    }
                }

                else {

                    recogWand.hasRecognizedWord = false;
                    recogWand.isEnabled = false;
                }
            }
        }

        private void Locomotion_OnGroundEvent(Vector3 groundPoint, Vector3 velocity, Collider groundCollider)
        {
            if (usedAscendio)
            {
                usedAscendio = false;
                Player.fallDamage = true;
            }
            else if (usedArrestoMomento) {


                usedArrestoMomento = false;
                
            
            }
        }

        public void Setup(float importSpeed, bool importMagicEffect, float importExpelliarmusPower)
        {
            spellSpeed = importSpeed;
            magicEffect = importMagicEffect;
            expelliarmusPower = importExpelliarmusPower;
        }

        private void SetTimer()
        {

            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(30000);
            // Hook up the Elapsed event for the timer. 

            aTimer.Elapsed += OnTimedEvent;

            aTimer.AutoReset = false;
            aTimer.Enabled = true;

        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            canMakeHorcrux = false;
        }
    }
}
