using System.Text;
using VkRadio.LowCode.AppGenerator.MetaModel;

namespace VkRadio.LowCode.AppGenerator;

class Program
{
    private static void WriteExceptionToConsole(Exception ex)
    {
        var unEx = ex as UniquinessException;
        Console.Write(unEx != null ? unEx.Message + " " + unEx.Id.ToString() : ex.ToString());
    }

    private static int Main(string[] args)
    {
        bool error;

        try
        {
            Console.InputEncoding = Encoding.ASCII;

            if (args == null || args.Length == 0)
            {
                throw new GeneratorException("Generation project file not set.");
            }

            var generationFile = args[0];

            var project = ArtefactGenerationProject.Load(generationFile);

            var success = false;

            try
            {
                foreach (var target in project.Targets)
                {
                    target.GenerateArtefacts();
                }

                success = true;
            }
            catch (GeneratorException ex)
            {
                Console.WriteLine($"Target generator exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General exception: [{ex.GetType().Name}] {ex.Message}");
            }

            Console.WriteLine(success ? project.Name : "Generation failed.");

            error = !success;
        }
        catch (Exception ex)
        when (ex is UniquinessException || ex is GeneratorException)
        {
            error = true;
            WriteExceptionToConsole(ex);
        }

        Console.ReadLine();

        return error ? 1 : 0;
    }
}
