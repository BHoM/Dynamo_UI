/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using BH.oM.Data.Collections;
using Dynamo.Wpf;
using Dynamo.Controls;
using BH.UI.Dynamo.Templates;
using System.Windows.Controls;
using System.Linq;
using BH.Engine.Data;
using BH.UI.Base;
using Dynamo.Engine;
using BH.oM.UI;
using Dynamo.Graph.Nodes;
using BH.Engine.Dynamo;
using Dynamo.Graph.Connectors;

namespace BH.UI.Dynamo.Templates
{
    public abstract partial class CallerComponent : NodeModel
    {
        /*******************************************/
        /**** Event Methods                     ****/
        /*******************************************/

        protected virtual void OnCallerModified(object sender, CallerUpdate update)
        {
            if (update == null)
                return;

            // Update the component details
            UpdateComponentDetails(update.ComponentUpdate);

            // Update the inputs
            update.InputUpdates.ForEach(x => UpdateInput(x as dynamic));

            // Update the outputs
            update.OutputUpdates.ForEach(x => UpdateOutput(x as dynamic));

            // Let the NodeView know that this was updated
            ModifiedByCaller?.Invoke(this, update);

            // Dynamo is not really capable of dealing with anything else that adding an input at the end of the list so need to fix it ourselves
            FixInputNodes();

            // Ask component to refresh 
            OnNodeModified(true);
        }

        /*******************************************/
        /**** Component Update Methods          ****/
        /*******************************************/

        protected virtual void UpdateComponentDetails(ComponentUpdate update)
        {
            if (update != null)
            {
                Name = update.Name;
                Description = update.Description;
            }
        }


        /*******************************************/
        /**** Input Update Methods              ****/
        /*******************************************/

        protected virtual void UpdateInput(ParamAdded update)
        {
            PortModel newPort = new PortModel(PortType.Input, this, update.Param.ToPortData());

            // If there is already a param with the same name, delete it but keep the wire connections
            // Same approach as `UpdateInput(ParamUpdated update)` but with index provided by ParamAdded
            PortModel match = InPorts.Where(x => x.Name == update.Name).FirstOrDefault();
            if (match != null)
            {
                MoveLinks(match, newPort, true);
                InPorts.Remove(match);
            }

            InPorts.Insert(update.Index, newPort);
        }

        /*******************************************/

        protected virtual void UpdateInput(ParamRemoved update)
        {
            PortModel match = InPorts.Where(x => x.Name == update.Name).FirstOrDefault();
            if (match != null)
                InPorts.Remove(match);
        }

        /*******************************************/

        protected virtual void UpdateInput(ParamUpdated update)
        {
            List<PortModel> ports = InPorts.ToList();
            int index = ports.FindIndex(x => x.Name == update.Name);
            if (index >= 0)
            {
                PortModel oldPort = ports[index];
                PortModel newPort = new PortModel(PortType.Input, this, update.Param.ToPortData());

                MoveLinks(oldPort, newPort, true);
                InPorts.Remove(oldPort);
                InPorts.Insert(index, newPort);
            }
        }

        /*******************************************/

        protected virtual void UpdateInput(ParamMoved update)
        {
            PortModel match = InPorts.Where(x => x.Name == update.Name).FirstOrDefault();
            if (match != null)
            {
                InPorts.Remove(match);
                InPorts.Insert(update.Index, match);
            }
        }

        /*******************************************/

        protected virtual void UpdateInput(IParamUpdate update)
        {
            // Do nothing
        }


        /*******************************************/
        /**** Output Update Methods             ****/
        /*******************************************/

        protected virtual void UpdateOutput(ParamAdded update)
        {
            PortModel newPort = new PortModel(PortType.Output, this, update.Param.ToPortData());

            // If there is already a param with the same name, delete it but keep the wire connections
            // Same approach as `UpdateInput(ParamUpdated update)` but with index provided by ParamAdded
            PortModel match = OutPorts.Where(x => x.Name == update.Name).FirstOrDefault();
            if (match != null)
            {
                MoveLinks(match, newPort, false);
                OutPorts.Remove(match);
            }

            OutPorts.Insert(update.Index, newPort);
        }

        /*******************************************/

        protected virtual void UpdateOutput(ParamRemoved update)
        {
            PortModel match = OutPorts.Where(x => x.Name == update.Name).FirstOrDefault();
            if (match != null)
                OutPorts.Remove(match);
        }

        /*******************************************/

        protected virtual void UpdateOutput(ParamUpdated update)
        {
            List<PortModel> ports = OutPorts.ToList();
            int index = ports.FindIndex(x => x.Name == update.Name);
            if (index >= 0)
            {
                PortModel oldPort = ports[index];
                PortModel newPort = new PortModel(PortType.Output, this, update.Param.ToPortData());

                MoveLinks(oldPort, newPort, false);
                OutPorts.Remove(oldPort);
                OutPorts.Insert(index, newPort);
            }
        }

        /*******************************************/

        protected virtual void UpdateOutput(ParamMoved update)
        {
            PortModel match = OutPorts.Where(x => x.Name == update.Name).FirstOrDefault();
            if (match != null)
            {
                OutPorts.Remove(match);
                OutPorts.Insert(update.Index, match);
            }
        }

        /*******************************************/

        protected virtual void UpdateOutput(IParamUpdate update)
        {
            // Do nothing
        }


        /*******************************************/
        /**** Helper Methods                    ****/
        /*******************************************/

        protected virtual void MoveLinks(PortModel oldPort, PortModel newPort, bool isInput)
        {
            foreach (ConnectorModel connector in oldPort.Connectors)
            {
                // Add the new connector
                ConnectorModel newConnector;
                if (isInput)
                    new ConnectorModel(connector.Start, newPort, Guid.NewGuid());
                else
                    new ConnectorModel(newPort, connector.End, connector.GUID);

                // Remove the old connector
                if (connector.Start != null && connector.Start.Connectors.Contains(connector))
                    connector.Start.Connectors.Remove(connector);
                if (connector.End != null && connector.End.Connectors.Contains(connector))
                    connector.End.Connectors.Remove(connector);
            }
        }

        /*******************************************/

        protected virtual void FixInputNodes()
        {
            for (int i = 0; i < InPorts.Count; i++)
            {
                if (InPorts[i].Connectors.Count > 0)
                {
                    PortModel source = InPorts[i].Connectors[0].Start;
                    InputNodes[i] = new Tuple<int, NodeModel>(source.Index, source.Owner);
                }
                else
                    InputNodes[i] = null;
            }
        }

        /*******************************************/
    }
}





