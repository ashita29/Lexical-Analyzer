/////////////////////////////////////////////////////////////////////////////////////////
// NavigatorServer.cs - File Server for WPF NavigatorClient Application                //
// ver 2.2                                                                             //
// Lanugage:  C#, VS 2017                                                              //
// Platform:  Lenovo ThinkPad T470s, Windows 10 Pro                                    //
// Source:    Jim Fawcett                                                              //
//            CSE681 - Software Modeling and Analysis, Fall 2018                       //
// Author:    Ashita Garg, Syracuse University                                         //
//            (315)455-0034, asgarg@syr.edu                                            //
/////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package defines a single NavigatorServer class that returns file
 * and directory information about its rootDirectory subtree.  It uses
 * a message dispatcher that handles processing of all incoming and outgoing
 * messages.
 * 
 * Public Interfcae
 * ----------------
 * 1.public void strongCompToString(CsGraph<string, string> graph-----------> used to display the strong component results
 * 
 * Required Files:
 * Environment.cs
 * FileMgr2.cs
 * IMessagePassingCommService.cs
 * MessagePassingCommService.cs
 * TestUtilities.cs  
 * TypeTable.cs
 * Toker.cs, Semi.cs
 * Parser.cs, DemoExecutive.cs, CSGraph.cs
 * 
 * 
 * Maintanence History:
 * --------------------
 * * ver 2.2 - 5th December 2017
 * - added getDependencymessage, getStrongComponent and AutomatedTestUnit dispatcher.
 * ver 2.1 - 24 Oct 2017
 * - added message dispatcher which works very well - see below
 * ver 2.0 - 24 Oct 2017
 * - added message dispatcher which works very well - see below
 * - added these comments
 * ver 1.0 - 22 Oct 2017
 * - first release
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePassingComm;
using Lexer;
using CsGraph;
using CodeAnalysis;

namespace Navigator
{
    public class NavigatorServer
    {
        IFileMgr localFileMgr { get; set; } = null;
        Comm comm { get; set; } = null;

        Dictionary<string, Func<CommMessage, CommMessage>> messageDispatcher =
          new Dictionary<string, Func<CommMessage, CommMessage>>();
        List<string> strongCompMessage = new List<string>();

        /*----< initialize server processing >-------------------------*/

        public NavigatorServer()
        {
            initializeEnvironment();
            Console.Title = "Navigator Server";
            localFileMgr = FileMgrFactory.create(FileMgrType.Local);
        }
        /*----< set Environment properties needed by server >----------*/

        void initializeEnvironment()
        {
            Environment.root = ServerEnvironment.root;
            Environment.address = ServerEnvironment.address;
            Environment.port = ServerEnvironment.port;
            Environment.endPoint = ServerEnvironment.endPoint;
        }
        /*----< define how each message will be processed >------------*/

        private void funcdep()
        {
            Func<CommMessage, CommMessage> getDependency = (CommMessage msg) =>
            {
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getDependency";
                Executive exe = new Executive();
                foreach (var item in msg.arguments)
                {
                    string path = System.IO.Path.Combine(Environment.root, item);
                    string b = System.IO.Path.GetFullPath(path);
                    exe.files.Add(b);
                }
                exe.typeAnalysis(exe.files);
                exe.dependencyAnalysis(exe.files);
                Repository repo = Repository.getInstance();
                repo.typeTable.show();
                repo.dependencyTable.show();
                repo.dependencyTable.depTabletoString();
                reply.arguments = repo.dependencyTable.msg;
                return reply;
            };
            messageDispatcher["getDependency"] = getDependency;

        }
        private void funcSC()
        {
            Func<CommMessage, CommMessage> getStrongComponent = (CommMessage msg) =>
            {
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getStrongComponent";
                Executive exe = new Executive();
                foreach (var item in msg.arguments)
                {
                    string path = System.IO.Path.Combine(Environment.root, item);
                    string b = System.IO.Path.GetFullPath(path);
                    exe.files.Add(b);
                }
                exe.typeAnalysis(exe.files);
                exe.dependencyAnalysis(exe.files);
                Repository repo = Repository.getInstance();
                repo.typeTable.show();
                repo.dependencyTable.show();
                Console.Write("\n\n  building dependency graph");
                Console.Write("\n ---------------------------");

                CsGraph<string, string> graph = exe.buildDependencyGraph();
                graph.showDependencies();

                Console.Write("\n\n  Strong Components:");
                Console.Write("\n --------------------");
                graph.strongComponents();
                foreach (var item in graph.strongComp)
                {
                    Console.Write("\n  component {0}", item.Key);
                    Console.Write("\n    ");
                    foreach (var elem in item.Value)
                    {
                        Console.Write("{0} ", elem.name);
                    }
                }
                strongCompToString(graph);
                Console.Write("\n\n");
                reply.arguments = strongCompMessage;
                return reply;
            };
            messageDispatcher["getStrongComponent"] = getStrongComponent;

        }

        //----< Executing ATU >-------------
        private void funcATU()
        {
            Func<CommMessage, CommMessage> ATU = (CommMessage msg) =>
            {
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "AutomatedTestUnit";
                return reply;
            };
            messageDispatcher["AutomatedTestUnit"] = ATU;
        }
        //----< getting Top directory >-------------
        private void funcTopFile()
        {
            Func<CommMessage, CommMessage> getTopFiles = (CommMessage msg) =>
            {
                localFileMgr.currentPath = "";
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getTopFiles";
                reply.arguments = localFileMgr.getFiles().ToList<string>();
                return reply;
            };
            messageDispatcher["getTopFiles"] = getTopFiles;
        }
        //----< getting root directory >-------------
        private void funcTopDir()
        {
            Func<CommMessage, CommMessage> getTopDirs = (CommMessage msg) =>
            {
                localFileMgr.currentPath = "";
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getTopDirs";
                reply.arguments = localFileMgr.getDirs().ToList<string>();
                return reply;
            };
            messageDispatcher["getTopDirs"] = getTopDirs;
        }
        //----< dmove into folder files >-------------
        private void funcMoveIntoFile()
        {
            Func<CommMessage, CommMessage> moveIntoFolderFiles = (CommMessage msg) =>
            {
                if (msg.arguments.Count() == 1)
                    localFileMgr.currentPath = msg.arguments[0];
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "moveIntoFolderFiles";
                reply.arguments = localFileMgr.getFiles().ToList<string>();
                return reply;
            };
            messageDispatcher["moveIntoFolderFiles"] = moveIntoFolderFiles;
        }
        //----< move into folder directory >-------------
        private void funcMoveIntoDir()
        {
            Func<CommMessage, CommMessage> moveIntoFolderDirs = (CommMessage msg) =>
            {
                if (msg.arguments.Count() == 1)
                    localFileMgr.currentPath = msg.arguments[0];
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "moveIntoFolderDirs";
                reply.arguments = localFileMgr.getDirs().ToList<string>();
                return reply;
            };
            messageDispatcher["moveIntoFolderDirs"] = moveIntoFolderDirs;
        }

        void initializeDispatcher()
        {
            //----< Executing ATU >-------------
            funcATU();

            //----< Checking Dependency >-------------
            funcdep();

            //----< Checking Strong Component >-------------
            funcSC();

            //----< getting root file >-------------
            funcTopFile();

            //----< getting root directory >-------------
            funcTopDir();

            //----< dmove into folder files >-------------
            funcMoveIntoFile();

            //----< move into folder directory >-------------
            funcMoveIntoDir();
        }

        //----< define how to process each message command >-------------
        void strongCompToString(CsGraph<string, string> graph)
        {
            strongCompMessage.Clear();
            foreach (var item in graph.strongComp)
            {
                string file = item.Key.ToString();
                strongCompMessage.Add(file);
                if (item.Value.Count == 0)
                {
                    strongCompMessage.Add(";");
                    continue;
                }
                strongCompMessage.Add(":");
                foreach (var elem in item.Value)
                {
                    strongCompMessage.Add(elem.name.ToString());
                    strongCompMessage.Add("\t");
                }
                strongCompMessage.Add(";");
            }
        }
    
        /*----< Server processing >------------------------------------*/
        /*
         * - all server processing is implemented with the simple loop, below,
         *   and the message dispatcher lambdas defined above.
         */
        static void Main(string[] args)
        {
            TestUtilities.title("Starting Navigation Server", '=');
            try
            {
                NavigatorServer server = new NavigatorServer();
                server.initializeDispatcher();
                server.comm = new MessagePassingComm.Comm(ServerEnvironment.address, ServerEnvironment.port);

                while (true)
                {
                    CommMessage msg = server.comm.getMessage();
                    if (msg.type == CommMessage.MessageType.closeReceiver)
                        break;
                    msg.show();
                    if (msg.command == null)
                        continue;
                    CommMessage reply = server.messageDispatcher[msg.command](msg);
                    reply.show();
                    server.comm.postMessage(reply);
                }
            }
            catch (Exception ex)
            {
                Console.Write("\n  exception thrown:\n{0}\n\n", ex.Message);
            }
        }
    }
}
