using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace BehaviorDesigner
{
    public class BehaviorWindow : EditorWindow
    {
        private IBehavior behavior;
        private BehaviorToolBar toolBar;
        private BehaviorNameView nameView;
        private DescriptionView descriptionView;
        private BehaviorView behaviorView;
        private InspectorView inspectorView;
        private VariablesView variablesView;
        private int serializedVersion;
        private static List<Type> sharedVariableTypes;
        public System.Action onUpdate;

        public IBehavior Behavior
        {
            get { return behavior; }
        }

        public BehaviorSource Source
        {
            get
            {
                if (behavior != null)
                {
                    return behavior.Source;
                }

                return null;
            }
        }

        public BehaviorView View
        {
            get { return behaviorView; }
        }

        public List<Type> SharedVariableTypes
        {
            get
            {
                if (sharedVariableTypes == null)
                {
                    sharedVariableTypes = new List<Type>();
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (type.IsAbstract)
                            {
                                continue;
                            }

                            if (!type.IsSubclassOf(typeof(SharedVariable)))
                            {
                                continue;
                            }

                            sharedVariableTypes.Add(type);
                        }
                    }
                }

                return sharedVariableTypes;
            }
        }

        [MenuItem("Window/Behavior Designer")]
        public static void ShowWindow()
        {
            GetWindow<BehaviorWindow>("Behavior Designer");
        }

        public static void ShowWindow(IBehavior behavior)
        {
            BehaviorWindow window = GetWindow<BehaviorWindow>("Behavior Designer");
            window.SetBehavior(behavior);
        }

        private void CreateGUI()
        {
            if (rootVisualElement.childCount > 0)
            {
                return;
            }

            VisualElement root = rootVisualElement;
            EditorBehaviorUtility.Load<VisualTreeAsset>("UXML/BehaviorWindow").CloneTree(root);
            root.styleSheets.Add(EditorBehaviorUtility.Load<StyleSheet>("Styles/BehaviorWindow"));
            toolBar = root.Q<BehaviorToolBar>();
            nameView = root.Q<BehaviorNameView>();
            descriptionView = root.Q<DescriptionView>();
            behaviorView = root.Q<BehaviorView>();
            inspectorView = root.Q<InspectorView>();
            variablesView = root.Q<VariablesView>();

            toolBar.Init(this);
            nameView.Init(this);
            behaviorView.Init(this);
            descriptionView.Init(this);
            inspectorView.Init(this);
            variablesView.Init(this);
            behaviorView.onNodeSelected += descriptionView.OnNodeSelected;
            behaviorView.onNodeUnselected += descriptionView.OnNodeUnselected;
            behaviorView.onNodeSelected += inspectorView.OnNodeSelected;
            behaviorView.onNodeUnselected += inspectorView.OnNodeUnselected;
        }

        private void OnEnable()
        {
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
        }

        private void OnInspectorUpdate()
        {
            onUpdate?.Invoke();
        }

        private void OnSelectionChange()
        {
            if (Selection.activeGameObject)
            {
                if (Selection.activeGameObject.TryGetComponent(out IBehavior behavior))
                {
                    Component c1 = (Component) behavior;
                    if (this.behavior == null || this.behavior.Object == null || this.behavior is not Component)
                    {
                        SetBehavior(behavior);
                    }
                    else if (this.behavior is Component c2 && c1.gameObject != c2.gameObject)
                    {
                        SetBehavior(behavior);
                    }
                }
                else
                {
                    SetBehavior(null);
                }
            }
            else
            {
                if (Selection.activeObject is IBehavior behavior)
                {
                    if (this.behavior == null || this.behavior.Object != behavior.Object)
                    {
                        SetBehavior(behavior);
                    }
                }
                else
                {
                    SetBehavior(null);
                }
            }
        }

        private void OnUndoRedoPerformed()
        {
            if (behavior == null)
            {
                return;
            }

            if (behavior.Object == null)
            {
                SetBehavior(null);
                return;
            }

            if (serializedVersion != Source.serializedVersion)
            {
                SetBehavior(behavior);
            }
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    toolBar?.ClearSelection();
                    OnSelectionChange();
                    break;
            }
        }

        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            if (toolBar != null)
            {
                toolBar.ClearSelection();
                toolBar.Refresh();
            }
        }

        public void SetBehavior(IBehavior behavior)
        {
            CreateGUI();
            this.behavior = behavior;
            onUpdate = null;
            toolBar.Refresh();
            nameView.Refresh();
            descriptionView.Refresh();
            inspectorView.Refresh();
            behaviorView.Refresh();
            variablesView.Refresh();
            serializedVersion = behavior != null ? Source.serializedVersion : -1;
        }

        public TaskNode CreateNode(Type type, Rect newPos)
        {
            UndoRecord("BehaviorTree Create Task");
            Task task = (Task) Activator.CreateInstance(type);
            TaskNameAttribute attribute = type.GetCustomAttribute<TaskNameAttribute>();
            task.Name = ObjectNames.NicifyVariableName(attribute != null ? attribute.name : type.Name);
            task.Guid = Guid.NewGuid().ToString();
            task.graphPosition = newPos;
            Source.Tasks.Add(task);
            return CreateNode(task);
        }

        public TaskNode CreateNode(Task task)
        {
            Type type = task.GetType();
            TaskNode node;
            if (type.IsSubclassOf(typeof(Composite)))
            {
                node = new CompositeNode();
            }
            else if (type.IsSubclassOf(typeof(Conditional)))
            {
                node = new ConditionalNode();
            }
            else if (type.IsSubclassOf(typeof(Decorator)))
            {
                node = new DecoratorNode();
            }
            else if (typeof(Root).IsAssignableFrom(type))
            {
                node = new RootNode();
            }
            else
            {
                node = new ActionNode();
            }

            node.Init(task, this);
            return node;
        }

        public void CopyTasks(List<Task> tasks, List<Task> newTasks)
        {
            foreach (Task task in tasks)
            {
                Task newTask = (Task) EditorBehaviorUtility.Copy(task, true);
                newTask.Guid = Guid.NewGuid().ToString();
                newTasks.Add(newTask);
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i] is ParentTask parentTask)
                {
                    for (int j = 0; j < parentTask.Children.Count; j++)
                    {
                        Task child = parentTask.Children[j];
                        ParentTask newParentTask = (ParentTask) newTasks[i];
                        int index = tasks.IndexOf(child);
                        if (index > 0)
                        {
                            newParentTask.Children.Add(newTasks[index]);
                        }
                    }
                }
            }
        }

        public void CopyVariables(List<SharedVariable> variables, List<SharedVariable> newVariables)
        {
            foreach (SharedVariable variable in variables)
            {
                newVariables.Add(variable.Clone());
            }
        }

        public void UndoRecord(string name)
        {
            Undo.RecordObject(behavior.Object, name);
            serializedVersion = ++Source.serializedVersion;
        }

        public void AddBehavior()
        {
            BehaviorTree behavior;
            if (Selection.activeGameObject)
            {
                if (!Selection.activeGameObject.TryGetComponent(out behavior))
                {
                    behavior = Undo.AddComponent<BehaviorTree>(Selection.activeGameObject);
                }
            }
            else
            {
                GameObject behaviorTree = new GameObject("BehaviorTree");
                Undo.RegisterCreatedObjectUndo(behaviorTree, "BehaviorTree Create GameObject");
                behavior = Undo.AddComponent<BehaviorTree>(behaviorTree);
                Selection.activeGameObject = behavior.gameObject;
            }

            SetBehavior(behavior);
        }

        public void ExportBehavior()
        {
            if (behavior == null)
            {
                EditorUtility.DisplayDialog("Unable to Save Behavior Tree", "Select a behavior tree from within the scene.", "Ok");
                return;
            }

            string path = EditorUtility.SaveFilePanelInProject("Save Behavior Tree", "Behavior", "asset", null);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ExternalBehavior external = CreateInstance<ExternalBehavior>();
            BehaviorSource source = new BehaviorSource();
            CopyTasks(behavior.Source.Tasks, source.Tasks);
            CopyVariables(behavior.Source.Variables, source.Variables);
            source.root = source.Tasks.Find(task => task is Root) as Root;
            external.Source = source;
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(external, path);
            AssetDatabase.Refresh();
            Selection.activeObject = external;
        }
    }
}