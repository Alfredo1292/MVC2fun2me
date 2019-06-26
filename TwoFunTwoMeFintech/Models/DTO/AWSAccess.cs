using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Runtime.CredentialManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;


namespace TwoFunTwoMeFintech.Models.DTO
{
    public class AWSAccess
    {

        public Solicitudes GetTestAsync(List<Tab_ConfigSys> Tab_ConfigSys, Solicitudes sol)
        {

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(sol),
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };
            Solicitudes _Solicitudes = new Solicitudes();

            var options = new CredentialProfileOptions
            {
                AccessKey = Tab_ConfigSys[0].llave_Config1,
                SecretKey = Tab_ConfigSys[0].llave_Config2
            };
            try
            {

                var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("AWSProfileName", options);
                profile.Region = RegionEndpoint.USWest1;
                var netSDKFile = new NetSDKCredentialsFile();
                netSDKFile.RegisterProfile(profile);

                float similarityThreshold = 70F;
                //String sourceImage = sol.arrImageSelfie;
                //String targetImage = sol.UrlFotoCedula;

                //using (AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(Tab_ConfigSys[0].llave_Config1, Tab_ConfigSys[0].llave_Config2, RegionEndpoint.USWest1))
                using (AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(Tab_ConfigSys[0].llave_Config1, Tab_ConfigSys[0].llave_Config2))
                {

                    Amazon.Rekognition.Model.Image imageSource = new Amazon.Rekognition.Model.Image();

                    //using (FileStream fs = new FileStream(new MemoryStream(bytes), FileMode.Open, FileAccess.Read))
                    //{
                    // byte[] data = new byte[fs.Length];
                    //  fs.Read(data, 0, (int)fs.Length);
                    imageSource.Bytes = new MemoryStream(sol.arrImageSelfie);
                    // }


                    Amazon.Rekognition.Model.Image imageTarget = new Amazon.Rekognition.Model.Image();

                    // using (FileStream fs = new FileStream(targetImage, FileMode.Open, FileAccess.Read))
                    //{
                    //  byte[] data = new byte[fs.Length];
                    //  data = new byte[fs.Length];
                    //  fs.Read(data, 0, (int)fs.Length);
                    imageTarget.Bytes = new MemoryStream(sol.arrImageCedulaFrontal);
                    // }


                    CompareFacesRequest compareFacesRequest = new CompareFacesRequest()
                    {
                        SourceImage = imageSource,
                        TargetImage = imageTarget,
                        SimilarityThreshold = similarityThreshold
                    };

                    // Call operation
                    CompareFacesResponse compareFacesResponse = rekognitionClient.CompareFaces(compareFacesRequest);

                    // Display results
                    //foreach (CompareFacesMatch match in compareFacesResponse.FaceMatches)
                    compareFacesResponse.FaceMatches.ForEach(match =>
                    {
                        ComparedFace face = match.Face;

                        BoundingBox position = face.BoundingBox;

                        _Solicitudes.PorcentMatched = face.Confidence;
                        _Solicitudes.PositionLeft = position.Left;
                        _Solicitudes.PositionTop = position.Top;
                    });

                    _Solicitudes.IdTipoIdentificacion = sol.IdTipoIdentificacion;
                    _Solicitudes.Identificacion = sol.Identificacion;

                    if (_Solicitudes.PorcentMatched == 0 || _Solicitudes.PorcentMatched == null)
                    {
                        _Solicitudes.UnMatchedFace = compareFacesResponse.UnmatchedFaces[0].Confidence;
                    }
                    else
                    {
                        _Solicitudes.UnMatchedFace = 0;
                    }
                    _Solicitudes.ImageRotationSource = compareFacesResponse.SourceImageOrientationCorrection;
                    _Solicitudes.ImageRotationTarget = compareFacesResponse.TargetImageOrientationCorrection;
                }
                return _Solicitudes;

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.IS_TELEGRAM = true;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
                _Solicitudes.Mensaje = "ERR_imageTarget";
                throw;
            }
        }

