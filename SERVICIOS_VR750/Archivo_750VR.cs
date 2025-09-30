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
        public static void GenerarBitacoraPDF(List<BEbitacora_750VR> eventos)
        {
            // 🗂 Guardar en carpeta "Bitacoras" dentro del bin
            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bitacoras");
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);

            string path = Path.Combine(carpeta, $"Bitacora_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            // 🌎 Cultura según idioma actual
            CultureInfo culture;
            switch (Lenguaje_750VR.ObtenerInstancia().IdiomaActual)
            {
                case "Español": culture = new CultureInfo("es-AR"); break;
                case "Ingles": culture = new CultureInfo("en-US"); break;
                case "Portugués": culture = new CultureInfo("pt-BR"); break;
                default: culture = CultureInfo.InvariantCulture; break;
            }

            // 🏷️ Etiquetas traducidas (agregá estas claves al JSON)
            string tTitulo = Lenguaje_750VR.ObtenerEtiqueta("BitacoraPDF.Titulo");
            string tLogin = Lenguaje_750VR.ObtenerEtiqueta("BitacoraPDF.Login");
            string tFecha = Lenguaje_750VR.ObtenerEtiqueta("BitacoraPDF.Fecha");
            string tHora = Lenguaje_750VR.ObtenerEtiqueta("BitacoraPDF.Hora");
            string tModulo = Lenguaje_750VR.ObtenerEtiqueta("BitacoraPDF.Modulo");
            string tEvento = Lenguaje_750VR.ObtenerEtiqueta("BitacoraPDF.Evento");
            string tCriticidad = Lenguaje_750VR.ObtenerEtiqueta("BitacoraPDF.Criticidad");

            // 📄 Documento
            Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            // 🔤 Fuentes simples
            var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            // 🖋️ Título
            var titulo = new iTextSharp.text.Paragraph(tTitulo, fontTitulo) { Alignment = Element.ALIGN_CENTER };
            doc.Add(titulo);
            doc.Add(new iTextSharp.text.Paragraph(" "));

            // 📊 Tabla
            PdfPTable tabla = new PdfPTable(6) { WidthPercentage = 100 };
            tabla.SetWidths(new float[] { 18f, 14f, 12f, 18f, 24f, 14f });

            // Cabeceras
            foreach (var h in new[] { tLogin, tFecha, tHora, tModulo, tEvento, tCriticidad })
            {
                var cellH = new PdfPCell(new Phrase(h, fontHeader)) { BackgroundColor = BaseColor.LIGHT_GRAY };
                tabla.AddCell(cellH);
            }

            // Filas
            foreach (var ev in eventos)
            {
                // Fecha según cultura (corta) y Hora según cultura
                string fechaStr = ev.Fecha.ToString("d", culture);
                string horaStr = DateTime.Today.Add(ev.Hora).ToString("t", culture); // TimeSpan -> hora local corta

                tabla.AddCell(new PdfPCell(new Phrase(ev.Login ?? "-", fontCell)));
                tabla.AddCell(new PdfPCell(new Phrase(fechaStr, fontCell)));
                tabla.AddCell(new PdfPCell(new Phrase(horaStr, fontCell)));
                tabla.AddCell(new PdfPCell(new Phrase(ev.Modulo ?? "-", fontCell)));
                tabla.AddCell(new PdfPCell(new Phrase(ev.Evento ?? "-", fontCell)));
                tabla.AddCell(new PdfPCell(new Phrase(ev.Criticidad.ToString(), fontCell)));
            }

            doc.Add(tabla);
            doc.Close();

            try { Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }); } catch { }
        }


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
