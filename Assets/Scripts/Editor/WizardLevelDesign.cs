using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;

namespace BaseDefender.Editor
{

    public class WizardLevelDesign : ScriptableWizard
    {
        public LevelData data;
        public LevelInitalizer initalizer;
        private Timer secTimer;


        private void OnEnable()
        {
            data = LevelData.Instance;
            if (secTimer == null)
            {
                secTimer = new Timer(1000);
                secTimer.Elapsed += OnTimerElapsed;
            }
        }


        private INodePathfinderAlgo aStar;
        // private INodePathfinderAlgo aStar2;

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
                    initalizer = new LevelInitalizer();
                    initalizer.Init(data);
                }
                if (GUILayout.Button("Clean Stage"))
                {
                    initalizer.ClearAll();
                }

                if (GUILayout.Button("Find Path A*"))
                {
                    var debuger = GameObject.FindObjectOfType<DebuggerPathfinding>();

                    // var nonWalkables = initalizer.pMonitor.obstacleListPosition.Select(o => o.Position).ToList();
                    var nonWalkables = initalizer.pMonitor.nonWalkablePositions;
                    aStar = PathfindingAlgo.GetAlgo();
                    aStar.SetUp(new Vector2Int(9, 9), new Vector2Int(15, 15), data.MapArea, nonWalkables);
                    aStar.SetUpDebugger(debuger, 3);

                    // aStar2 = PathfindingAlgo.GetAlgo();
                    // aStar2.SetUp(new Vector2Int(14, 1), new Vector2Int(8, 8), data.MapArea, nonWalkables);
                    // aStar2.SetUpDebugger(debuger, 6);
                }

                if (GUILayout.Button("Step in Path A*"))
                {
                    aStar?.FindStep();
                    // aStar2?.FindStep();
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

            if (aStar.FindStep() != null)
            {
                secTimer.Stop();
            }
        }

    }
}
