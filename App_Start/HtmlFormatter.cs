using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DevSumScheduler
{
    public class HtmlFormatter : MediaTypeFormatter
    {
        public HtmlFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xhtml+xml"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(
                new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }

        public override Task WriteToStreamAsync(
            Type type,
            object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext)
        {
            var encoding = SelectCharacterEncoding(content.Headers);

            string contentValue = Convert.ToString(value);
            var contentBytes = encoding.GetBytes(contentValue);

            writeStream.Write(contentBytes, 0, contentBytes.Length);
            writeStream.Flush();

            return Task.FromResult<object>(null);
        }

        public override Task<object> ReadFromStreamAsync(
            Type type,
            Stream readStream,
            HttpContent content,
            IFormatterLogger formatterLogger)
        {
            throw new NotSupportedException();
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }
    }
}