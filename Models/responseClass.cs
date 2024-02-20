namespace HomeBankingMindHub.Models
{
    public class responseClass<T>
    {
        public T Object { get; set; }
        public string message { get; set; }
        public responseClass(T Object,string mess, int code)
        {
            this.Object = Object;
            message = mess;
            this.code = code;
        }
        public int code { get; set; }
    }
}
