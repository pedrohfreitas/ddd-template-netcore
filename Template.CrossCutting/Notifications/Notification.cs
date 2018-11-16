namespace Template.CrossCutting.FluentValidator
{
    public sealed class Notification
    {
        public Notification(string property, string message, bool customError = false)
        {
            Property = property;
            Message = message;
            CustomError = customError;
        }

        public string Property { get; private set; }
        public string Message { get; private set; }
        public bool CustomError { get; set; }
    }
}
