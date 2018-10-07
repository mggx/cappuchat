namespace Chat.Client.Framework
{
    public interface IModalDialog<T> : IModalDialog
    {
        T GetResult();
    }

    public interface IModalDialog : IDialog
    {
        ModalResult ModalResult { get; set; }
    }

    public enum ModalResult
    {
        Ok,
        Aborted,
        Closed
    }
}
