using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace sc_server
{
    class Servertest
    {


        static void Main() 
        {
            int id = 0;
            string input;

            IPAddress host;

            int port;

            /*
             * 不正な入力でないか正規表現を使って判断する
             * 不正であればもう一度入力
             */
            while (true)
            {
                // サーバのIPアドレスを入力
                Console.Write("IP address: ");
                input = Console.ReadLine();
                if(Regex.IsMatch(input, @"^((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: This IP address is not incorrect.");
                }
            }

            while(true)
            {
                // ポート番号を入力
                Console.Write("Port: ");
                port = Int32.Parse(Console.ReadLine());
                if(port >= 0 && port <= 65535)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: This port is not incorrect.");
                }
            }

            Console.WriteLine("OK Connecting " + input + ":" + port);
            Console.ReadKey();
            host = IPAddress.Parse(input);
            IPEndPoint ipe = new IPEndPoint(host, port);
            TcpListener svr = null;
            string recvl, sendl = "";
            bool outflg = false;
            byte[] buf = new byte[1024];
            Regex reg = new Regex("\0");
            int i = 0;

            try
            {
                svr = new TcpListener(ipe);
                Console.WriteLine("Listening......");
                svr.Start();
                while (true)
                {
                    using (var clnt = svr.AcceptTcpClient())
                    {
                        Console.WriteLine("Connect success");
                        using (var stream = clnt.GetStream())
                        {
                            try
                            {
                                // idを送る
                                sendl = "urid=" + id.ToString();
                                buf = Encoding.UTF8.GetBytes(sendl);
                                stream.Write(buf, 0, buf.Length);

                                id++;
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Error: " + e);
                            }
                            finally
                            {
                                Array.Clear(buf, 0, buf.Length);
                            }
                            while ((i = stream.Read(buf, 0, buf.Length)) != 0)
                            {
                                recvl = reg.Replace(Encoding.UTF8.GetString(buf), "");
                                Console.WriteLine("client: " + recvl);
                                if (recvl == "bye")
                                {
                                    outflg = true;
                                    break;
                                }
                                Array.Clear(buf, 0, buf.Length);
                            }
                        }
                    }
                    if (outflg)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            finally
            {
                svr.Stop();
                Console.WriteLine("Server closed.");
            }

        }

    }
}


