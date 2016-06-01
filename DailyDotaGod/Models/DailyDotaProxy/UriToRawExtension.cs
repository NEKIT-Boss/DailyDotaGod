using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using System.IO;
using Windows.Storage.Streams;
using System.Diagnostics;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    public static class UriToRawExtension
    {
        public static async Task<byte[]> DownloadRawAsync(this Uri image_uri)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(image_uri))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + " " +  image_uri.AbsoluteUri);
                        throw;
                    }

                    try
                    {
                        using (IInputStream inputStream = await response.Content.ReadAsInputStreamAsync())
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                inputStream.AsStreamForRead().CopyTo(memoryStream);
                                return memoryStream.ToArray();
                            }
                        }
                    }

                    catch
                    {
                        Debug.WriteLine("Something went wrong with data");
                        throw;
                    }

                }
            }
        }
    }
}
