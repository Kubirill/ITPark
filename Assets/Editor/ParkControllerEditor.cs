using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParkController))]
public class ParkControllerEditor : Editor
{
    delegate void ComandGUI(ProgramStep step);
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var targetObject = serializedObject.targetObject as ParkController;
       // base.OnInspectorGUI();
        EditorGUILayout.LabelField("attraction programming");
        
        EditorGUILayout.LabelField(" ");
        
        if (targetObject.programs == null) targetObject.programs = new List<ProgramStep>();
        for (int i=0;i< targetObject.programs.Count; i++)
        {
             EditorGUILayout.LabelField("___________________________________________________________________________");
            ComandGUI comandGUI;
            var current = targetObject.programs[i];

            {
                Method now_method = targetObject.ChangeRotate;
                // Задаю имя кнопки
                
                string buttonName = SetButtonBehaiborName(current.Step,targetObject, out comandGUI);
                
                void AddMenuItemForProgram(GenericMenu menu, string menuPath, Method program)
                {
                    // the menu item is marked as selected if it matches the current value of m_Color
                    menu.AddItem(new GUIContent(menuPath), current.Step.Equals(program), OnStepChange, program);
                }
                void OnStepChange(object program)
                {
                    current.Step = (Method)program;
                    

                }
                Undo.RecordObject(targetObject, "Adding");
                if (GUILayout.Button(buttonName))
                {
                    // create the menu and add items to it
                    GenericMenu menu = new GenericMenu();
                    Undo.RecordObject(targetObject, "Adding");
                    // forward slashes nest menu items under submenus
                    AddMenuItemForProgram(menu, "Change Rotate", targetObject.ChangeRotate);
                    AddMenuItemForProgram(menu, "Change Random Rotate", targetObject.ChangeSeets);
                    AddMenuItemForProgram(menu, "Change Piston", targetObject.ChangePistons);
                    AddMenuItemForProgram(menu, "Activate or Diactivate Piston", targetObject.ChangePistonsActive);

                    // display the menu
                    menu.ShowAsContext();
                }
            }//Всплывающее меню
            if (comandGUI!=null) comandGUI(current);
            
            if (GUILayout.Button("Delete"))
            {
                Undo.RecordObject(targetObject, "Adding");
                targetObject.programs.RemoveAt(i);
                serializedObject.ApplyModifiedProperties();
            }
        }


        EditorGUILayout.LabelField("___________________________________________________________________________");
        
        if (GUILayout.Button("Add"))
        {
            Undo.RecordObject(targetObject, "Adding");
            ProgramStep newStep = new ProgramStep();
            serializedObject.ApplyModifiedProperties();
            targetObject.programs.Add(newStep);
        }
        // 
        Undo.RecordObject(targetObject, "Adding");
        targetObject.seetSpeed = EditorGUILayout.FloatField(targetObject.seetSpeed, GUILayout.Width(120));
        serializedObject.ApplyModifiedProperties();
        Debug.Log(targetObject.programs.Count);
    }


    string SetButtonBehaiborName(Method step,ParkController park,out ComandGUI comandGUI)
    {
        if (step == park.ChangeRotate)
        {
            comandGUI = WriteChangeRotateEditor;
            return "Change Rotate";
        }
        if (step == park.ChangeSeets)
        {
            comandGUI = WriteChangeRandomRotateEditor;
            return "Change Random Rotate";
        }
        if (step == park.ChangePistons)
        {
            comandGUI = WriteChangePistonEditor;
            return "Change Piston";
        }
        if (step == park.ChangePistonsActive)
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
        step.longStep = EditorGUILayout.FloatField(step.longStep, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
    void WriteChangeRandomRotateEditor(ProgramStep step)
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
