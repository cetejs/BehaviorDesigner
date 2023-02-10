using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorDesigner.Editor
{
    public class TaskPort : Port
    {
        public Action<Edge, bool> onConnected;
        public Action<Edge, bool> onDisconnected;

        public static TaskPort Create<TEdge>(
            Direction direction,
            Capacity capacity,
            Type type)
            where TEdge : Edge, new()
        {
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            TaskPort ele = new TaskPort(direction, capacity, type)
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(listener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }

        protected TaskPort(Direction portDirection, Capacity portCapacity, Type type) : base(Orientation.Vertical, portDirection, portCapacity, type)
        {
        }

        public override void Connect(Edge edge)
        {
            Connect(edge, false);
        }

        public override void Disconnect(Edge edge)
        {
            Disconnect(edge, false);
        }

        public void Connect(Edge edge, bool isManual)
        {
            base.Connect(edge);
            onConnected?.Invoke(edge, isManual);
        }

        public void Disconnect(Edge edge, bool isManual)
        {
            base.Disconnect(edge);
            onDisconnected?.Invoke(edge, isManual);
        }

        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private GraphViewChange graphViewChange;
            private List<Edge> edgesToCreate;
            private List<GraphElement> edgesToDelete;

            public DefaultEdgeConnectorListener()
            {
                edgesToCreate = new List<Edge>();
                edgesToDelete = new List<GraphElement>();
                graphViewChange.edgesToCreate = edgesToCreate;
            }

            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
            }

            public void OnDrop(GraphView graphView, Edge edge)
            {
                edgesToCreate.Clear();
                edgesToCreate.Add(edge);
                edgesToDelete.Clear();
                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.input.connections)
                    {
                        if (connection != edge)
                            edgesToDelete.Add(connection);
                    }
                }

                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.output.connections)
                    {
                        if (connection != edge)
                            edgesToDelete.Add(connection);
                    }
                }

                if (edgesToDelete.Count > 0)
                {
                    graphView.DeleteElements(edgesToDelete);
                }

                if (graphView.graphViewChanged != null)
                {
                    edgesToCreate = graphView.graphViewChanged(graphViewChange).edgesToCreate;
                }

                foreach (Edge edge1 in edgesToCreate)
                {
                    graphView.AddElement(edge1);
                    edge.input.Connect(edge1);
                    edge.output.Connect(edge1);
                }
            }
        }
    }
}