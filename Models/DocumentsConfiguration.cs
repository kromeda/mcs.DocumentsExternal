namespace DocumentsExternal.Models
{
    public class DocumentsConfiguration
    {
        public string SeqHttpHost { get; set; }

        public string SeqApiKey { get; set; }

        public string Id { get; set; }

        public string Token { get; set; }

        public HttpHostList Hosts { get; set; }

        public class HttpHostList
        {
            public string AvaxApi { get; set; }
        }
    }
}
