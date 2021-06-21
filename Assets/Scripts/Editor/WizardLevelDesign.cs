using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace BaseDefender.Editor
{

    public class WizardLevelDesign : ScriptableWizard
    {
        public LevelData data;
        private Timer secTimer;


        private void OnEnable()
        {
            data = new LevelData();
            if (secTimer == null)
            {
                secTimer = new Timer(1000);
                secTimer.Elapsed += OnTimerElapsed;
            }
        }


        private AStarAlgo aStar;
        private AStarAlgo aStar2;

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
                    aStar.SetUp(new Vector2Int(1, 1), new Vector2Int(8, 8), data, 3);

                    aStar2 = new AStarAlgo();
                    aStar2.SetUp(new Vector2Int(14, 1), new Vector2Int(8, 8), data, 6);
                }

                if (GUILayout.Button("Step in Path A*"))
                {
                    aStar?.FindStep();
                    aStar2?.FindStep();
                }

                EditorGUILayout.BeginHorizontal();
                var btnWidth = GUILayout.MaxWidth(150f);
                if (GUILayout.Button("Calculate All A*", btnWidth))
                {
                    secTimer.Start();
                }
                if (GUILayout.Button("Stop Calculate All A*", btnWidth))
                {
                    secTimer.Stop();
                }
                EditorGUILayout.EndHorizontal();

            }
        }


        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Debug.Log("timer je");

            if (aStar.FindStep())
            {
                secTimer.Stop();
            }
        }

    }
}
