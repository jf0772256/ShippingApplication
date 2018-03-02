using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace shipapp.Connections.HelperClasses
{
    class Serialize
    {
        public Serialize()
        {

        }
        /// <summary>
        /// Takes regular readable string value and serializes the value and returns that value to the caller to be placed in the database
        /// </summary>
        /// <param name="value">String: Value to be serialized</param>
        /// <returns>String: serialized version of value</returns>
        public string SerializeValue(string value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, value);
                string retVal = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return retVal;
            }
        }
        /// <summary>
        /// Takes serialized string value and serializes the value and returns the deserialized value to the caller to be returned to user or object
        /// </summary>
        /// <param name="value">String: serialized version of value to be desealized</param>
        /// <returns>String: Value after being de serialized</returns>
        public string DeSerializeValue(string value)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(value)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object retVal = bf.Deserialize(ms);
                ms.Close();
                return (string)retVal;
            }
        }
    }
}
