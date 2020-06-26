using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkMaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        
        private string getIPGlobalProperties()
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            
            /*MachineTextBlock.Text = "Hostname | DomainName | DHCPScopeName | IsWINSProxy | NodeType\n" +
                computerProperties.HostName + " | " +
                computerProperties.DomainName + " | " +
                computerProperties.DhcpScopeName + " | " +
                computerProperties.IsWinsProxy + " | " +
                computerProperties.NodeType + "ghfhgfhfh";
            */
            return MachineTextBlock.Text;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //Global value string       
            var allProp = "===IPGlobalProperties=== \n";

            //global properties ip
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();

            //get properties of class IPGlobalProperties
            PropertyInfo[] pi = computerProperties.GetType().GetProperties();
            foreach (PropertyInfo s in pi)
            {
                allProp += s.Name +" = "+ s.GetValue(computerProperties, null)+"\n";
            }

            allProp += "===ActiveTcpConnection=== \n";
            TcpConnectionInformation[] tcpCI = computerProperties.GetActiveTcpConnections();
            foreach(TcpConnectionInformation s in tcpCI)
            {
                allProp += "local TCP "+GetDNSAddr(s.LocalEndPoint.Address)+"= "+ s.LocalEndPoint.ToString()+" <==> remote " +GetDNSAddr(s.RemoteEndPoint.Address)+"= "+ s.RemoteEndPoint.ToString() +"\n";
            }

            allProp += "===ActiveUdpListeners=== \n";
            IPEndPoint[] udpCI = computerProperties.GetActiveUdpListeners();
            foreach (IPEndPoint s in udpCI)
            {
                allProp += "UDP Addres Family >" + s.AddressFamily.ToString() + "< UDP Address >" + s.Address.ToString() + "< UDP Port >" + s.Port.ToString() + "< "+ s.ToString() +"\n";
            }

            allProp += "===IPv4GlobalStatistics=== \n";
            IPGlobalStatistics gip4stat = computerProperties.GetIPv4GlobalStatistics();
            
                allProp += "UDP Addres Family >" + gip4stat.ReceivedPacketsWithAddressErrors + "< UDP Address >" + gip4stat.ToString() + "\n";
         
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            
                foreach (NetworkInterface nip in nics)
                {
                    allProp += "ID ="+ nip.Id +" Speed =" + nip.Speed/1024 + "Mbps <> MAC Address =" + nip.GetPhysicalAddress().ToString() + " <"+nip.Description + ">\n";
                    foreach (IPAddressInformation ipaic in nip.GetIPProperties().AnycastAddresses)
                {
                    allProp += "____"+ ipaic.Address +"____\n";
                }
                    
                }
            


            // always on bottom
            MachineTextBlock.Text = allProp;



        }
        public string GetDNSAddr(IPAddress address)
        {
            try
            {
                var host = Dns.GetHostEntry(address);
                return host.HostName;
            }catch (System.Net.Sockets.SocketException s) {

                return s.Message;
            }
        }
    }
}
