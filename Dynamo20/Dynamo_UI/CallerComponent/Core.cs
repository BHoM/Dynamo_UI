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
        /**** Events                            ****/
        /*******************************************/

        public event EventHandler<CallerUpdate> ModifiedByCaller;


        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        [JsonIgnore]
        public abstract Caller Caller { get; }

        [JsonIgnore]
        protected Guid InstanceID { get; } = Guid.NewGuid();

        public string SerialisedCaller
        {
            get {
                return Caller.Write();
            }
            set {
                Caller.Read(value);
            }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerComponent() : base()
        {
            Initialise();
        }

        /*******************************************/

        [JsonConstructor]
        public CallerComponent(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            Initialise();
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected virtual void Initialise()
        {
            Category = "BHoM." + Caller.Category;
            ArgumentLacing = LacingStrategy.Auto;

            if (InPorts.Count == 0)
            {
                foreach (ParamInfo info in Caller.InputParams.Where(x => x.IsSelected))
                    InPorts.Add(new PortModel(PortType.Input, this, info.ToPortData()));
            }

            if (OutPorts.Count == 0)
            {
                foreach (ParamInfo info in Caller.OutputParams.Where(x => x.IsSelected))
                    OutPorts.Add(new PortModel(PortType.Output, this, info.ToPortData()));
            }
                
            string instanceId = InstanceID.ToString();
            DataAccessor_Dynamo dataAccessor = new DataAccessor_Dynamo();
            dataAccessor.InPorts = InPorts;
            Caller.SetDataAccessor(dataAccessor);

            BH.Engine.Dynamo.Compute.Callers[instanceId] = Caller;
            BH.Engine.Dynamo.Compute.DataAccessors[instanceId] = dataAccessor;
            BH.Engine.Dynamo.Compute.Nodes[instanceId] = this;

            Caller.Modified += OnCallerModified;
            Caller.SolutionExpired += (s, e) => OnNodeModified(true);

            PortConnected += (port, connector) => PortConnectionChanged(port);
            PortDisconnected += (port) => PortConnectionChanged(port);
        }

        /*******************************************/

        protected void PortConnectionChanged(PortModel port)
        {
            if (port.PortType == PortType.Input && port.Index >= 0)
                Caller.UpdateInput(port.Index);
        }

        /*******************************************/
    }
}

