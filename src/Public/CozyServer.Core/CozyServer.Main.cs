﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using CozyServer.Plugin;

namespace CozyServer.Core
{
    public partial class CozyServer
    {
        public const int MaxBlockSize = 8192;

        private NetServer InnerServer { get; set; } 

        public int MaximumConnections
        {
            get
            {
                if (InnerServer == null) throw new NullReferenceException("Server is null");
                return InnerServer.Configuration.MaximumConnections;
            }
            set
            {
                if (InnerServer == null) throw new NullReferenceException("Server is null");
                InnerServer.Configuration.MaximumConnections = value;
            }
        }

        public int Port
        {
            get
            {
                if (InnerServer == null) throw new NullReferenceException("Server is null");
                return InnerServer.Port;
            }
        }

        public bool IsRunning
        {
            get
            {
                return InnerServer.Status == NetPeerStatus.Running;
            }
        }

        public bool IsInitComplete { get; set; }

        public PluginManager PluginMgr { get; set; } = new PluginManager();

        public CozyServer(string ServerName, int MaxConnect, int port)
        {
            if(ServerName == null || ServerName.Length == 0)
            {
                throw new NullReferenceException("Server Name is null");
            }
            var config  = new NetPeerConfiguration(ServerName);
            config.Port = port;
            InnerServer = new NetServer(config);

            Init();
        }

        private void Init()
        {
            LoadPlugin();

            IsInitComplete = true;
        }

        public void Listen()
        {
            if(IsInitComplete && IsRunning)
            {
                throw new Exception("Server is already running");
            }
            InnerServer.Start();
        }

        public void Connect(string ip, int port)
        {
            if (IsInitComplete)
            {
                InnerServer.Connect(ip, port);
            }
        }

        public void Shutdown()
        {
            if(IsInitComplete && IsRunning)
            {
                InnerServer.Shutdown("ShutDown");
            }
        }

        public void RecivePacket()
        {
            NetIncomingMessage msg;
            while ((msg = InnerServer.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryRequest:
                        InnerServer.SendDiscoveryResponse(null, msg.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        OnStatusMessage(msg);
                        break;
                    case NetIncomingMessageType.Data:
                        OnDataMessage(msg);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnStatusMessage(NetIncomingMessage buff)
        {
            PluginMgr.NotifyStatus(InnerServer, buff);
        }

        private void OnDataMessage(NetIncomingMessage buff)
        {
            PluginMgr.NotifyData(InnerServer, buff);
        }
    }
}