﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BaseDefender.Editor
{

    public class WizardLevelDesign : ScriptableWizard
    {
        public LevelData data;

        private void OnEnable()
        {
            data = new LevelData();
            // data = LevelData.loadMe();
            // SceneView.duringSceneGui += onDuringSceneGui;
        }

        [MenuItem("Base Defender/Level Design")]
        private static void MenuEntryCall()
        {
            DisplayWizard<WizardLevelDesign>("Level Design Wizard", "Kreiraj");
        }
        private void OnGUI()
        {
            DrawLevelData();
        }

        private void DrawLevelData()
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                data.SizeX = EditorGUILayout.IntField("Width Nodes", data.SizeX);
                data.SizeY = EditorGUILayout.IntField("Width Nodes", data.SizeY);

                GUILayout.Space(20f);

                if (GUILayout.Button("Create Random Level!"))
                {
                    LevelInitalizer initalizer = new LevelInitalizer();
                    initalizer.Init(data);

                }
            }
        }
    }
}
