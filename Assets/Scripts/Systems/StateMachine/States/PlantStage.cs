/*
 *  Author: Calvin Soueid
 *  Date:   27/10/2021
 */

using Plant;
using Plant.Data;
using System.Reflection;
using UnityEngine;

namespace StateMachine
{
    public class PlantStage : State
    {
        public PlantStageInfo Info;

        // Initialise the stage by setting objects
        public override void Start()
        {
            if (Info.IsCheckConditionsVisible)
            {
                PlantEnvironment.Instance.CheckConditionsButton.SetActive(true);
            }
            else
            {
                PlantEnvironment.Instance.CheckConditionsButton.SetActive(false);
            }

            FieldInfo[] requirements = Info.Requirements.GetType().GetFields();

            for (int i = 0; i < requirements.Length; i++)
            {
                MonoBehaviour interactable = PlantEnvironment.InteractablesByType[requirements[i].FieldType]();
                int enumValue = (int)requirements[i].GetValue(Info.Requirements);

                if (interactable == null)
                {
                    throw new System.NullReferenceException($"Given interactable ({interactable}) found from '{requirements[i].Name}' is null. Please check if the object is disabled or destroyed on start.");
                }

                switch (enumValue)
                {
                    case -2: //Disabled
                        interactable.gameObject.SetActive(false);
                        break;

                    case -1: //Uninteractable
                        ((Interfaces.IDisableable)interactable).SetActive(false);
                        interactable.gameObject.SetActive(true);
                        break;
                            
                    default:
                        ((Interfaces.IDisableable)interactable).SetActive(true);
                        interactable.gameObject.SetActive(true);
                        break;
                }
            }

            DialogueSystem.Instance.onDialogueClose.AddListener(() => { UserInterfaceStack.Instance.Show("GameUI", true); });

            PlantEnvironment.Instance.GetHint(true);
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }

        public override void End()
        {
        }

        // The chickens have come to roost. This is the downside of the enum methodology.
        public PlantStage(State next, StageRequirement requirements, PlantStageInfo info)
        {
            transitions = new Transition[]
            {
                new Transition(next)
            };

            Info = PlantStageInfo.Clone(info);

            // Each of these is a single condition function. We only implement those that are not disabled.

            if (requirements.SoilDepth != SoilDepth.Disabled && requirements.SoilDepth != SoilDepth.Uninteractable)
            {
                transitions[0].AddConditions(() =>
                {
                    int requirement = (int)requirements.SoilDepth;
                    int current = (int)PlantEnvironment.Instance.CurrentSoilDepth;
                    (string, ConditionResult) result;
                    result.Item1 = "Soil Depth";

                    result.Item2 = (ConditionResult)Utility.Math.Compare(current, requirement);
                    return result;
                });
            }

            if (requirements.SoilType != SoilType.Disabled && requirements.SoilType != SoilType.Uninteractable)
            {
                transitions[0].AddConditions(() =>
                {
                    (string, ConditionResult) result;
                    result.Item1 = "Soil Type";
                    result.Item2 = requirements.SoilType == PlantEnvironment.Instance.CurrentSoilType ? ConditionResult.Correct : ConditionResult.Incorrect;
                    return result;
                });
            }

            if (requirements.WaterLevel != WaterLevel.Disabled && requirements.WaterLevel != WaterLevel.Uninteractable)
            {
                transitions[0].AddConditions(() =>
                {
                    int requirement = (int)requirements.WaterLevel;
                    int current = (int)PlantEnvironment.Instance.CurrentWaterLevel;
                    (string, ConditionResult) result;
                    result.Item1 = "Water Level";

                    result.Item2 = (ConditionResult)Utility.Math.Compare(current, requirement);
                    return result;
                });
            }

            if (requirements.SunExposure != SunExposure.Disabled && requirements.SunExposure != SunExposure.Uninteractable)
            {
                transitions[0].AddConditions(() =>
                {
                    int requirement = (int)requirements.SunExposure;
                    int current = (int)PlantEnvironment.Instance.CurrentSunExposure;
                    (string, ConditionResult) result;
                    result.Item1 = "Sun Exposure";

                    result.Item2 = (ConditionResult)Utility.Math.Compare(current, requirement);
                    return result;
                });
            }

            if (requirements.Fertiliser != Fertiliser.Disabled && requirements.Fertiliser != Fertiliser.Uninteractable)
            {
                transitions[0].AddConditions(() =>
                {
                    (string, ConditionResult) result;
                    result.Item1 = "Fertiliser";
                    result.Item2 = requirements.Fertiliser == PlantEnvironment.Instance.CurrentFertilizer ? ConditionResult.Correct : ConditionResult.Incorrect;
                    return result;
                });
            }

            if (requirements.Pollinator != Pollinator.Disabled && requirements.Pollinator != Pollinator.Uninteractable)
            {
                transitions[0].AddConditions(() =>
                {
                    (string, ConditionResult) result;
                    result.Item1 = "Pollinator";
                    result.Item2 = requirements.Pollinator == PlantEnvironment.Instance.CurrentPollinator ? ConditionResult.Correct : ConditionResult.Incorrect;
                    return result;
                });
            }

            if (requirements.DispersalMechanism != Dispersal.Disabled && requirements.DispersalMechanism != Dispersal.Uninteractable)
            {
                transitions[0].AddConditions(() =>
                {
                    (string, ConditionResult) result;
                    result.Item1 = "DispersalMechanism";
                    result.Item2 = requirements.DispersalMechanism == PlantEnvironment.Instance.CurrentDispersal ? ConditionResult.Correct : ConditionResult.Incorrect;
                    return result;
                });
            }
        }
    }
}