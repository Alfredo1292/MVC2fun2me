using System;
using System.Drawing;
using System.IO;
using System.Linq;
namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Utilitarios
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static bool ValidarFichero(string Carpeta)
        {
            //obtenemos sólo la carpeta (quitamos el ejecutable) 
            string carpeta = Path.GetDirectoryName(Carpeta);
            try
            {
                //si no existe la carpeta temporal la creamos 
                if (!(Directory.Exists(carpeta)))
                {
                    Directory.CreateDirectory(carpeta);
                    return true;
                }

                if (Directory.Exists(carpeta))
                {
                    DirectoryInfo directory = new DirectoryInfo(carpeta);

                    //FileInfo[] files = directory.GetFiles("*.*");
                    directory.EnumerateFiles().ToList().ForEach(x => x.Delete());

                    //if (File.Exists())
                    //    File.Delete(file);

                    //Directory.Delete(carpeta);
                    return true;
                }
            }
            catch//(Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool ValidarExtension(string NombreArchivo)
        {
            //bool valida = false;
            String fileExtension = System.IO.Path.GetExtension(NombreArchivo).ToLower();
            String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };

            var res = allowedExtensions.Where(x => x.ToString().Equals(fileExtension.ToString()));

            //foreach (var FileEXT in allowedExtensions)
            //{
            //    if (fileExtension == FileEXT)
            //    {
            //        valida = true;
            //        return valida;
            //    }
            //    else
            //    {
            //        valida = false;
            //    }

            //}
            return res.Any();
        }

        public static string Renamefile(string pathFileName, string Nombrefile, string Extension)
        {
            string s_newfilename = "";
            string carpeta = Path.GetDirectoryName(pathFileName);
            int NumFileTemp = 0;
            DirectoryInfo directory = new DirectoryInfo(carpeta);

            FileInfo[] files = directory.GetFiles("*.*");
            if (files.FirstOrDefault() != null)
            {
                int i = 0;
                foreach (var File in files)
                {
                    i++;
                    string NomTemp = Nombrefile + "_" + i + Extension;

                    string nombre = File.Name;
                    int start = nombre.IndexOf("_") + 1;
                    int end = nombre.IndexOf(".", start);
                    NumFileTemp = Convert.ToInt32(nombre.Substring(start, end - start));


                }

                if (NumFileTemp != 0)
                {
                    NumFileTemp = NumFileTemp + 1;
                    s_newfilename = Nombrefile + "_" + NumFileTemp + Extension;
                }
            }
            else
            {
                s_newfilename = Nombrefile + "_1" + Extension;
            }
            return carpeta + "/" + s_newfilename;
        }

		public static Bitmap BytesToBitmap(byte[] byteArray)
		{
			using (MemoryStream ms = new MemoryStream(byteArray))
			{
				Bitmap img = (Bitmap)Image.FromStream(ms);
				return img;
    }
		}
	}
}
