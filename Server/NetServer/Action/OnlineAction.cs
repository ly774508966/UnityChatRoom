﻿using System;
using System.Collections.Generic;
using NetServer.Data;
using NetServer.Session;

namespace NetServer.Action
{
    public class OnlineAction : ActionBase
    {
        private const int ACTIONTYPE = 100;

        public override int ActionType { get { return ACTIONTYPE; } }

        public override bool Check(ActionParameter parameter)
        {
            return true;
        }

        public override bool Process(ActionParameter parameter)
        {
            List<string> onlineList = new List<string>();
            foreach (var session in SessionClientPool.GetOnlineSession())
            {
                onlineList.Add(session.GetRemoteAddress());
            }
            DynamicBuffer buffer = new DynamicBuffer();
            buffer.WriteObject(onlineList);
            DataPackage packet = new DataPackage(buffer, 100);
            Session.Send(packet);
            return true;
        }
    }
}
