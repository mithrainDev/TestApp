namespace TestApp
{
    public class RequestOptions
    {
        public const string Section = "RequestOptions";

        public Uri BaseUri { get 
            {
                return new Uri($"{Host}:{Port}");
            } }

        public string Request { get; set; }

        public string Host { get; set; }
        public string Port { get; set; }
        public int Rpm { get; set; }
    }
}
