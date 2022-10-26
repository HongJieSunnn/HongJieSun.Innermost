﻿namespace MongoDB.Driver
{
    public static class IClientSessionHandleExtensions
    {
        public static string GetSessionId(this IClientSessionHandle sessionHandle)
        {
            return sessionHandle.ServerSession.Id["id"].ToString()!;
        }
    }
}