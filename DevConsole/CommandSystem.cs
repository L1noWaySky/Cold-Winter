using Godot;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string CommandName { get; private set; }
    public CommandAttribute(string name) => CommandName = name;
}

public class CommandSystem
{
    static readonly List<object> objects = new(); // Список объектов
    public static void RegisterObject(object obj) => objects.Add(obj); // Регистрация объекта в список
    public static void UnRegisterObject(object obj) => objects.Remove(obj); // Удаление объекта из списка

    public void ExecuteCommand(string text) // выделение и вызов команды
    {
        string[] parts = text.Split(" "); // Разделение текста

        string selectCommand = parts[0].ToLower(); // Выделение самой команды в виде текста(string)

        foreach (var obj in objects) // Проверка на наличие команды в объектах внутри списка
        {
            MethodInfo? method = 
                obj.GetType().GetMethods().
                    FirstOrDefault(
                        selectMethod => selectMethod.
                        GetCustomAttribute<CommandAttribute>()?.CommandName == selectCommand);

            if (method != null) { method.Invoke(obj, null); return; }
        }
        
        

        
    }
}
