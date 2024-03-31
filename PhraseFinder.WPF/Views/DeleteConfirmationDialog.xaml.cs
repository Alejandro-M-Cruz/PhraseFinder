using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PhraseFinder.WPF.Views;

public partial class DeleteConfirmationDialog : UserControl
{
    public string Title { get; set; } = "¿Está seguro de que desea eliminar este recurso?";
    public string Message { get; set; } = "Una vez eliminado, no se podrá recuperar.";
    public ICommand? ConfirmButtonCommand { get; set; }

    public DeleteConfirmationDialog()
    {
        InitializeComponent();
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
	    ConfirmButton.IsEnabled = false;
        CancelButton.IsEnabled = false;
        CloseButton.IsEnabled = false;
    }
}