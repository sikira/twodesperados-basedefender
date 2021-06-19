using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BaseDefender.Editor
{

    public class WizardLevelDesign : ScriptableWizard
    {
        LevelData data;

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


    }
}
