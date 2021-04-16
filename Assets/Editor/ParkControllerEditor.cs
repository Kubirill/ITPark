using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ParkController))]
public class ParkControllerEditor : Editor
{
    [Serializable]
    delegate void ComandGUI(ProgramStep step);
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Undo.RecordObject(serializedObject.targetObject as ParkController, "Adding");
        var targetObject = serializedObject.targetObject as ParkController;
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("attraction programming");
        EditorGUILayout.LabelField(" ");
        
        if (targetObject.programs == null) targetObject.programs = new List<ProgramStep>();

        for (int i = 0; i < targetObject.programs.Count; i++)
        {
            EditorGUILayout.LabelField("___________________________________________________________________________");
            ComandGUI comandGUI;
            var current = targetObject.programs[i];

            string buttonName = SetButtonBehaiborName(current.Step, targetObject, out comandGUI);

            void AddMenuItemForProgram(GenericMenu menu, string menuPath, string program)
            {
                menu.AddItem(new GUIContent(menuPath), current.Step.Equals(program), OnStepChange, program);
            }

            void OnStepChange(object program)
            {
                current.Step = (string)program;
            }

            if (GUILayout.Button(buttonName))
            {
                GenericMenu menu = new GenericMenu();

                AddMenuItemForProgram(menu, "Change Rotate", "Change Rotate");
                AddMenuItemForProgram(menu, "Change Random Rotate", "Change Random Rotate");
                AddMenuItemForProgram(menu, "Change Piston", "Change Piston");
                AddMenuItemForProgram(menu, "Activate or Diactivate Piston", "Activate or Diactivate Piston");

                menu.ShowAsContext();
            }
                if (comandGUI != null) comandGUI(current);

                if (GUILayout.Button("Delete"))
                {
                    targetObject.programs.RemoveAt(i);
                }
            
        }

        EditorGUILayout.LabelField("___________________________________________________________________________");
        
        if (GUILayout.Button("Add"))
        {
            ProgramStep newStep = new ProgramStep();
            targetObject.programs.Add(newStep);
        }
        // 
        targetObject.seetSpeed = EditorGUILayout.FloatField(targetObject.seetSpeed, GUILayout.Width(120));
        serializedObject.ApplyModifiedProperties();
    }


    string SetButtonBehaiborName(string step,ParkController park,out ComandGUI comandGUI)
    {
        if (step == "Change Rotate")
        {
            comandGUI = WriteChangeRotateEditor;
            return "Change Rotate";
        }
        if (step == "Change Random Rotate")
        {
            comandGUI = WriteChangeRandomRotateEditor;
            return "Change Random Rotate";
        }
        if (step == "Change Piston")
        {
            comandGUI = WriteChangePistonEditor;
            return "Change Piston";
        }
        if (step == "Activate or Diactivate Piston")
        {
            comandGUI = WriteChangePistonActivateEditor;
            return "Activate or Diactivate Piston";
        }
        comandGUI = default;
        return "Select Behaivor";
    }

    void WriteChangeRotateEditor(ProgramStep step)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Speed");
        step.x = EditorGUILayout.FloatField(step.x, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Accelerate");
        step.y = EditorGUILayout.FloatField(step.y,GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.longStep = EditorGUILayout.FloatField(step.longStep, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
    void WriteChangeRandomRotateEditor(ProgramStep step)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Speed");
        step.x = EditorGUILayout.FloatField(step.x, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Accelerate");
        step.y = EditorGUILayout.FloatField(step.y, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Waiting to change rotate (sec)");
        step.z = EditorGUILayout.FloatField(step.z, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.longStep = EditorGUILayout.FloatField(step.longStep, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }

    void WriteChangePistonEditor(ProgramStep step)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max angle");
        step.x = EditorGUILayout.FloatField(step.x, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Min angle");
        step.y = EditorGUILayout.FloatField(step.y, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Speed");
        step.z = EditorGUILayout.FloatField(step.z, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.longStep = EditorGUILayout.FloatField(step.longStep, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
    void WriteChangePistonActivateEditor(ProgramStep step)
    {     
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.longStep = EditorGUILayout.FloatField(step.longStep, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
}
