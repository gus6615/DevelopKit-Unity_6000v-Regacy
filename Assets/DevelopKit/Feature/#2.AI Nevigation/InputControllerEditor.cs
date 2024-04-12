using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.AI;

[CustomEditor(typeof(InputController))]
public class InputControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InputController inputController = (InputController)target;

        inputController.inputMode = (InputController.InputMode)EditorGUILayout.EnumPopup("Input Mode", inputController.inputMode);

        switch (inputController.inputMode)
        {
            case InputController.InputMode.Keyboard:
                inputController.playerInput = (PlayerInput)EditorGUILayout.ObjectField("Player Input", inputController.playerInput, typeof(PlayerInput), true);
                break;
        }

        inputController.agent = (NavMeshAgent)EditorGUILayout.ObjectField("Nav Mesh Agent", inputController.agent, typeof(NavMeshAgent), true);
    }
}
