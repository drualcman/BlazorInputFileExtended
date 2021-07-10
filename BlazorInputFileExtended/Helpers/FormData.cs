using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended.Helpers
{
    class FormData
    {

        public static MultipartFormDataContent SetMultipartFormDataContent<TModel>(TModel data)
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();
            Type t = data.GetType();
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);      //get properties only
            int c = properties.Length;
            for (int i = 0; i < c; i++)
            {
                try
                {
                    if (properties[i].PropertyType.Name == nameof(DateTime)) formData.Add(new StringContent(Convert.ToDateTime(properties[i].GetValue(data)).ToString("yyyy/MM/dd HH:mm:ss")), properties[i].Name);
                    else formData.Add(new StringContent(properties[i].GetValue(data).ToString()), properties[i].Name);
                }
                catch { }
            }
            return formData;
        }
    }
}
