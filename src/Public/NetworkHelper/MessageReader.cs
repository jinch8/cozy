﻿using Lidgren.Network;
using NetworkProtocol;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NetworkHelper
{
    public static class MessageReader
    {
        private static Dictionary<uint, Type> IdToTypeDictionary    = new Dictionary<uint, Type>();
        private static object ObjLocker                             = new object();

        public static void RegisterType<T>(uint id)
            where T : IMessage
        {
            Type t = typeof(T);
            lock (ObjLocker)
            {
                IdToTypeDictionary[id] = t;
            }
        }

        public static void RegisterType(Type type, uint id)
        {
            lock(ObjLocker)
            {
                IdToTypeDictionary[id] = type;
            }
        }

        public static IMessage GetTypeInstance(uint id, NetBuffer stream)
        {
            lock (ObjLocker)
            {
                if (IdToTypeDictionary.ContainsKey(id))
                {
                    var instance = (IMessage)Activator.CreateInstance(IdToTypeDictionary[id]);
                    instance.Read(stream);
                    return instance;
                }
                throw new KeyNotFoundException("Unknow Type");
            }
        }

        public static void RegisterTypeWithAssembly(string Ass, string Ns)
        {
            Assembly asm = Assembly.Load(Ass);
            if (asm != null)
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (type.Namespace == Ns)
                    {
                        uint id = ((IMessage)Activator.CreateInstance(type)).Id;
                        RegisterType(type, id);
                    }
                }
            }
        }
    }
}