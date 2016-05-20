using System.Web.Script.Serialization;

namespace Exon.Recab.Infrastructure.Utility.Extension
{
    public static class ObjectExtension
    {

        public static object AddNewField(this object obj, string key, object value)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            string data = js.Serialize(obj);

            string newPrametr = "\"" + key + "\":" + js.Serialize(value);

            if (data.Length == 2)
            {
                data = data.Insert(1, newPrametr);
            }
            else
            {
                data = data.Insert(data.Length - 1, "," + newPrametr);
            }

            return js.DeserializeObject(data);
        }

        public static object ConvertTo(this string obj, string type)
        {
            switch (type.ToUpper())
            {


                case "INT":
                    int tempInt = -1;

                    if (int.TryParse(obj, out tempInt))
                        return tempInt;

                    return obj;

                case "BOOL":
                    bool tempBool = false;

                    if (bool.TryParse(obj, out tempBool))
                        return tempBool;

                    return obj;
  
            }


            return obj;
        }

        public static string ToJsonString (this object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            return js.Serialize(obj);

        }

    }

}
