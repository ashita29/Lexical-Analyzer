////////////////////////////////////////////////////////////////////////////
// NavigatorClient.xaml.cs - Demonstrates Directory Navigation in WPF App //
// ver 2.2                                                                //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017        //
////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package defines WPF application processing by the client.  The client
 * displays a local FileFolder view, and a remote FileFolder view.  It supports
 * navigating into subdirectories, both locally and in the remote Server.
 * 
 * It also supports viewing local files.
 * 
 *Public Interfcae
 * ----------------
 * 1.public void strongCompToString(CsGraph<string, string> graph-----------> used to display the strong component results
 * 2.public void getFile()-------------------------------------------------->to get getTopFiles and getTopDirs
 * 3.private void autoTest()------------------------------------------------> run the automate test requirements
 * 4.initializeMessageDispatcher()------------------------------------------>define how to process each message command
 * 5.public void setTestElements(Stack<string> te)-------------------------->store element on stack so as to display on GUI
 * 6.public Stack<string> getTestElements()--------------------------------->return testElement
 * 
 * Required Files:
 * Environment.cs
 * FileMgr2.cs
 * IMessagePassingCommService.cs
 * MessagePassingCommService.cs
 * TestUtilities.cs 
 * 
 * Maintenance History:
 * --------------------
 * ver 2.2 : 5th December 2017
 * - removed the local file view as it is not required for this project
 * - implemeneted AutomatedTestUnit, getDependency and getStrongComponent under initializeMessageDispatcher
 * - so as to process dependency and strong component result.
 * - Have automated the application using RoutedEventArgs. I have commented the code below.

 * 
 * ver 2.1 : 26 Oct 2017
 * - relatively minor modifications to the Comm channel used to send messages
 *   between NavigatorClient and NavigatorServer
 * ver 2.0 : 24 Oct 2017
 * - added remote processing - Up functionality not yet implemented
 *   - defined NavigatorServer
 *   - added the CsCommMessagePassing prototype
 * ver 1.0 : 22 Oct 2017
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using MessagePassingComm;

namespace Navigator
{
    public partial class MainWindow : Window
    {
        private IFileMgr fileMgr { get; set; } = null;  // note: Navigator just uses interface declarations
        Comm comm { get; set; } = null;
        Dictionary<string, Action<CommMessage>> messageDispatcher = new Dictionary<string, Action<CommMessage>>();
        Thread rcvThread = null;
        Stack<string> testElement = new Stack<string>();

        public MainWindow()
        {
            InitializeComponent();
            initializeEnvironment();
            Console.Title = "Navigator Client";
            fileMgr = FileMgrFactory.create(FileMgrType.Local); // uses Environment                                                                
            comm = new Comm(ClientEnvironment.address, ClientEnvironment.port);
            initializeMessageDispatcher();
            getFile();
            rcvThread = new Thread(rcvThreadProc);
            rcvThread.Start();
        }

        //----< fetching root files and directory >-------------
        public void getFile()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.author = "Jim Fawcett";
            msg1.command = "getTopFiles";
            msg1.arguments.Add("");
            comm.postMessage(msg1);
            CommMessage msg2 = msg1.clone();
            msg2.command = "getTopDirs";
            comm.postMessage(msg2);
        }

        public void getClientPort()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "ClientPort";
            comm.postMessage(msg1);
        }

        /*****************************This below code is written for automating the application***************************************/
        /*private void autoTest()
        {
            Req1();
            Req2();
            Req3();
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(1);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(2);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(3);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(4);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(5);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(6);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(7);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            remoteFiles.SelectedItem = remoteFiles.Items.GetItemAt(8);
            buttonAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            DependencyCheck_Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Req4();
            StrongComponenetCheck_Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            Req5();
            Dependency_r.IsSelected = true;
            Strong_r.IsSelected = true;
            Req6();
        }*/

        private void Req1()
        {
            Console.WriteLine("----------------------Demonstrating Requirement 1----------------------------------");
            Console.WriteLine("Requirement 1 is :");
            Console.WriteLine("Shall use Visual Studio 2017 and its C# Windows Console Projects, as provided in the ECS computer labs.");
            Console.WriteLine("This project is build using Visual Studio 2017 in C# as a Console Application");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }
        public void Req2()
        {
            Console.WriteLine("----------------------Demonstrating Requirement 2----------------------------------");
            Console.WriteLine("Requirement 2 is :");
            Console.WriteLine("Shall use the .Net System.IO and System.Text for all I/O.");
            Console.WriteLine("This project is build using Visual Studio 2017 using .Net framework which includes .Net System.IO and System.Text for all inputs.");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }

        public void Req3()
        {
            Console.WriteLine("----------------------Demonstrating Requirement 3----------------------------------");
            Console.WriteLine("Requirement 3 is :");
            Console.WriteLine("Shall provide C# packages as described in the Purpose section.");
            Console.WriteLine("This project has CsGrapg, DemoExecuitve, DemoReqs, DependencyTable, Display, Element, Environment,FileMgr packages ");
            Console.WriteLine("This project has IMessagePassingCommService, IMessagePassingCommService, Client, Server,Parser, Toker, SemiExpression, Type Table and Test Utilities packages. ");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }

        public void Req4()
        {
            Console.WriteLine("----------------------Demonstrating Requirement 4----------------------------------");
            Console.WriteLine("Requirement 4 is :");
            Console.WriteLine("The Server packages shall evaluate all the dependencies between files in a specified file set, based on received request messages");
            Console.WriteLine("---------------------------------Check the output below----------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }

        public void Req5()
        {
            Console.WriteLine("----------------------Demonstrating Requirement 5----------------------------------");
            Console.WriteLine("Requirement 5 is :");
            Console.WriteLine("TThe Server packages shall find all strong components, if any, in the file collection, based on the dependency analysis, cited above.");
            Console.WriteLine("---------------------------------Check the output below----------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }

        public void Req6()
        {
            Console.WriteLine("----------------------Demonstrating Requirement 6----------------------------------");
            Console.WriteLine("Requirement 6 is :");
            Console.WriteLine("The Client packages shall display requested results in a well formated GUI display.");
            Console.WriteLine("---------------------------------Check the GUI below----------------------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }

        public void Req7()
        {
            Console.WriteLine("----------------------Demonstrating Requirement 7----------------------------------");
            Console.WriteLine("Requirement 7 is :");
            Console.WriteLine("Shall include an automated unit test suite that demonstrates you meet all of the functional requirements, stated above");
            Console.WriteLine("-------------------------------------------------------------------------------------");
        }
        //----< make Environment equivalent to ClientEnvironment >-------

        void initializeEnvironment()
        {
            Environment.root = ClientEnvironment.root;
            Environment.address = ClientEnvironment.address;
            Environment.port = ClientEnvironment.port;
            Environment.endPoint = ClientEnvironment.endPoint;
        }

        /*----< Load ATU >------------*/
        private void dispatcherATU()
        {
            messageDispatcher["AutomatedTestUnit"] = (CommMessage msg) =>
            {
                Req1();
                Req2();
                Req3();
                Req4();
                Req5();
                Req6();
                string[] file = { "File1.cs", "File2.cs", "File3.cs" };
                CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
                msg1.from = ClientEnvironment.endPoint;
                msg1.to = ServerEnvironment.endPoint;
                msg1.command = "getDependency";
                foreach (var item in file)
                {
                    msg1.arguments.Add(item.ToString());
                }
                comm.postMessage(msg1);
                CommMessage msg2 = msg1.clone();
                msg2.command = "getStrongComponent";
                comm.postMessage(msg2);
                Req7();
            };
        }

        /*----< load getDependency Result on GUI >------------*/
        private void dispatchDependency()
        {
            messageDispatcher["getDependency"] = (CommMessage msg) =>
            {
                Result.Clear();
                List<string> files = msg.arguments;
                foreach (string file in files)
                {
                    if ((file.Contains(";")))
                        Result.Text += "\r\n";
                    else if ((file.Contains(",")))
                        Result.Text += file;
                    else
                        Result.Text += file + " ";
                }
            };
        }

        /*----< load strong component on GUI >------------*/
        private void dispatchStrong()
        {
            messageDispatcher["getStrongComponent"] = (CommMessage msg) =>
            {
                strongComponent.Clear();
                List<string> files = msg.arguments;
                foreach (string file in files)
                {
                    if ((file.Contains(";")))
                        strongComponent.Text += "\r\n";
                    else if ((file.Contains(",")))
                        strongComponent.Text += file;
                    else
                        strongComponent.Text += file + " ";
                }
            };
        }
        /*----< load files from root on GUI >------------*/
        private void dispatchgetTopFiles()
        {
            messageDispatcher["getTopFiles"] = (CommMessage msg) =>
            {
                remoteFiles.Items.Clear();
                foreach (string file in msg.arguments)
                {
                    remoteFiles.Items.Add(file);
                }
                /*----< Uncomment autoTest() and comment ATU to see automation of GUI >------------*/
                // autoTest(); 
            };
        }

        // load remoteDirs listbox with dirs from root
        private void dispatchgetTopDirs()
        {
            messageDispatcher["getTopDirs"] = (CommMessage msg) =>
            {
                remoteDirs.Items.Clear();
                foreach (string dir in msg.arguments)
                {
                    remoteDirs.Items.Add(dir);
                }
            };
        }

        // load remoteFiles listbox with files from folder
        private void dispatchmoveIntoFolderFiles()
        {
            messageDispatcher["moveIntoFolderFiles"] = (CommMessage msg) =>
            {
                remoteFiles.Items.Clear();
                foreach (string file in msg.arguments)
                {
                    remoteFiles.Items.Add(file);
                }
            };
        }

        // load remoteDirs listbox with dirs from folder
        private void dispatchmoveIntoFolderDirs()
        {
            messageDispatcher["moveIntoFolderDirs"] = (CommMessage msg) =>
            {
                remoteDirs.Items.Clear();
                foreach (string dir in msg.arguments)
                {
                    remoteDirs.Items.Add(dir);
                }
            };
        }
        //----< define how to process each message command >-------------

        void initializeMessageDispatcher()
        {
            /*----< Load ATU >------------*/
            dispatcherATU();

            /*----< load getDependency Result on GUI >------------*/
            dispatchDependency();

            /*----< load strong component on GUI >------------*/
            dispatchStrong();

            /*----< load files from root on GUI >------------*/
            dispatchgetTopFiles();

            // load remoteDirs listbox with dirs from root
            dispatchgetTopDirs();

            // load remoteFiles listbox with files from folder
            dispatchmoveIntoFolderFiles();

            // load remoteDirs listbox with dirs from folder
            dispatchmoveIntoFolderDirs();
        }
        //----< define processing for GUI's receive thread >-------------

        void rcvThreadProc()
        {
            Console.Write("\n  Starting Navigation Client ");
            Console.Write("\n====================================== ");
            Console.Write("\n  starting client's receive thread");
            Console.Write("\n  Connecting Client on port", Environment.port);
            while (true)
            {
                CommMessage msg = comm.getMessage();
                msg.show();
                if (msg.command == null)
                    continue;
                // pass the Dispatcher's action value to the main thread for execution
                Dispatcher.Invoke(messageDispatcher[msg.command], new object[] { msg });
            }
        }
        //----< shut down comm when the main window closes >-------------

        private void Window_Closed(object sender, EventArgs e)
        {
            comm.close();

            // The step below should not be nessary, but I've apparently caused a closing event to 
            // hang by manually renaming packages instead of getting Visual Studio to rename them.

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //----< not currently being used >-------------------------------

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        /*----< fetch root file and directory >------------*/
        private void RemoteTop_Click(object sender, RoutedEventArgs e)
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.author = "Jim Fawcett";
            msg1.command = "getTopFiles";
            msg1.arguments.Add("");
            comm.postMessage(msg1);
            CommMessage msg2 = msg1.clone();
            msg2.command = "getTopDirs";
            comm.postMessage(msg2);
        }
        //----< load remote files on GUI >-------
        static List<string> testnames { get; set; } = new List<string>();
        private void remoteFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {   
            Stack<string> s = new Stack<string>();
            if (remoteFiles.IsLoaded)
            {
                foreach (string lbi in remoteFiles.SelectedItems)
                {
                    testnames.Add(lbi);
                    s.Push(lbi);
                }
            }
            setTestElements(s);
        }
        /*----< Add files selected on GUI >------------*/
        public void setTestElements(Stack<string> te)
        {
            testElement = te;
            // selectedFiles.Items.Clear();
            foreach (string file in testElement)
            {
                selectedFiles.Items.Add(file);
            }
        }

        public Stack<string> getTestElements()
        {
            return testElement;
        }
        /*----< push selected files on stack >------------*/
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Stack<string> s = new Stack<string>();
            if (remoteFiles.IsLoaded)
            {
                foreach (string lbi in remoteFiles.SelectedItems)
                {
                    testnames.Add(lbi);
                    s.Push(lbi);
                }
            }
            setTestElements(s);
        }


        //----< move to parent directory of current remote path >--------

        private void RemoteUp_Click(object sender, RoutedEventArgs e)
        {
            // coming soon
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.author = "Jim Fawcett";
            msg1.command = "moveIntoFolderFiles";
            msg1.arguments.Add("");
            comm.postMessage(msg1);
            CommMessage msg2 = msg1.clone();
            msg2.command = "moveIntoFolderDirs";
            comm.postMessage(msg2);
        }

        /*----< requesting server to do Dependency check >------------*/
        private void DependencyCheck_Click(object sender, RoutedEventArgs e)
        {
            // coming soon
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.author = "Jim Fawcett";
            msg1.command = "getDependency";
            selectedFiles.SelectAll();
            foreach (string lbi in selectedFiles.SelectedItems)
            {
                msg1.arguments.Add(lbi);
            }
            comm.postMessage(msg1);
        }

        /*----< requesting server to do strong component check >------------*/
        private void StrongComponent_Click(object sender, RoutedEventArgs e)
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.author = "Jim Fawcett";
            msg1.command = "getStrongComponent";
            selectedFiles.SelectAll();
            foreach (string lbi in selectedFiles.SelectedItems)
            {
                msg1.arguments.Add(lbi);
            }
            comm.postMessage(msg1);
        }

        //----< move into remote subdir and display files and subdirs >--
        /*
         * - sends messages to server to get files and dirs from folder
         * - recv thread will create Action<CommMessage>s for the UI thread
         *   to invoke to load the remoteFiles and remoteDirs listboxs
         */
        private void remoteDirs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "moveIntoFolderFiles";
            msg1.arguments.Add(remoteDirs.SelectedValue as string);
            comm.postMessage(msg1);
            CommMessage msg2 = msg1.clone();
            msg2.command = "moveIntoFolderDirs";
            comm.postMessage(msg2);
        }

        /*----< double click on file to see the file >------------*/
        private void remoteFiles_MouseDoubleClick_FileAnalaysis(object sender, MouseButtonEventArgs e)
        {
            string fileName = remoteFiles.SelectedValue as string;
            try
            {
                string path = System.IO.Path.Combine(ClientEnvironment.root, fileName);
                string contents = File.ReadAllText(path);
                CodePopUp popup = new CodePopUp();
                popup.codeView.Text = contents;
                popup.Show();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        /*----< functionality to delete a selected file >------------*/
        private void removeFile(Object sender, RoutedEventArgs e)
        {
            string currentItem = selectedFiles.SelectedValue.ToString();
            int currentIndex = selectedFiles.SelectedIndex;
            selectedFiles.Items.RemoveAt(selectedFiles.Items.IndexOf(selectedFiles.SelectedItem));
        }

        /*----< sending msg request to server for running ATU >------------*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "AutomatedTestUnit";
            comm.postMessage(msg1);
        }

    }
}
