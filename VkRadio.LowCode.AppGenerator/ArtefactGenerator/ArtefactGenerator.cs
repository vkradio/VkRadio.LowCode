using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp2;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator;

public abstract class ArtefactGenerator
{
    public ArtefactGenerationTarget Target { get; private set; }

    //public MetaModel.MetaModel MetaModel { get; private set; }

    public ArtefactGenerator(ArtefactGenerationTarget target) => Target = target;

    /// <summary>
    /// Generate an artefact package
    /// </summary>
    public abstract string? Generate();

    /// <summary>
    /// Create instabce of Artefact Generator for a particular type
    /// </summary>
    /// <param name="target">Generation target</param>
    /// <param name="metaModel">MetaModel</param>
    /// <returns>Artefact Generator</returns>
    public static ArtefactGenerator CreateConcrete(ArtefactGenerationTarget target, MetaModel.MetaModel metaModel)
    {
        ArtefactGenerator generator;

        switch (target)
        {
            case TargetMsSql targetSql:
                generator = new ArtefactGeneratorSql(null);
                break;
            //case ArtefactTypeCodeEnum.PhpZf:
            //    generator = new ArtefactGeneratorPhpZf();
            //    break;
            case GenerationTargetCSharp targetCSharp:
                generator = new ArtefactGeneratorCSharp(null);
                break;
            //case TargetCSharpApplication targetCSharpApplication:
            //    generator = new ArtefactGeneratorCSharpApplication(null);
            //    break;
            //case TargetCSharpProjectModel targetCSharpProjectModel:
            //    generator = new ArtefactGeneratorCSharpProjectModel(null);
            //    break;
            //case ArtefactTypeCodeEnum.CSharpOldVersionSave:
            //    generator = new ArtefactGeneratorCSharpOldVersionSave() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            //case ArtefactTypeCodeEnum.CSharpProjectVersion:
            //    generator = new ArtefactGeneratorCSharpProjectVersion() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            //case ArtefactTypeCodeEnum.InnoSetup:
            //    generator = new ArtefactGeneratorInnoSetup() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            //case ArtefactTypeCodeEnum.MSBuild:
            //    generator = new ArtefactGeneratorMSBuild() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            default:
                throw new ApplicationException($"Unrecognized target type: {target.GetType().Name}.");
        }
        //generator.MetaModel = in_metaModel;
        //generator.Target = in_target;

        return generator;
    }
}
