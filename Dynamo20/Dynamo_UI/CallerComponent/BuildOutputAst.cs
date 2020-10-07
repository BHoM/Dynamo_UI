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
        /**** Override Methods                  ****/
        /*******************************************/

        // TODO: Look into FunctionDefinitionNode and ArgumentSignatureNode as well as NodeToCode to figure out how to force input types (lacing) and potentially generic method call 
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // Check if the component has all the inputs it needs
            if (!IsReady(inputAstNodes))
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };

            // Apppend replication guide to the input parameter based on lacing strategy
            UseLevelAndReplicationGuide(inputAstNodes);

            // Define function to call
            Tuple<List<AssociativeNode>, List<AssociativeNode>> processed = GetProcessedInputs(inputAstNodes);
            AssociativeNode callerId = AstFactory.BuildStringNode(InstanceID.ToString());
            List<AssociativeNode> arguments = new List<AssociativeNode>() { callerId }.Concat(processed.Item1).ToList();
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Compute", "RunCaller", arguments);

            // Produce the output
            List<AssociativeNode> transforms = processed.Item2;
            List<AssociativeNode> assignments = CreateOutputAssignments(functionCall, callerId);
            return transforms.Concat(assignments).ToList();
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected bool IsReady(List<AssociativeNode> inputAstNodes)
        {
            List<bool> hasDefaultList = Caller.InputParams.Where(x => x.IsSelected).Select(x => x.HasDefaultValue).ToList();
            bool isReady = inputAstNodes != null && inputAstNodes.Count == hasDefaultList.Count();
            if (isReady)
            {
                for (int i = 0; i < inputAstNodes.Count; i++)
                {
                    if (inputAstNodes[i].Kind == AstKind.Null && !hasDefaultList[i])
                    {
                        isReady = false;
                        break;
                    }
                }
            }

            return isReady;
        }

        /*******************************************/

        protected Tuple<List<AssociativeNode>, List<AssociativeNode>> GetProcessedInputs(List<AssociativeNode> inputAstNodes)
        {
            // Get the params from the method caller and make sure they are the correct length
            List<ParamInfo> paramInfos = Caller.InputParams.Where(x => x.IsSelected).ToList();
            if (paramInfos.Count != inputAstNodes.Count)
                return new Tuple<List<AssociativeNode>, List<AssociativeNode>>(inputAstNodes, new List<AssociativeNode>());

            // Define inputs transforms if any
            string prefix = Guid.NewGuid().ToString() + "_";
            List<AssociativeNode> arguments = new List<AssociativeNode>();
            List<AssociativeNode> transforms = new List<AssociativeNode>();

            for (int i = 0; i < inputAstNodes.Count; i++)
            {
                int depth = paramInfos[i].Depth();
                string transformerName = (depth == 1) ? "ListWrapper" : (depth == 2) ? "TreeWrapper" : "";

                if (transformerName == "")
                    arguments.Add(inputAstNodes[i]);
                else
                {
                    AssociativeNode transform = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Create", transformerName, new List<AssociativeNode> { inputAstNodes[i] });
                    AssociativeNode newVar = AstFactory.BuildIdentifier(prefix + i.ToString());
                    transforms.Add(AstFactory.BuildAssignment(newVar, transform));
                    arguments.Add(newVar);
                }
            }
            
            return new Tuple<List<AssociativeNode>, List<AssociativeNode>>(arguments, transforms);
        }

        /*******************************************/

        protected List<AssociativeNode> CreateOutputAssignments(AssociativeNode functionCall, AssociativeNode callerId)
        {
            List<ParamInfo> outParams = Caller.OutputParams;
            List<AssociativeNode> assignments = new List<AssociativeNode>();

            int nbOutputs = Math.Min(outParams.Count, OutPorts.Count);
            
            if (outParams.Count == 0)
            {
                //Do nothing ?
            }
            else if (outParams.Count == 1 && nbOutputs == 1)
            {
                assignments.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall));
            }
            else
            {
                AssociativeNode newVar = AstFactory.BuildIdentifier(InstanceID.ToString());
                assignments.Add(AstFactory.BuildAssignment(newVar, functionCall));

                for (int i = 0; i < nbOutputs; i++)
                {
                    AssociativeNode accessor = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Query", "ItemFromCustom", new List<AssociativeNode> { newVar, AstFactory.BuildIntNode(i) });
                    assignments.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(i), accessor));
                }
            }

            return assignments;
        }

        /*******************************************/
    }
}

