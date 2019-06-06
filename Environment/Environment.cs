/////////////////////////////////////////////////////////////////////////////////////////
// Environment.cs - defines environment properties for Client and Server               //
// ver 1.1                                                                             //
// Lanugage:  C#, VS 2017                                                              //
// Platform:  Lenovo ThinkPad T470s, Windows 10 Pro                                    //
// Source:    Jim Fawcett                                                              //
//            CSE681 - Software Modeling and Analysis, Fall 2018                       //
// Author:    Ashita Garg, Syracuse University                                         //
//            (315)455-0034, asgarg@syr.edu                                            //
/////////////////////////////////////////////////////////////////////////////////////////
/*
 *Package Operations:
 * -------------------
 * Demonstrates how to set up environment variables for client and server 
 * 
 * 
 * Maintenance History:
 * --------------------
 * ver 1.1 : 5th Dec 2018
 * - chnaged ServerEnvironment root path
 * ver 1.0 : 23 Oct 2017
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigator
{
    //----< getting structure for Environment >-------------
    public struct Environment
  {
    public static string root { get; set; }
    public const long blockSize = 1024;
    public static string endPoint { get; set; }
    public static string address { get; set; }
    public static int port { get; set; }
    public static bool verbose { get; set; }
  }

    //----< setting structure for client Environment >-------------
    public struct ClientEnvironment
  {
    public static string root { get; set; } = "../../../ClientFiles/";
    public const long blockSize = 1024;
    public static string endPoint { get; set; } = "http://localhost:8090/IMessagePassingComm";
    public static string address { get; set; } = "http://localhost";
    public static int port { get; set; } = 8090;
    public static bool verbose { get; set; } = false;
  }
    //----< setting structure for server Environment >-------------

    public struct ServerEnvironment
  {
    public static string root { get; set; } = "../../../SampleFiles";
    public const long blockSize = 1024;
    public static string endPoint { get; set; } = "http://localhost:8080/IMessagePassingComm";
    public static string address { get; set; } = "http://localhost";
    public static int port { get; set; } = 8080;
    public static bool verbose { get; set; } = false;
  }
}
