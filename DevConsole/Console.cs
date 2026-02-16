using Godot;
using System;
using System.Security.Cryptography;

public partial class Console : CanvasLayer
{
    public LineEdit PrintLine;
    public RichTextLabel OutLabel;
    CommandSystem cs;
    

    public override void _Ready()
    {
        CommandSystem.RegisterObject(this);

        PrintLine = GetNode<LineEdit>("VBoxContainer/LineEdit");
        OutLabel = GetNode<RichTextLabel>("VBoxContainer/Panel/RichTextLabel");
        PrintLine.TextSubmitted += OnLineEntered;
        cs = new();
        
        PrintLine.CallDeferred(LineEdit.MethodName.GrabFocus);
    }

    void OnLineEntered(string text)
    {
        if (text.StartsWith("/")) 
        {
            cs.ExecuteCommand(text);
        }
        else
        {
            OutLabel.AppendText($"\n[{DateTime.Now.ToString("HH:mm:ss")}] > {text}");
        }
        
        PrintLine.Clear();
        PrintLine.GrabFocus();
        Input.ActionPress("ui_accept"); // ...
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event.IsActionPressed("HideConsole"))
        {
            this.Visible = !Visible;
        }
    }



    [Command("/clear")] public void ClearConsole() => GD.Print(OutLabel.Text);

}