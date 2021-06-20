using System.Collections;
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
        }

        private AStarAlgo aStar;

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
                data.SizeY = EditorGUILayout.IntField("Height Nodes", data.SizeY);
                data.NumberOfEnemySpawner = EditorGUILayout.IntField("Enemy Spawner", data.NumberOfEnemySpawner);

                GUILayout.Space(20f);

                if (GUILayout.Button("Create Random Level!"))
                {
                    LevelInitalizer initalizer = new LevelInitalizer();
                    initalizer.Init(data);
                }

                if (GUILayout.Button("Find Path A*"))
                {
                    aStar = new AStarAlgo();
                    aStar.Find(new Vector2Int(1, 2), new Vector2Int(10, 9), data);
                }

                if (GUILayout.Button("Step in Path A*"))
                {
                    aStar?.FindStep();                    
                }

            }
        }
    }
}
