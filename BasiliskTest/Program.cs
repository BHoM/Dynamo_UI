using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using BHoM.Geometry;
using BHoM.Global;
using BHoM.Structural;

namespace BasiliskTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMongo();

            Console.Read();
        }

        static void TestMongo()
        {
            // Create a fake project
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < 10; i++)
                nodes.Add(new Node(i, 2, 3));

            List<Bar> bars = new List<Bar>();
            for (int i = 1; i < 10; i++)
                bars.Add(new Bar(nodes[i - 1], nodes[i]));

            var project = Project.ActiveProject;
            foreach (Node node in nodes)
                project.AddObject(node);
            foreach (Bar bar in bars)
                project.AddObject(bar);

            // Create database
            var mongo = new BHoM_Engine.Databases.Mongo.MongoLink();
            Console.WriteLine("Database link created");

            // Add Objects to the dtabase
            //mongo.SaveObjects(project.Objects); 
            //Console.WriteLine("Objects added to the database");

            // Get all object from database
            IEnumerable<BHoM.Global.BHoMObject> objects = mongo.GetObjects("{}");
            Console.WriteLine("Objects obtained from the database: {0}", objects.Count());
        }
    }
}
