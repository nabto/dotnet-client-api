using System;
using System.Xml;
using System.Xml.Serialization;

namespace Nabto.Client.QueryModel
{
    public partial class unabto_queries
    {
        /// <summary>
        /// Tries to deserialize the specified device query definition file.
        /// </summary>
        /// <param name="path">Path to a query defintion file (a device specific unabto_queries.xml file).</param>
        /// <param name="queries">The deserialized query definition or null.</param>
        /// <returns>True if deserialization succeeded.</returns>
        static public bool TryDeserialize(string path, out unabto_queries queries)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(unabto_queries));
                using (XmlReader xmlReader = XmlReader.Create(path))
                {
                    queries = (unabto_queries)xmlSerializer.Deserialize(xmlReader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                queries = null;
                return false;
            }

            return true;
        }
    }
}
