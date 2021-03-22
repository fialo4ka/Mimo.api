namespace Mimo.Common.DataModels
{
    //this is necessary to customize different types of errors from api, if it not needed this model is not necessary
    public class DataResponceModel<T>
    {
        public T Model { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
