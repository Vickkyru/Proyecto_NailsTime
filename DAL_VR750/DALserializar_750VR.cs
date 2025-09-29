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
    public class DALserializar_750VR
    {
        /// <summary> Serializa a XML la lista de clientes en la ruta indicada. Crea la carpeta si no existe. </summary>
        public void GuardarXml(string filePath, List<BECliente_750VR> clientes)
        {
            if (clientes == null) throw new ArgumentNullException(nameof(clientes));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Ruta inválida.", nameof(filePath));

            string dir = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(dir)) throw new DirectoryNotFoundException("No se pudo resolver la carpeta de destino.");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            XmlSerializer serializer = new XmlSerializer(typeof(List<BECliente_750VR>));
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, clientes);
            }
        }
    }
}
