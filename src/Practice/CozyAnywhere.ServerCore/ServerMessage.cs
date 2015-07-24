﻿using CozyAnywhere.Protocol;
using CozyAnywhere.Protocol.Messages;
using NetworkHelper;
using CozyAnywhere.PluginBase;
using NetworkProtocol;

namespace CozyAnywhere.ServerCore
{
    public partial class AnywhereServer
    {
        public void InitServerMessage()
        {
            MessageReader.RegisterType<CozyAnywhere.Protocol.Messages.CommandMessage>(MessageId.CommandMessage);
            MessageReader.RegisterType<PluginLoadMessage>(MessageId.PluginLoadMessage);
        }

        public void OnCommandMessage(IMessage msg)
        {
            var comm = (CozyAnywhere.Protocol.Messages.CommandMessage)msg;

            if (comm.Command != null)
            {
                var result = ServerPluginMgr.ParsePluginCommand(comm.Command);

                if (result != null)
                {
                    var rspMsg = new CommandMessageRsp()
                    {
                        PluginName  = result.PluginName,
                        MethodName  = result.MethodName,
                        RspType     = result.MethodReturnValue.DataType,
                    };

                    if(rspMsg.RspType == PluginMethodReturnValueType.StringDataType)
                    {
                        rspMsg.StringCommandRsp = result.MethodReturnValue.Data as string;
                    }
                    else if(rspMsg.RspType == PluginMethodReturnValueType.BinaryDataType)
                    {
                        rspMsg.BinaryCommandRsp = result.MethodReturnValue.Data as byte[];
                    }

                    client.SendMessage(rspMsg);
                }
            }
        }

        public void OnPluginLoadMessage(IMessage msg)
        {
            var loadMsg = (PluginLoadMessage)msg;

            var list = EnumPluginFolder();
            ServerPluginMgr.AddPluginsWithFileNames(list);

            var rspMsg = new PluginQueryMessage()
            {
                Plugins = ServerPluginMgr.AllPluginName(),
            };
            client.SendMessage(rspMsg);
        }
    }
}