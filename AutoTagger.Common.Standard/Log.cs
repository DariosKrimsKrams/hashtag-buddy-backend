namespace AutoTagger.Contract.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Log : ILog
    {
        public int Id { get; set; }

        public string Data { get; set; }

        public DateTime Created { get; set; }

        public IDictionary<string, object> GetDataAsList()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(this.Data);
        }

        public void SetData(IDictionary<string, object> data)
        {
            this.Data = JsonConvert.SerializeObject(data);
        }
    }
}
