using BE_VR750;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL_VR750
{
    public class DALdesserializar_750VR
    {
        /// <summary> Lee un XML y devuelve la lista de clientes. </summary>
        public List<BECliente_750VR> LeerXml(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Ruta inválida.", nameof(filePath));
            if (!File.Exists(filePath)) throw new FileNotFoundException("No se encontró el archivo XML.", filePath);

            XmlSerializer serializer = new XmlSerializer(typeof(List<BECliente_750VR>));
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return (List<BECliente_750VR>)serializer.Deserialize(fs);
            }
        }
    }
}
