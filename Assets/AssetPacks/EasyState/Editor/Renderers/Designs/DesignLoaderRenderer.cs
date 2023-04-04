using EasyState.DataModels;
using EasyState.Core.Models;
using EasyState.Data;
using EasyState.Editor.Templates;
using EasyState.Editor.Utility;
using EasyState.Core.Validators;
using EasyState.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace EasyState.Editor.Renderers.Designs
{
    public class DesignLoaderRenderer : IDisposable
    {
        private DataTypeModel _allDataTypes = new DataTypeModel("-1") { Name = "All Data Types" };
        private DesignDataShort _nullDesign = new DesignDataShort { Id = "-1", Name = "None" };
        private List<DesignDataShort> _allDesigns;
        private List<DesignDataShort> _sourceDesignList;
        private Label _createButton;
        private PopupField<DataTypeModel> _dataTypeFilterPopup;
        private PopupField<DesignDataShort> _sourceDesignPopup;
        private VisualElement _designList;
        private VisualElement _designLoader;
        private TextField _designNameInput;
        private List<DesignDataShort> _filteredDesigns;
        private PopupField<DataTypeModel> _newDesignDataTypePopup;
        private Label _newDesignErrorLabel;
        private TabContainer _tabContainer;
        private readonly List<DataTypeModel> _dataTypes;
        private VisualElement _contentContainer;
        private BehaviorCollectionData _behaviors;
        private string _searchTerm;
        public DesignLoaderRenderer(VisualElement root, TabContainer tabContainer)
        {
            _dataTypes = new DataTypeDatabase().Load().DataTypes;
            _designLoader = TemplateFactory.CreateDesignLoaderTemplate();
            _contentContainer = _designLoader.Q<VisualElement>("content");
            _behaviors = new BehaviorCollectionDatabase().Load();
            root.Add(_designLoader);
            _tabContainer = tabContainer;
            tabContainer.AddDesignRequest += Show;
            var cancelButton = _designLoader.Q<Label>("cancel");
            cancelButton.AddManipulator(new Clickable(Hide));
            var closeButton = _designLoader.Q<VisualElement>("close-btn");
            closeButton.AddManipulator(new Clickable(Hide));
            _allDesigns = new DesignCollectionDatabase().Load().Designs;
            _filteredDesigns = new List<DesignDataShort>(_allDesigns);
            var scrollView = _designLoader.Q<ScrollView>("designs");
            scrollView.style.paddingRight = 17;
            _designList = scrollView.Q<VisualElement>("body");
            _createButton = _designLoader.Q<Label>("create-design-button");
            _newDesignErrorLabel = _designLoader.Q<Label>("new-design-error");
            _createButton.AddManipulator(new Clickable(OnCreateDesign));
            _designNameInput = _designLoader.Q<TextField>("new-design-name");
            UpdateDesignList();
            var dtContainer = _designLoader.Q<VisualElement>("data-type-filter");
            var searchLabel = new Label("Search");
            var search = new TextField(25, false, false, '*');
            search.style.minWidth = 135;
            searchLabel.style.alignSelf = Align.Center;
            search.RegisterValueChangedCallback(x =>
            {
                _searchTerm = x.newValue;
                OnDataTypeFilterChanged(null);

            });
            dtContainer.parent.Insert(0, searchLabel);
            dtContainer.parent.Insert(1, search);
            var newDesignSource = _designLoader.Q<VisualElement>("source-design");
            _sourceDesignList = new List<DesignDataShort> { _nullDesign };
            _sourceDesignPopup = new PopupField<DesignDataShort>("Source Design", _sourceDesignList, _nullDesign, formatListItemCallback: x => x.Name, formatSelectedValueCallback: x => x.Name);
            newDesignSource.Add(_sourceDesignPopup);
            LoadSourceDesignPopup(_dataTypes.FirstOrDefault());
            BuildDataTypeFilters();
            UpdateCreateButtonStatus();
            _designNameInput.RegisterValueChangedCallback((x) => UpdateCreateButtonStatus());
            _tabContainer.DesignClosed += TabContainer_DesignClosed;

            Hide();

        }

        private void TabContainer_DesignClosed(Design obj)
        {
            if (_tabContainer.DesignTabs.Count == 0)
            {
                Show();
            }
        }

        public void Dispose()
        {
            _tabContainer.AddDesignRequest += Show;
            _tabContainer.DesignClosed -= TabContainer_DesignClosed;
        }

        public void OnCreateDesign()
        {
            _newDesignErrorLabel.text = "";
            var newDesignModel = new DesignData(_newDesignDataTypePopup.value.Id)
            {
                Name = _designNameInput.value,
            };
            var result = Validator.Validate(newDesignModel, _allDesigns);
            if (result)
            {
                Design newDesign;
                if (_sourceDesignPopup.value == _nullDesign)
                {
                    newDesign = BuildNewDesign(newDesignModel);
                   
                }
                else
                {
                    newDesign = CopyDesignFromSource(newDesignModel);
                }

                _tabContainer.OnDesignAdded(newDesign);
                _designNameInput.value = "";
                _allDesigns.Add(((IDataModel<DesignDataShort>)newDesign).Serialize());
                OnDataTypeFilterChanged(null);
                Hide();
                newDesign.OnShowToast($"{newDesignModel.Name} created.", 1500);
            }
            else
            {
                _newDesignErrorLabel.text = result.GetErrorMessage();

            }
        }

        private Design CopyDesignFromSource(DesignData newDesignModel)
        {
            var sourceDesignData = new DesignDatabase(_sourceDesignPopup.value.Id).Load();
            sourceDesignData.Id = newDesignModel.Id;
            sourceDesignData.Name = newDesignModel.Name;
            sourceDesignData.DataTypeID = newDesignModel.DataTypeID;
            sourceDesignData = DesignExetensions.RandomizeStateIDs(sourceDesignData);
            var newDesignDB = new DesignDatabase(sourceDesignData.Id);
            newDesignDB.Save(sourceDesignData);
            Design newDesign = DesignLoader.Load(sourceDesignData);
            return newDesign;
        }

        private Design BuildNewDesign(DesignData newDesignModel)
        {
            var settings = EasyStateSettings.Instance;
            var newDesign = new Design(settings, newDesignModel, _newDesignDataTypePopup.value);
            var entryNode = settings.NodePresetCollection.BuildEntryNode();

            var xOffset = _designLoader.worldBound.width / 4f;
            var entryPosition = _designLoader.worldBound.center - (new Vector2(171, 50) / 2) - new Vector2(xOffset, 0);

            if (settings.SnapToGrid)
            {
                var step = settings.SnapToGridStep;
                int stepsX = Mathf.RoundToInt(entryPosition.x / step);
                int stepsY = Mathf.RoundToInt(entryPosition.y / step);
                entryPosition = new Vector2(step * stepsX, step * stepsY);
            }

            entryNode.SetPositionSilently(entryPosition);
            newDesign.Nodes.Add(entryNode);
            var db = new DesignDatabase(newDesign.Id);
            db.Save(newDesign.Serialize());
            return newDesign;
        }

        private void BuildDataTypeFilters()
        {
            if (!_dataTypes.Any())
            {
                _contentContainer.Clear();
                _contentContainer.Add(new Label("There are no data types in the database. Create a data type by the Assests menu in the toolbar or right clicking in the project window Easy State/New Data Type"));
            }

            var dtContainer = _designLoader.Q<VisualElement>("data-type-filter");
            var newDesignDt = _designLoader.Q<VisualElement>("new-design-data-type");


            dtContainer.Clear();
            newDesignDt.Clear();

            var options = new List<DataTypeModel>(_dataTypes);
            options.Insert(0, _allDataTypes);
            if (_dataTypeFilterPopup != null)
            {
                var selectedValue = _dataTypeFilterPopup.value;
                _dataTypeFilterPopup.UnregisterValueChangedCallback(OnDataTypeFilterChanged);
                _dataTypeFilterPopup = new PopupField<DataTypeModel>("Filter Designs by Data Type", options, selectedValue, formatSelectedValueCallback: x => x.Name, formatListItemCallback: x => x.Name);
                _dataTypeFilterPopup.RegisterValueChangedCallback(OnDataTypeFilterChanged);
            }
            else
            {
                _dataTypeFilterPopup = new PopupField<DataTypeModel>("Filter Designs by Data Type", options, 0, formatSelectedValueCallback: x => x.Name, formatListItemCallback: x => x.Name);
                _dataTypeFilterPopup.RegisterValueChangedCallback(OnDataTypeFilterChanged);
            }
            dtContainer.Add(_dataTypeFilterPopup);


            if (_newDesignDataTypePopup != null)
            {
                var selectedValue = _newDesignDataTypePopup.value;
                _newDesignDataTypePopup = new PopupField<DataTypeModel>("Data Type", _dataTypes, 0, formatSelectedValueCallback: x => x.Name, formatListItemCallback: x => x.Name);
            }
            else
            {
                _newDesignDataTypePopup = new PopupField<DataTypeModel>("Data Type", _dataTypes, 0, formatSelectedValueCallback: x => x.Name, formatListItemCallback: x => x.Name);
            }
            _newDesignDataTypePopup.RegisterValueChangedCallback(x =>
            {
                LoadSourceDesignPopup(x.newValue);

            });
            newDesignDt.Add(_newDesignDataTypePopup);

        }

        private void LoadSourceDesignPopup(DataTypeModel newDataType)
        {
            _sourceDesignList.Clear();
            _sourceDesignList.Add(_nullDesign);
            _sourceDesignPopup.SetValueWithoutNotify(_nullDesign);
            if (newDataType == null)
            {
                return;
            }

            string currentDataTypeID = newDataType.Id;
            while (!string.IsNullOrEmpty(currentDataTypeID))
            {
                var dataType = _dataTypes.First(y => y.Id == currentDataTypeID);
                _sourceDesignList.AddRange(_allDesigns.Where(y => y.DataTypeID == currentDataTypeID));
                currentDataTypeID = dataType.ParentDataTypeID;
            }
        }

        private void Hide()
        {
            _designLoader.style.display = DisplayStyle.None;
            _tabContainer.OnAddDesignRequestCompleted();
        }

        private void OnDataTypeFilterChanged(ChangeEvent<DataTypeModel> evt)
        {
            if (_dataTypeFilterPopup.value == _allDataTypes || evt is null)
            {
                _filteredDesigns = new List<DesignDataShort>(_allDesigns);
            }
            else
            {
                _filteredDesigns = _allDesigns.Where(x => x.DataTypeID == evt.newValue.Id).ToList();
            }
            if (!string.IsNullOrEmpty(_searchTerm))
            {
                _filteredDesigns = _filteredDesigns.Where(x => x.Name.StartsWith(_searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            UpdateDesignList();
        }

        private void OnDeleteButtonClicked(DesignGridRowRenderer gridRow)
        {
            string message = "Are you sure you want to delete this design?";
            bool hasAssociatedBehavior = _behaviors.Behaviors.FirstOrDefault(x => x.DesignId == gridRow.Design.Id) != null;
            if (hasAssociatedBehavior)
            {
                message += " This design has an associated behavior which will be deleted as well when the design is deleted.";
            }
            if (!EditorUtility.DisplayDialog("Delete Design?", message, "Yes", "No"))
            {
                return;
            }
            var db = new DesignDatabase(gridRow.Design.Id);
            db.DeleteFile();
            _allDesigns.Remove(gridRow.Design);
            _tabContainer.OnDesignDeleted(gridRow.Design);
            gridRow.Element.RemoveFromHierarchy();
            //OnDataTypeFilterChanged(null);

        }


        private void OnLoadButtonClicked(DesignGridRowRenderer gridRow)
        {
            var openTab = _tabContainer.DesignTabs.FirstOrDefault(x => x.Design.Id == gridRow.Design.Id);
            if (openTab != null)
            {
                _tabContainer.OnTabClicked(openTab);
            }
            else
            {
                var db = new DesignDatabase(gridRow.Design.Id);
                var loadedDesign = db.Load();
                var design = DesignLoader.Load(loadedDesign);
                _tabContainer.OnDesignAdded(design);
                design.OnShowToast($"{design.Name} loaded.", 1500);
            }
            Hide();
        }

        public void Show()
        {
            _behaviors = new BehaviorCollectionDatabase().Load();
            _designLoader.style.display = DisplayStyle.Flex;
        }

        private void UpdateCreateButtonStatus()
        {
            bool isValid = !string.IsNullOrWhiteSpace(_designNameInput.value) && _newDesignDataTypePopup != null;

            if (isValid)
            {
                _createButton.style.opacity = 1;
                _createButton.pickingMode = PickingMode.Position;
            }
            else
            {
                _createButton.style.opacity = .3f;
                _createButton.pickingMode = PickingMode.Ignore;
            }
        }

        private void UpdateDesignList()
        {
            _designList.Clear();
            if (_filteredDesigns.Any())
            {
                foreach (var design in _filteredDesigns.OrderBy(x => x.Name))
                {
                    bool hasBehavior = _behaviors?.Behaviors.FirstOrDefault(x => x.DesignId == design.Id) != null;
                    var view = new DesignGridRowRenderer(design, _designList, OnDeleteButtonClicked, OnLoadButtonClicked, hasBehavior);
                    //var designLabel = new Label(design.Name);
                    //designLabel.AddToClassList("design-loader-tab");
                    //designLabel.AddManipulator(new Clickable(() => OnDesignSelected(designLabel, design.Id)));
                    //_designList.Add(designLabel);
                }
            }
            else
            {
                _designList.Add(new Label("No designs found."));
            }
        }
    }
}