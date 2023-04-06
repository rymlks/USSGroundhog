using EasyState.DataModels;
using EasyState.Core.Models;
using EasyState.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace EasyState.Data
{
    public static class DesignLoader
    {
        public static Design Load(string designID)
        {
            var design = new DesignDatabase(designID).Load();
            return Load(design);
        }
        public static Design Load(DesignData designModel)
        {
            var options = EasyStateSettings.Instance;
            var dataTypes = new DataTypeDatabase().Load();
            if (dataTypes.DataTypes.Count == 0)
            {
                Debug.LogError("Trying to load a data type when no designs have been saved");
                return null;
            }
            var dataType = dataTypes.DataTypes.FirstOrDefault(x => x.Id == designModel.DataTypeID);
            if (dataType is null)
            {
                throw new InvalidOperationException("Could not find this data type");
            }

            var design = new Design(options, designModel, dataType);
            var designCollection = new DesignCollectionDatabase().Load();


            design.SuspendRefresh = true;
            LoadNodes(designModel, design, dataType);
            LoadNotes(designModel, design);
            LoadGroups(designModel, design);

            design.SuspendRefresh = false;

            return design;
        }

        private static ConditionalExpression LoadExpression(ConditionalExpressionData expressionData, List<FunctionModel> conditions)
        {
            var loadedExpression = new ConditionalExpression(expressionData);
            loadedExpression.Condition = conditions.FirstOrDefault(x => x.Id == expressionData.ConditionID);
            return loadedExpression;
        }

        private static ConditionalExpressionRow LoadExpressionRow(ConditionalExpressionRowData row, List<FunctionModel> conditions)
        {
            var loadedRow = new ConditionalExpressionRow(row);

            if (row.InitialExpressionData != null)
            {
                loadedRow.InitialExpression = LoadExpression(row.InitialExpressionData, conditions);
            }
            if (row.AdditionalExpressions.Any())
            {
                loadedRow.AdditionalExpressions = row.AdditionalExpressions.Select(x => LoadExpression(x, conditions)).ToList();
            }
            return loadedRow;
        }

        private static ConditionalExpressionSet LoadExpressionSet(ConditionalExpressionSetData data, List<FunctionModel> conditions)
        {
            var loadedSet = new ConditionalExpressionSet(data);

            if (data.InitialConditionalRow != null)
            {
                loadedSet.InitialConditionalRow = LoadExpressionRow(data.InitialConditionalRow, conditions);
            }
            if (data.AdditionalRows.Any())
            {
                loadedSet.AdditionalRows = data.AdditionalRows.Select(x => LoadExpressionRow(x, conditions)).ToList();
            }
            return loadedSet;
        }

        private static void LoadGroups(DesignData designModel, Design design)
        {
            if (designModel.Groups.Any())
            {
                foreach (var group in designModel.Groups)
                {
                    var groupables = new List<IGroupable>();
                    var nodes = design.Nodes.Where(x => group.ChildrenIDs.Contains(x.Id));
                    groupables.AddRange(nodes);
                    var notes = design.Notes.Where(x => group.ChildrenIDs.Contains(x.Id));
                    groupables.AddRange(notes);
                    var loadedGroup = new Group(group, groupables);

                    design.AddGroup(loadedGroup);
                }
            }
        }

        private static void LoadNodeActions(List<FunctionModel> functions, NodeData node, Node loadedNode)
        {
            foreach (var action in node.NodeActions)
            {
                var loadedAction = loadedNode.Actions.First(x => x.Id == action.Id);
                if (!string.IsNullOrEmpty(action.ActionID))
                {
                    loadedAction.Action = functions.FirstOrDefault(x => x.Id == action.ActionID);
                }
                if (!string.IsNullOrEmpty(action.ConditionID))
                {
                    loadedAction.Condition = functions.FirstOrDefault(x => x.Id == action.ConditionID);
                }

            }
        }

        private static void LoadNodeConnections(Design design, List<FunctionModel> functions, ConnectionData conn)
        {
            var sourceNode = design.Nodes.First(x => x.Id == conn.SourceNodeID);
            Node destNode = null;
            if (conn.DestNodeID != null)
            {
                destNode = design.Nodes.First(x => x.Id == conn.DestNodeID);
            }
            ConditionalExpressionSet expSet = null;
            if (conn.ConditionalExpression != null)
            {
                expSet = LoadExpressionSet(conn.ConditionalExpression, functions);
            }
            var evaluator = functions.FirstOrDefault(x => x.Id == conn.EvaluatorID);
            var loadedConnection = new Connection(conn, expSet, sourceNode, destNode, evaluator);

            sourceNode.AddConnection(loadedConnection, true);
        }

        private static void LoadNodes(DesignData designModel, Design design, DataTypeModel dataType )
        {
            if (designModel.Nodes.Any())
            {
                var designCollection = new DesignCollectionDatabase().Load();
                
                foreach (var node in designModel.Nodes)
                {
                    LoadNode(design, designCollection, dataType.Functions, node);
                }
                foreach (var node in designModel.Nodes)
                {
                    foreach (var conn in node.Connections)
                    {
                        LoadNodeConnections(design, dataType.Functions, conn);
                    }
                }
            }
        }

        private static void LoadNode(Design design, DesignDatabaseData designCollection, List<FunctionModel> functions, NodeData node)
        {
            var loadedNode = new Node(node);
            design.Nodes.Add(loadedNode);

            LoadNodeActions(functions, node, loadedNode);
            //if node is a jump node
            if (node.IsJumpNode && node.JumpDesignID != null)
            {
                LoadJumpNode(designCollection, node, loadedNode);

            }
        }

        private static void LoadJumpNode(DesignDatabaseData designs, NodeData node, Node loadedNode)
        {
            var designData = designs.Designs.FirstOrDefault(x => x.Id == node.JumpDesignID);
            //design still exists
            if (designData != null)
            {
                loadedNode.JumpDesign = designData;
                var jumpNode = designData.Nodes.FirstOrDefault(x => x.Id == node.JumpNodeID);
                // node still exists
                if (jumpNode != null)
                {
                    loadedNode.JumpNode = jumpNode;
                }
                //node was deleted
                else
                {
                    var entryNode = designData.Nodes.FirstOrDefault(x => x.Name == NodePresetCollection.ENTRY_NODE);
                    loadedNode.JumpNode = entryNode;
                }
            }
            //design was deleted
            else
            {
                loadedNode.JumpDesign = null;
                loadedNode.JumpNode = null;
            }
        }

        private static void LoadNotes(DesignData designModel, Design design)
        {
            if (designModel.Notes.Any())
            {
                foreach (var note in designModel.Notes)
                {
                    var loadedNote = new Note(note);
                    design.Notes.Add(loadedNote);
                }
            }
        }
    }
}