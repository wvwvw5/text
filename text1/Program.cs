using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}

public class FileIO
{
    public Figure LoadFigure(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath);

        switch (fileExtension)
        {
            case ".txt":
                return LoadTxtFigure(filePath);
            case ".json":
                return LoadJsonFigure(filePath);
            case ".xml":
                return LoadXmlFigure(filePath);
            default:
                throw new NotSupportedException("Unsupported file format");
        }
    }

    private Figure LoadTxtFigure(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 3)
            throw new InvalidDataException("Invalid TXT file format");

        return new Figure
        {
            Name = lines[0],
            Width = double.Parse(lines[1]),
            Height = double.Parse(lines[2])
        };
    }

    private Figure LoadJsonFigure(string filePath)
    {
        using (StreamReader sr = new StreamReader(filePath))
        {
            string json = sr.ReadToEnd();
            return JsonConvert.DeserializeObject<Figure>(json);
        }
    }

    private Figure LoadXmlFigure(string filePath)
    {
        throw new NotSupportedException("XML deserialization is not implemented.");
    }

    public void SaveFigure(string filePath, Figure figure)
    {
        string fileExtension = Path.GetExtension(filePath);

        switch (fileExtension)
        {
            case ".txt":
                SaveTxtFigure(filePath, figure);
                break;
            case ".json":
                SaveJsonFigure(filePath, figure);
                break;
            case ".xml":
                SaveXmlFigure(filePath, figure);
                break;
            default:
                throw new NotSupportedException("Unsupported file format");
        }
    }

    private void SaveTxtFigure(string filePath, Figure figure)
    {
        File.WriteAllText(filePath, $"{figure.Name}\n{figure.Width}\n{figure.Height}");
    }

    private void SaveJsonFigure(string filePath, Figure figure)
    {
        string json = JsonConvert.SerializeObject(figure, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(filePath, json);
    }


    private void SaveXmlFigure(string filePath, Figure figure)
    {
        throw new NotSupportedException("XML serialization is not implemented.");
    }
}

public class TextEditor
{
    private Figure currentFigure;
    private FileIO fileIO;

    public TextEditor()
    {
        fileIO = new FileIO();
    }

    public void LoadFile(string filePath)
    {
        currentFigure = fileIO.LoadFigure(filePath);
        Console.WriteLine("File loaded successfully.");
    }

    public void EditFigure()
    {
        Console.WriteLine("Editing Figure:");
        Console.WriteLine($"Name: {currentFigure.Name}");
        Console.WriteLine($"Width: {currentFigure.Width}");
        Console.WriteLine($"Height: {currentFigure.Height}");

        Console.WriteLine("Enter new name (or press Enter to keep the current value):");
        string name = Console.ReadLine();
        if (!string.IsNullOrEmpty(name))
        {
            currentFigure.Name = name;
        }

        Console.WriteLine("Enter new width (or press Enter to keep the current value):");
        string widthInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(widthInput))
        {
            if (double.TryParse(widthInput, out double width))
            {
                currentFigure.Width = width;
            }
            else
            {
                Console.WriteLine("Invalid input for width. Width not changed.");
            }
        }

        Console.WriteLine("Enter new height (or press Enter to keep the current value):");
        string heightInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(heightInput))
        {
            if (double.TryParse(heightInput, out double height))
            {
                currentFigure.Height = height;
            }
            else
            {
                Console.WriteLine("Invalid input for height. Height not changed.");
            }
        }

        Console.WriteLine("Figure edited successfully.");
    }

    public void SaveFile(string filePath)
    {
        fileIO.SaveFigure(filePath, currentFigure);
        Console.WriteLine("File saved successfully.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Text Editor Console Application");
        TextEditor textEditor = new TextEditor();

        Console.WriteLine("Enter the file path to load:");
        string filePath = Console.ReadLine();

        try
        {
            textEditor.LoadFile(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return;
        }

        bool running = true;
        while (running)
        {
            Console.WriteLine("Press F1 to save, Escape to exit, or any other key to edit the figure.");
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.F1:
                    Console.WriteLine("Enter the file path to save:");
                    string saveFilePath = Console.ReadLine();
                    try
                    {
                        textEditor.SaveFile(saveFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    break;
                case ConsoleKey.Escape:
                    running = false;
                    break;
                default:
                    textEditor.EditFigure();
                    break;
            }
        }

        Console.WriteLine("Text Editor is closed.");
    }
}
