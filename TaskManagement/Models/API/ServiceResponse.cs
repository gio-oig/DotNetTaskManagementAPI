namespace TaskManagement.Models.API
{
    public class ServiceResponse<T>
    {
        public ServiceResponse(T _data = default(T))
        {
            this.Data = _data;
        }

        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = null;
        public string Error { get; set; } = null;
        public List<string> ErrorMessages { get; set; } = null;
    }
}
