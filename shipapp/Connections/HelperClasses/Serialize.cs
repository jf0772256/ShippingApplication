using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace shipapp.Connections.HelperClasses
{
    /// <summary>
    /// Class to try to minimize the ability for a general lookie leu from seeing contentce with out strong encryption that was unfeesible for clients systems and needs
    /// </summary>
    class Serialize
    {
        /// <summary>
        /// Constructor (Note this is statically constructed fro project wide use, see DataConnectionClass)
        /// </summary>
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
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, value);
                    string retVal = Convert.ToBase64String(ms.ToArray());
                    ms.Close();
                    return retVal;
                }
                catch (Exception)
                {
                    return null;
                }
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
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    object retVal = bf.Deserialize(ms);
                    ms.Close();
                    return (string)retVal;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
