/*
 *  Author: Calvin Soueid
 *  Date:   15/11/2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class VariableLock : VisualElement
{
    private Type value;
    public Type Value { 
        get { return value; } 
        set 
        { 
            selector.SetValueWithoutNotify(dropdownValuesInverse[value]);
            this.value = value;
        } 
    }

    private PopupField<string> selector;

    public Action<Type> OnValueChanged;

    private readonly Dictionary<string, Type> dropdownValues = new Dictionary<string, Type>
    {
        {"Soil Depth", typeof(Plant.SoilDepth) },
        {"Soil Type", typeof(Plant.SoilType) },
        {"Water Level", typeof(Plant.WaterLevel) },
        {"Sun Exposure", typeof(Plant.SunExposure) },
        {"Fertiliser", typeof(Plant.Fertiliser) },
        {"Pollinator", typeof(Plant.Pollinator) },
        {"Dispersal", typeof(Plant.Dispersal) }
    };

    private readonly Dictionary<Type, string> dropdownValuesInverse;

    public VariableLock()
    {
        dropdownValuesInverse = dropdownValues.ToDictionary((kvp) => kvp.Value, (kvp) => kvp.Key);
        selector = new PopupField<string>(dropdownValues.Keys.ToList(), 0);
        selector.RegisterValueChangedCallback((changeEvent) =>
        {
            Value = dropdownValues[changeEvent.newValue];
            OnValueChanged?.Invoke(Value);
        });
        

        Add(selector);
        
    }

    public new class UxmlFactory : UxmlFactory<VariableLock> { }
}


