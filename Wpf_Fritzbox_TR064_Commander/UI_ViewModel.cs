// Wpf_Fritzbox_TR064_Commander
// This App is a little example in C# showing some functions which can be started on a Fritzbox (router/Dectphone combination)
// via the TR-064 protocol using the 'FBoxAPI' library
//
// - Logging in with username and password using secure strings with the PasswordBox control
// - Ring the loacal Dect phones (can be useed to signal an alarm condition)
// - Dial not local phone number
// - Retreive count of Dect devices
// - Retreive count and some properties of hosts


// https://www.codeproject.com/Tips/549109/Working-with-SecureString

// https://stackoverflow.com/questions/1483892/how-to-bind-to-a-passwordbox-in-mvvm


using System;
using System.Runtime.InteropServices;
using System.Net;
using System.Security;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using FBoxAPI;
using System.IO;

namespace Wpf_Fritzbox_TR064_Commander
{
    public class UI_ViewModel : BaseViewModel
    {
        FritzBoxTR64 fritzBoxTR64;
        
        public ICommand log_in_Button_Clicked_Command { get; private set; }
        public ICommand starte_Wahlrundruf_Clicked_Command { get; private set; }
        public ICommand get_Dects_Clicked_Command { get; private set; }

        public ICommand get_Hosts_Clicked_Command { get; private set; }
        public ICommand dial_Tel_Number_Clicked_Command { get; private set; }

        private bool canExecute = true;

        private int _dect_Number = 0;
        private int _host_Number = 0;
        private string _friBoUsername = "Roland";
        private string _log_In_Message = "";
        private string _host_Entry;
        private string _hostEntries;
        private string _hostsPath;
        private string _dialTelNumber;

        private string _session_Id = "0000000000000000";

        public System.Windows.Media.Brush _log_In_Color = System.Windows.Media.Brushes.LightGray;

        public System.Windows.Media.Brush _DialTelNumber_Color = System.Windows.Media.Brushes.LightGray;
        public SecureString SecurePassword { private get; set; }

        #region Constructor
        public UI_ViewModel()
        {
            log_in_Button_Clicked_Command = new RelayCommand(Log_In_Button_Clicked_Action, param => this.canExecute);
            starte_Wahlrundruf_Clicked_Command = new RelayCommand(Starte_Wahlrundruf_Clicked_Action, param => this.canExecute);
            get_Dects_Clicked_Command = new RelayCommand(Get_Dects_Clicked_Action, param => this.canExecute);
            get_Hosts_Clicked_Command = new RelayCommand(Get_Hosts_Clicked_Action, param => this.canExecute);
            dial_Tel_Number_Clicked_Command = new RelayCommand(Dial_Tel_Number_Clicked_Action, param => this.canExecute); 
        }
        #endregion

        #region Binding DialTelNUmber
        public string DialTelNumber
        {
            get
            {
                return _dialTelNumber;
            }
            set
            {
                if (SetProperty(ref _dialTelNumber, value))
                { }
            }
        }
        #endregion

        #region Binding DialTelNumber_Color
        public System.Windows.Media.Brush DialTelNumber_Color
        {
            get
            {
                return _DialTelNumber_Color;
            }
            set
            {
                if (SetProperty(ref _DialTelNumber_Color, value))
                { }
            }
        }
        #endregion

        #region Binding Log_In_Color
        public System.Windows.Media.Brush Log_In_Color
        {
            get
            {
                return _log_In_Color;
            }
            set
            {
                if (SetProperty(ref _log_In_Color, value))
                { }
            }
        }
        #endregion

        #region Binding FriBoUsername
        public string FriBoUsername
        {
            get
            {
                return _friBoUsername;
            }
            set
            {
                if (SetProperty(ref _friBoUsername, value))
                { }
            }
        }
        #endregion

        #region Binding Log_In_Message
        public string Log_In_Message
        {
            get
            {
                return _log_In_Message;
            }
            set
            {
                if (SetProperty(ref _log_In_Message, value))
                { }
            }
        }

        #endregion

        #region Binding Host_Number
        public int Host_Number
        {
            get
            {
                return _host_Number;
            }
            set
            {
                if (SetProperty(ref _host_Number, value))
                { }
            }
        }
        #endregion

        #region Binding HostsPath
        public string HostsPath
        {
            get
            {
                return _hostsPath;
            }
            set
            {
                if (SetProperty(ref _hostsPath, value))
                { }
            }
        }
        #endregion

        #region Binding Host_Entry
        public string Host_Entry
        {
            get
            {
                return _host_Entry;
            }
            set
            {
                if (SetProperty(ref _host_Entry, value))
                { }
            }
        }
        #endregion

        #region Binding HostEntries
        public string HostEntries
        {
            get
            {
                return _hostEntries;
            }
            set
            {
                if (SetProperty(ref _hostEntries, value))
                { }
            }
        }
        #endregion

        #region Binding Dect_Number
        public int Dect_Number
        {
            get
            {
                return _dect_Number;
            }
            set
            {
                if (SetProperty(ref _dect_Number, value))
                { }
            }
        }
        #endregion

        #region get_Dects_Clicked_Command
        public ICommand Get_Dects_Clicked_Command
        {
            get { return get_Dects_Clicked_Command; }
            set { get_Dects_Clicked_Command = value; }
        }
        #endregion

