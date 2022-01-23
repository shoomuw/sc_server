using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;


namespace sc_client
{
    class Clienttest
    {
        static void Main(string[] args)
        {
            int id = 0;
            string input = null;

            IPAddress host;
            int port = 0;

            /*
             * 不正な入力でないか正規表現を使って判断する
             * 不正な入力であればもう一度入力
             */
            while (true)
            {
                // クライアントのIPアドレスを入力
                Console.Write("IP address: ");
                input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^((2[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.){3}(2[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$"))
                {
                    break;
                }
                else
                {

                    Console.WriteLine("Error: This IP address is not incorrect.");
                }
            }

            while (true)
            {
                // ポート番号の入力
                Console.Write("Port: ");
                port = Int32.Parse(Console.ReadLine());
                if (port >= 0 && port <= 65535)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: This port is not incprrect");
                }
            }

            Console.WriteLine("OK Connecting " + input + ":" + port);
            Console.ReadKey();
            host = IPAddress.Parse(input);
            IPEndPoint ipe = new IPEndPoint(host, port);
            string recvID, sendl = "";
            byte[] wbuf, rbuf = new byte[1024];

            Regex reg = new Regex("\0");
            Regex confirm= new Regex("urid=");
            try
            {
                using (var clnt = new TcpClient())
                {

                    clnt.Connect(ipe);
                    using (var stream = clnt.GetStream())
                    {
                        // サーバからIDを受信
                        stream.Read(rbuf, 0, rbuf.Length);
                        recvID = reg.Replace(Encoding.UTF8.GetString(rbuf), "");
                        if (Regex.IsMatch(recvID, @"[0-9]{0,}$"))
                        {
                            Console.WriteLine(recvID);
                            recvID = recvID.Replace("urid=", "");
                            id = Int32.Parse(recvID);
                            Console.WriteLine("Your ID is " + id);
                            Array.Clear(rbuf, 0, rbuf.Length);
                            while (sendl != "bye")
                            {
                                // 文章を入力
                                Console.WriteLine("Please input:");
                                sendl = Console.ReadLine();

                                // サーバに送信
                                wbuf = Encoding.UTF8.GetBytes(sendl);
                                stream.Write(wbuf, 0, wbuf.Length);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            finally
            {
                Console.WriteLine("Bye...");
                Console.ReadKey();
            }
        }
    }
}