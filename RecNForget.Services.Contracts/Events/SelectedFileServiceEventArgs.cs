namespace RecNForget.Services.Contracts.Events;

public class SelectedFileServiceEventArgs
{
    public string FileName { get; private set; }
    public bool HasFileSelected { get => !string.IsNullOrEmpty(FileName); }

    public SelectedFileServiceEventArgs(string fileName = null)
    {
        FileName = fileName;
    }
}
