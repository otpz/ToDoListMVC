namespace ToDoListMVC.Service.Helpers.PdfGenerator
{
    public interface IPdfGenerator
    {
        Task<string> GenerateUserDataPdfById(int userId);
    }
}
