using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Shutdown.ServiceSelf
{
    partial class ServiceHost : ServiceBase
    {
        public string log_file_path = "C:\\Users\\ismdeep\\shutdown_log.txt";
        public string config_file_path = "C:\\Users\\ismdeep\\shutdown_service_self\\config\\";
        public ServiceHost()
        {
            InitializeComponent();
        }

        public string shift_string (string str)
        {
            string ans = "";
            bool flag = true;
            for (int i = 0; flag && i < str.Length; i++)
            {
                if (str[i] == 0)
                {
                    flag = false;
                }
                else
                {
                    ans += str[i];
                }
            }
            return ans;
        }

        public string[] read_command_from_config_file (string config_id)
        {
            // File command_file = new File(config_file_path + config_id + ".txt");
            string[] command_line = File.ReadAllLines(config_file_path + config_id + ".txt");
            return command_line;
        }

        public void NoneThread ()
        {

            TcpListener listener = new TcpListener(IPAddress.Any, 12354);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream client_stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int read_cnt = 0;
                read_cnt = client_stream.Read(buffer, 0, 1024);
                string str = System.Text.Encoding.ASCII.GetString(buffer);
                str = shift_string(str);
                // System.Console.WriteLine(str);
                System.IO.File.AppendAllText(log_file_path, str + "\r\n");


                string[] _command_ = read_command_from_config_file(str);

                
                    // ---- Process.Start("cmd.exe", _command_[i]);
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();

                for (int i = 0; i < _command_.Length; i++)
                {
                    System.IO.File.AppendAllText(log_file_path, ">> " + _command_[i] + "\r\n");
                    p.StandardInput.WriteLine(_command_[i]);
                }

            }
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            System.IO.File.AppendAllText(log_file_path, "Service Is Started……" + DateTime.Now.ToString() + "\r\n");
            ServiceHost p = new ServiceHost();
            Thread t = new Thread(new ThreadStart(p.NoneThread));
            t.Start();
            // ---- System.IO.File.AppendAllText(log_file_path, Convert.ToString(  read_command_from_config_file("4").Length  ));
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            System.IO.File.AppendAllText(log_file_path, "Service Is Stopped……" + DateTime.Now.ToString() + "\r\n");
        }
    }
}
