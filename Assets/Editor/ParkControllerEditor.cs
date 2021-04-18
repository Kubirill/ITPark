using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ParkController))]
public class ParkControllerEditor : Editor
{
    bool clickerRotate;
    bool clickerRandomRotate;
    bool clickerPiston;
    bool clickerHorizontalPiston;
    bool programmingButton;

    List<bool> namesRotate= new List<bool>();
    List<bool> namesRandomRotate= new List<bool>();
    List<bool> namesPiston= new List<bool>();
    List<bool> namesHorizontalPiston = new List<bool>();

    [Serializable]
    delegate void ComandGUI(ProgramStep step);
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Undo.RecordObject(serializedObject.targetObject as ParkController, "Adding");
        var targetObject = serializedObject.targetObject as ParkController;
        //base.OnInspectorGUI();
        RotateEditor<RotateGroups,Rotate>(targetObject.rotateGroups, ref namesRotate,ref clickerRotate );
        RotateEditor<RandomRotateGroups,RandomRotate>(targetObject.randomRotateGroups, ref namesRandomRotate, ref clickerRandomRotate);
        RotateEditor<PistonGroups, Piston>(targetObject.pistonGroups, ref namesPiston, ref clickerPiston);
        RotateEditor<HorizontalPistonGroups, HorizontalPiston>(targetObject.horizontalPistonGroups, ref namesHorizontalPiston, ref clickerHorizontalPiston);


        EditorGUILayout.Space(1f);

        EditorGUILayout.Separator();

        
        programmingButton = EditorGUILayout.Foldout(programmingButton," Attraction programming");
        if (programmingButton)
        {
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

                void AddMenuItemForGroup(GenericMenu menu, string menuPath, int groupNum)
                {
                    menu.AddItem(new GUIContent(menuPath), current.num.Equals(groupNum), OnGroupChange, groupNum);
                }

                void OnGroupChange(object program)
                {
                    current.num = (int)program;
                }
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(buttonName, GUILayout.MinWidth(200)))
                {
                    GenericMenu menu = new GenericMenu();

                    AddMenuItemForProgram(menu, "Change Rotate", "Change Rotate");
                    AddMenuItemForProgram(menu, "Change Random Rotate", "Change Random Rotate");
                    AddMenuItemForProgram(menu, "Change Piston", "Change Piston");
                    AddMenuItemForProgram(menu, "Change Horizontal Piston", "Change Horizontal Piston");
                    //AddMenuItemForProgram(menu, "Activate or Diactivate Piston", "Activate or Diactivate Piston");
                    //AddMenuItemForProgram(menu, "Activate or Diactivate Horizontal Piston", "Activate or Diactivate Horizontal Piston");

                    menu.ShowAsContext();
                }

                string groupbuttonName = "<no objects>";
                if (buttonName == "Change Rotate")
                {
                    if (targetObject.rotateGroups.Count > 0) groupbuttonName = targetObject.rotateGroups[current.num].groupName;
                    if (GUILayout.Button(groupbuttonName, GUILayout.MinWidth(120)))
                    {
                        GenericMenu menu = new GenericMenu();
                        for (int j = 0; j < targetObject.rotateGroups.Count; j++)
                        {
                            AddMenuItemForGroup(menu, targetObject.rotateGroups[j].groupName, j);
                        }
                        menu.ShowAsContext();
                    }
                }
                if (buttonName == "Change Random Rotate")
                {
                    if (targetObject.randomRotateGroups.Count > 0) groupbuttonName = targetObject.randomRotateGroups[current.num].groupName;
                    if (GUILayout.Button(groupbuttonName, GUILayout.MinWidth(120)))
                    {
                        GenericMenu menu = new GenericMenu();
                        for (int j = 0; j < targetObject.randomRotateGroups.Count; j++)
                        {
                            AddMenuItemForGroup(menu, targetObject.randomRotateGroups[j].groupName, j);
                        }
                        menu.ShowAsContext();
                    }
                }
                if ((buttonName == "Change Piston") || (buttonName == "Activate or Diactivate Piston"))
                {
                    if (targetObject.pistonGroups.Count > 0) groupbuttonName = targetObject.pistonGroups[current.num].groupName;
                    if (GUILayout.Button(groupbuttonName, GUILayout.MinWidth(120)))
                    {
                        GenericMenu menu = new GenericMenu();
                        for (int j = 0; j < targetObject.pistonGroups.Count; j++)
                        {
                            AddMenuItemForGroup(menu, targetObject.pistonGroups[j].groupName, j);
                        }
                        menu.ShowAsContext();
                    }
                }
                if ((buttonName == "Change Horizontal Piston") || (buttonName == "Activate or Diactivate Horizontal Piston"))
                {
                    if (targetObject.horizontalPistonGroups.Count > 0) groupbuttonName = targetObject.horizontalPistonGroups[current.num].groupName;
                    if (GUILayout.Button(groupbuttonName, GUILayout.MinWidth(120)))
                    {
                        GenericMenu menu = new GenericMenu();
                        for (int j = 0; j < targetObject.horizontalPistonGroups.Count; j++)
                        {
                            AddMenuItemForGroup(menu, targetObject.horizontalPistonGroups[j].groupName, j);
                        }
                        menu.ShowAsContext();
                    }
                }