        #region Get_Dects_Clicked_Action
        private void Get_Dects_Clicked_Action(object obj)
        {
            int numberOfEntries = 0;
            string hostsPath = string.Empty;
            if (fritzBoxTR64.DECT.GetNumberOfDectEntries(ref numberOfEntries))
            {
                Dect_Number = numberOfEntries;
            }          
        }
        #endregion

        #region get_Hosts_Clicked_Command
        public ICommand Get_Hosts_Clicked_Command
        {
            get { return get_Hosts_Clicked_Command; }
            set { get_Hosts_Clicked_Command = value; }
        }
        #endregion

        #region Get_Hosts_Clicked_Action
        private void Get_Hosts_Clicked_Action(object obj)
        {
            int numberOfEntries = 0;
            string hostsPath = string.Empty;
              
            if (fritzBoxTR64.Hosts.GetHostNumberOfEntries(ref numberOfEntries))
            {
                Host_Number = numberOfEntries;
               
            }
            else
            {
                throw new Exception("Getting of Hostsnumber failed");
            }
                
            if (fritzBoxTR64.Hosts.GetHostListPath(ref hostsPath))
            {
                HostsPath = hostsPath;
                
            }

            var hosts = new StringBuilder();

            for (int i = 0; i < numberOfEntries; i++)
            {
                HostEntry hostEntry = null;
                if (fritzBoxTR64.Hosts.GetGenericHostEntry(i, ref hostEntry))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(hostEntry.GetType());
                    var buffer = new StringBuilder();
                    var writer = XmlWriter.Create(buffer);
                    xmlSerializer.Serialize(writer, hostEntry);
                    string xmlString = buffer.ToString();
                    StringReader stringReader = new StringReader(xmlString);
                    XDocument document = XDocument.Load(stringReader);
                    List<string> selectors = new List<string>() { "HostName", "MACAddress", "IPAddress" };
                    var hostEntryProperties = document.Descendants("HostEntry").Elements();                   
                    hosts.Append("<HostEntry>");
                    foreach (XElement property in hostEntryProperties)
                    {
                        if (selectors.Contains(property.Name.ToString()))
                        {
                            hosts.Append(property);
                        }
                    }
                    hosts.Append("</HostEntry>");
                    hosts.Append("\r\n");
                }
                else
                {
                    throw new Exception("Getting of Hosts failed");
                }
            }
            HostEntries = hosts.ToString();  
        }
        #endregion

        #region Dial_Tel_Number_Command
        public ICommand Dial_Tel_Number_Clicked_Command
        {
            get { return dial_Tel_Number_Clicked_Command; }
            set { dial_Tel_Number_Clicked_Command = value; }
        }

        #endregion

        #region Dial_Tel_Number_Clicked_Action
        private void Dial_Tel_Number_Clicked_Action(object obj)
        {
            if (SecurePassword != null && _friBoUsername != null)
            {
                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var nationalPhoneNumber = DialTelNumber;

                PhoneNumbers.PhoneNumber phoneNumber = null;
                try
                {
                    phoneNumber = phoneNumberUtil.Parse(nationalPhoneNumber, "DE");
                }
                catch
                {                
                    DialTelNumber_Color = System.Windows.Media.Brushes.Red;
                }
                
                if (phoneNumber != null && phoneNumberUtil.IsValidNumberForRegion(phoneNumber, "DE") )
                {
                    
                    DialTelNumber_Color = System.Windows.Media.Brushes.LightGreen;                  
                    fritzBoxTR64.X_voip.DialNumber(DialTelNumber);
                }
                else            
                {
                    DialTelNumber_Color = System.Windows.Media.Brushes.Red;
                    return;
                }
                System.Threading.Thread.Sleep(10000);
                fritzBoxTR64.X_voip.DialHangup();
            }
        }
        #endregion

        #region Starte_Wahlrundruf_Clicked_Command
        public ICommand Starte_Wahlrundruf_Clicked_Command
        {
            get { return starte_Wahlrundruf_Clicked_Command; }
            set { starte_Wahlrundruf_Clicked_Command = value; }
        }
        #endregion

        #region Starte_Wahlrundruf_Clicked_Action
        private void Starte_Wahlrundruf_Clicked_Action(object obj)
        {
            if (SecurePassword != null && _friBoUsername != null)
            {
                fritzBoxTR64.X_voip.DialNumber("**9");
                
                System.Threading.Thread.Sleep(7000);
                fritzBoxTR64.X_voip.DialHangup();            
            }
        }
        #endregion

        #region Log_In_Button_Clicked_Command
        public ICommand Log_In_Button_Clicked_Command
        {
            get { return log_in_Button_Clicked_Command; }
            set { log_in_Button_Clicked_Command = value; }
        }
        #endregion

        #region Log_In_Button_Clicked_Action
        private void Log_In_Button_Clicked_Action(object obj)
        {
            if (SecurePassword != null && _friBoUsername != null)
            {
                fritzBoxTR64 = new FritzBoxTR64("fritz.box", 4000, new NetworkCredential(_friBoUsername, SecurePassword ));
                if (fritzBoxTR64.Deviceconfig.GetSessionID(ref _session_Id))
                {
                    Log_In_Message = "";
                    Log_In_Color = System.Windows.Media.Brushes.LightGreen;         
                }
                else
                {                  
                    Log_In_Message = "LogIn failed";
                    Log_In_Color = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                
                Log_In_Message = "LogIn failed";             
                Log_In_Color = System.Windows.Media.Brushes.Red;
            }
        }
        #endregion

        #region method convertToUNSecureString
        public string convertToUNSecureString(SecureString secstrPassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secstrPassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        #endregion
    }
}
