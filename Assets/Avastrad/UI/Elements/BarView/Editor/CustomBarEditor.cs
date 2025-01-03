using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Avastrad.UI.Elements.BarView.Editor
{
    [CustomEditor(typeof(Bar))]
    public sealed class BarEditor : UnityEditor.Editor
    {
        private SerializedProperty _fill;
        private SerializedProperty _minValue;
        private SerializedProperty _maxValue;
        private SerializedProperty _value;
        private SerializedProperty _wholeNumbers;
        private SerializedProperty _onValueChange;

        private void OnEnable()
        {
            _fill = serializedObject.FindProperty("fill");
            _minValue = serializedObject.FindProperty("minValue");
            _maxValue = serializedObject.FindProperty("maxValue");
            _value = serializedObject.FindProperty("value");
            _wholeNumbers = serializedObject.FindProperty("wholeNumbers");
            _onValueChange = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawFillField();
            DrawOtherFields();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFillField()
        {
            using (var fillChanged = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(_fill, new GUIContent("Fill"));

                if (fillChanged.changed)
                {
                    var setFill = typeof(Bar).GetMethod("SetFill", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    var customBar = serializedObject.targetObject;
                    setFill?.Invoke(customBar, new object[]{_fill.objectReferenceValue});
                }
            }  
        }
        
        private void DrawOtherFields()
        {
            if (_wholeNumbers.boolValue)
                _minValue.floatValue = (float)Math.Round(_minValue.floatValue, MidpointRounding.AwayFromZero);
            EditorGUILayout.PropertyField(_minValue, new GUIContent("Min Value"));
            
            _maxValue.floatValue = Mathf.Clamp(_maxValue.floatValue, _minValue.floatValue, float.MaxValue);
            if (_wholeNumbers.boolValue)
                _maxValue.floatValue = (float)Math.Round(_maxValue.floatValue, MidpointRounding.AwayFromZero);
            EditorGUILayout.PropertyField(_maxValue, new GUIContent("Max Value"));
            
            EditorGUILayout.PropertyField(_wholeNumbers, new GUIContent("Whole Numbers"));

            DrawValueSlider();
            
            EditorGUILayout.PropertyField(_onValueChange, new GUIContent("On Value Change"));
        }

        private void DrawValueSlider()
        {
            using (var valueChanged = new EditorGUI.ChangeCheckScope())
            {
                _value.floatValue = Mathf.Clamp(_value.floatValue, _minValue.floatValue, _maxValue.floatValue);
                if (_wholeNumbers.boolValue)
                    _value.floatValue = (float)Math.Round(_value.floatValue, MidpointRounding.AwayFromZero);
                EditorGUILayout.Slider(_value, _minValue.floatValue, _maxValue.floatValue);
                
                if (valueChanged.changed)
                {
                    var customBar = serializedObject.targetObject as Bar;
                    customBar.SetValueWithoutNotification(_value.floatValue);
                }
            }  
        }
    }
}