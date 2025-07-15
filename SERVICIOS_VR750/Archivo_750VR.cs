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
using System.Globalization; 

namespace SERVICIOS_VR750
{
    public class Archivo_750VR
    {
        public static void GenerarFacturaPDF(BEfactura_750VR factura)
        {
            // 📂 Carpeta Facturas
            string carpetaFacturas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Facturas");
            if (!Directory.Exists(carpetaFacturas))
                Directory.CreateDirectory(carpetaFacturas);

            string ruta = Path.Combine(carpetaFacturas, $"Factura_{factura.CodFactura_750VR}.pdf");

            // 📄 Documento
            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(ruta, FileMode.Create));
            doc.Open();

            // 🔤 Fuentes
            Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font valorFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            // 🌎 Cultura según idioma actual
            CultureInfo culture;
            switch (Lenguaje_750VR.ObtenerInstancia().IdiomaActual)
            {
                case "Español": culture = new CultureInfo("es-AR"); break;
                case "Ingles": culture = new CultureInfo("en-US"); break;
                case "Portugués": culture = new CultureInfo("pt-BR"); break;
                default: culture = CultureInfo.InvariantCulture; break;
            }

            // 🏷️ Etiquetas traducidas
            string tFactura = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.TituloPDF");
            string lCodigo = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Codigo");
            string lReserva = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Reserva");
            string lFecha = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Fecha");
            string lHora = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Hora");
            string lTotal = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Total");
            string lMetodo = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.MetodoPago");
            string lTitular = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Titular");
            string lGracias = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.MensajeGracias");

            // 🖋️ Título
            var titulo = new iTextSharp.text.Paragraph(tFactura.ToUpper(culture), tituloFont) { Alignment = Element.ALIGN_CENTER };
            doc.Add(titulo);
            doc.Add(new iTextSharp.text.Paragraph(" "));

            // 📊 Tabla
            PdfPTable tabla = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10,
                SpacingAfter = 10
            };
            tabla.SetWidths(new float[] { 35f, 65f });

            void Fila(string label, string valor)
            {
                tabla.AddCell(new PdfPCell(new Phrase(label, labelFont)) { Border = Rectangle.NO_BORDER });
                tabla.AddCell(new PdfPCell(new Phrase(valor, valorFont)) { Border = Rectangle.NO_BORDER });
            }

            Fila($"{lCodigo}:", factura.CodFactura_750VR.ToString(culture));
            Fila($"{lReserva}:", factura.CodReserva_750VR.ToString(culture));
            Fila($"{lFecha}:", factura.fecha_750VR.ToString("d", culture));
            Fila($"{lHora}:", factura.horaEmision_750VR.ToString(@"hh\:mm"));
            Fila($"{lTotal}:", factura.total_750VR.ToString("C", culture));
            Fila($"{lMetodo}:", factura.metodoPago_750VR ?? "-");
            Fila($"{lTitular}:", factura.titular_750VR ?? "-");

            doc.Add(tabla);

            // 🙏 Mensaje final
            var gracias = new iTextSharp.text.Paragraph(lGracias, valorFont) { Alignment = Element.ALIGN_CENTER };
            doc.Add(gracias);

            doc.Close();

            // Abrir el PDF
            try
            {
                Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
            }
            catch { /* ignorar si no puede abrir */ }
        }


    }

}
