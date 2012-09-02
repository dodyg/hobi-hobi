
namespace HobiHobi.Core.Framework
{
    public class EmptyHttpReponse
    {
        public static EmptyHttpReponse Instance
        {
            get
            {
                return new EmptyHttpReponse { };
            }
        }
    }
}
