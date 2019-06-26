using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.Runtime.CredentialManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using TwoFunTwoMe_DataAccess;

namespace TwoFunTwoMe.Models.DTO
{
    public class AWSAccess
    {

        public Solicitudes GetTestAsync(List<ConfigSys> configSys, List<Solicitudes> sol)
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
                AccessKey = configSys[0].llave_Config1,
                SecretKey = configSys[0].llave_Config2
            };
            try
            {
                var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("AWSProfileName", options);
                profile.Region = RegionEndpoint.USWest1;
                var netSDKFile = new NetSDKCredentialsFile();
                netSDKFile.RegisterProfile(profile);

                float similarityThreshold = 70F;
                String sourceImage = sol[0].UrlFotoSelfie;
                String targetImage = sol[0].UrlFotoCedula;

                using (AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient(configSys[0].llave_Config1, configSys[0].llave_Config2))
                {

                    Amazon.Rekognition.Model.Image imageSource = new Amazon.Rekognition.Model.Image();

                    using (FileStream fs = new FileStream(sourceImage, FileMode.Open, FileAccess.Read))
                    {
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, (int)fs.Length);
                        imageSource.Bytes = new MemoryStream(data);
                    }


                    Amazon.Rekognition.Model.Image imageTarget = new Amazon.Rekognition.Model.Image();

                    using (FileStream fs = new FileStream(targetImage, FileMode.Open, FileAccess.Read))
                    {
                        byte[] data = new byte[fs.Length];
                        data = new byte[fs.Length];
                        fs.Read(data, 0, (int)fs.Length);
                        imageTarget.Bytes = new MemoryStream(data);
                    }


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

                    _Solicitudes.IdTipoIdentificacion = sol.FirstOrDefault().IdTipoIdentificacion;
                    _Solicitudes.Identificacion = sol.FirstOrDefault().Identificacion;

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
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
                _Solicitudes.Mensaje = "ERR_imageTarget";
                throw;
            }
        }

        public Solicitudes DetectFace(List<ConfigSys> configSys, string UrlFoto, string Identificacion, string IdTipoIdentificacion, int OPC)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            bool resp = false;
            var param = new
            {
                UriFoto = UrlFoto,
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

            var options = new CredentialProfileOptions
            {
                AccessKey = configSys[0].llave_Config1,
                SecretKey = configSys[0].llave_Config2
            };

            try
            {
                var profile = new Amazon.Runtime.CredentialManagement.CredentialProfile("AWSProfileName", options);
                profile.Region = RegionEndpoint.USWest1;
                var netSDKFile = new NetSDKCredentialsFile();
                netSDKFile.RegisterProfile(profile);

                String targetImage = UrlFoto;

                // Using USWest2, not the default region
                using (AmazonRekognitionClient rekoClient = new AmazonRekognitionClient(configSys[0].llave_Config1, configSys[0].llave_Config2, RegionEndpoint.USEast1))
                {
                    Amazon.Rekognition.Model.Image img = new Amazon.Rekognition.Model.Image();
                    byte[] data = null;
                    using (FileStream fs = new FileStream(targetImage, FileMode.Open, FileAccess.Read))
                    {
                        data = new byte[fs.Length];
                        fs.Read(data, 0, (int)fs.Length);
                    }
                    img.Bytes = new MemoryStream(data);

                    DetectTextRequest dfr = new DetectTextRequest();
                    dfr.Image = img;
                    var outcome = rekoClient.DetectText(dfr);

                    //List<string> Detectedtext = new List<string>();

                    //foreach (var texto in outcome.TextDetections)
                    outcome.TextDetections.ForEach(texto =>
                    {
                        string cedula = "";
                        //Detectedtext.Add(texto.DetectedText);
                        cedula = texto.DetectedText;
                        cedula = cedula.Replace(" ", "").Trim();
                        //if (OPC == 2)
                        //{

                        var cedresp = cedula.Split(':');
                        var respuesta = cedresp.Where(x => x.ToString().Equals(Identificacion) || cedula.Equals(Identificacion)).Any();
                        if (respuesta)
                            resp = respuesta;

                        //if (resp.Equals(true)) texto.s;
                        //foreach (var text in cedresp)
                        //{
                        //    if (text == Identificacion)
                        //    {
                        //        resp = true;
                        //    }
                        //}

                        //}
                        //if (cedula == Identificacion && OPC == 1)
                        //{
                        //    resp = true;
                        //}
                    });
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(outcome.TextDetections.Select(x => x.DetectedText));


                    _Solicitudes.result = resp;
                    _Solicitudes.DetectedText = jsonString;
                }
                //return outcome.TextDetections.Select(x => x.DetectedText).ToList();
                return _Solicitudes;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
                _Solicitudes.Mensaje = "ERR_Detect Text";
                throw;
            }
        }

    }
}