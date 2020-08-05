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

using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Base.Components;
using BH.UI.Base;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;
using System;
using BH.oM.UI;
using BH.Engine.Dynamo;
using System.Linq;
using Newtonsoft.Json;

namespace BH.UI.Dynamo.Components
{
    [NodeName("CreateObject")]
    [NodeCategory("BHoM.oM")]
    [NodeDescription("Creates an instance of a selected type of BHoM object")]
    [IsDesignScriptCompatible]
    public class CreateObjectComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateObjectCaller();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CreateObjectComponent() : base()
        {
            CreateObjectCaller caller = Caller as CreateObjectCaller;
            if (caller != null)
                caller.Modified += Caller_InputToggled;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void Caller_InputToggled(object sender, CallerUpdate update)
        {
            if (update.Cause != CallerUpdateCause.InputSelection)
                return;

            foreach (IParamUpdate paramUpdate in update.InputUpdates)
            {
                if (paramUpdate is ParamAdded)
                    InPorts.Add(new PortModel(PortType.Input, this, paramUpdate.Param.ToPortData()));
                else
                {
                    string name = paramUpdate.Param.Name.ToLower();
                    int index = InPorts.ToList().FindIndex(x => x.Name.ToLower() == name);
                    if (index >= 0)
                        InPorts.RemoveAt(index);
                }
            }

            RaisesModificationEvents = true;
            OnNodeModified();
        }


        /*******************************************/
    }
}

