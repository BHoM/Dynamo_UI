/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.UI;
using BH.UI.Base;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using BH.Engine.Dynamo;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Dynamo.Objects;
using BH.Engine.UI;
using System.Reflection;
using System.Collections;
using BH.UI.Dynamo.Global;
using System.Xml;
using Dynamo.Graph;
using Dynamo.Graph.Connectors;
using Newtonsoft.Json;

namespace BH.UI.Dynamo.Templates
{
    public abstract partial class CallerComponent : NodeModel
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void Initialise()
        {
            Category = "BHoM." + Caller.Category;
            ArgumentLacing = LacingStrategy.Auto;

            string instanceId = InstanceID.ToString();
            DataAccessor_Dynamo dataAccessor = new DataAccessor_Dynamo();
            Caller.SetDataAccessor(dataAccessor);
            BH.Engine.Dynamo.Compute.Callers[instanceId] = Caller;
            BH.Engine.Dynamo.Compute.DataAccessors[instanceId] = dataAccessor;
            BH.Engine.Dynamo.Compute.Nodes[instanceId] = this;

            Caller.Modified += (sender, e) => RefreshComponent();
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected void RegisterInputs()
        {
            if (Caller == null)
                return;

            List<ParamInfo> inputs = Caller.InputParams;
            int nbNew = inputs.Count;
            int nbOld = InPorts.Count;

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                ReplacePort(InPorts[i], inputs[i].ToPortData());

            for (int i = nbOld - 1; i >= nbNew; i--)
                InPorts.RemoveAt(i);

            for (int i = nbOld; i < nbNew; i++)
                InPorts.Add(new PortModel(PortType.Input, this, inputs[i].ToPortData()));

            RaisesModificationEvents = true;
            OnNodeModified();
        }

        /*******************************************/

        protected void RegisterOutputs()
        {
            if (Caller == null)
                return;

            List<ParamInfo> outputs = Caller.OutputParams;
            int nbNew = outputs.Count;
            int nbOld = OutPorts.Count;

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                ReplacePort(OutPorts[i], outputs[i].ToPortData());

            for (int i = nbOld - 1; i >= nbNew; i--)
                OutPorts.RemoveAt(i);

            for (int i = nbOld; i < nbNew; i++)
                OutPorts.Add(new PortModel(PortType.Output, this, outputs[i].ToPortData()));

            RaisesModificationEvents = true;
            OnNodeModified();
        }

        /*******************************************/

        protected void ReplacePort(PortModel model, PortData data)
        {
            Type portModelType = typeof(PortModel);

            if (model.Name != data.Name)
            {
                PropertyInfo prop = typeof(PortModel).GetProperty("Name");
                if (prop != null)
                    prop.SetValue(model, data.Name);
            }

            if (model.DefaultValue != data.DefaultValue)
            {
                PropertyInfo prop = typeof(PortModel).GetProperty("DefaultValue");
                if (prop != null)
                    prop.SetValue(model, data.DefaultValue);
            }

            model.ToolTip = data.ToolTipString;
        }

        /*******************************************/
    }
}

