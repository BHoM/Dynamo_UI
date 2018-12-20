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
using System.Text;
using System.Threading.Tasks;

namespace Dynamo.Graph.Nodes
{
    public class BasicVariableOutputNodeController : VariableOutputNodeController
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BasicVariableOutputNodeController(VariableOutputNode node) : base(node)
        {
            model = node;
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void RemoveOutputBase()
        {
            base.RemoveOutputFromModel();
        }

        /*******************************************/

        public void AddOutputBase()
        {
            base.AddOutputToModel();
        }

        /*******************************************/

        public int GetOutputIndexBase()
        {
            return base.GetOutputIndexFromModel();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override string GetOutputName(int index)
        {
            return model.GetOutputName(index);
        }

        /*******************************************/

        protected override string GetOutputTooltip(int index)
        {
            return model.GetOutputTooltip(index);
        }

        /*******************************************/

        protected override void AddOutputToModel()
        {
            model.AddOutput();
        }

        /*******************************************/

        protected override void RemoveOutputFromModel()
        {
            model.RemoveOutput();
        }

        /*******************************************/

        public override int GetOutputIndexFromModel()
        {
            return model.GetOutputIndex();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private readonly VariableOutputNode model;


        /*******************************************/
    }
}
