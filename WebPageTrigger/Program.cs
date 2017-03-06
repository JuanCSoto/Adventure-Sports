// <copyright file="Program.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>

namespace WebPageTrigger
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Receives an URL and save the result in a text file 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main point of the console program
        /// </summary>
        /// <param name="args">Only one string parameter with the page URL to download (consult)</param>
        public static void Main(string[] args)
        {            
            string token = EncriptMD5(string.Concat(SecretWord(), DateTime.Now.ToString("yyyyMMddhhmm")));
            Console.WriteLine(string.Concat("El token es: ", token));

            string url = string.Concat(args[0].TrimEnd('/'), "/", token);
            Console.WriteLine(string.Concat("La URL a disparar es: ", url));

            Uri uri;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                Console.WriteLine("En progreso, un momento por favor...");
                WebClient client = new WebClient();
                string result = client.DownloadString(uri);
                string[] uriSplit = args[0].Split('/');
                string directory = uriSplit[uriSplit.Length - 1];
                int length = directory.IndexOf('?');
                directory = directory.Substring(0, length > 0 ? length : directory.Length);

                string fullPath = string.Concat(path, "\\", directory);
                string fileName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");

                Console.WriteLine(string.Concat("Comprobando si el directorio existe: ", fullPath));
                if (!Directory.Exists(fullPath))
                {
                    Console.WriteLine("No existe, creándolo");
                    Directory.CreateDirectory(fullPath);
                }

                Console.WriteLine("Guardando el archivo.");

                File.WriteAllText(string.Concat(fullPath, "\\", fileName, ".txt"), result);

                Console.WriteLine("Se guardo el resultado de la pagina.");
            }
            else
            {
                Console.WriteLine("¡La URL no es valida!");
            }

            Console.WriteLine("Proceso terminado");
        }

        /// <summary>
        /// encrypts a string to <c>md5</c>
        /// </summary>
        /// <param name="value">value to encrypt</param>
        /// <returns>returns the string encrypt</returns>
        public static string EncriptMD5(string value)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(value));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Always return the same value for token generation
        /// </summary>
        /// <returns>A string value for token generation</returns>
        public static string SecretWord()
        {
            return "este es el.m3jor secreto para gen3rar.lLaves";
        }
    }
}
