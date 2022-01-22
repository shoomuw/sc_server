using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace sc_server
{
    class Server
    {
        static string input;
        // 
        static IPAddress host;

        static void Main()
        {
            Connect();


        }

        static void Connect()
        {
            // サーバのIPアドレスを入力
            Console.Write("IP address: ");
            input = Console.ReadLine();

            /*
             * 不正な入力でないか正規表現を使って判断する 
             * 不正であればもう一度入力
             */
            if (Regex.IsMatch(input, @"^((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$"))
            {
                Console.WriteLine("OK Connecting " + input);
                Console.ReadKey();
                host = IPAddress.Parse(input);
                int port = 8765;
                IPEndPoint ipe = new IPEndPoint(host, port);
                TcpListener svr = null;
                string recvl = null;
            }
            else
            {
                Console.WriteLine("false");
                Console.ReadKey();
            }
        }
    }
}

