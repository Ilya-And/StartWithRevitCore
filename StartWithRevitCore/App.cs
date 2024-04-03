using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.UI.Selection;
//using Dynamo.Core;
//using Dynamo.Applications;
using System.Reflection;

namespace StartWithRevitCore
{
    public class App : IExternalApplication
    {
        const string _test_project_filepath = "C:/Users/Илья/Desktop/Проект2.rvt";

        UIApplication _uiapp;
        public static string AssemblyPath { get; private set; }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication a)
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var arrayStr = thisAssemblyPath.Split('\\').ToList();
            arrayStr.Remove(arrayStr[arrayStr.Count - 1]);
            string folderPath = string.Join("\\", arrayStr.ToArray());
            AssemblyPath = folderPath;

            try
            {
                Assembly.LoadFrom(folderPath + "\\PresentationFramework.dll");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }

            a.ControlledApplication.ApplicationInitialized += OnApplicationInitialized;

            return Result.Succeeded;
        }

        void OnApplicationInitialized(object sender, ApplicationInitializedEventArgs e)
        {
            // Sender is an Application instance:

            Autodesk.Revit.ApplicationServices.Application? app = sender as Autodesk.Revit.ApplicationServices.Application;

            // However, UIApplication can be 
            // instantiated from Application.

            UIApplication uiapp = new UIApplication(app);

            var uidoc = uiapp.OpenAndActivateDocument(_test_project_filepath);
            var doc = uidoc.Document;
            var allWalls = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements();
            MessageBox.Show($"В этом проекте {allWalls.Count} стен.");

            /*string Dynamo_Journal_Path = @"C:\Users\Илья\Desktop\TestScriptToStert.dyn";
            DynamoRevit dynamoRevit = new DynamoRevit();
            DynamoRevitCommandData dynamoRevitCommandData = new DynamoRevitCommandData();
            dynamoRevitCommandData.Application = uiapp;
            IDictionary<string, string> journalData = new Dictionary<string, string>
            {
                {Dynamo.Applications.JournalKeys.ShowUiKey, false.ToString() }, // don't show DynamoUI at runtime
                {Dynamo.Applications.JournalKeys.AutomationModeKey, true.ToString() }, // run journal automatically
                {Dynamo.Applications.JournalKeys.DynPathKey, "" }, // run node at this file path
                {Dynamo.Applications.JournalKeys.DynPathExecuteKey, true.ToString() }, // The journal file can specify if the Dynamo workspace opened
                {Dynamo.Applications.JournalKeys.ForceManualRunKey, false.ToString() }, // don't run in manual mode
                {Dynamo.Applications.JournalKeys.ModelShutDownKey, true.ToString() },
                {Dynamo.Applications.JournalKeys.ModelNodesInfo, false.ToString() }
            };

            dynamoRevitCommandData.JournalData = journalData;
            dynamoRevit.ExecuteCommand(dynamoRevitCommandData);
            DynamoRevit.RevitDynamoModel.OpenFileFromPath(Dynamo_Journal_Path, true);
            DynamoRevit.RevitDynamoModel.ForceRun();*/

        }
    }


}

