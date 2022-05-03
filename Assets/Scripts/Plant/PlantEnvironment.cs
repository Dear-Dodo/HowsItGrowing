/*
 *  Author: Calvin Soueid
 *  Date:   27/10/2021
 */

using Plant.Data;
using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityAsync;
using UnityEngine;

namespace Plant
{
    public class PlantEnvironment : MonoBehaviour
    {
        public static PlantEnvironment Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("An instance of PlantEnvironment already exists");
            }

            Instance = this;
        }

        /// <summary>
        /// Camera array indexed by Plant.Data.CameraPosition enum values
        /// </summary>
        [Tooltip("Scene cameras ordered from closest to farthest")]
        public Camera[] Cameras;

        public GameObject PlantLocation;
        public GameObject CheckConditionsButton;

        public Animator LeafTransition;

        public float LeafTransitionDelay;

        public Conversation FailConversation;

        /// <summary>
        /// Stores association between requirement enums as Type and their Monobehaviour interactables
        /// </summary>
        public static Dictionary<Type, Func<MonoBehaviour>> InteractablesByType = new Dictionary<Type, Func<MonoBehaviour>>()
        {
            { typeof(SoilDepth), () => SoilPile.Instance },
            { typeof(SoilType), () => SoilBags.Instance },
            { typeof(WaterLevel), () => WaterTank.Instance },
            { typeof(SunExposure), () => ShadeCloth.Instance },
            { typeof(Fertiliser), () => FertiliserBags.Instance },
            { typeof(Pollinator), () => Pollinators.Instance },
            { typeof(Dispersal), () => DispersalMechanisms.Instance },
        };

        [Header("Debug")]
        public SoilDepth CurrentSoilDepth;

        public SoilType CurrentSoilType;
        public WaterLevel CurrentWaterLevel;
        public SunExposure CurrentSunExposure;
        public Fertiliser CurrentFertilizer;
        public Pollinator CurrentPollinator;
        public Dispersal CurrentDispersal;

        public PlantInfo CurrentPlant;
        private GameObject PlantInstance;
        public PlantStage CurrentStage;

        public void Start()
        {
            if (CurrentPlant != null)
            {
                StartPlant(CurrentPlant);
            }
        }

        public void StartPlant(PlantInfo target)
        {
            for (int i = target.PlantStages.Length - 1; i >= 0; i--)
            {
                PlantStage next = null;
                if (i + 1 < target.PlantStages.Length)
                {
                    next = target.PlantStages[i + 1].Stage;
                }
                target.PlantStages[i].Stage = new PlantStage(next, target.PlantStages[i].Requirements, target.PlantStages[i]);
            }
            CurrentPlant = target;
            CurrentStage = target.PlantStages[0].Stage;
            CurrentStage.Start();
            SetCurrentCamera((int)target.PlantStages[0].CameraPos);
            SetCurrentPlantInstance();
        }

        public void GetHint(bool isSuggestion = false)
        {
            DialogueElement[] hint = CurrentStage.Info.HelpSteps[CurrentStage.Info.FailCount].HintDialogue;
            Conversation hintConversation = (Conversation)ScriptableObject.CreateInstance(typeof(Conversation));
            hintConversation.Dialogue = hint;
            if (isSuggestion)
            {
                DialogueSystem.Instance.SuggestStartDialogue(hintConversation);
            }
            else
            {
                DialogueSystem.Instance.Play(hintConversation);
            }

            DialogueSystem.Instance.onDialogueClose.AddListener(() => { UserInterfaceStack.Instance.Show("GameUI"); });
        }

        public async void CheckConditions()
        {
            CurrentStage.TestTransitions(out TestResult result);
            if (result.Target != null)
            {
                CurrentStage.End();
                CurrentStage = (PlantStage)result.Target;

                LeafTransition.SetTrigger("PlayTransition");
                UserInterfaceStack.Instance.HideAllMenus();
                await Await.Seconds(LeafTransitionDelay);
                UserInterfaceStack.Instance.Show("GameUI");

                CurrentStage.Start();
                SetCurrentCamera((int)((PlantStage)result.Target).Info.CameraPos);
                SetCurrentPlantInstance();
            }
            else if (result.FailedConditions.Count == 0)
            {
                loadMinigame();
            }
            else
            {
                RegisterFailure();
            }
        }

        public void RegisterFailure()
        {
            CurrentStage.Info.FailCount++;

            List<Type> toLock = new List<Type>();

            for (int i = 0; i < CurrentStage.Info.FailCount; i++)
            {
                if (i >= CurrentStage.Info.HelpSteps.Length)
                {
                    throw new Exception($"FailureCount exceeded amount of HelpSteps!");
                }

                toLock = CurrentStage.Info.HelpSteps[i].LockedVariables.Select(t => t.Value).ToList();
                LockVariables(toLock);
            }

            StartFailDialogue();
        }

        /// <summary>
        /// Lock variables matching the given types in toLock.
        /// </summary>
        /// <param name="toLock"></param>
        private void LockVariables(List<Type> toLock)
        {
            // Use reflection to find the field matching type 't'. Set the value to that of the plant stage requiremnent.
            // for example, t = typeof(WaterLevel) will result in this.CurrentWaterLevel being replaced by Info.Requirements.WaterLevel.
            foreach (Type t in toLock)
            {
                FieldInfo toGet = Utility.Reflection.FindFieldByType(CurrentStage.Info.Requirements, t);
                FieldInfo toSet = Utility.Reflection.FindFieldByType(this, t);

                object requirement = toGet.GetValue(CurrentStage.Info.Requirements);

                if ((int)requirement < 0)
                {
                    continue;
                }

                toSet.SetValue(this, requirement);

                MonoBehaviour interactable = InteractablesByType[t]();
                ((Interfaces.IDisableable)interactable).SetActive(false);
                interactable.gameObject.SetActive(true);
                foreach (Transform transform in interactable.GetComponentsInChildren<Transform>(true))
                {
                    transform.gameObject.layer = 8;
                }
                ((Interfaces.IDirtyable)interactable).SetDirty();
            }
        }

        /// <summary>
        /// Begin the appropriate fail dialogue depending on active overrides.
        /// </summary>
        private void StartFailDialogue()
        {
            HelpStep last = CurrentStage.Info.HelpSteps[CurrentStage.Info.FailCount - 1];

            Conversation overiddenFailConversation = (Conversation)ScriptableObject.CreateInstance(typeof(Conversation));

            if (last.IsFailDialogueOverridden)
            {
                overiddenFailConversation.Dialogue = new DialogueElement[] { last.FailDialogue };
                DialogueSystem.Instance.Play(overiddenFailConversation);
            }
            else if (CurrentStage.Info.IsFailDialogueOverridden)
            {
                overiddenFailConversation.Dialogue = new DialogueElement[] { CurrentStage.Info.FailDialogue };
                DialogueSystem.Instance.Play(overiddenFailConversation);
            }
            else
            {
                DialogueSystem.Instance.Play(FailConversation);
            }

            DialogueSystem.Instance.onDialogueClose.AddListener(() => { UserInterfaceStack.Instance.Show("GameUI"); });
        }

        /// <summary>
        /// Set the camera at the provided index as the only active renderer.
        /// </summary>
        /// <param name="index"></param>
        public void SetCurrentCamera(int index)
        {
            for (int i = 0; i < Cameras.Length; i++)
            {
                if (i == index)
                {
                    Cameras[i].gameObject.SetActive(true);
                }
                else
                {
                    Cameras[i].gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Sets the current instance of the plant from the stage's prefab
        /// </summary>
        public void SetCurrentPlantInstance()
        {
            Destroy(PlantInstance);
            PlantInstance = Instantiate(CurrentStage.Info.PlantPrefab, PlantLocation.transform.position, PlantLocation.transform.rotation);
        }

        /// <summary>
        /// Modify 'target' enum by 'delta' value and return result.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="delta"></param>
        /// <param name="hasChanged">Whether or not the result is different to the initial value.</param>
        /// <returns></returns>
        private Enum ModifyEnum(Enum target, int delta, out bool hasChanged)
        {
            if (Enum.GetUnderlyingType(target.GetType()) != typeof(int))
            {
                throw new ArgumentException("Behaviour is undocumented when passing non-int32 backed enums.");
            }

            const int min = 0;
            int max = Enum.GetValues(target.GetType()).Length - 1;

            int oldValue = Convert.ToInt32(target);
            int result = Mathf.Clamp(oldValue + delta, min, max);

            hasChanged = result != oldValue ? true : false;

            return (Enum)Enum.ToObject(target.GetType(), result);
        }

        /// <summary>
        /// Attempt to chang the soil depth by 'delta' increments. Returns whether the soil depth was changed.
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool ModifySoilDepth(int delta)
        {
            CurrentSoilDepth = (SoilDepth)ModifyEnum(CurrentSoilDepth, delta, out bool hasChanged);
            return hasChanged;
        }

        /// <summary>
        /// Attempt to set the soil type. Returns whether the soil type was changed.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SetSoilType(SoilType type)
        {
            if (type != CurrentSoilType)
            {
                CurrentSoilType = type;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to chang the water level by 'delta' increments. Returns whether the water level was changed.
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool ModifyWaterLevel(int delta)
        {
            CurrentWaterLevel = (WaterLevel)ModifyEnum(CurrentWaterLevel, delta, out bool hasChanged);
            return hasChanged;
        }

        /// <summary>
        /// Attempt to chang the sun exposure by 'delta' increments. Returns whether the sun exposure was changed.
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool ModifySunExposure(int delta)
        {
            CurrentSunExposure = (SunExposure)ModifyEnum(CurrentSunExposure, delta, out bool hasChanged);
            return hasChanged;
        }

        /// <summary>
        /// Attempt to set the fertiliser. Returns whether the fertiliser was changed.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SetFertiliser(Fertiliser type)
        {
            if (type != CurrentFertilizer)
            {
                CurrentFertilizer = type;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempt to set the pollinator. Returns whether the pollinator was changed.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool SetPollinator(Pollinator type)
        {
            if (type != CurrentPollinator)
            {
                CurrentPollinator = type;
                return true;
            }

            return false;
        }

        //Function to load the plant's minigame scene - A
        public void loadMinigame()
        {
            SceneController.Load(CurrentPlant.MinigameSceneName);
        }
    }
}