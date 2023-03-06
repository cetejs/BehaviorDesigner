using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace BehaviorDesigner.Editor
{
    public class BehaviorWindow : EditorWindow
    {
        private IBehavior behavior;
        private TaskNodeFactory nodeFactory;
        private FieldResolverFactory fieldFactory;
        private BehaviorToolBar toolBar;
        private BehaviorNameView nameView;
        private DescriptionView descriptionView;
        private BehaviorView behaviorView;
        private InspectorView inspectorView;
        private VariablesView variablesView;
        private List<IBehavior> selectedBehaviors = new List<IBehavior>();

        private int behaviorId;
        private long behaviorFileId;
        private int serializeVersion;
        private int selectedIndex = -1;
        private bool isManualSelect;

        public event System.Action onDataChanged; 

        public IBehavior Behavior
        {
            get { return behavior; }
        }

        public BehaviorSource Source
        {
            get { return behavior.Source; }
        }

        public BehaviorView View
        {
            get { return behaviorView; }
        }

        public long BehaviorFileId
        {
            get { return behaviorFileId; }
        }

        [MenuItem("Window/Behavior Designer")]
        public static void ShowWindow()
        {
            BehaviorWindow window = GetWindow<BehaviorWindow>("Behavior Designer");
            if (!HasOpenInstances<BehaviorWindow>())
            {
                window.minSize = new Vector2(700f, 100f);
                window.Init();
            }
        }

        public static void ShowWindow(IBehavior behavior)
        {
            BehaviorWindow window = GetWindow<BehaviorWindow>("Behavior Designer");
            window.minSize = new Vector2(700f, 100f);
            window.Select(behavior);
        }

        private void Init()
        {
            Clear();
            rootVisualElement.Clear();
            VisualElement root = rootVisualElement;
            BehaviorUtils.Load<VisualTreeAsset>("UXML/BehaviorWindow").CloneTree(root);
            root.styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            root.Q<BehaviorToolBar>().Init(this);
            root.Q<BehaviorNameView>().Init();
            root.Q<BehaviorView>().Init();
        }

        private void Init(IBehavior behavior)
        {
            this.behavior = behavior;
            behaviorId = behavior.InstanceID;
            behaviorFileId = BehaviorUtils.GetFileId(behavior.Object);
            serializeVersion = Source.Version;
            rootVisualElement.Clear();
            VisualElement root = rootVisualElement;
            BehaviorUtils.Load<VisualTreeAsset>("UXML/BehaviorWindow").CloneTree(root);
            root.styleSheets.Add(BehaviorUtils.Load<StyleSheet>("Styles/BehaviorWindow"));
            nodeFactory = new TaskNodeFactory();
            fieldFactory = new FieldResolverFactory();
            toolBar = root.Q<BehaviorToolBar>();
            nameView = root.Q<BehaviorNameView>();
            descriptionView = root.Q<DescriptionView>();
            behaviorView = root.Q<BehaviorView>();
            inspectorView = root.Q<InspectorView>();
            variablesView = root.Q<VariablesView>();
            toolBar.Init(this);
            nameView.Init(this);
            descriptionView.Init(this);
            behaviorView.Init(this);
            inspectorView.Init(this);
            variablesView.Init(this);
            Restore();
            Undo.ClearUndo(behavior.Object);
        }

        private void Clear()
        {
            behavior = null;
            nodeFactory = null;
            fieldFactory = null;
            toolBar = null;
            nameView = null;
            descriptionView = null;
            behaviorView = null;
            inspectorView = null;
            variablesView = null;
            onDataChanged = null;
            behaviorId = -1;
            behaviorFileId = -1;
            serializeVersion = -1;
        }

        public void Select(IBehavior behavior)
        {
            if (behavior == null || !behavior.Object)
            {
                Init();
                SelectObject(null);
                return;
            }

            Init(behavior);
            if (selectedIndex < selectedBehaviors.Count - 1)
            {
                selectedBehaviors.RemoveRange(selectedIndex + 1, selectedBehaviors.Count - selectedIndex - 1);
            }

            selectedBehaviors.Add(behavior);
            selectedIndex = Mathf.Clamp(selectedIndex + 1, 0, selectedBehaviors.Count);
            SelectObject(behavior);
        }

        public void UndoSelect(bool isRedo)
        {
            if (this.behavior != null)
            {
                selectedIndex += isRedo ? 1 : -1;
            }

            if (selectedIndex < 0 || selectedIndex >= selectedBehaviors.Count)
            {
                selectedIndex = Mathf.Clamp(selectedIndex, 0, selectedBehaviors.Count - 1);
                return;
            }

            IBehavior behavior = selectedBehaviors[selectedIndex];
            if (behavior == null || !behavior.Object)
            {
                Init();
                SelectObject(null);
                selectedIndex = -1;
                selectedBehaviors.Clear();
            }
            else
            {
                Init(behavior);
                SelectObject(behavior);
            }
        }

        private void SelectObject(IBehavior behavior)
        {
            Object selectObj = null;
            if (behavior != null)
            {
                if (behavior.Object is Component component)
                {
                    selectObj = component.gameObject;
                }
                else
                {
                    selectObj = behavior.Object;
                }
            }

            if (Selection.activeObject != selectObj)
            {
                isManualSelect = true;
                Selection.activeObject = selectObj;
            }
        }

        private void Refresh()
        {
            selectedBehaviors.Clear();
            Object obj = EditorUtility.InstanceIDToObject(behaviorId);
            if (obj is IBehavior behavior)
            {
                selectedIndex = -1;
                selectedBehaviors.Clear();
                Select(behavior);
            }
            else
            {
                Init();
            }
        }

        public void RegisterVariableChangedCallback(Action<IEnumerable<SharedVariable>> callback)
        {
            variablesView.onVariableChanged += callback;
        }

        public void RegisterUndo(string undoName)
        {
            Undo.RegisterCompleteObjectUndo(behavior.Object, undoName);
            serializeVersion = ++Source.Version;
        }

        private void Restore()
        {
            Source.Load();
            behaviorView.Restore();
            inspectorView.Restore();
            variablesView.Restore();
        }

        public void Save()
        {
            if (Application.isPlaying)
            {
                behaviorView.Save();
                return;
            }

            Save(behavior);
            onDataChanged?.Invoke();
        }

        private void Save(IBehavior behavior)
        {
            if (!behavior.Object)
            {
                return;
            }

            behaviorView.Save();
            Source.Save(behavior.Source);
            EditorUtility.SetDirty(behavior.Object);
            AssetDatabase.SaveAssetIfDirty(behavior.Object);
        }

        public void SaveAs()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save As...", "Behavior", "asset", null);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ExternalBehavior external = CreateInstance<ExternalBehavior>();
            Save(external);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(external, path);
            AssetDatabase.Refresh();
            Selection.activeObject = external;
        }

        public TaskNode CreateNode(Task task)
        {
            return nodeFactory.Create(task, this);
        }

        public IFieldResolver CreateField(FieldInfo fieldInfo)
        {
            return fieldFactory.Create(fieldInfo, this);
        }

        private void UndoRedoPerformed()
        {
            if (behavior == null)
            {
                return;
            }

            if (serializeVersion != Source.Version)
            {
                serializeVersion = Source.Version;
                Restore();
            }
        }

        private void OnSelectionChanged()
        {
            if (isManualSelect)
            {
                isManualSelect = false;
                return;
            }

            bool isSelectBehavior = false;
            if (Selection.activeObject != null)
            {
                if (Selection.activeObject is GameObject go)
                {
                    if (go.TryGetComponent(out IBehavior behavior) && !BehaviorUtils.HasComponent(go, behaviorId))
                    {
                        isSelectBehavior = true;
                        Select(behavior);
                    }
                }
                else if (Selection.activeObject is ExternalBehavior behavior)
                {
                    isSelectBehavior = true;
                    Select(behavior);
                }
            }

            if (!isSelectBehavior)
            {
                Init();
            }
        }

        private void PlayModeStateChange(PlayModeStateChange state)
        {
            switch (state)
            {
                case UnityEditor.PlayModeStateChange.EnteredPlayMode:
                case UnityEditor.PlayModeStateChange.EnteredEditMode:
                    Refresh();
                    break;
            }
        }

        private void OnEnable()
        {
            Refresh();
            Undo.undoRedoPerformed += UndoRedoPerformed;
            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.playModeStateChanged += PlayModeStateChange;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= UndoRedoPerformed;
            Selection.selectionChanged -= OnSelectionChanged;
            EditorApplication.playModeStateChanged -= PlayModeStateChange;
        }

        private void Update()
        {
            variablesView?.Update();
            descriptionView?.DoDraw();
            inspectorView?.DoDraw();
        }
    }
}