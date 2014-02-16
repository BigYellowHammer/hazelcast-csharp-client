﻿using System;
using Hazelcast.IO;
using Hazelcast.IO.Serialization;
using Hazelcast.Serialization.Hook;

namespace Hazelcast.Client.Request.Base
{
    /// <summary>
    ///     Server response wrapper
    /// </summary>
    public class ClientResponse : IdentifiedDataSerializable,IIdentifiedDataSerializable
    {
        private int _callId;
        private bool _event;
        private Data _response;

        private GenericError _error;

        public ClientResponse()
        {
        }

        public ClientResponse(Data response, int callId, bool _event = false)
        {
            _response = response;
            _callId = callId;
            this._event = _event;
        }

        public int GetFactoryId()
        {
            return ClientDataSerializerHook.Id;
        }

        public int GetId()
        {
            return ClientDataSerializerHook.ClientResponse;
        }

        public void WriteData(IObjectDataOutput output)
        {
            throw new NotSupportedException();
        }

        public void ReadData(IObjectDataInput input)
        {
            _callId = input.ReadInt();
            _event = input.ReadBoolean();
            var isError = input.ReadBoolean();
            if (isError)
            {
                _error = input.ReadObject<GenericError>();
                _response = null;
            }
            else
            {
                _response = new Data();
                _response.ReadData(input);
            }
        }

        public GenericError Error
        {
            get { return _error; }
        }

        public Data Response
        {
            get { return _response; }
        }

        public bool IsEvent
        {
            get { return _event; }
        }

        public int CallId
        {
            get { return _callId; }
        }
    }
}