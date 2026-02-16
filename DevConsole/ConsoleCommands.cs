using Godot;
using System;

public partial class ConsoleCommands : Node
{
    Console This;

    public override void _Ready()
    {
        CommandSystem.RegisterObject(this);

        This = GetParent<Console>();

        GD.Print("Есть контакт");
        GD.Print($"Родитель: {This.Name}");
    }
    public override void _ExitTree() => CommandSystem.UnRegisterObject(this);

    [Command("/allnodes")] public void Nodes()
    {
        Node targetParent = This.GetParent();

        bool next = true;

        while (targetParent.GetParent<Node>() != null && targetParent.GetParent() is not Window)
        {
            targetParent = targetParent.GetParent<Node>();
            GD.Print($"New parent: {targetParent.Name}");
        }

        This.OutLabel.AppendText($"\nAll Nodes({targetParent.GetType()}): {This.GetParent().GetTreeString()}");

        return;
    }

}
