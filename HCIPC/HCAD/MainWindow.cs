using System;
using System.IO;
using System.Threading.Tasks;
using Gtk;
using HCAD;
using HCIPC;

public partial class MainWindow : Gtk.Window
{
    public string Arquivo { get; set; }
    public Interpretador Interpretador { get; set; }

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        Arquivo = "";
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnExecutarBotaoClicked(object sender, EventArgs e)
    {
        EntradaSaidaAD es = new EntradaSaidaAD()
        {
            Entrada = entradaCampo,
            Saida = saidaCampo,
            EntradaLiberada = false,
            Editor = editorCampo
        };
        Interpretador = new Interpretador();
        Interpretador.EntradaSaidaPadrao = es;
        string tmp = editorCampo.Buffer.Text;
        saidaCampo.ModifyFont(Pango.FontDescription.FromString("monospace 16"));
        Task t = new Task(() =>
        {
            Interpretador.AdicionarCodigoFonte("", tmp, true);
            if (Interpretador.Executar())
            {
                Gtk.Application.Invoke(delegate
                {
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Executado com Sucesso");
                });
            }
            else
            {
                Gtk.Application.Invoke(delegate
                {
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Executado com Erro");
                });
            }
        });
        t.Start();
    }

    protected void OnOkBotaoClicked(object sender, EventArgs e)
    {
        ((EntradaSaidaAD)Interpretador.EntradaSaidaPadrao).EntradaLiberada = true;
    }

    protected void OnSaidaCampoSizeAllocated(object o, SizeAllocatedArgs args)
    {
        saidaCampo.ScrollToIter(saidaCampo.Buffer.EndIter,0,false, 0,0);
    }

    protected void OnNovoBotaoClicked(object sender, EventArgs e)
    {
        editorCampo.Buffer.Text = "";
    }

    protected void OnAbrirBotaoClicked(object sender, EventArgs e)
    {
        Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog("Abrir Código Fonte", null, Gtk.FileChooserAction.Open);

        fcd.SelectMultiple = false;

        fcd.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
        fcd.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);

        fcd.DefaultResponse = Gtk.ResponseType.Ok;

        var f = new FileFilter()
        {
            Name = "Código Fonte"
        };
        f.AddPattern("*.hcp");
        fcd.AddFilter(f);

        Gtk.ResponseType response = (Gtk.ResponseType)fcd.Run();
        if (response == Gtk.ResponseType.Ok)
        {
            editorCampo.Buffer.Text = File.ReadAllText(fcd.Filename);
        }
        fcd.Destroy();
    }

    protected void OnSalvarBotaoClicked(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(Arquivo))
        {

            Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog("Salvar Código Fonte", null, Gtk.FileChooserAction.Save);

            fcd.SelectMultiple = false;

            fcd.AddButton(Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);
            fcd.AddButton(Gtk.Stock.Open, Gtk.ResponseType.Ok);

            fcd.DefaultResponse = Gtk.ResponseType.Ok;

            var f = new FileFilter()
            {
                Name = "Código Fonte"
            };
            f.AddPattern("*.hcp");
            fcd.AddFilter(f);

            Gtk.ResponseType response = (Gtk.ResponseType)fcd.Run();
            if (response == Gtk.ResponseType.Ok)
            {
                Arquivo = fcd.Filename;
                File.WriteAllText(fcd.Filename, editorCampo.Buffer.Text);
            }
            fcd.Destroy();
        }
        else
        {
            File.WriteAllText(Arquivo, editorCampo.Buffer.Text);
        }
    }
}
