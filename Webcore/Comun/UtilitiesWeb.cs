// <copyright file="Utilities.cs" company="Intergrupo">
//     Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Ferney Osorio</author>

namespace Webcore.Comun
{
   using System.Collections.Generic;
   using System.Configuration;
   using System.Web;
   using Domain.Entities;

   public class UtilitiesWeb
   {
      /// <summary>
      /// Metas the tags home.
      /// </summary>
      /// <returns>Lista de Meta Tags.</returns>
      public List<KeyValuePair<KeyValue, KeyValue>> MetaTagsHome(HttpRequestBase request)
      {
         List<KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>> collmeta = new List<KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>>();

         collmeta.Add(
             new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
                 new Domain.Entities.KeyValue("name", "title"),
                 new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["TitleHome"])));

         collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
             new Domain.Entities.KeyValue("name", "description"),
             new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["DescriptionHome"])));

         collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
             new Domain.Entities.KeyValue("property", "og:title"),
             new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["TitleHome"])));

         collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
             new Domain.Entities.KeyValue("property", "og:description"),
             new Domain.Entities.KeyValue("content", ConfigurationManager.AppSettings["DescriptionHome"])));

         collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
             new Domain.Entities.KeyValue("property", "og:url"),
             new Domain.Entities.KeyValue("content", ("http://" + request.Url.Host + request.ApplicationPath).TrimEnd('/') + "/")));

         collmeta.Add(new KeyValuePair<Domain.Entities.KeyValue, Domain.Entities.KeyValue>(
             new Domain.Entities.KeyValue("property", "og:image"),
             new Domain.Entities.KeyValue("content", ("http://" + request.Url.Host + request.ApplicationPath).TrimEnd('/') + "/1024.png")));
         
         return collmeta;
      }
   }
}