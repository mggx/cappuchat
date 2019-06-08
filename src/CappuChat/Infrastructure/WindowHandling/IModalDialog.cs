namespace Chat.Client.Framework
{
    public interface IModalDialog : IDialog
    {
        ModalResult ModalResult { get; set; }
    }

    public interface IModalDialog<T> : IModalDialog
    {
        T GetResult();
    }
}
