///////////////////////////////////////////////////////////////////////////
// DependencyTable.cs  -  Manage Dependency information                  //
// ver 1.2                                                               //
// Language:    C#, Visual Studio 2017, .Net Framework 4.5               //
// Platform:    Dell XPS 8920, Windows 10 Pro                            //
// Application: Pr#3 demo, CSE681, Fall 2018                             //
// Source:      Jim Fawcett, CST 2-187, Syracuse University              //
//              (315) 443-3948, jfawcett@twcny.rr.com                    //
// Author:      Ashita Garg, Syracuse University                         //
//              (315)455-0034, asgarg@syr.edu                            //
///////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * DependencyTable class manages file information generated during type-based
 * code analysis, in its dependency analysis phase.
 * 
 * Public Interfcae
 * ----------------
 * 1.public void addParent(string parentFile); ------------------------>adds the parent file to dependency table
 * 2.public void add(string parentFile, string childFile)-------------->add child file dependency
 * 3.public bool contains(string parentFile)---------------------------> to check whether parentFile a key for dependency table
 * 4.public void clear()----------------------------------------------->used to clear contents of dependency table
 * 5.public void show(bool fullyQualified = false)--------------------->used to display contents of dependency table
 * 6.public void depTabletoString()------------------------------------>used to display contents of dependency table on GUI
 * 
 * Required Files:
 * ---------------
 * DependencyTable.cs
 *   
 * Maintenance History:
 * --------------------
 * * ver 1.2 : 5th Dec 2018
 *   - added new function depTabletoString() to display the desired output format on GUI
 * ver 1.0 : 29 Nov 2018
 *   - first release
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    public class DependencyTable
    {
        public Dictionary<string, List<string>> dependencies { get; set; }
          = new Dictionary<string, List<string>>();

        public List<string> msg = new List<string>();

        //----< add parent file >----------------------------------------

        public void addParent(string parentFile)
        {
            if (dependencies.ContainsKey(parentFile))
                return;
            List<string> deps = new List<string>();
            dependencies.Add(parentFile, deps);
        }
        //----< add child file dependency >------------------------------

        public void add(string parentFile, string childFile)
        {
            if (parentFile == childFile)
                return;
            if (dependencies.ContainsKey(parentFile))
            {
                if (dependencies[parentFile].Contains(childFile))
                    return;
                dependencies[parentFile].Add(childFile);
            }
            else
            {
                List<string> children = new List<string>();
                children.Add(childFile);
                dependencies.Add(parentFile, children);
            }
        }
        //----< is parentFile a key for dependency table? >--------------

        public bool contains(string parentFile)
        {
            return dependencies.ContainsKey(parentFile);
        }
        //----< clear contents of table >--------------------------------

        public void clear()
        {
            dependencies.Clear();
        }
        //----< display contents of dependency table >-------------------

        public void show(bool fullyQualified = false)
        {
            foreach (var item in dependencies)
            {
                string file = item.Key;
                if (!fullyQualified)
                    file = System.IO.Path.GetFileName(file);
                Console.Write("\n  {0}", file);
                if (item.Value.Count == 0)
                    continue;
                Console.Write("\n    ");
                foreach (var elem in item.Value)
                {
                    string child = elem;
                    if (!fullyQualified)
                        child = System.IO.Path.GetFileName(child);
                    Console.Write("{0} ", child);
                }
            }
        }
        //----< display contents of dependency table on GUI>-------------------
        public void depTabletoString()
        {
            foreach (var item in dependencies)
            {
                string file = item.Key;
                file = System.IO.Path.GetFileName(file);
                msg.Add(file.ToString());
                if (item.Value.Count == 0)
                {
                    msg.Add(";");
                    continue;
                }
                msg.Add(":");
                foreach (var elem in item.Value)
                {
                    string child = elem;
                    child = System.IO.Path.GetFileName(child);
                    msg.Add(child.ToString());
                    msg.Add("\t");
                }
                msg.Add(";");
            }
            Console.Write("hello");
        }
    }
    class TestDependencyTable
    {
#if (TEST_DEPENDENCYTABLE)
        static void Main(string[] args)
        {
            Console.Write("\n  Tested in Executive");
            Console.Write("\n\n");
        }
#endif
    }
}
