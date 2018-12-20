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

using Autodesk.DesignScript.Runtime;
using BH.UI.Dynamo.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("Execute (old)")]
    [NodeDescription("Execute a command in the external software")]
    [NodeCategory("BHoM.Adapter")]
    [InPortNames("Adapter", "Command", "Params", "Config", "Active")]
    [InPortTypes("object", "string", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "Command to run", "Parameters of the command\nDefault: null", "Execute config\nDefault: null", "Execute\nDefault: false")]
    [OutPortNames("Success")]
    [OutPortTypes("bool")]
    [OutPortDescriptions("Coomand ran successfully")]
    [IsDesignScriptCompatible]
    [IsVisibleInDynamoLibrary(false)]
    public class ExecuteNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ExecuteNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Execute";

            DefaultValues = new Dictionary<int, AssociativeNode> {
                { 2, AstFactory.BuildNullNode() },
                { 3, AstFactory.BuildNullNode() },
                { 4, AstFactory.BuildBooleanNode(false) }
            };

            RegisterAllPorts();
        }


        /*******************************************/
    }
}