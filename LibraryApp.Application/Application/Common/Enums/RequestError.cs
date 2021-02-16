namespace LibraryApp.Application.Common.Enums
{
    public enum RequestError
    {
        None = 0, // No errors, default state
        NotFound, // Requested resource not found
        AlreadyExists, // Fail while resource adding, already exists
        ValidationError,
        ApplicationException, // Internal app error
        OtherError // Not classified error
    }
}