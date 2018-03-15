using System;
using UsersDomain.Exceptions.Confirmation;

namespace UsersDomain.ValueTypes.Confirmation
{
    public class Link
    {
        private readonly Uri uri;

        public Link(string url)
        {
            uri = TryCreateFrom(url);
        }

        private Uri TryCreateFrom(string url)
        {
            Uri result;
            var created = Uri.TryCreate(url, UriKind.Absolute, out result);
            var isHttp = result.Scheme == Uri.UriSchemeHttp;
            var isHttps = result.Scheme == Uri.UriSchemeHttps;
            
            if(!created || !isHttp && !isHttps)
            {
                throw new LinkException("Bad link");
            }

            return result;
        }

        public override string ToString()
        {
            return uri.ToString();
        }
    }
}
