using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using TwoFunTwoMe.Models.DTO;
using TwoFunTwoMe_DataAccess;

namespace TwoFunTwoMe.Models.Utility
{
    public class ConvertidorHtmlToPdf
    {
        private TwoFunTwoMe.Models.Manager.Manager manager;
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };
        public PagareContrato TraeDatosContratoPagare(PagareContrato pagareContrato)
        {
            Guid x = Guid.NewGuid();
            manager = new Manager.Manager();
            Solicitudes solicitudes = new Solicitudes();
            var pagareCon = new List<PagareContrato>();
            var datosContrato = new List<PagareContrato>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };

            try
            {


                solicitudes.IdTipoIdentificacion = pagareContrato.IdTipoIdentificacion;
                solicitudes.Identificacion = pagareContrato.Identificacion;

                var dto_DatosCredito = manager.CargarDatosCredito(solicitudes);

                if (dto_DatosCredito.Any())
                {

                    //pagareCon = manager.TraeDocumentoPagare();
                    pagareContrato.IdSolicitud = dto_DatosCredito.FirstOrDefault().IdSolicitud;
                    pagareContrato.fechagenerapagare = DateTime.Today;
                    datosContrato = manager.PagareContrato(pagareContrato);
                    return pagareContrato = datosContrato[0];

                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return null;
        }

        public string CrearPdf(PagareContrato pagareContrato)
        {
            Guid x = Guid.NewGuid();
            manager = new Manager.Manager();
            Utilitario Util = new Utilitario();
            Solicitudes solicitudes = new Solicitudes();
            var pagareCon = new List<PagareContrato>();
            var datosContrato = new List<PagareContrato>();

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "DIRECTORIO";
            dto_Config.llave_Config5 = "CONTRATOPAGARE";

            solicitudes.IdTipoIdentificacion = pagareContrato.IdTipoIdentificacion;
            solicitudes.Identificacion = pagareContrato.Identificacion;

            var dto_DatosCredito = manager.CargarDatosCredito(solicitudes);

            var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
            string URL = dto_interval.Where(y => y.llave_Config5 == "CONTRATOPAGARE").Select(y => y.Dato_Char1).FirstOrDefault();

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                if (dto_DatosCredito.Any())
                {

                    pagareCon = manager.TraeDocumentoPagare();
                    pagareContrato.IdSolicitud = dto_DatosCredito.FirstOrDefault().IdSolicitud;
                    pagareContrato.fechagenerapagare = DateTime.Today;
                    datosContrato = manager.PagareContrato(pagareContrato);
                    pagareContrato.Pagare = pagareCon.FirstOrDefault().Pagare;
                    pagareContrato.Contrato = pagareCon.FirstOrDefault().Contrato;

                    string Contrato = pagareContrato.Contrato;
                    string Pagare = pagareContrato.Pagare;
                    string fileName = @"Pagare y contrato_" + datosContrato[0].Nombre + ".docx";

                    //Remplazo en la cadena los datos para generar el contrato
                    Contrato = Contrato.Replace("<Nombre>", datosContrato[0].Nombre);
                    Contrato = Contrato.Replace("<Identificacion>", datosContrato[0].Identificacion);
                    Contrato = Contrato.Replace("<MontoProductoLetras>", datosContrato[0].MontoProductoLetras);
                    Contrato = Contrato.Replace("<Moneda>", datosContrato[0].Moneda);
                    Contrato = Contrato.Replace("<MontoProducto>", Convert.ToString(datosContrato[0].MontoProducto));
                    Contrato = Contrato.Replace("<CantidadCuotas>", Convert.ToString(datosContrato[0].CantidadCuotas));
                    Contrato = Contrato.Replace("<Frecuencia>", datosContrato[0].Frecuencia);
                    Contrato = Contrato.Replace("<Cuota>", Convert.ToString(datosContrato[0].Cuota));
                    Contrato = Contrato.Replace("<FechaPrimerPagoLetras>", datosContrato[0].FechaPrimerPagoLetras);
                    Contrato = Contrato.Replace("<FechaUltimoPagoLetras>", datosContrato[0].FechaUltimoPagoLetras);
                    Contrato = Contrato.Replace("<Interes>", Convert.ToString(datosContrato[0].Interes));
                    Contrato = Contrato.Replace("<InteresLetras>", datosContrato[0].InteresLetras);
                    Contrato = Contrato.Replace("<CtaCliente>", datosContrato[0].CtaCliente);
                    Contrato = Contrato.Replace("<FechaHoy>", datosContrato[0].FechaHoy);
                    Contrato = Contrato.Replace("<CuotaenLetras>", datosContrato[0].CuotaenLetras);
                    Contrato = Contrato.Replace("<Frecuencia2>", datosContrato[0].Frecuencia2);
                    Contrato = Contrato.Replace("<Frecuencia3>", datosContrato[0].Frecuencia3);
                    Contrato = Contrato.Replace("<CantidadCuotasLetras>", datosContrato[0].CantidadCuotasLetras);

                    //Reemplazo los datos para generar el Pagaré
                    Pagare = Pagare.Replace("<Nombre>", datosContrato[0].Nombre);
                    Pagare = Pagare.Replace("<Identificacion>", datosContrato[0].Identificacion);
                    Pagare = Pagare.Replace("<MontoProductoLetras>", datosContrato[0].MontoProductoLetras);
                    Pagare = Pagare.Replace("<Moneda>", datosContrato[0].Moneda);
                    Pagare = Pagare.Replace("<MontoProductoPagare>", Convert.ToString(datosContrato[0].MontoProductoPagare));
                    Pagare = Pagare.Replace("<MontoProductoLetrasPagare>", Convert.ToString(datosContrato[0].MontoProductoLetrasPagare));
                    Pagare = Pagare.Replace("<CantidadCuotas>", Convert.ToString(datosContrato[0].CantidadCuotas));
                    Pagare = Pagare.Replace("<Frecuencia>", datosContrato[0].Frecuencia);
                    Pagare = Pagare.Replace("<Cuota>", Convert.ToString(datosContrato[0].Cuota));
                    Pagare = Pagare.Replace("<FechaPrimerPagoLetras>", datosContrato[0].FechaPrimerPagoLetras);
                    Pagare = Pagare.Replace("<FechaUltimoPagoLetras>", datosContrato[0].FechaUltimoPagoLetras);
                    Pagare = Pagare.Replace("<Interes>", Convert.ToString(datosContrato[0].Interes));
                    Pagare = Pagare.Replace("<InteresLetras>", datosContrato[0].InteresLetras);
                    Pagare = Pagare.Replace("<CtaCliente>", datosContrato[0].CtaCliente);
                    Pagare = Pagare.Replace("<FechaHoy>", datosContrato[0].FechaHoy);
                    Pagare = Pagare.Replace("<CuotaenLetras>", datosContrato[0].CuotaenLetras);
                    Pagare = Pagare.Replace("<CantidadCuotasLetras>", datosContrato[0].CantidadCuotasLetras);
                    Pagare = Pagare.Replace("<Frecuencia2>", datosContrato[0].Frecuencia2);
                    Pagare = Pagare.Replace("<pagoProximasCuotas>", datosContrato[0].PagoProximasCuotas);

                    //creamos la imagen editada de la firma para agregarla luego al documento
                    if (datosContrato[0].FotoFirma != "")
                    {
                        editarImagen(datosContrato[0].FotoFirma);
                    }

                    //Doy formato al archivo PDF
                    iTextSharp.text.Font fontHeader_1 = FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));
                    //Genero el documento PDF con la libreria iTextSharp
                    using (Document pdfDoc = new Document(PageSize.LETTER, 25f, 25f, 25f, 25f))
                    {
                        iTextSharp.text.Document oDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER);
                        //Escribo el documento en el archivo PDF generado
                        //PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);

                        var OutputPathDocumento = AppDomain.CurrentDomain.BaseDirectory + "/Machote/archivos/Documento-" + datosContrato.FirstOrDefault().Nombre.ToString() + x + ".pdf";
                        using (PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(OutputPathDocumento, FileMode.Create)))
                        {

                            pdfDoc.Open();

                            var espacio = new Paragraph(" ");
                            //Ingreso el texto que corresponde al Contrato
                            var tituloContrato = new Paragraph("Solicitud N°: " + datosContrato.FirstOrDefault().Id.ToString(), FontFactory.GetFont("Calibri", 14, BaseColor.BLACK));
                            tituloContrato.Alignment = Element.ALIGN_CENTER;
                            tituloContrato.Font.Size = 14;
                            pdfDoc.Add(tituloContrato);
                            pdfDoc.Add(espacio);
                            var Contratopdf = new Paragraph(Contrato, fontHeader_1);
                            Contratopdf.Alignment = Element.ALIGN_JUSTIFIED;
                            Contratopdf.Font.Size = 8;
                            pdfDoc.Add(Contratopdf);


                            //si existe foto de la firma lo agregamos al contrato
                            if (datosContrato[0].FotoFirma != "")
                            {
                                //creamos la imagen de la firma para el PDF
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
                                imagen.BorderWidth = 0;
                                imagen.Alignment = Element.ALIGN_LEFT;
                                float percentage = 0.0f;
                                percentage = 70 / imagen.Width;
                                imagen.ScalePercent(percentage * 100);

                                //creamos una tabla para agregar la imagen
                                PdfPTable tablaImagen = new PdfPTable(1); ;
                                PdfPCell c1 = new PdfPCell();
                                PdfPCell c2 = new PdfPCell(imagen);
                                c1.Border = 0;
                                c2.Border = 0;
                                c2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                c2.HorizontalAlignment = Element.ALIGN_LEFT;
                                tablaImagen.AddCell(c1); //celda con espacio a la derecha
                                tablaImagen.AddCell(c2); //celda con la imagen de la firma

                                // Insertamos la tabla con la imagen de la firma en el contrato
                                pdfDoc.Add(tablaImagen);
                            }
                            else
                            {
                                pdfDoc.Add(espacio);
                                pdfDoc.Add(espacio);
                            }

                            //Agregamos la info final del contrato
                            string textoFinal = "               ……………………………………………                                                                                          ……………………………………………\n                                   El DEUDOR                                                                                                                                   LA ACREEDORA\n\n\n                                    " + datosContrato.FirstOrDefault().Identificacion.ToString() + "\n               …………………………………………      \n                                     CÉDULA ";
                            var parrafoFinal = new Paragraph(textoFinal, fontHeader_1);
                            pdfDoc.Add(parrafoFinal);

                            //Ingreso el titulo del Pagaré
                            pdfDoc.NewPage();
                            var SubtituloPdf = new Paragraph("Pagaré N°: " + datosContrato.FirstOrDefault().Id.ToString(), FontFactory.GetFont("Calibri", 14, BaseColor.BLACK));
                            SubtituloPdf.Alignment = Element.ALIGN_CENTER;
                            SubtituloPdf.Font.Size = 14;
                            pdfDoc.Add(SubtituloPdf);
                            pdfDoc.Add(espacio);
                            //Ingreso Texto de Pagare
                            var PagarePdf = new Paragraph(Pagare, FontFactory.GetFont("Calibri", 10, BaseColor.BLACK));
                            PagarePdf.Alignment = Element.ALIGN_JUSTIFIED;
                            PagarePdf.Font.Size = 10;
                            pdfDoc.Add(PagarePdf);

                            if (datosContrato[0].FotoFirma != "")
                            {
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
                                imagen.BorderWidth = 0;
                                imagen.Alignment = Element.ALIGN_LEFT;
                                float percentage = 0.0f;
                                percentage = 70 / imagen.Width;
                                imagen.ScalePercent(percentage * 100);
                                // Insertamos la imagen de la firma en el Pagare
                                PdfPTable tablaImagenPagare = new PdfPTable(5); ;
                                iTextSharp.text.Phrase fraseFirma = new iTextSharp.text.Phrase("Firma Deudor:");
                                fraseFirma.Font.Size = 10;
                                PdfPCell espacioCelda = new PdfPCell();
                                PdfPCell celdaTexto = new PdfPCell(fraseFirma);
                                PdfPCell celdaFirma = new PdfPCell(imagen);
                                espacioCelda.Border = 0;
                                celdaTexto.Border = 0;
                                celdaTexto.VerticalAlignment = Element.ALIGN_MIDDLE;
                                celdaTexto.HorizontalAlignment = Element.ALIGN_LEFT;
                                celdaFirma.Border = 0;
                                celdaFirma.VerticalAlignment = Element.ALIGN_MIDDLE;
                                celdaFirma.HorizontalAlignment = Element.ALIGN_LEFT;
                                tablaImagenPagare.AddCell(celdaTexto); //celda con el texto de la firma
                                tablaImagenPagare.AddCell(celdaFirma); //celda con la imagen de la firma
                                tablaImagenPagare.AddCell(espacioCelda);//celda con espacio en blanco
                                tablaImagenPagare.AddCell(espacioCelda);//celda con espacio en blanco
                                tablaImagenPagare.AddCell(espacioCelda);//celda con espacio en blanco
                                pdfDoc.Add(tablaImagenPagare);
                            }
                            else
                            {
                                var parrafoFirma = new Paragraph("\n\n                     Firma Deudor: \n\n\n", fontHeader_1);
                                parrafoFirma.Font.Size = 10;
                                pdfDoc.Add(parrafoFirma);
                            }
                            //Agregamos la info final del contrato
                            string textoFinalPagare = "                     Nombre Completo:   " + datosContrato[0].Nombre + "\n\n\n                     Número de Cédula:   " + datosContrato[0].Identificacion;
                            var parrafoFinalPagare = new Paragraph(textoFinalPagare, fontHeader_1);
                            parrafoFinalPagare.Font.Size = 10;
                            pdfDoc.Add(parrafoFinalPagare);
                            //Cierro el archivo PDF
                            pdfDoc.Close();
                            wri.Close();
                        }
                        oDoc.Close();
                    }

                    //Proceso para crear y agregar el formulario BCCR a el pdf final
                    var path = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\formulario-bac.html";
                    var pathOriginal = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\formulario-bac-sin-remplazo.html";
                    string html = File.ReadAllText(pathOriginal);
                    string EncodedString = "";
                    string ret = "";
                    string DecodedStringFormulario;
                    string htmlOriginal = html; //este sera el html orinal sin hacer los remplazas para luego de todo el proceso volver a guaradr el archivo html con los valores originales
                    using (StringWriter writer = new StringWriter())
                    {
                        HttpUtility.HtmlEncode(html, writer);

                        //Server.HtmlEncode(html, writer);
                        EncodedString = html.ToString();
                        ret = completarFormularioDomiciliacion(EncodedString, datosContrato[0]); // replazamos el html con la info dinamica
                        DecodedStringFormulario = HttpUtility.HtmlDecode(ret); //Server.HtmlDecode(ret);
                        writer.Close();
                    }
                    using (StreamWriter sw = new StreamWriter(path)) //sobrescribimos el html con la nueva info
                    {
                        sw.WriteLine(DecodedStringFormulario);
                        sw.Close();
                    }
                    //Generamos el pdf del formulario caon base en el archivo html

                    NReco.PdfGenerator.HtmlToPdfConverter pdfFormulario = new NReco.PdfGenerator.HtmlToPdfConverter();
                    pdfFormulario.PageHeight = 279;
                    pdfFormulario.PageWidth = 216;
                    pdfFormulario.GeneratePdfFromFile(path, null, @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosContrato.FirstOrDefault().Nombre + x + ".pdf");

                    //llamamos al metodo interno mergePdfs para unir los pdfs del contrato, el pagare y el formulario
                    string rutaDocumentoFinal = URL + "/" + pagareContrato.Identificacion + "/" + "Contrato_Pagare_" + pagareContrato.Identificacion + ".pdf";


                    if (Util.ValidarFichero(rutaDocumentoFinal) == true)
                    {
                        rutaDocumentoFinal = Util.Renamefile(rutaDocumentoFinal, pagareContrato.Identificacion, ".pdf");
                        string[] archivos = { AppDomain.CurrentDomain.BaseDirectory + "/Machote/archivos/Documento-" + datosContrato.FirstOrDefault().Nombre + x + ".pdf", @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosContrato.FirstOrDefault().Nombre + x + ".pdf" };
                        mergePdfs(rutaDocumentoFinal, archivos);

                    }
                    //volvemos a guaradar el archivo html Original(sin los valores remplazados)
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.WriteLine(htmlOriginal);
                        sw.Close();
                    }
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/Machote/archivos/Documento-" + datosContrato.FirstOrDefault().Nombre + x + ".pdf");
                    File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosContrato.FirstOrDefault().Nombre + x + ".pdf");
                    File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");

                    return rutaDocumentoFinal;

                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                pagareContrato.Mensaje = "ERR";
            }

            return null;
        }
        //public Stream ConvertToBase64(Stream stream)
        //{
        //	Byte[] inArray = new Byte[(int)stream.Length];
        //	Char[] outArray = new Char[(int)(stream.Length * 1.34)];
        //	stream.Read(inArray, 0, (int)stream.Length);
        //	Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
        //	return new MemoryStream(Encoding.UTF8.GetBytes(outArray));
        //}

        private string completarFormularioDomiciliacion(string textoHtml, PagareContrato pagare)
        {

            //sustituimos los valores en el texto html
            textoHtml = textoHtml.Replace("{Nombre-titular-cuenta-cliente}", pagare.Nombre);
            textoHtml = textoHtml.Replace("{email-cliente}", pagare.Correo);
            textoHtml = textoHtml.Replace("{telefono-cliente}", pagare.TelefonoCel.ToString());
            textoHtml = textoHtml.Replace("{codigo-cliente}", pagare.Identificacion.ToString());
            if (pagare.FotoFirma != "")
            {
                textoHtml = textoHtml.Replace("{Foto-Firma}", pagare.FotoFirma);
            }
            else
            {
                textoHtml = textoHtml.Replace("{Foto-Firma}", "");
                //textoHtml = textoHtml.Replace("{Foto-Firma}", "FotoFirma.jpeg");
            }
            //textoHtml = textoHtml.Replace("{monto-total}", pagare.MontoTotal.ToString());
            textoHtml = remplazarCedulaPDF(textoHtml, pagare);
            textoHtml = remplazarCuentaClientePDF(textoHtml, pagare);
            textoHtml = remplazarMontoTotalPDF(textoHtml, pagare);


            return textoHtml;
        }
        private string remplazarCedulaPDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            for (int i = 1; i <= 20; i++)
            {
                if (i <= pagare.Identificacion.Length)
                {
                    caracter = pagare.Identificacion[i - 1] + "";
                    textoHtml = textoHtml.Replace("{a" + i + "}", caracter);
                }
                else
                {
                    textoHtml = textoHtml.Replace("{a" + i + "}", "");
                }
            }


            return textoHtml;
        }
        private string remplazarCuentaClientePDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            for (int i = 1; i <= 22; i++)
            {
                if (i <= pagare.CtaCliente.Length)
                {
                    caracter = pagare.CtaCliente[i - 1] + "";
                    textoHtml = textoHtml.Replace("{b" + i + "}", caracter);
                }
                else
                {
                    textoHtml = textoHtml.Replace("{b" + i + "}", "");
                }
            }

            return textoHtml;
        }
        private string remplazarMontoTotalPDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            int montoEntero = Convert.ToInt32(pagare.Cuota * pagare.CantidadCuotas);
            string monto = montoEntero.ToString();
            int contador = monto.Length;

            for (int i = 9; i >= 1; i--)
            {
                //vamos a poner ceros en los valores despues de la coma.
                if (i > 7)
                {
                    textoHtml = textoHtml.Replace("{c" + i + "}", "0");
                }
                else
                {
                    if (contador > 0)
                    {
                        caracter = monto[contador - 1].ToString();
                        textoHtml = textoHtml.Replace("{c" + i + "}", caracter);
                        contador--;
                    }
                    else
                    {
                        textoHtml = textoHtml.Replace("{c" + i + "}", "");
                    }

                }

            }

            return textoHtml;
        }

        static void editarImagen(string rutaImagen)
        {
            System.Drawing.Bitmap imagen;
            //cargar la imagen de la ruta de la imagen
            imagen = (Bitmap)Bitmap.FromFile(rutaImagen);
            //crear la secuencia de filtros
            AForge.Imaging.Filters.FiltersSequence filter = new AForge.Imaging.Filters.FiltersSequence();
            //agregar los filtros a la secuencia de filtros
            filter.Add(new AForge.Imaging.Filters.ContrastCorrection(5));
            filter.Add(new AForge.Imaging.Filters.BrightnessCorrection(5));
            //aplicar los filtros 
            imagen = filter.Apply(imagen);
            imagen.Save(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
            imagen.Dispose();
        }
        private string remplazarCodigoServicioPDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            string idSolicitud = pagare.Id.ToString();
            if (idSolicitud != null)
            {
                for (int i = 1; i <= 21; i++)
                {
                    if (i <= idSolicitud.Length)
                    {
                        caracter = idSolicitud[i - 1].ToString();
                        textoHtml = textoHtml.Replace("{d" + i + "}", caracter);
                    }
                    else
                    {
                        textoHtml = textoHtml.Replace("{d" + i + "}", "");
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 21; i++)
                {
                    textoHtml = textoHtml.Replace("{d" + i + "}", "");
                }
            }


            return textoHtml;
        }
        private void mergePdfs(string rutaDocumentoFinal, String[] archivos)
        {
            string[] source_files = archivos;
            string result = rutaDocumentoFinal;
            //create Document object
            using (Document document = new Document())
            {
                //create PdfCopy object
                using (PdfCopy copy = new PdfCopy(document, new FileStream(result, FileMode.Create)))
                {
                    //open the document
                    document.Open();
                    //PdfReader variable
                    //PdfReader reader;
                    for (int i = 0; i < source_files.Length; i++)
                    {
                        //create PdfReader object
                        using (PdfReader reader = new PdfReader(source_files[i]))
                        {
                            //merge combine pages
                            for (int page = 1; page <= reader.NumberOfPages; page++)
                                copy.AddPage(copy.GetImportedPage(reader, page));
                            reader.Close();
                        }
                    }

                    //close the document object
                    document.Close();
                    copy.Close();

                }
            }
        }
    }
}