        public Solicitudes DetectText(List<Tab_ConfigSys> Tab_ConfigSys, byte[] bytes, string Identificacion, int IdTipoIdentificacion, int? OPC)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            bool resp = false;
            var param = new
            {
                UriFoto = bytes,
                TipoIdentificacion = IdTipoIdentificacion,
                cedula = Identificacion,
                OPCe = OPC
            };
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                FEC_CREACION = DateTime.Now,
                STR_PARAMETROS = JsonConvert.SerializeObject(param),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString()
            };
            Solicitudes _Solicitudes = new Solicitudes();
            _Solicitudes.Identificacion = Identificacion;
            ManagerSolcitudes managerSolcitudes = new ManagerSolcitudes();
            var FecVencCedula = managerSolcitudes.ConsultaFechaVencimientoCedula(_Solicitudes);

            var options = new CredentialProfileOptions
            {
                AccessKey = Tab_ConfigSys[0].llave_Config1,
                SecretKey = Tab_ConfigSys[0].llave_Config2
            };

            try
            {
                var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("AWSProfileName", options);
                profile.Region = RegionEndpoint.USWest1;
                var netSDKFile = new NetSDKCredentialsFile();
                netSDKFile.RegisterProfile(profile);

                using (AmazonRekognitionClient rekoClient = new AmazonRekognitionClient(Tab_ConfigSys[0].llave_Config1, Tab_ConfigSys[0].llave_Config2, RegionEndpoint.USEast1))
                {
                    Amazon.Rekognition.Model.Image img = new Amazon.Rekognition.Model.Image();

                    img.Bytes = new MemoryStream(bytes);

                    DetectTextRequest dfr = new DetectTextRequest();

                    dfr.Image = img;
                    var outcome = rekoClient.DetectText(dfr);
                    bool dia = false;
                    bool mes = false;
                    bool anio = false;
                    foreach (var texto in outcome.TextDetections)
                    {
                        string cedula = "";

                        cedula = texto.DetectedText;
                        cedula = cedula.Replace(" ", "").Trim();


                        var cedresp = cedula.Split(':');
                        var respuesta = cedresp.Where(x => x.ToString().Equals(Identificacion) || cedula.Equals(Identificacion)).Any();
                        if (respuesta)
                        {
                            resp = respuesta;
                        }
                        if (FecVencCedula != null)
                        {
                            var resDia = cedresp.Where(x => x.ToString().Equals(Convert.ToString(FecVencCedula.Dia)) || cedula.Equals(Convert.ToString(FecVencCedula.Dia))).Any();
                            if (resDia)
                            {
                                dia = resDia;
                            }
                            var resMes = cedresp.Where(x => x.ToString().Equals(Convert.ToString(FecVencCedula.Mes)) || cedula.Equals(Convert.ToString(FecVencCedula.Mes))).Any();
                            if (resMes)
                            {
                                mes = resMes;
                            }
                            var resAnio = cedresp.Where(x => x.ToString().Equals(Convert.ToString(FecVencCedula.Anio)) || cedula.Equals(Convert.ToString(FecVencCedula.Anio))).Any();
                            if (resAnio)
                            {
                                anio = resAnio;
                            }

                            if (respuesta == true && dia == true && mes == true && anio == true)
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (respuesta) break;
                        }
                    }
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(outcome.TextDetections.Select(x => x.DetectedText));


                    _Solicitudes.Result = resp;
                    _Solicitudes.Dia = Convert.ToInt32(dia);
                    _Solicitudes.Mes = Convert.ToInt32(mes);
                    _Solicitudes.Anio = Convert.ToInt32(anio);
                    _Solicitudes.DetectedText = jsonString;

                }
                //return outcome.TextDetections.Select(x => x.DetectedText).ToList();
                return _Solicitudes;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.IS_TELEGRAM = true;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
                _Solicitudes.Mensaje = "ERR_Detect Text";
                throw;
            }
        }

        public bool IsBlank(string imageFileName)
        {
            double stdDev = GetStdDev(imageFileName);
            return stdDev < 100000;
        }

        public static double GetStdDev(string imageFileName)
        {
            double total = 0, totalVariance = 0;
            int count = 0;
            double stdDev = 0;

            // First get all the bytes
            using (Bitmap b = new Bitmap(imageFileName))
            {
                BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);
                int stride = bmData.Stride;
                IntPtr Scan0 = bmData.Scan0;
                unsafe
                {
                    byte* p = (byte*)(void*)Scan0;
                    int nOffset = stride - b.Width * 3;
                    for (int y = 0; y < b.Height; ++y)
                    {
                        for (int x = 0; x < b.Width; ++x)
                        {
                            count++;

                            byte blue = p[0];
                            byte green = p[1];
                            byte red = p[2];

                            int pixelValue = Color.FromArgb(0, red, green, blue).ToArgb();
                            total += pixelValue;
                            double avg = total / count;
                            totalVariance += Math.Pow(pixelValue - avg, 2);
                            stdDev = Math.Sqrt(totalVariance / count);

                            p += 3;
                        }
                        p += nOffset;
                    }
                }

                b.UnlockBits(bmData);
            }

            return stdDev;
        }

        public RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            //orientationValue = 3;
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }

        public byte[] keysArray = new byte[]{
            (byte)0x27,
            (byte)0x30,
            (byte)0x04,
            (byte)0xA0,
            (byte)0x00,
            (byte)0x0F,
            (byte)0x93,
            (byte)0x12,
            (byte)0xA0,
            (byte)0xD1,
            (byte)0x33,
            (byte)0xE0,
            (byte)0x03,
            (byte)0xD0,
            (byte)0x00,
            (byte)0xDf,
            (byte)0x00
            };

    }
}