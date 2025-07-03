using BE_VR750;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SERVICIOS_VR750
{
    public class Archivo_750VR
    {
        public static void GenerarFacturaPDF(BEfactura_750VR factura)
        {
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"Factura_{factura.CodFactura_750VR}.pdf");

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(ruta, FileMode.Create));
            doc.Open();

            Font titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            doc.Add(new iTextSharp.text.Paragraph("FACTURA", titulo));
            doc.Add(new iTextSharp.text.Paragraph(" ")); // Espacio en blanco

            doc.Add(new iTextSharp.text.Paragraph($"Código factura: {factura.CodFactura_750VR}", normal));
            doc.Add(new iTextSharp.text.Paragraph($"Reserva asociada: {factura.CodReserva_750VR}", normal));
            doc.Add(new iTextSharp.text.Paragraph($"Fecha de emisión: {factura.fecha_750VR.ToShortDateString()}", normal));
            doc.Add(new iTextSharp.text.Paragraph($"Hora: {factura.horaEmision_750VR}", normal));
            doc.Add(new iTextSharp.text.Paragraph($"Total: ${factura.total_750VR}", normal));
            doc.Add(new iTextSharp.text.Paragraph($"Método de pago: {factura.metodoPago_750VR}", normal));
            doc.Add(new iTextSharp.text.Paragraph($"Titular: {factura.titular_750VR}", normal));

            doc.Add(new iTextSharp.text.Paragraph(" "));
            doc.Add(new iTextSharp.text.Paragraph("¡Gracias por su compra!", normal));

            doc.Close();

            Process.Start(ruta); // Abre el PDF automáticamente
        }


    }
    
}
