using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular;
using ArtefactGenerationProject.ArtefactGenerator.Sql;

namespace ArtefactGenerationProject.ArtefactGenerator
{
    /// <summary>
    /// Абстрактный генератор артефактов
    /// </summary>
    public abstract class ArtefactGeneratorJson
    {
        /// <summary>
        /// Цель генерации
        /// </summary>
        public ArtefactGenerationTargetJson Target { get; private set; }
        /// <summary>
        /// Метамодель
        /// </summary>
        //public MetaModel.MetaModel MetaModel { get; private set; }

        public ArtefactGeneratorJson(ArtefactGenerationTargetJson target) => Target = target;

        /// <summary>
        /// Генерирование пакета артефактов
        /// </summary>
        public abstract void Generate();

        /// <summary>
        /// Создание экземпляра генератор артефактов конкретного типа
        /// </summary>
        /// <param name="in_target">Цель генерации</param>
        /// <param name="in_metaModel">Метамодель</param>
        /// <returns>Генератор артефактов</returns>
        //public static ArtefactGenerator CreateConcrete(ArtefactGenerationTarget in_target, MetaModel.MetaModel in_metaModel)
        //{
        //    ArtefactGenerator generator;

        //    switch (in_target)
        //    {
        //        case TargetSql targetSql:
        //            generator = new ArtefactGeneratorSql(null);
        //            break;
        //        //case ArtefactTypeCodeEnum.PhpZf:
        //        //    generator = new ArtefactGeneratorPhpZf();
        //        //    break;
        //        case TargetCSharp targetCSharp:
        //            generator = new ArtefactGeneratorCSharpClassic(null);
        //            break;
        //        case TargetCSharpApplication targetCSharpApplication:
        //            generator = new ArtefactGeneratorCSharpApplication(null);
        //            break;
        //        case TargetCSharpProjectModel targetCSharpProjectModel:
        //            generator = new ArtefactGeneratorCSharpProjectModel(null);
        //            break;
        //        //case ArtefactTypeCodeEnum.CSharpOldVersionSave:
        //        //    generator = new ArtefactGeneratorCSharpOldVersionSave() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
        //        //    break;
        //        //case ArtefactTypeCodeEnum.CSharpProjectVersion:
        //        //    generator = new ArtefactGeneratorCSharpProjectVersion() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
        //        //    break;
        //        //case ArtefactTypeCodeEnum.InnoSetup:
        //        //    generator = new ArtefactGeneratorInnoSetup() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
        //        //    break;
        //        //case ArtefactTypeCodeEnum.MSBuild:
        //        //    generator = new ArtefactGeneratorMSBuild() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
        //        //    break;
        //        default:
        //            throw new ApplicationException($"Unrecognized target type: {in_target.GetType().Name}.");
        //    }
        //    //generator.MetaModel = in_metaModel;
        //    //generator.Target = in_target;

        //    return generator;
        //}
    };
}
