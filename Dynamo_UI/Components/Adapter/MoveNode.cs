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
    [NodeName("Move (old)")]
    [NodeDescription("Move objects from one external software to another")]
    [NodeCategory("BHoM.Adapter")]
    [InPortNames("Source", "Target", "Query", "Config", "Active")]
    [InPortTypes("object", "object", "object", "object", "bool")]
    [InPortDescriptions("Adapter the data is pulled from", "Adapter the data is pushed to", "BHoM Query\nDefault: new FilterQuery()", "Move config (custom object)\nDefault: null", "Execute the move\nDefault: false")]
    [OutPortNames("Success")]
    [OutPortTypes("bool")]
    [OutPortDescriptions("Confirms if the operation was successful")]
    [IsDesignScriptCompatible]
    [IsVisibleInDynamoLibrary(false)]
    public class MoveNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MoveNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Move";

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