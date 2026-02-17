using Godot;
using System;
using Godot.Collections;

public partial class Console : CanvasLayer
{

    public LineEdit PrintLine;
    public RichTextLabel OutLabel;
    CommandSystem cs;

    [ExportGroup("Hide Consoole Key")]
    [Export] public StringName ActionName = "HideConsole";
    [Export] public Key ActionKey = Key.F12;
    
    InputEventKey TargetAction;

    public override void _Ready()
    {
        CommandSystem.RegisterObject(this);

        PrintLine = GetNode<LineEdit>("VBoxContainer/LineEdit");
        OutLabel = GetNode<RichTextLabel>("VBoxContainer/Panel/RichTextLabel");
        PrintLine.TextSubmitted += OnLineEntered;
        cs = new();
        TargetAction = new();
        TargetAction.Keycode = ActionKey;

        if (InputMap.HasAction(ActionName) == false) 
        {
            InputMap.AddAction(ActionName);
            InputMap.ActionAddEvent(ActionName, TargetAction);
        }
        
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

    public void HideConsole()
    {
        
    }





    [Command("/clear")] public void ClearConsole() => OutLabel.Clear();

}