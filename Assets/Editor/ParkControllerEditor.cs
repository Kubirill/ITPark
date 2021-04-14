using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParkController))]
public class ParkControllerEditor : Editor
{
    delegate void ComandGUI(Vector4 step);
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var targetObject = serializedObject.targetObject as ParkController;
       // base.OnInspectorGUI();
        EditorGUILayout.LabelField("attraction programming");
        
        EditorGUILayout.LabelField(" ");
        
        if (targetObject.editVariable == null) targetObject.editVariable = new List<Vector4>();
        for (int i=0;i< targetObject.editVariable.Count; i++)
        {
            EditorGUILayout.LabelField("___________________________________________________________________________");
            ComandGUI comandGUI;
            var current = targetObject.editVariable[i];

            {
                Method now_method = targetObject.ChangeRotate;
                // Задаю имя кнопки
                
                string buttonName = SetButtonBehaiborName(current.x,targetObject, out comandGUI);
                
                void AddMenuItemForProgram(GenericMenu menu, string menuPath, float program)
                {
                    // the menu item is marked as selected if it matches the current value of m_Color
                    menu.AddItem(new GUIContent(menuPath), current.x.Equals(program), OnStepChange, program);
                }
                void OnStepChange(object program)
                {
                    current.x = (float)program;
                    

                }
                Undo.RecordObject(targetObject, "Adding");
                if (GUILayout.Button(buttonName))
                {
                    // create the menu and add items to it
                    GenericMenu menu = new GenericMenu();
                    Undo.RecordObject(targetObject, "Adding");
                    // forward slashes nest menu items under submenus
                    AddMenuItemForProgram(menu, "Change Rotate", 1);
                    AddMenuItemForProgram(menu, "Change Random Rotate", 2);
                    AddMenuItemForProgram(menu, "Change Piston", 3);
                    AddMenuItemForProgram(menu, "Activate or Diactivate Piston", 4);

                    // display the menu
                    menu.ShowAsContext();
                }
            }//Всплывающее меню
            if (comandGUI!=null) comandGUI(current);
            
            if (GUILayout.Button("Delete"))
            {
                Undo.RecordObject(targetObject, "Adding");
                targetObject.editVariable.RemoveAt(i);
                serializedObject.ApplyModifiedProperties();
            }
        }


        EditorGUILayout.LabelField("___________________________________________________________________________");
        
        if (GUILayout.Button("Add"))
        {
            Undo.RecordObject(targetObject, "Adding");
            ProgramStep newStep = new ProgramStep();
            serializedObject.ApplyModifiedProperties();
            targetObject.editVariable.Add(Vector4.zero);
        }
        // 
        Undo.RecordObject(targetObject, "Adding");
        targetObject.seetSpeed = EditorGUILayout.FloatField(targetObject.seetSpeed, GUILayout.Width(120));
        serializedObject.ApplyModifiedProperties();
        Debug.Log(targetObject.editVariable.Count);
    }


    string SetButtonBehaiborName(float step,ParkController park,out ComandGUI comandGUI)
    {
        if (step == 1)
        {
            comandGUI = WriteChangeRotateEditor;
            return "Change Rotate";
        }
        if (step == 2)
        {
            comandGUI = WriteChangeRandomRotateEditor;
            return "Change Random Rotate";
        }
        if (step == 3)
        {
            comandGUI = WriteChangePistonEditor;
            return "Change Piston";
        }
        if (step == 4)
        {
            comandGUI = WriteChangePistonActivateEditor;
            return "Activate or Diactivate Piston";
        }
        comandGUI = default;
        return "Select Behaivor";
    }

    void WriteChangeRotateEditor(Vector4 step)
    {
        EditorGUILayout.BeginHorizontal();
        Undo.RecordObject(serializedObject.targetObject as ParkController, "Adding");
        EditorGUILayout.LabelField("Speed");
        step.x = EditorGUILayout.FloatField(step.x, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Accelerate");
        step.y = EditorGUILayout.FloatField(step.y,GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.z = EditorGUILayout.FloatField(step.z, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
    void WriteChangeRandomRotateEditor(Vector4 step)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Speed");
        var targetObject = serializedObject.targetObject as ParkController;
        targetObject.seetSpeed = EditorGUILayout.FloatField(targetObject.seetSpeed, GUILayout.Width(120));
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
        step.w = EditorGUILayout.FloatField(step.w, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }

    void WriteChangePistonEditor(Vector4 step)
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
        step.w = EditorGUILayout.FloatField(step.w, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
    void WriteChangePistonActivateEditor(Vector4 step)
    {     
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.w = EditorGUILayout.FloatField(step.w, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
}
