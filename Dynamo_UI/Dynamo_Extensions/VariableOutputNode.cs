/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using System.Linq;
using System.Xml;
using Dynamo.Core;
using Dynamo.Graph.Workspaces;
using Dynamo.Graph.Nodes;
using Dynamo.Graph;

namespace Dynamo.Graph.Nodes
{
    /// <summary>
    /// Base class for nodes that have dynamic outgoing ports.
    /// E.g. list.create.
    /// </summary>
    public abstract class VariableOutputNode : NodeModel
    {
        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public abstract string GetOutputTooltip(int index);

        public abstract string GetOutputName(int index);


        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public BasicVariableOutputNodeController VariableOutputController { get; set; }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        protected VariableOutputNode()
        {
            VariableOutputController = new BasicVariableOutputNodeController(this);
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public virtual void RemoveOutput()
        {
            VariableOutputController.RemoveOutputBase();
        }

        /*******************************************/

        public virtual void AddOutput()
        {
            VariableOutputController.AddOutputBase();
        }

        /*******************************************/

        public virtual int GetOutputIndex()
        {
            return VariableOutputController.GetOutputIndexBase();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void OnBuilt()
        {
            VariableOutputController.OnBuilt();
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            VariableOutputController.SerializeCore(element, context);
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.DeserializeCore(nodeElement, context);
            VariableOutputController.DeserializeCore(nodeElement, context);
        }

        /*******************************************/
    }
}
