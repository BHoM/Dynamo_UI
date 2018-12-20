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
    [NodeName("UpdateProperty (old)")]
    [NodeDescription("Update property of a selection of objectsin the external software")]
    [NodeCategory("BHoM.Adapter")]
    [InPortNames("Adapter", "Filter", "Property", "NewValue", "Config", "Active")]
    [InPortTypes("object", "object", "string", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "Filter Query", "Name of the property to change", "New value to assign to the property", "UpdateProperty config\nDefault: null", "Execute the update\nDefault: false")]
    [OutPortNames("#Updated")]
    [OutPortTypes("int")]
    [OutPortDescriptions("Number of objects updated")]
    [IsDesignScriptCompatible]
    [IsVisibleInDynamoLibrary(false)]
    public class UpdatePropertyNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public UpdatePropertyNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "UpdateProperty";

            DefaultValues = new Dictionary<int, AssociativeNode> {
                { 4, AstFactory.BuildNullNode() },
                { 5, AstFactory.BuildBooleanNode(false) }
            };

            RegisterAllPorts();
        }


        /*******************************************/
    }
}