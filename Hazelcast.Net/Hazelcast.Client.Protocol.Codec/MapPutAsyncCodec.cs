using Hazelcast.Client.Protocol;
using Hazelcast.Client.Protocol.Util;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;
using System.Collections.Generic;

namespace Hazelcast.Client.Protocol.Codec
{
    internal sealed class MapPutAsyncCodec
    {

        public static readonly MapMessageType RequestType = MapMessageType.MapPutAsync;
        public const int ResponseType = 105;
        public const bool Retryable = false;

        //************************ REQUEST *************************//

        public class RequestParameters
        {
            public static readonly MapMessageType TYPE = RequestType;
            public string name;
            public IData key;
            public IData value;
            public long threadId;
            public long ttl;

            public static int CalculateDataSize(string name, IData key, IData value, long threadId, long ttl)
            {
                int dataSize = ClientMessage.HeaderSize;
                dataSize += ParameterUtil.CalculateDataSize(name);
                dataSize += ParameterUtil.CalculateDataSize(key);
                dataSize += ParameterUtil.CalculateDataSize(value);
                dataSize += Bits.LongSizeInBytes;
                dataSize += Bits.LongSizeInBytes;
                return dataSize;
            }
        }

        public static ClientMessage EncodeRequest(string name, IData key, IData value, long threadId, long ttl)
        {
            int requiredDataSize = RequestParameters.CalculateDataSize(name, key, value, threadId, ttl);
            ClientMessage clientMessage = ClientMessage.CreateForEncode(requiredDataSize);
            clientMessage.SetMessageType((int)RequestType);
            clientMessage.SetRetryable(Retryable);
            clientMessage.Set(name);
            clientMessage.Set(key);
            clientMessage.Set(value);
            clientMessage.Set(threadId);
            clientMessage.Set(ttl);
            clientMessage.UpdateFrameLength();
            return clientMessage;
        }

        //************************ RESPONSE *************************//


        public class ResponseParameters
        {
            public IData response;
        }

        public static ResponseParameters DecodeResponse(IClientMessage clientMessage)
        {
            ResponseParameters parameters = new ResponseParameters();
            IData response = null;
            bool response_isNull = clientMessage.GetBoolean();
            if (!response_isNull)
            {
            response = clientMessage.GetData();
            parameters.response = response;
            }
            return parameters;
        }

    }
}