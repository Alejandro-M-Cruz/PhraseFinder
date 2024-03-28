using System.Windows.Controls;
using System.Windows.Input;

namespace PhraseFinder.WPF.Views;

public partial class DeleteConfirmationDialog : UserControl
{
    public string Title { get; set; } = "¿Está seguro de que desea eliminar este recurso?";
    public ICommand? ConfirmButtonCommand { get; set; }

    public DeleteConfirmationDialog()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
	    ConfirmButton.Click += (sender, args) =>
	    {
		    ConfirmButton.IsEnabled = false;
		    CancelButton.IsEnabled = false;
	    };
	    base.OnInitialized(e);
    }
}