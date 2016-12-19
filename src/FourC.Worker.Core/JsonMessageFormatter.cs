using System;
using System.IO;
using System.Messaging;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace FourC.Worker.Core
{
    // From: https://gist.githubusercontent.com/jchadwick/2430984/raw/b1fe3bd8336b45722e83b249af80b0de3240603b/JsonMessageFormatter.cs
    public class JsonMessageFormatter : IMessageFormatter
    {
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly JsonSerializerSettings _serializerSettings;

        public JsonMessageFormatter(IOptions<MvcJsonOptions> options) : this(options.Value.SerializerSettings)
        {
        }

        public JsonMessageFormatter(JsonSerializerSettings serializerSettings)
        {
            _serializerSettings = serializerSettings;
        }


        public bool CanRead(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            var stream = message.BodyStream;

            return stream != null
                   && stream.CanRead
                   && stream.Length > 0;
        }

        public object Clone()
        {
            return new JsonMessageFormatter(_serializerSettings);
        }

        public object Read(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (CanRead(message) == false)
                return null;

            using (var reader = new StreamReader(message.BodyStream, _encoding))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject(json, _serializerSettings);
            }
        }

        public void Write(Message message, object obj)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (obj == null)
                throw new ArgumentNullException("obj");

            string json = JsonConvert.SerializeObject(obj, Formatting.None, _serializerSettings);

            message.BodyStream = new MemoryStream(_encoding.GetBytes(json));

            //Need to reset the body type, in case the same message
            //is reused by some other formatter.
            message.BodyType = 0;
        }
    }
}