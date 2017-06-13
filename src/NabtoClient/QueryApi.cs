//using System.Linq;
//using Nabto.Client.QueryModel;
//using System;
//using System.Diagnostics;
//using System.Dynamic;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
//using System.Text;

//namespace Nabto.Client
//{
//    /// <summary>
//    /// THIS CLASS IS EXPERIMENTAL!
//    /// Represents a dynamic query interface to a Nabto device.
//    /// </summary>
//    public class QueryApi : DynamicObject
//    {
//        Session session;
//        string scheme;
//        string deviceId;

//        string queryDefinitionFileName;

//        unabto_queriesQuery[] queries;

//        /// <summary>
//        /// EXPERIMENTAL!
//        /// Creates a dynamic query interface to the specified device.
//        /// </summary>
//        /// <param name="session"></param>
//        /// <param name="deviceId"></param>
//        public QueryApi(Session session, string deviceId)
//        {
//            this.session = session;

//            if (deviceId.Contains("://"))
//            {
//                var parts = deviceId.Split(new string[] { "://" }, System.StringSplitOptions.RemoveEmptyEntries);

//                scheme = parts[0];
//                this.deviceId = parts[1];
//            }
//            else
//            {
//                scheme = "nabto";
//                this.deviceId = deviceId;
//            }

//            queryDefinitionFileName = Path.Combine(ClientEnvironment.HomeDirectory, "html_dd", this.deviceId, "nabto", "unabto_queries.xml");
//        }

//        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
//        {
//            if (queries == null)
//            {
//                LoadQueryDefinitions();
//            }

//            var queryName = binder.Name.ToLower();
//            var queryAlternativeName = queryName + ".json";

//            var query = (from q in queries where q.name == queryName || q.name == queryAlternativeName select q).SingleOrDefault();
//            if (query == null)
//            {
//                result = null;
//                return false;
//            }

//            var expectedParameterCount = query.request.parameter == null ? 0 : query.request.parameter.Length;
//            var actualParameterCount = args == null ? 0 : args.Length;
//            if (expectedParameterCount != actualParameterCount)
//            {
//                throw new ArgumentException("Bad number of arguments.");
//            }

//            // create request string
//            var requestUrl = new StringBuilder(string.Format("{0}://{1}/{2}?", scheme, deviceId, query.name));
//            // ...add request parameters
//            for (var i = 0; i < args.Length; i++)
//            {
//                var parameter = query.request.parameter[i];
//                //if(query.request.parameter[i].

//                requestUrl.Append(string.Format("{0}={1}", parameter.name, args[i]));

//                if (i < (args.Length - 1))
//                {
//                    requestUrl.Append("&");
//                }
//            }

//            string responseMimeType;
//            var responseBytes = session.FetchUrl(requestUrl.ToString(), out responseMimeType);
//            var responseStream = new MemoryStream(responseBytes);

//            // verify response mime type
//            Debug.WriteLine("Response MIME type: " + responseMimeType);

//            var jsonSerializer = new DataContractJsonSerializer(typeof(QueryResponse));
//            var response = jsonSerializer.ReadObject(responseStream) as QueryResponse;

//            //dynamic response = new ExpandoObject();
//            result = response;
//            return true;
//        }

//        void GetPagedList(string listName, int startingIndex, int numberOfEntries)
//        {

//        }

//        void LoadQueryDefinitions()
//        {
//            if (File.Exists(queryDefinitionFileName) == false)
//            {
//                session.FetchUrl(string.Format("{0}://{1}", scheme, deviceId));

//                if (File.Exists(queryDefinitionFileName) == false)
//                {
//                    throw new NabtoClientException(Interop.NabtoStatus.Failed, "Unable to acquire query definition file.");
//                }
//            }

//            unabto_queries q;
//            if (unabto_queries.TryDeserialize(queryDefinitionFileName, out q) == false)
//            {
//                throw new NabtoClientException(Interop.NabtoStatus.Failed, "Unable to deserialize query definition file.");
//            }

//            queries = q.query;
//        }
//    }

//    [DataContract]
//    class QueryResponseError
//    {
//        [DataMember(Name = "event")]
//        public int Event { get; set; }

//        [DataMember(Name = "header")]
//        public string Header { get; set; }

//        [DataMember(Name = "detail")]
//        public string Detail { get; set; }

//        [DataMember(Name = "body")]
//        public string Body { get; set; }
//    }

//    [DataContract]
//    class QueryResponse
//    {
//        [DataMember(Name = "error")]
//        public QueryResponseError Error { get; set; }

//        [DataMember(Name = "request")]
//        public unabto_queriesQueryRequest Request { get; set; }

//        [DataMember(Name = "response")]
//        public unabto_queriesQueryResponse Response { get; set; }
//    }

//    [DataContract]
//    partial class unabto_queriesQueryRequest
//    {
//    }

//    [DataContract]
//    partial class unabto_queriesQueryResponse
//    {
//    }
//}