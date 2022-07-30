using System.Text.RegularExpressions;

namespace BulkFileRename
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length != 1)
      {
        Console.WriteLine("Usage: BulkFileRename.exe <mode>");
        Console.WriteLine("<mode> can be 'rename' or 'delete'");
        return;
      }
      else
      {
        Console.WriteLine("Input directory:");
        string? directory = Console.ReadLine();
        if (!Directory.Exists(directory))
        {
          while (!Directory.Exists(directory))
          {
            Console.WriteLine("Directory does not exist. Try again:");
            directory = Console.ReadLine();
          }
        }

        Console.WriteLine("RegEx Search Pattern:");
        string? searchPattern = Console.ReadLine();
        if (string.IsNullOrEmpty(searchPattern))
        {
          while (string.IsNullOrEmpty(searchPattern))
          {
            Console.WriteLine("Search pattern is empty. Try again:");
            searchPattern = Console.ReadLine();
          }
        }

        Console.WriteLine("Recursively search sub-directories? (y/n)||(Y/N)");
        string? recursive = Console.ReadLine();
        if (string.IsNullOrEmpty(recursive) || (recursive != "y" && recursive != "n" && recursive != "Y" && recursive != "N"))
        {
          while (string.IsNullOrEmpty(recursive) || (recursive != "y" && recursive != "n" && recursive != "Y" && recursive != "N"))
          {
            Console.WriteLine("Invalid input. Try again:");
            recursive = Console.ReadLine();
          }
        }
        bool recursiveBool = (recursive == "y" || recursive == "Y");

        switch (args[0])
        {
          case "rename":

            Console.WriteLine("RegEx Replace Pattern:");
            string? replacePattern = Console.ReadLine();
            if (string.IsNullOrEmpty(replacePattern))
            {
              while (string.IsNullOrEmpty(replacePattern))
              {
                Console.WriteLine("Replace pattern is empty. Try again:");
                replacePattern = Console.ReadLine();
              }
            }
            RenameFiles(directory, searchPattern, replacePattern, recursiveBool);
            break;
          case "delete":
            DeleteFiles(directory, searchPattern, recursiveBool);
            break;
          default:
            Console.WriteLine("Usage: BulkFileRename.exe <mode>");
            Console.WriteLine("<mode> can be 'rename' or 'delete'");
            break;
        }
      }
    }

    static void RenameFiles(string directory, string searchPattern, string replacePattern, bool recursive)
    {
      if (recursive)
      {
        foreach (string subdirectory in Directory.GetDirectories(directory))
        {
          RenameFiles(subdirectory, searchPattern, replacePattern, recursive);
        }
      }

      Regex rx = new Regex(searchPattern);

      string[] files = Directory.GetFiles(directory);
      foreach (string file in files)
      {
        if (rx.IsMatch(file))
        {
          string newFileName = rx.Replace(file, replacePattern);
          Console.WriteLine(file + " -> " + newFileName);
          try
          {
            File.Move(file, newFileName);
          }
          catch (Exception e)
          {
            Console.WriteLine("File Renaming Error with message: {0}", e.Message);
          }
        }
      }
    }

    static void DeleteFiles(string directory, string searchPattern, bool recursive)
    {
      if (recursive)
      {
        foreach (string subdirectory in Directory.GetDirectories(directory))
        {
          DeleteFiles(subdirectory, searchPattern, recursive);
        }
      }

      Regex rx = new Regex(searchPattern);

      string[] files = Directory.GetFiles(directory);
      foreach (string file in files)
      {
        if (rx.IsMatch(file))
        {
          try
          {
            File.Delete(file);
          }
          catch (Exception e)
          {
            Console.WriteLine("File Deletion Error with message: {0}", e.Message);
          }
        }
      }
    }
  }
}