                EditorGUILayout.EndHorizontal();
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
        }
        
        
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
        if (step == "Change Horizontal Piston")
        {
            comandGUI = WriteChangeHorizontalPistonEditor;
            return "Change Horizontal Piston";
        }
        if (step == "Activate or Diactivate Horizontal Piston")
        {
            comandGUI = WriteChangeHorizontalPistonActivateEditor;
            return "Activate or Diactivate Horizontal Piston";
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
        EditorGUILayout.LabelField("Switch active?");
        step.change = EditorGUILayout.Toggle(step.change, GUILayout.Width(120));
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
        EditorGUILayout.LabelField("Switch active?");
        step.change = EditorGUILayout.Toggle(step.change, GUILayout.Width(120));
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
        EditorGUILayout.LabelField("Switch active?");
        step.change = EditorGUILayout.Toggle(step.change, GUILayout.Width(120));
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

    void WriteChangeHorizontalPistonEditor(ProgramStep step)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max distance");
        step.x = EditorGUILayout.FloatField(step.x, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Min distance");
        step.y = EditorGUILayout.FloatField(step.y, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Speed");
        step.z = EditorGUILayout.FloatField(step.z, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Switch active?");
        step.change = EditorGUILayout.Toggle(step.change, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.longStep = EditorGUILayout.FloatField(step.longStep, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }
    void WriteChangeHorizontalPistonActivateEditor(ProgramStep step)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Command duration");
        step.longStep = EditorGUILayout.FloatField(step.longStep, GUILayout.Width(120));
        EditorGUILayout.EndHorizontal();
    }

    void RotateEditor<T,N>(List<T> targetObject, ref List<bool> namesPickers,ref bool nameClicker) where T : ParkMechanism<N>,new()
    {
        nameClicker = EditorGUILayout.Foldout(nameClicker, GetStringTypeName(targetObject));
        if (nameClicker)
        {
            EditorGUI.indentLevel++;

            for (int i = 0; i < targetObject.Count; i++)
            {
                if (namesPickers.Count <= i) namesPickers.Add(new bool());
                EditorGUILayout.BeginHorizontal();
                namesPickers[i] = EditorGUILayout.Foldout(namesPickers[i], targetObject[i].groupName);
                if (GUILayout.Button("Delete Group", GUILayout.Width(100)))
                {
                    targetObject.RemoveAt(i);
                    namesPickers.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                if (namesPickers[i])
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Group name:");
                    targetObject[i].groupName = EditorGUILayout.TextField(targetObject[i].groupName, GUILayout.Width(120));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(GetStringTypeProperty(targetObject[i]) + i + "].objects"));
                    EditorGUI.indentLevel--;
                }
            }
            if (GUILayout.Button("Add Group"))
            {
                T newGroup = new T();
                newGroup.groupName = "new group" + (targetObject.Count + 1);
                namesPickers.Add(new bool());
                targetObject.Add(newGroup);
            }
            EditorGUI.indentLevel--;
        }

    }
    string GetStringTypeProperty<T>(T type)
    {
        
        RotateGroups trashRo = new RotateGroups();
        if (type.GetType() == trashRo.GetType())
        {
            return "rotateGroups.Array.data[";
        }

        RandomRotateGroups trashRa = new RandomRotateGroups();
        if (type.GetType() == trashRa.GetType())
        {
            return "randomRotateGroups.Array.data[";
        }

        PistonGroups trashPi = new PistonGroups();
        if (type.GetType() == trashPi.GetType())
        {
            return "pistonGroups.Array.data[";
        }

        HorizontalPistonGroups trashHo = new HorizontalPistonGroups();
        if (type.GetType() == trashHo.GetType())
        {
            return "horizontalPistonGroups.Array.data[";
        }
        return "";
    }

    string GetStringTypeName<T>(T type)
    {
        List<RotateGroups> trashRo = new List<RotateGroups>();
        if (type.GetType() == trashRo.GetType())
        {
            return "Rotates:";
        }
        List<RandomRotateGroups> trashRa = new List<RandomRotateGroups>();
        if (type.GetType() == trashRa.GetType())
        {
            return "Random rotates";
        }
        List<PistonGroups> trashPi = new List<PistonGroups>();
        if (type.GetType() == trashPi.GetType())
        {
            return "Pistones:";
        }
        List<HorizontalPistonGroups> trashHo = new List<HorizontalPistonGroups>();
        if (type.GetType() == trashHo.GetType())
        {
            return "Horizontal pistons:";
        }
        return "";
    }

    void CallGUINamesGroup(string comandName,ParkController targetObject)
    {
       
    }
       



}
