using EasyState.Core.Behaviors;
using EasyState.DataModels;
using EasyState.Core.Layers;
using EasyState.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Core.Models
{
    public enum ContextMenuResponse
    {
        CreateNode,
        CreateJumperNode,
        CreateGroup,
        DeleteNode,
        UngroupNode,
        UngroupGroup,
        DeleteGroup,
        CreateTransition,
        DeleteTransition,
        CreateNote,
        DeleteNote,
        CreateJumper,
        Duplicate,
    }

    public enum ContextMenuType { Background, Node, Group, Note, JumpNode, Connection }

    public enum DesignState { Idle, Panning, DraggingItem, CreatingTransition, ContextMenuOpen, DrawingRectangleSelect }

    public class Design : Model, IDataModel<DesignData>, IDataModel<DesignDataShort>
    {
        #region Events

        public delegate void ContextMenuCallback(ContextMenuType menuType, Vector2 screenPoint, SelectableModel modelSource);

        public delegate void CreateConnectionCallback(Node node);

        public delegate void StateChangedCallback(DesignState oldState, DesignState newState);
        public delegate void JumpToDesignCallback(string designID, string nodeID);

        public event ContextMenuCallback ContextMenuRequest;

        public event Action<ContextMenuResponse, Vector2, string> ContextMenuResponse;

        public event CreateConnectionCallback CreateConnection;

        public event Action<Model> DetailsPanelRequested;

        public event Action<Group> GroupAdded;

        public event Action<Group> GroupRemoved;

        public event Action<Group> GroupUngrouped;

        public event Action<object, string> Message;

        public event Action<Model> ModelDeleted;

        public event Action<Model, MouseDownEvent> MouseDown;

        public event Action<Model, MouseLeaveEvent> MouseLeft;

        public event Action<Model, MouseMoveEvent> MouseMove;

        public event Action<Model, MouseUpEvent> MouseUp;

        public event Action<Model, WheelEvent> MouseWheelMoved;

        public event Action PanChanged;

        public event Action<Model, MouseDownEvent> RightMouseDown;

        public event Action<Model, MouseUpEvent> RightMouseUp;

        public event Action<SelectableModel> SelectionChanged;

        public event Action<string, long> ShowToast;

        public event StateChangedCallback StateChanged;

        public event Action ZoomChanged;

        public JumpToDesignCallback JumpToDesign;

        public event Action ValidateDesign;

        public event Action ExportDesign;


        #endregion Events

        #region Event Triggers

        public void OnContextMenuRequested(ContextMenuType type, Vector2 screenPoint, SelectableModel model = null)
        {
            if (model != null)
            {
                SelectModel(model, false);
            }
            ContextMenuRequest?.Invoke(type, screenPoint, model);
        }

        public void OnContextMenuResponse(ContextMenuResponse reponseType, Vector2 screenPoint, string data = null) => ContextMenuResponse?.Invoke(reponseType, screenPoint, data);

        public void OnCreateConnection(Node source) => CreateConnection?.Invoke(source);

        public void OnDetailsPanelRequested(Model requestor) => DetailsPanelRequested?.Invoke(requestor);

        public void OnDebugMessage(object sender, string message) => Message?.Invoke(sender, message);

        public void OnModelDeleted(Model model) => ModelDeleted?.Invoke(model);

        public void OnMouseDown(Model model, MouseDownEvent evt) => MouseDown?.Invoke(model, evt);

        public void OnMouseLeft(Model model, MouseLeaveEvent evt) => MouseLeft?.Invoke(model, evt);

        public void OnMouseMove(Model model, MouseMoveEvent evt)
        {
            MousePosition = evt.mousePosition;
            MouseMove?.Invoke(model, evt);
        }      
        public void OnMouseUp(Model model, MouseUpEvent evt) => MouseUp?.Invoke(model, evt);

        public void OnMouseWheelMoved(Model model, WheelEvent evt) => MouseWheelMoved?.Invoke(model, evt);

        public void OnRightMouseDown(Model model, MouseDownEvent evt) => RightMouseDown?.Invoke(model, evt);

        public void OnRightMouseUp(Model model, MouseUpEvent evt) => RightMouseUp?.Invoke(model, evt);

        public void OnShowToast(string message, long duration) => ShowToast?.Invoke(message, duration);
        public void OnJumpToDesign(string designID, string nodeID) => JumpToDesign?.Invoke(designID, nodeID);
        public void OnValidateDesign()=> ValidateDesign?.Invoke();
        public void OnExportDesign()=> ExportDesign?.Invoke();

        #endregion Event Triggers

        public DataTypeModel DataType { get; }
        public string DataTypeID => DataType.Id;
        public IReadOnlyList<Group> Groups => _groups;
        public Vector2 MousePosition { get; private set; }
        public NodeLayer Nodes { get; }
        public BaseLayer<Note> Notes { get; }
        public Vector2 Pan { get; private set; }
        public EasyStateSettings Settings { get; }
        public bool IsVisible { get; set; }
        public DesignState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    var oldValue = _state;
                    _state = value;
                    StateChanged?.Invoke(oldValue, _state);
                    Refresh();
                }
            }
        }

        public bool SuspendRefresh { get; set; }
        public Vector2 ToolbarOffset => new Vector2(22, 69)/ Zoom;
        public float Zoom { get; private set; } = 1;
        private List<Group> _groups = new List<Group>();
        private DesignState _state;

        public Design(EasyStateSettings settings, DataTypeModel dataType, string id = null) : base(id)
        {
            DataType = dataType;
            Settings = settings;
            Nodes = new NodeLayer(this);
            Notes = new BaseLayer<Note>(this);
            AddDefaultBehaviors();
        }

        public Design(EasyStateSettings settings, DesignData designModel, DataTypeModel dataType) : this(settings, dataType , designModel.Id)
        {
            Name = designModel.Name;
            Pan = designModel.Pan;
            Zoom = designModel.Zoom;
            Name = designModel.Name;
            if (Zoom == 0)
            {
                Zoom = 1;
            }
        }

        public void Batch(Action batchAction)
        {
            if (SuspendRefresh)
            {
                batchAction();
                return;
            }
            SuspendRefresh = true;
            batchAction();
            SuspendRefresh = false;
        }

        public Vector2 GetAbsolutePosition(Vector2 relativePosition)
        {
            return (relativePosition * Zoom) + Pan;
        }

        public Vector2 GetRelativePosition(Vector2 position)
        {           
            return ((position - Pan) / Zoom);
        }

        public override void Refresh()
        {
            if (SuspendRefresh)
                return;
            base.Refresh();

            foreach (var nodes in Nodes)
            {
                nodes.Refresh();
                nodes.RefreshConnections();
            }
        }

        public DesignData Serialize()
        {
            var data = new DesignData() { 
                Nodes = Nodes.Select(x=> x.Serialize()).ToList(),
                Groups = Groups.Select(x=> x.Serialize()).ToList(),
                Notes = Notes.Select(x=> x.Serialize()).ToList(),
                Pan = Pan,
                Zoom = Zoom,
                DataTypeID = DataTypeID
            
            };
            data.SetModelData(this);
            return data;
        }

        public void UpdatePan(Vector2 delta)
        {
            Pan += delta;
            PanChanged?.Invoke();
        }

        public void UpdateZoom(float newZoom)
        {
            Zoom = newZoom;
            ZoomChanged?.Invoke();
        }

        #region Groups

        public void AddGroup(Group group)
        {
            if (!_groups.Contains(group))
            {
                _groups.Add(group);
                GroupAdded?.Invoke(group);
            }
        }

        public void RemoveGroup(Group group)
        {
            if (_groups.Contains(group))
            {
                Batch(() =>
                {
                    foreach (var child in group.Children)
                    {
                        child.OnDelete(this);
                    }
                    _groups.Remove(group);
                });
                GroupRemoved?.Invoke(group);
                OnModelDeleted(group);
            }
        }

        public void UngroupGroup(Group group)
        {
            if (_groups.Contains(group))
            {
                foreach (var child in group.Children)
                {
                    child.Group = null;
                }
                _groups.Remove(group);
                GroupUngrouped?.Invoke(group);
            }
        }

        #endregion Groups

        #region Selection

        public IEnumerable<SelectableModel> GetSelectedModels()
        {
            foreach (var node in Nodes)
            {
                if (node.Selected)
                    yield return node;
            }

            foreach (var conn in Nodes.SelectMany(x => x.Connections))
            {
                if (conn.Selected)
                    yield return conn;
            }
            foreach (var group in _groups)
            {
                if (group.Selected)
                    yield return group;
            }
            foreach (var note in Notes)
            {
                if(note.Selected)
                yield return note;
            }
        }

        public IEnumerable<Node> GetSelectedNodes()
        {
            foreach (var node in Nodes)
            {
                if (node.Selected)
                {
                    yield return node;
                }
            }
        }

        public void SelectModel(SelectableModel model, bool unselectOthers = true)
        {
            if (model.Selected)
                return;
            if (unselectOthers)
                UnselectAll();

            model.Selected = true;
            model.Refresh();
            SelectionChanged?.Invoke(model);
        }

        public void UnselectAll()
        {
            foreach (var model in GetSelectedModels())
            {
                UnselectModel(model);
            }
        }

        public void UnselectModel(SelectableModel model)
        {
            if (!model.Selected)
                return;

            model.Selected = false;
            model.Refresh();
            SelectionChanged?.Invoke(model);
        }

        #endregion Selection

        #region Behaviors

        private readonly Dictionary<Type, Behavior> _behaviors = new Dictionary<Type, Behavior>();

        private void AddDefaultBehaviors()
        {
            UnRegisterAllBehaviors();
            RegisterBehavior(new PanBehavior(this));
            RegisterBehavior(new ZoomBehavior(this));
            RegisterBehavior(new SelectableBehavior(this));
            RegisterBehavior(new DebugEventsBehavior(this));
            RegisterBehavior(new DragMoveableBehavior(this));
            RegisterBehavior(new ConnectionMenuResponseBehavior(this));
            RegisterBehavior(new ContextMenuBehavior(this));
            RegisterBehavior(new NodeMenuResponseBehavior(this));
            RegisterBehavior(new RectangleSelectBehavior(this));
            RegisterBehavior(new GroupMenuResponseBehavior(this));
            RegisterBehavior(new NoteMenuResponseBehavior(this));
            RegisterBehavior(new DuplicationBehavior(this));
        }
        public void AddDebugBehaviors()
        {
            UnRegisterAllBehaviors();
            RegisterBehavior(new PanBehavior(this));
            RegisterBehavior(new ZoomBehavior(this));
        }
        private void UnRegisterAllBehaviors()
        {
            foreach (var behavior in _behaviors.Values)
            {
                behavior.Dispose();
            }
            _behaviors.Clear();
        }
        private void RegisterBehavior(Behavior behavior)
        {
            var type = behavior.GetType();
            if (_behaviors.ContainsKey(type))
                throw new Exception($"Behavior '{type.Name}' already registered");

            _behaviors.Add(type, behavior);
        }


        #endregion Behaviors
        DesignDataShort IDataModel<DesignDataShort>.Serialize()
        {     
            var data = new DesignDataShort() { 
                ConnectionCount = Nodes.Sum(x => x.Connections.Count),
                Nodes = Nodes.Select(x => new NodeDataShort(x.Id,x.Name)).ToList(),
                DataTypeName = DataType.Name,
                DataTypeID = DataTypeID,

            };
            data.SetModelData(this);           
            
            return data;
        }
    }
}