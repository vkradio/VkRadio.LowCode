using System.Text;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.AppConsole;

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

            static DBSchemaDomainModel DbSchemaDomainModelConstructor(DomainModel domainModel, ArtefactGeneratorSql artefactGeneratorSql)
            {
                DBSchemaDomainModel schemaDomainModel = artefactGeneratorSql.Type switch
                {
                    ArtefactTypeEnum.MsSql => new MsSqlDBSchemaMetaModel(domainModel, artefactGeneratorSql),
                    _ => throw new ApplicationException($"Unsupported SQL dialect code: {artefactGeneratorSql.Type}.")
                };

                return schemaDomainModel;
            }

            static ArtefactGenerator ArtefactGeneratorConstructor(ArtefactTypeEnum type, DomainModel domainModel, Target target)
            {
                ArtefactGenerator generator = type switch
                {
                    //ArtefactTypeEnum.MySql or ArtefactTypeEnum.MsSql or ArtefactTypeEnum.SQLite => (ArtefactGenerator)new ArtefactGeneratorSql() { _code = type, _metaModel = domainModel, _target = target },
                    ArtefactTypeEnum.MsSql => new ArtefactGeneratorSql(DbSchemaDomainModelConstructor, type, domainModel, target),
                    //ArtefactTypeEnum.PhpZf => (ArtefactGenerator)new ArtefactGeneratorPhpZf() { _code = type, _metaModel = domainModel, _target = target },
                    //ArtefactTypeEnum.CSharp => (ArtefactGenerator)new ArtefactGeneratorCSharp() { _code = type, _metaModel = domainModel, _target = target },
                    //ArtefactTypeEnum.CSharpOldVersionSave => (ArtefactGenerator)new ArtefactGeneratorCSharpOldVersionSave() { _code = type, _metaModel = domainModel, _target = target },
                    //ArtefactTypeEnum.CSharpProjectVersion => (ArtefactGenerator)new ArtefactGeneratorCSharpProjectVersion() { _code = type, _metaModel = domainModel, _target = target },
                    //ArtefactTypeEnum.InnoSetup => (ArtefactGenerator)new ArtefactGeneratorInnoSetup() { _code = type, _metaModel = domainModel, _target = target },
                    //ArtefactTypeEnum.MSBuild => (ArtefactGenerator)new ArtefactGeneratorMSBuild() { _code = type, _metaModel = domainModel, _target = target },
                    _ => throw new ApplicationException($"Unsupported ArtefactTypeEnum value: {type}."),
                };

                return generator;
            }

            var project = ArtefactGenerationProject.Load(generationFile, ArtefactGeneratorConstructor);

            var success = false;

            try
            {
                foreach (var target in project.Targets)
                {
                    var message = target.GenerateArtefacts();

                    if (message is not null)
                    {
                        Console.WriteLine(message);
                    }
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
        catch (Exception ex) when (ex is UniquinessException || ex is GeneratorException)
        {
            error = true;
            WriteExceptionToConsole(ex);
        }

        Console.ReadLine();

        return error ? 1 : 0;
    }
}
